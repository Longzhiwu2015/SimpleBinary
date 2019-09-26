using System;
using System.Reflection;

namespace SimpleBinary.Compiler
{
    /// <summary>
    /// 运行时编译服务
    /// </summary>
    public interface IRuntimeCompiler
    {
        /// <summary>
        /// 要编译的代码
        /// </summary>
        string Code { get; set; }
        /// <summary>
        /// 程序集名称
        /// </summary>
        string AssemblyName { get; set; }
        /// <summary>
        /// 程序集输出路径
        /// </summary>
        string SavePath { get; set; }
        /// <summary>
        /// 添加引用程序集
        /// </summary>
        /// <param name="referencePaths"></param>
        void AddMetadataReferences(params string[] referencePaths);
        /// <summary>
        /// 编译程序集
        /// </summary>
        /// <returns></returns>
        bool Compile();
        /// <summary>
        /// 编译并输出程序集
        /// </summary>
        /// <returns></returns>
        [Obsolete("系统统一规定，建议使用AssemblyLoader.Load(路径)加载程序集")]
        Assembly CompileAndLoad();
        /// <summary>
        /// 编译程序集并返回生成的程序集创建的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        [Obsolete("系统统一规定，建议使用AssemblyLoader.Load(路径)加载程序集")]
        T CompileAndCreateInstance<T>(string fullTypeName);
        /// <summary>
        /// 编译程序集并返回生成的程序集创建的实例
        /// </summary>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        [Obsolete("系统统一规定，建议使用AssemblyLoader.Load(路径)加载程序集")]
        object CompileAndCreateInstance(string fullTypeName);
    }
}