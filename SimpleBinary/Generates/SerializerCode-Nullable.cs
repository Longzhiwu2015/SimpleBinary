using System;
using System.Text;

namespace SimpleBinary.Generates
{
    partial class SerializerCode
    {
        /// <summary>
        /// 创建可空类型
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="propertyType"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        void buildNullable(StringBuilder builder, StringBuilder deserializeBuilder,
            Type propertyType, string serializeFieldName, string deserializeFieldName)
        {
            var genericArgType = propertyType.GetGenericArguments()[0];
            builder.AppendLine($"            if (!{serializeFieldName}.HasValue)");
            builder.AppendLine("            {");
            builder.AppendLine("                stream.WriteByte(SimpleBinary.Serializers.SerializerHelper.falseByte);");
            builder.AppendLine("            }");
            builder.AppendLine("            else");
            builder.AppendLine("            {");
            builder.AppendLine("                stream.WriteByte(SimpleBinary.Serializers.SerializerHelper.trueByte);");
            deserializeBuilder.AppendLine($"            if (SimpleBinary.Serializers.SerializerHelper.readByte(stream) == 1)");
            deserializeBuilder.AppendLine("            {");
            generateCodeByType(genericArgType, builder, deserializeBuilder, serializeFieldName + ".Value", deserializeFieldName);
            deserializeBuilder.AppendLine("            }");

            builder.AppendLine("            }");
        }
    }
}