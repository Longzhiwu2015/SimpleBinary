using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleBinary.Generates
{
    partial class SerializerCode
    {
        /// <summary>
        /// 创建Task[]
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="propertyType"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        void buildTask(StringBuilder builder, StringBuilder deserializeBuilder,
            Type propertyType, string serializeFieldName, string deserializeFieldName)
        {
            var genericArgType = propertyType.GetGenericArguments()[0];
            builder.AppendLine($"            if ({serializeFieldName} == null)");
            builder.AppendLine("            {");
            builder.AppendLine("                stream.WriteByte(SimpleBinary.Serializers.SerializerHelper.falseByte);");
            builder.AppendLine("            }");
            builder.AppendLine("            else");
            builder.AppendLine("            {");
            builder.AppendLine("                stream.WriteByte(SimpleBinary.Serializers.SerializerHelper.trueByte);");
            deserializeBuilder.AppendLine($"            if (SimpleBinary.Serializers.SerializerHelper.readByte(stream) == 1)");
            deserializeBuilder.AppendLine("            {");
            deserializeBuilder.AppendLine($"                {Utils.GetFullName(genericArgType)} tempTaskItem;");
            generateCodeByType(genericArgType, builder, deserializeBuilder, serializeFieldName + ".Result", "tempTaskItem");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = System.Threading.Tasks.Task.FromResult(tempTaskItem);");
            deserializeBuilder.AppendLine("            }");
            
            builder.AppendLine("            }");
        }
    }
}