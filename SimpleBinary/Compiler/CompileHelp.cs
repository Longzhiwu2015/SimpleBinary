using SimpleBinary.Compiler;
using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace SimpleBinary.Compiler
{
    /// <summary>
    /// 编译帮助类
    /// </summary>
    public class CompileHelp
    {
        /// <summary>
        /// 编译程序集
        /// </summary>
        /// <param name="code"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static bool CompileDll(string code, string savePath)
        {
            return CompileDll(code, savePath, "Dynamic_" + Guid.NewGuid().ToString("N"));
        }
        /// <summary>
        /// 编译程序集
        /// </summary>
        /// <param name="code"></param>
        /// <param name="savePath"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static bool CompileDll(string code, string savePath, string assemblyName)
        {
            var compiler = new DefaultRuntimeCompiler();
            compiler.AssemblyName = assemblyName;
            compiler.SavePath = savePath;
            compiler.Code = code;
            return compiler.Compile();
        }
        /// <summary>
        /// 编译程序集并返回生成的程序集创建的实例
        /// </summary>
        /// <param name="code"></param>
        /// <param name="savePath"></param>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        public static object CompileAndCreateInstance(string code, string savePath, string fullTypeName)
        {
            return CompileAndCreateInstance(code, savePath, "Dynamic_" + Guid.NewGuid().ToString("N"), fullTypeName);
        }
        /// <summary>
        /// 编译程序集并返回生成的程序集创建的实例
        /// </summary>
        /// <param name="code"></param>
        /// <param name="savePath"></param>
        /// <param name="assemblyName"></param>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        public static object CompileAndCreateInstance(string code, string savePath, string assemblyName, string fullTypeName)
        {
            var compiler = new DefaultRuntimeCompiler();
            compiler.AssemblyName = assemblyName;
            compiler.SavePath = savePath;
            compiler.Code = code;
            return compiler.CompileAndCreateInstance(fullTypeName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="savePath"></param>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        public static T CompileAndCreateInstance<T>(string code, string savePath, string fullTypeName)
        {
            return CompileAndCreateInstance<T>(code, savePath, "Dynamic_" + Guid.NewGuid().ToString("N"), fullTypeName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="savePath"></param>
        /// <param name="assemblyName"></param>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        public static T CompileAndCreateInstance<T>(string code, string savePath, string assemblyName, string fullTypeName)
        {
            var compiler = new DefaultRuntimeCompiler();
            compiler.AssemblyName = assemblyName;
            compiler.SavePath = savePath;
            compiler.Code = code;
            return compiler.CompileAndCreateInstance<T>(fullTypeName);
        }
        /// <summary>
        /// 编译并返回程序集
        /// </summary>
        /// <param name="code"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static Assembly CompileAndLoad(string code, string savePath)
        {
            return CompileAndLoad(code, savePath, "Dynamic_" + Guid.NewGuid().ToString("N"));
        }
        /// <summary>
        /// 编译并返回程序集
        /// </summary>
        /// <param name="code"></param>
        /// <param name="savePath"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly CompileAndLoad(string code, string savePath, string assemblyName)
        {
            var compiler = new DefaultRuntimeCompiler();
            compiler.AssemblyName = assemblyName;
            compiler.SavePath = savePath;
            compiler.Code = code;
            return compiler.CompileAndLoad();
        }
    }
}