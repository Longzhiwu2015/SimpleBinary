using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using DependencyContextCompilationOptions = Microsoft.Extensions.DependencyModel.CompilationOptions;

namespace SimpleBinary.Compiler
{
    /// <summary>
    /// 运行时编译服务
    /// </summary>
    public class DefaultRuntimeCompiler : IRuntimeCompiler
    {
        static readonly List<MetadataReference> globalMetadataReferences = new List<MetadataReference>();
        /// <summary>
        /// 运行时编译服务
        /// </summary>
        /// <param name="pluginDllFullPaths">提供引用依赖插件模块等路径</param>
        public static void InitReference(params string[] pluginDllFullPaths)
        {
            if (pluginDllFullPaths == null || pluginDllFullPaths.Length == 0) return;
            var hashSet = new HashSet<MetadataReference>(globalMetadataReferences);
            var newList = new List<MetadataReference>();
            foreach (var filePath in pluginDllFullPaths)
            {
                var dllFullPath = Utils.MapPath(filePath);
                var assembly = AssemblyLoader.Load(dllFullPath);
                if (assembly == null) continue;
                using (var stream = File.OpenRead(dllFullPath))
                {
                    var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                    var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                    var referenceMetadata = assemblyMetadata.GetReference(filePath: dllFullPath);
                    if(hashSet.Add(referenceMetadata)) newList.Add(referenceMetadata);
                }
            }
            lock (((ICollection)globalMetadataReferences).SyncRoot)
            {
                globalMetadataReferences.AddRange(newList);
            }
        }
        /// <summary>
        /// 要编译的代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 添加引用程序集
        /// </summary>
        /// <param name="referencePaths"></param>
        public void AddMetadataReferences(params string[] referencePaths)
        {
            foreach(var reference in referencePaths)
            {
                metadataReferences.Add(MetadataReference.CreateFromFile(reference));
            }
        }
        HashSet<MetadataReference> metadataReferences;
        string _savePath;
        /// <summary>
        /// 程序集输出路径
        /// <para>如果不设置，则默认输出到内存中</para>
        /// </summary>
        public string SavePath
        {
            get { return _savePath; }
            set
            {
                if (value != null && value.Length != 0)
                {
                    _savePath = Utils.MapPath(value);
                    Directory.CreateDirectory(Path.GetDirectoryName(_savePath));
                }
                else
                    _savePath = value;
            }
        }
        DependencyContextCompilationOptions GetDependencyContextCompilationOptions()
        {
            var dependencyContext = DependencyContext.Load(Assembly.GetEntryAssembly());

            if (dependencyContext?.CompilationOptions != null)
            {
                return dependencyContext.CompilationOptions;
            }

            return DependencyContextCompilationOptions.Default;
        }
        CSharpParseOptions GetParseOptions(DependencyContextCompilationOptions dependencyContextOptions)
        {
            var configurationSymbol = "RELEASE";
            var defines = dependencyContextOptions.Defines.Concat(new[] { configurationSymbol });

            var parseOptions = new CSharpParseOptions(preprocessorSymbols: defines);

            if (!string.IsNullOrEmpty(dependencyContextOptions.LanguageVersion))
            {
                if (LanguageVersionFacts.TryParse(dependencyContextOptions.LanguageVersion, out var languageVersion))
                {
                    parseOptions = parseOptions.WithLanguageVersion(languageVersion);
                }
                else
                {
                    System.Console.WriteLine($"LanguageVersion {languageVersion} specified in the deps file could not be parsed.");
                }
            }
            
            return parseOptions;
        }
        CSharpCompilationOptions GetCompilationOptions(DependencyContextCompilationOptions dependencyContextOptions)
        {
            var csharpCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            // Disable 1702 until roslyn turns this off by default
            csharpCompilationOptions = csharpCompilationOptions.WithSpecificDiagnosticOptions(
                new Dictionary<string, ReportDiagnostic>
                {
                    {"CS1701", ReportDiagnostic.Suppress}, // Binding redirects
                    {"CS1702", ReportDiagnostic.Suppress},
                    {"CS1705", ReportDiagnostic.Suppress}
                });

            if (dependencyContextOptions.AllowUnsafe.HasValue)
            {
                csharpCompilationOptions = csharpCompilationOptions.WithAllowUnsafe(
                    dependencyContextOptions.AllowUnsafe.Value);
            }

            OptimizationLevel optimizationLevel;
            if (dependencyContextOptions.Optimize.HasValue)
            {
                optimizationLevel = dependencyContextOptions.Optimize.Value ?
                    OptimizationLevel.Release :
                    OptimizationLevel.Debug;
            }
            else
            {
                optimizationLevel = OptimizationLevel.Release;
            }
            csharpCompilationOptions = csharpCompilationOptions.WithOptimizationLevel(optimizationLevel);

            if (dependencyContextOptions.WarningsAsErrors.HasValue)
            {
                var reportDiagnostic = dependencyContextOptions.WarningsAsErrors.Value ?
                    ReportDiagnostic.Error :
                    ReportDiagnostic.Default;
                csharpCompilationOptions = csharpCompilationOptions.WithGeneralDiagnosticOption(reportDiagnostic);
            }
            
            return csharpCompilationOptions;
        }
        CSharpParseOptions parseOptions;
        CSharpParseOptions ParseOptions
        {
            get
            {
                if (parseOptions == null)
                {
                    lock (this)
                    {
                        var dependencyContextOptions = GetDependencyContextCompilationOptions();
                        parseOptions = GetParseOptions(dependencyContextOptions);
                        compilationOptions = GetCompilationOptions(dependencyContextOptions);
                        var metadataReferenceManager = new DefaultMetadataReferenceManager();
                        if(metadataReferences == null) metadataReferences = new HashSet<MetadataReference>();
                        foreach(var reference in metadataReferenceManager.Resolve(Assembly.GetEntryAssembly()))
                        {
                            metadataReferences.Add(reference);
                        }
                        foreach(var reference in globalMetadataReferences)
                        {
                            metadataReferences.Add(reference);
                        }
                    }
                }
                return parseOptions;
            }
        }
        CSharpCompilationOptions compilationOptions;
        SyntaxTree CreateSyntaxTree(SourceText sourceText)
        {
            return CSharpSyntaxTree.ParseText(sourceText, options: ParseOptions);
        }
        CSharpCompilation CreateCompilation(string compilationContent, string assemblyName)
        {
            SourceText sourceText = SourceText.From(compilationContent, Encoding.UTF8);
            SyntaxTree syntaxTree = CreateSyntaxTree(sourceText).WithFilePath(assemblyName);

            CSharpCompilation compilation = CreateCompilation(assemblyName).AddSyntaxTrees(syntaxTree);

            return Rewrite(compilation);
        }
        static CSharpCompilation Rewrite(CSharpCompilation compilation)
        {
            var rewrittenTrees = new List<SyntaxTree>();
            foreach (var tree in compilation.SyntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(tree, ignoreAccessibility: true);
                var rewriter = new ExpressionRewriter(semanticModel);

                var rewrittenTree = tree.WithRootAndOptions(rewriter.Visit(tree.GetRoot()), tree.Options);
                rewrittenTrees.Add(rewrittenTree);
            }

            return compilation.RemoveAllSyntaxTrees().AddSyntaxTrees(rewrittenTrees);
        }
        CSharpCompilation CreateCompilation(string assemblyName)
        {
            return CSharpCompilation.Create(
                assemblyName,
                options: compilationOptions,
                references: metadataReferences);
        }
        /// <summary>
        /// 编译程序集
        /// </summary>
        /// <returns></returns>
        public bool Compile()
        {
            MemoryStream assemblyStream = new MemoryStream();
            MemoryStream pdbStream = new MemoryStream();
            try
            {
                compile(assemblyStream, pdbStream);
                return true;
            }
            finally
            {
                assemblyStream.Dispose();
                pdbStream.Dispose();
            }
        }
        void compile(MemoryStream assemblyStream, MemoryStream pdbStream)
        {
            if (Code == null || Code.Length == 0)
            {
                throw new ArgumentNullException("待编译代码不能为空");
            }
            var compilation = CreateCompilation(Code, AssemblyName ?? Path.GetRandomFileName());
            var pdbFormat = SymbolsUtility.SupportsFullPdbGeneration() ?
                DebugInformationFormat.Pdb :
                DebugInformationFormat.PortablePdb;
            var result = compilation.Emit(
                assemblyStream,
                pdbStream,
                options: new EmitOptions(debugInformationFormat: pdbFormat));

            if (!result.Success)
            {
                List<Diagnostic> errorsDiagnostics = result.Diagnostics
                        .Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error)
                        .ToList();

                StringBuilder builder = new System.Text.StringBuilder();
                builder.AppendLine("Failed to compile generated Razor template:");

                var errorMessages = new List<string>();
                foreach (Diagnostic diagnostic in errorsDiagnostics)
                {
                    FileLinePositionSpan lineSpan = diagnostic.Location.SourceTree.GetMappedLineSpan(diagnostic.Location.SourceSpan);
                    string errorMessage = diagnostic.GetMessage();
                    string formattedMessage = $"- ({lineSpan.StartLinePosition.Line}:{lineSpan.StartLinePosition.Character}) {errorMessage}";

                    errorMessages.Add(formattedMessage);
                    builder.AppendLine(formattedMessage);
                }

                builder.AppendLine("\nSee CompilationErrors for detailed information");

                throw new Exception(string.Format(builder.ToString(), errorMessages));
            }
            assemblyStream.Seek(0, SeekOrigin.Begin);
            pdbStream.Seek(0, SeekOrigin.Begin);
            if (SavePath != null && SavePath.Length != 0)
            {
                File.WriteAllBytes(SavePath, assemblyStream.ToArray());
                var pdbFilePath = Path.Combine(Path.GetDirectoryName(SavePath), Path.GetFileNameWithoutExtension(SavePath) + ".pdb");
                //pdbStream.Close();
                File.WriteAllBytes(pdbFilePath, pdbStream.ToArray());
            }
        }
        /// <summary>
        /// 编译并输出程序集
        /// </summary>
        /// <returns></returns>
        [Obsolete("系统建议使用AssemblyLoader.Load(路径)加载程序集")]
        public Assembly CompileAndLoad()
        {
            MemoryStream assemblyStream = new MemoryStream();
            MemoryStream pdbStream = new MemoryStream();
            try
            {
                compile(assemblyStream, pdbStream);
                if(string.IsNullOrEmpty(SavePath)) return Assembly.Load(assemblyStream.ToArray(), pdbStream.ToArray());
            }
            finally
            {
                assemblyStream.Dispose();
                pdbStream.Dispose();
            }
            return AssemblyLoader.Load(SavePath);
        }
        /// <summary>
        /// 编译程序集并返回生成的程序集创建的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        [Obsolete("系统建议使用AssemblyLoader.Load(路径)加载程序集")]
        public T CompileAndCreateInstance<T>(string fullTypeName)
        {
            MemoryStream assemblyStream = new MemoryStream();
            MemoryStream pdbStream = new MemoryStream();
            try
            {
                compile(assemblyStream, pdbStream);
                if (string.IsNullOrEmpty(SavePath)) return (T)Assembly.Load(assemblyStream.ToArray(), pdbStream.ToArray()).CreateInstance(fullTypeName);
            }
            finally
            {
                assemblyStream.Dispose();
                pdbStream.Dispose();
            }
            return (T)AssemblyLoader.Load(SavePath).CreateInstance(fullTypeName);
        }
        /// <summary>
        /// 编译程序集并返回生成的程序集创建的实例
        /// </summary>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        [Obsolete("系统建议使用AssemblyLoader.Load(路径)加载程序集")]
        public object CompileAndCreateInstance(string fullTypeName)
        {
            MemoryStream assemblyStream = new MemoryStream();
            MemoryStream pdbStream = new MemoryStream();
            try
            {
                compile(assemblyStream, pdbStream);
                if (string.IsNullOrEmpty(SavePath)) return Assembly.Load(assemblyStream.ToArray(), pdbStream.ToArray()).CreateInstance(fullTypeName);
            }
            finally
            {
                assemblyStream.Dispose();
                pdbStream.Dispose();
            }
            return AssemblyLoader.Load(SavePath).CreateInstance(fullTypeName);
        }
    }
}