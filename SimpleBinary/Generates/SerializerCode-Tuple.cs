using System;
using System.Text;

namespace SimpleBinary.Generates
{
    partial class SerializerCode
    {
        /// <summary>
        /// Tuple[T]
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="propertyType"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        void buildTuple(StringBuilder builder, StringBuilder deserializeBuilder,
            Type propertyType, string serializeFieldName, string deserializeFieldName)
        {
            if (!propertyType.IsValueType)
            {
                builder.AppendLine($"            if ({serializeFieldName} == default({Utils.GetFullName(propertyType)}))");
                builder.AppendLine("            {");
                builder.AppendLine("                stream.WriteByte(SimpleBinary.Serializers.SerializerHelper.falseByte);");
                builder.AppendLine("            }");
                builder.AppendLine("            else");
                builder.AppendLine("            {");
                builder.AppendLine("                stream.WriteByte(SimpleBinary.Serializers.SerializerHelper.trueByte);");
                deserializeBuilder.AppendLine($"            if (SimpleBinary.Serializers.SerializerHelper.readByte(stream) == 1)");
                deserializeBuilder.AppendLine("            {");
            }
            var genericArguments = propertyType.GetGenericArguments();
            var arguments = new string[genericArguments.Length];
            var argumentTypes = new string[genericArguments.Length];
            for (var i = 0; i < arguments.Length; i++)
            {
                var genericType = genericArguments[i];
                var genericTypeFullName = Utils.GetFullName(genericType);
                var tempTupleModelName = $"tuple_{index++}";
                arguments[i] = tempTupleModelName;
                argumentTypes[i] = genericTypeFullName;

                deserializeBuilder.AppendLine($"                {genericTypeFullName} {tempTupleModelName};");
                generateCodeByType(genericType, builder, deserializeBuilder, $"{serializeFieldName}.Item{i + 1}", tempTupleModelName);
            }
            if (propertyType.IsValueType)
            {
                deserializeBuilder.AppendLine($"                {deserializeFieldName} = System.ValueTuple.Create<{string.Join(",", argumentTypes)}>({string.Join(",", arguments)});");
            }
            else
            {
                deserializeBuilder.AppendLine($"                {deserializeFieldName} = System.Tuple.Create<{string.Join(",", argumentTypes)}>({string.Join(",", arguments)});");
                builder.AppendLine("            }");
                deserializeBuilder.AppendLine("            }");
            }
        }
    }
}