using SimpleBinary.Compiler;
using SimpleBinary.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBinary.Generates
{
    /// <summary>
    /// 生成序列化接口帮助类
    /// </summary>
    public class SerializerCodeHelper : SerializerCodeBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                Console.WriteLine("捕获未处理异常，运行时即将终止:" + e.ExceptionObject);
                Console.WriteLine(e.ExceptionObject);
            }
            else
                Console.WriteLine(e.ExceptionObject);
        }
        static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
        }
        /// <summary>
        /// 在搜索程序集失败时执行
        /// <para>将会缓存程序集</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //尝试获取
            string name = string.Concat(args.Name.Split(',')[0], ".dll");
            string filePath = GetDllNotFoundRealPath(name);
            var assembly = AssemblyLoader.Load(filePath);
            if (assembly == null)
            {
                throw new DllNotFoundException(args.Name);
            }
            return assembly;
        }
        /// <summary>
        /// 获取缺失的程序集路径
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        public static string GetDllNotFoundRealPath(string dllName)
        {
            string pluginPath = Utils.MapPath(defaultDllDirectory);
            string filePath = GetAssemblyFile(pluginPath, dllName, false);
            if (filePath != null) return filePath;
            return GetAssemblyFile(pluginPath, dllName, true);
        }
        const string defaultDllDirectory = "/";
        /// <summary>
        /// 获取第一个程序集路径
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="fileName"></param>
        /// <param name="onlyGetInTopDir"></param>
        /// <returns></returns>
        static string GetAssemblyFile(string dirPath, string fileName, bool onlyGetInTopDir = false)
        {
            if (!Directory.Exists(dirPath))
                return null;
            var fileExt = Path.GetExtension(fileName);
            var dirInfo = new DirectoryInfo(dirPath);
            var files = dirInfo.GetFiles();
            foreach (var file in files)
            {
                if (file.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                    return file.FullName;
            }
            if (onlyGetInTopDir)
            {
                return null;
            }
            foreach (var tempDir in dirInfo.GetDirectories())
            {
                var tempFileName = GetAssemblyFile(tempDir.FullName, fileName);
                if (tempFileName == null)
                    continue;
                return tempFileName;
            }
            return null;
        }




        static SerializerCodeHelper()
        {
            Init(new SerializerCodeHelper());
        }
        /// <summary>
        /// 获取缓存键名
        /// </summary>
        /// <param name="tempType"></param>
        /// <returns></returns>
        static string GetCacheKey(Type tempType)
        {
            return "Gen=" + Utils.GetFullName(tempType).Replace('<', '[').Replace('>', ']');
        }
        const string saveOutPutPath = "/SimpleBinaryGenerates/";
        /// <summary>
        /// 根据类型生成接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override ISerializer<T> Build<T>()
        {
            return buildCodeAndCompile(typeof(T)) as ISerializer<T>;
        }
        /// <summary>
        /// 根据类型生成接口
        /// </summary>
        /// <param name="type"></param>
        public override object Build(Type type)
        {
            return buildCodeAndCompile(type);
        }
        /// <summary>
        /// 根据类型生成接口
        /// </summary>
        /// <param name="type"></param>
        object buildCodeAndCompile(Type type)
        {
            var assemblyTime = AssemblyLoader.GetAssemblyTime(type);
            //已存在，并且编译的接口程序集版本较新，则无需再重新编译
            var savePath = Path.Combine(Utils.MapPath(saveOutPutPath), GetCacheKey(type) + ".dll");
            if (File.Exists(savePath) && File.GetLastWriteTime(savePath) >= assemblyTime)
            {
                var assembly = AssemblyLoader.Load(savePath);
                return Activator.CreateInstance(assembly.ExportedTypes.First());
            }
            var genCode = new SerializerCode(type);
            var code = genCode.GenCode();
            return CompileHelp.CompileAndCreateInstance(code, savePath, genCode.ThisClassFullName);
        }
    }
    partial class SerializerCode
    {
        /// <summary>
        /// 序列化的类型是否完整支持
        /// </summary>
        /// <param name="checkType"></param>
        /// <param name="classFullName"></param>
        /// <returns></returns>
        bool isSysSuport(Type checkType, string classFullName = null)
        {
            //ITuple
            if (typeof(ITuple).IsAssignableFrom(checkType))
            {
                return true;
            }
            if (classFullName == null) classFullName = Utils.GetFullName(checkType);
            //基本类型
            if (SerializerHelper.IsSuportSerializerHelper(classFullName))
            {
                return true;
            }
            //枚举
            if (SerializerHelper.IsEnumSuportSerializerHelper(checkType))
            {
                return true;
            }
            //可空类型
            if (Utils.IsNullable(checkType))
            {
                return true;
            }
            if (checkType.IsArray) return true;
            if (checkType.IsGenericType)
            {
                var arguments = checkType.GetGenericArguments();
                if (arguments.Length == 1)
                {
                    if (typeof(ISet<>).MakeGenericType(arguments).IsAssignableFrom(checkType))
                    {
                        return true;
                    }
                    else if (typeof(IList<>).MakeGenericType(arguments).IsAssignableFrom(checkType))
                    {
                        return true;
                    }
                    else if (typeof(IEnumerable<>).MakeGenericType(arguments).IsAssignableFrom(checkType))
                    {
                        return true;
                    }
                }
                //字典
                else if (arguments.Length == 2)
                {
                    if (typeof(KeyValuePair<,>).MakeGenericType(arguments).IsAssignableFrom(checkType))
                    {
                        return true;
                    }
                    else if (typeof(SortedList<,>).MakeGenericType(arguments).IsAssignableFrom(checkType))
                    {
                        return true;
                    }
                    else if (typeof(IDictionary<,>).MakeGenericType(arguments).IsAssignableFrom(checkType))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}