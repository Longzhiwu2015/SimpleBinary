using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace SimpleBinary
{
    /// <summary>
    /// 使用缓存优化的程序集加载类
    /// </summary>
    [Serializable]
    public class AssemblyLoader
    {
        static readonly Dictionary<string, AssemblyLoader> ExtensionAssemblyDict = new Dictionary<string, AssemblyLoader>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 获取其他引用路径
        /// <para>用于支持运行时编译</para>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetOtherDllPath()
        {
            lock (((ICollection)ExtensionAssemblyDict).SyncRoot)
            {
                foreach (var item in ExtensionAssemblyDict)
                    yield return item.Key;
            }
        }
        /// <summary>
        /// 使用缓存优化的程序集加载类
        /// </summary>
        internal AssemblyLoader() { }
        /// <summary>
        /// 上次写入时间
        /// </summary>
        DateTime LastWriteTime { get; set; }
        Assembly _mainAssembly;
        /// <summary>
        /// 主要的程序集
        /// </summary>
        public Assembly MainAssembly { get { return _mainAssembly; } set { Interlocked.Exchange(ref _mainAssembly, value); } }
        /// <summary>
        /// 获取已加载的程序集
        /// <para>重载版本，增加获取程序集有没有更新</para>
        /// <para>从缓存获取程序集(如果指定路径的程序集较新,则更新缓存)</para>
        /// </summary>
        /// <param name="dllPath"></param>
        /// <param name="isNewer"></param>
        /// <param name="ifNewerAction">如果指定路径程序集较新时执行的委托(新程序集，旧程序集，指定路径程序集最新写入时间)</param>
        /// <returns></returns>
        public static Assembly Load(string dllPath, out bool isNewer, Action<Assembly, Assembly, DateTime> ifNewerAction = null)
        {
            if (dllPath == null)
            {
                isNewer = false;
                return null;
            }
            var fullPath = Utils.MapPath(dllPath);//.ToLower(System.Globalization.CultureInfo.CurrentCulture);
            if (!File.Exists(fullPath))
            {
                isNewer = false;
                return null;
            }
            var lastWriteTime = File.GetLastWriteTime(fullPath);
            lock (((ICollection)ExtensionAssemblyDict).SyncRoot)
            {
                AssemblyLoader wrapper;
                Assembly oldAssembly = null;
                if (ExtensionAssemblyDict.TryGetValue(fullPath, out wrapper))
                {
                    if (wrapper.LastWriteTime == lastWriteTime)
                    {
                        isNewer = false;
                        return wrapper.MainAssembly;
                    }
                    oldAssembly = wrapper.MainAssembly;
                }
                var newAssembly = GetAssembly(fullPath);
                if (newAssembly == null)
                {
                    isNewer = false;
                    return oldAssembly;
                }
                if (oldAssembly != null)
                {
                    if (ifNewerAction != null)
                    {
                        ifNewerAction(newAssembly, wrapper.MainAssembly, lastWriteTime);
                    }
                    wrapper.LastWriteTime = lastWriteTime;
                    wrapper.MainAssembly = newAssembly;
                    isNewer = true;
                    return newAssembly;
                }
                ExtensionAssemblyDict[fullPath] = new AssemblyLoader { LastWriteTime = lastWriteTime, MainAssembly = newAssembly };
                isNewer = true;
                return newAssembly;
            }
        }
        /// <summary>
        /// 获取已加载的程序集
        /// <para>从缓存获取程序集(如果指定路径的程序集较新,则更新缓存)</para>
        /// </summary>
        /// <param name="dllPath"></param>
        /// <param name="ifNewerAction">如果指定路径程序集较新时执行的委托(新程序集，旧程序集，指定路径程序集最新写入时间)</param>
        /// <returns></returns>
        public static Assembly Load(string dllPath, Action<Assembly, Assembly, DateTime> ifNewerAction = null)
        {
            if (dllPath == null)
            {
                return null;
            }
            var fullPath = Utils.MapPath(dllPath);//.ToLower(System.Globalization.CultureInfo.CurrentCulture);
            if (!File.Exists(fullPath))
            {
                return null;
            }
            var lastWriteTime = File.GetLastWriteTime(fullPath);
            lock (((ICollection)ExtensionAssemblyDict).SyncRoot)
            {
                AssemblyLoader wrapper;
                Assembly oldAssembly = null;
                if (ExtensionAssemblyDict.TryGetValue(fullPath, out wrapper))
                {
                    if (wrapper.LastWriteTime == lastWriteTime)
                        return wrapper.MainAssembly;
                    oldAssembly = wrapper.MainAssembly;
                }
                var newAssembly = GetAssembly(fullPath);
                if (newAssembly == null)
                    return oldAssembly;
                if (oldAssembly != null)
                {
                    if (ifNewerAction != null)
                    {
                        ifNewerAction(newAssembly, wrapper.MainAssembly, lastWriteTime);
                    }
                    wrapper.LastWriteTime = lastWriteTime;
                    wrapper.MainAssembly = newAssembly;
                    return newAssembly;
                }
                ExtensionAssemblyDict[fullPath] = new AssemblyLoader { LastWriteTime = lastWriteTime, MainAssembly = newAssembly };
                return newAssembly;
            }
        }
        /// <summary>
        /// 获取已加载的程序集
        /// <para>从缓存获取程序集(如果指定路径的程序集较新,则更新缓存)</para>
        /// </summary>
        /// <param name="dllPath"></param>
        /// <param name="newAssembly"></param>
        /// <returns></returns>
        internal static void Regist(string dllPath, Assembly newAssembly)
        {
            if (dllPath == null || newAssembly == null)
            {
                return;
            }
            var fullPath = Utils.MapPath(dllPath);//.ToLower(System.Globalization.CultureInfo.CurrentCulture);
            if (!File.Exists(fullPath))
            {
                return;
            }
            var lastWriteTime = File.GetLastWriteTime(fullPath);
            lock (((ICollection)ExtensionAssemblyDict).SyncRoot)
            {
                ExtensionAssemblyDict[fullPath] = new AssemblyLoader { LastWriteTime = lastWriteTime, MainAssembly = newAssembly };
            }
        }
        /// <summary>
        /// 从指定程序集中获取指定类型
        /// <para>用来加载放在Bin下无引用关系的程序集</para>
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <param name="assemblyFullName"></param>
        /// <returns></returns>
        public static Type GetType(string typeFullName, string assemblyFullName)
        {
            lock (((ICollection)ExtensionAssemblyDict).SyncRoot)
            {
                foreach (var item in ExtensionAssemblyDict.Values)
                {
                    if (item.MainAssembly.FullName.IndexOf(assemblyFullName, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        return item.MainAssembly.GetType(typeFullName, false, true);
                    }
                }
                var type = Type.GetType(typeFullName, false, true);
                if (type == null)
                {
                    return AppDomain.CurrentDomain.Load(assemblyFullName).GetType(typeFullName, false, true);
                }
                return type;
            }
        }
        const int BufferSize = 8192;
        /// <summary>
        /// 从路径加载dll
        /// </summary>
        /// <param name="dllFullPath"></param>
        /// <param name="loadDebugInformation">是否加载pdb调试信息</param>
        /// <returns></returns>
        internal static Assembly GetAssembly(string dllFullPath, bool loadDebugInformation = true)
        {
            //return Assembly.LoadFile(dllFullPath);
            MemoryStream stream = null;
            FileStream fstream = null;
            MemoryStream pdbMemoryStream = null;
            FileStream pdbStream = null;
            try
            {
                fstream = new FileStream(dllFullPath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, false);
                byte[] buffer = new byte[BufferSize];
                int readCount;
                if (loadDebugInformation)
                {
                    var tempIndex = dllFullPath.LastIndexOf('.');
                    if (tempIndex != -1)
                    {
                        var pdbFullPath = dllFullPath.Substring(0, tempIndex) + ".pdb";
                        if (File.Exists(pdbFullPath))
                        {
                            pdbStream = new FileStream(pdbFullPath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, false);
                            pdbMemoryStream = new MemoryStream();
                            while ((readCount = pdbStream.Read(buffer, 0, BufferSize)) > 0)
                            {
                                pdbMemoryStream.Write(buffer, 0, readCount);
                                //Array.Clear(buffer, 0, OtherConst.BufferSize);
                            }
                            pdbMemoryStream.Close();
                        }
                    }
                }
                stream = new MemoryStream();
                while ((readCount = fstream.Read(buffer, 0, BufferSize)) > 0)
                {
                    stream.Write(buffer, 0, readCount);
                    //Array.Clear(buffer, 0, OtherConst.BufferSize);
                }
                stream.Close();
                if (pdbMemoryStream != null)
                {
                    return Assembly.Load(stream.ToArray(), pdbMemoryStream.ToArray());
                }
                return Assembly.Load(stream.ToArray());
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
                if (fstream != null)
                    fstream.Dispose();
                if (pdbMemoryStream != null)
                    pdbMemoryStream.Dispose();
                if (pdbStream != null)
                    pdbStream.Dispose();
            }
        }
        /// <summary>
        /// 获取程序集最后写入时间
        /// <para>用于支持运行时编译</para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateTime GetAssemblyTime(Type type)
        {
            return GetAssemblyTime(type.Assembly);
        }
        /// <summary>
        /// 获取程序集最后写入时间
        /// <para>用于支持运行时编译</para>
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static DateTime GetAssemblyTime(Assembly assembly)
        {
            if (assembly.IsDynamic) return DateTime.MinValue;
            if (assembly.Location != null && File.Exists(assembly.Location))
                return System.IO.File.GetLastWriteTime(assembly.Location);
            lock (((ICollection)ExtensionAssemblyDict).SyncRoot)
            {
                foreach (var item in ExtensionAssemblyDict.Values)
                {
                    if (item._mainAssembly.FullName == assembly.FullName)
                    {
                        return item.LastWriteTime;
                    }
                }
            }
            return DateTime.MinValue;
        }
    }
}