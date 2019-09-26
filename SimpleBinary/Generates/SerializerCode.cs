using SimpleBinary.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleBinary.Generates
{
    partial class SerializerCode
    {
        readonly Type topClassType;
        /// <summary>
        /// 生成序列化代码
        /// </summary>
        /// <param name="type"></param>
        public SerializerCode(Type type)
        {
            this.topClassType = type;
        }
        (IEnumerable<PropertyInfo>, IEnumerable<FieldInfo>) getTypeMembers(Type classType, bool getReadOnly = false)
        {
            IEnumerable<PropertyInfo> properties;
            IEnumerable<FieldInfo> fields;
            if (getReadOnly)
            {
                properties = classType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanRead && x.GetCustomAttribute<BinaryIgnoreAttribute>() == null && x.GetIndexParameters().Length == 0);
                fields = classType.GetFields(BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttribute<BinaryIgnoreAttribute>() == null);
            }
            else
            {
                properties = classType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanRead && x.CanWrite && x.GetCustomAttribute<BinaryIgnoreAttribute>() == null && x.GetIndexParameters().Length == 0);
                fields = classType.GetFields(BindingFlags.Instance | BindingFlags.Public).Where(x => !x.IsInitOnly && x.GetCustomAttribute<BinaryIgnoreAttribute>() == null);
            }
            return (properties,fields);
        }
        const string DefaultNamespace = "SimpleBinary.GenCodes";
        /// <summary>
        /// 生成的类的全名
        /// </summary>
        public string ThisClassFullName => DefaultNamespace + "." + thisClassName;
        string thisClassName;
        /// <summary>
        /// 生成序列化反序列化接口代码
        /// </summary>
        /// <returns></returns>
        public string GenCode()
        {
            //if (type.GetConstructor(Type.EmptyTypes) == null) throw new Exception($"参与序列化和反序列化的类 {Utils.GetFullName(type)} 必须有无参构造函数");
            var builder = new StringBuilder();
            builder.AppendLine($"using System.Collections.Generic;");
            builder.AppendLine($"using System;");
            builder.AppendLine($"using System.IO;");
            builder.AppendLine($"using System.Linq;");
            builder.AppendLine($"namespace {DefaultNamespace}");
            builder.AppendLine("{");
            var deserializeBuilder = new StringBuilder();
            topClassFullName = Utils.GetFullName(topClassType);
            thisClassName = $"Serialize_{Guid.NewGuid().ToString("N")}";
            builder.AppendLine("    /// <summary>");
            builder.AppendLine($"    /// 类型 {topClassFullName} 的序列化反序列化支持接口");
            builder.AppendLine("    /// </summary>");
            builder.AppendLine($"   public class {thisClassName} : SimpleBinary.SerializeBase<{topClassFullName}>");
            builder.AppendLine("    {");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 类型全名");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public override string ClassFullName => \"{topClassFullName}\";");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 类型Id");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public override long TypeId => {Utils.HashCodeLong(topClassFullName)};");
            if (Utils.IsAnonymousType(topClassType))
            {
                throw new Exception($"暂未支持匿名类型 {topClassFullName} 的二进制反序列化");
            }
            IEnumerable<PropertyInfo> properties;
            IEnumerable<FieldInfo> fields;
            if (isSysSuport(topClassType, topClassFullName))
            {
                isTopReadOnly = true;
                properties = new List<PropertyInfo>(0);
                fields = new List<FieldInfo>(0);
            }
            else
            {
                (properties, fields) = getTypeMembers(topClassType);
            }
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 序列化");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine("        /// <param name=\"model\"></param>");
            builder.AppendLine("        /// <param name=\"stream\"></param>");
            builder.AppendLine("        /// <returns></returns>");
            builder.AppendLine($"        public override void Serialize({topClassFullName} model, Stream stream)");
            builder.AppendLine("        {");
            builder.AppendLine("            stream.WriteByte(memberHash1);");
            builder.AppendLine("            stream.WriteByte(memberHash2);");
            builder.AppendLine("            stream.WriteByte(memberHash3);");
            builder.AppendLine("            stream.WriteByte(memberHash4);");
            builder.AppendLine("            stream.WriteByte(memberHash5);");
            builder.AppendLine("            stream.WriteByte(memberHash6);");
            builder.AppendLine("            stream.WriteByte(memberHash7);");
            builder.AppendLine("            stream.WriteByte(memberHash8);");

            var currentSerializeEntity = new SerializeEntity(topClassType);
            genCodeDict[topClassFullName] = currentSerializeEntity;
            builder.AppendLine($"           {currentSerializeEntity.SerializeName}(model, stream);");
            //序列化
            startMember();
            addMember(topClassFullName);
            if (isTopReadOnly)
                generateCodeByType(topClassType, currentSerializeEntity.SerializeBuilder, currentSerializeEntity.DerializeBuilder, "model", "model", topClassFullName);
            else
                generateCode(topClassType, currentSerializeEntity.SerializeBuilder, currentSerializeEntity.DerializeBuilder, properties, fields, "model", "model");
            endMember();
            builder.AppendLine("        }");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 反序列化");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine("        /// <param name=\"stream\"></param>");
            builder.AppendLine("        /// <returns>返回反序列化后的对象</returns>");
            builder.AppendLine($"        public override {topClassFullName} Deserialize(Stream stream)");
            builder.AppendLine("        {");
            builder.AppendLine("            if (stream.ReadByte() != memberHash1");
            builder.AppendLine("            || stream.ReadByte() != memberHash2");
            builder.AppendLine("            || stream.ReadByte() != memberHash3");
            builder.AppendLine("            || stream.ReadByte() != memberHash4");
            builder.AppendLine("            || stream.ReadByte() != memberHash5");
            builder.AppendLine("            || stream.ReadByte() != memberHash6");
            builder.AppendLine("            || stream.ReadByte() != memberHash7");
            builder.AppendLine("            || stream.ReadByte() != memberHash8)");
            builder.AppendLine("            throw new Exception(\"反序列化验证失败，成员类型/数量/顺序不一致\");");
            //反序列化代码
            builder.AppendLine($"           return {currentSerializeEntity.DerializeName}(stream);");
            builder.AppendLine("        }");
            foreach (var item in genCodeDict.Values)
            {
                builder.AppendLine("        " + item.ToString());
            }


            //memberBuilder



            //var memberHash = Utils.HashCodeLong(getMemberDescription(topClassFullName, properties, fields));
            var memberHash = Utils.HashCodeLong(memberBuilder.ToString());
            var tempByteIndex = 1;
            foreach (var b in BitConverter.GetBytes(memberHash))
            {
                builder.AppendLine($"        const byte memberHash{tempByteIndex++} = 0X{binaryToHex(b)};");
            }
            bool hasSomeField = properties.Count() > 0 || fields.Count() > 0;
            if (!hasSomeField)
            {
                properties = topClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanRead && x.GetCustomAttribute<BinaryIgnoreAttribute>() == null && x.GetIndexParameters().Length == 0);
                if (!hasSomeField)
                {
                    isTopReadOnly = true;
                    //throw SysException.New($"类型 {Utils.GetFullName(type)} 不包含任何可序列化的元素");
                }
            }
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 序列化的成员Hash");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public override long MemberHash => {memberHash};");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 获取成员和类型描述Json");
            builder.AppendLine("        /// </summary>");
            //builder.AppendLine($"        public override string MemberJson => \"{getMemberJson(properties, fields)}\";");
            builder.AppendLine($"        public override string MemberJson => \"{memberBuilder.ToString()}\";");




            builder.AppendLine("    }");
            builder.AppendLine("}");
            return builder.ToString();
        }
        readonly Dictionary<string, SerializeEntity> genCodeDict = new Dictionary<string, SerializeEntity>();
        int index = 1;
        bool isTopReadOnly;
        string topClassFullName;
        /// <summary>
        /// 是否当前处理的类是只读类
        /// </summary>
        /// <param name="checkType"></param>
        /// <returns></returns>
        bool isTopReadOnlyType(Type checkType) => isTopReadOnlyType(Utils.GetFullName(checkType));
        /// <summary>
        /// 是否当前处理的类是只读类
        /// </summary>
        /// <param name="checkTypeFullName"></param>
        /// <returns></returns>
        bool isTopReadOnlyType(string checkTypeFullName) => isTopReadOnly && checkTypeFullName == topClassFullName;
    }
}