using System;
using System.Text;

namespace SimpleBinary.Generates
{
    partial class SerializerCode
    {
        /// <summary>
        /// KeyValuePair[K,V]
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        /// <param name="argumentTypes"></param>
        void buildKeyValuePair(StringBuilder builder, StringBuilder deserializeBuilder,
            string serializeFieldName, string deserializeFieldName, Type[] argumentTypes)
        {
            var keyValuePairKeyName = $"keyValuePairKey_{index++}";
            var keyTypeFullName = Utils.GetFullName(argumentTypes[0]);
            deserializeBuilder.AppendLine($"           {keyTypeFullName} {keyValuePairKeyName};");
            generateCodeByType(argumentTypes[0], builder, deserializeBuilder, serializeFieldName + ".Key", keyValuePairKeyName, keyTypeFullName);
            var keyValuePairValueName = $"keyValuePairValue_{index++}";
            var valueTypeFullName = Utils.GetFullName(argumentTypes[1]);
            deserializeBuilder.AppendLine($"           {valueTypeFullName} {keyValuePairValueName};");
            generateCodeByType(argumentTypes[1], builder, deserializeBuilder, serializeFieldName + ".Value", keyValuePairValueName, valueTypeFullName);
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = new System.Collections.Generic.KeyValuePair<{keyTypeFullName}, {valueTypeFullName}>({keyValuePairKeyName}, {keyValuePairValueName});");
        }
        /// <summary>
        /// IDictionary[K,V]
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="propertyType"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        /// <param name="argumentTypes"></param>
        void buildDictionary(StringBuilder builder, StringBuilder deserializeBuilder,
            Type propertyType, string serializeFieldName, string deserializeFieldName, Type[] argumentTypes)
        {
            
            builder.AppendLine($"            if ({serializeFieldName} == default({Utils.GetFullName(propertyType)}))");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.SerializerNull(stream);");
            builder.AppendLine("            }");
            builder.AppendLine($"            else");
            builder.AppendLine("            {");
            builder.AppendLine($"               foreach (var b in System.BitConverter.GetBytes({serializeFieldName}.Count))");
            builder.AppendLine("                {");
            builder.AppendLine("                    stream.WriteByte(b);");
            builder.AppendLine("                }");
            builder.AppendLine($"               foreach (var keyValueItem in {serializeFieldName})");
            builder.AppendLine("                {");
            var tempDictCountName = $"dictCount_{index++}";

            var dictFullTypeName = Utils.GetFullName(propertyType);

            deserializeBuilder.AppendLine($"            SimpleBinary.Serializers.SerializerHelper.Deserialize(stream, out int {tempDictCountName});");
            deserializeBuilder.AppendLine($"            if ({tempDictCountName} == -1)");
            deserializeBuilder.AppendLine("            {");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = null;");
            deserializeBuilder.AppendLine("            }");
            deserializeBuilder.AppendLine($"            else");
            deserializeBuilder.AppendLine("            {");
            deserializeBuilder.AppendLine($"                var tempDict = new {dictFullTypeName}();");
            deserializeBuilder.AppendLine($"                for (var i = 0; i < {tempDictCountName}; i++)");
            deserializeBuilder.AppendLine("                {");
            var keyTypeFullName = Utils.GetFullName(argumentTypes[0]);
            var valueTypeFullName = Utils.GetFullName(argumentTypes[1]);
            deserializeBuilder.AppendLine($"                    System.Collections.Generic.KeyValuePair<{keyTypeFullName}, {valueTypeFullName}> keyValueItem;");
            buildKeyValuePair(builder, deserializeBuilder, "keyValueItem", "keyValueItem", argumentTypes);
            deserializeBuilder.AppendLine($"                    tempDict[keyValueItem.Key] = keyValueItem.Value;");
            builder.AppendLine("                }");
            builder.AppendLine("            }");
            deserializeBuilder.AppendLine("                }");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = tempDict;");
            deserializeBuilder.AppendLine("            }");
        }
        /// <summary>
        /// SortedList[K,V]
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="propertyType"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        /// <param name="argumentTypes"></param>
        void buildSortedList(StringBuilder builder, StringBuilder deserializeBuilder,
            Type propertyType, string serializeFieldName, string deserializeFieldName, Type[] argumentTypes)
        {
            builder.AppendLine($"            if ({serializeFieldName} == default({Utils.GetFullName(propertyType)}))");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.SerializerNull(stream);");
            builder.AppendLine("            }");
            builder.AppendLine($"            else");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.Serialize({serializeFieldName}.Count, stream);");
            builder.AppendLine($"               foreach (var keyValueItem in {serializeFieldName})");
            builder.AppendLine("                {");
            var tempDictCountName = $"dictCount_{index++}";

            var dictFullTypeName = Utils.GetFullName(propertyType);

            deserializeBuilder.AppendLine($"            SimpleBinary.Serializers.SerializerHelper.Deserialize(stream, out int {tempDictCountName});");
            deserializeBuilder.AppendLine($"            if ({tempDictCountName} == -1)");
            deserializeBuilder.AppendLine("            {");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = null;");
            deserializeBuilder.AppendLine("            }");
            deserializeBuilder.AppendLine($"            else");
            deserializeBuilder.AppendLine("            {");
            deserializeBuilder.AppendLine($"                var tempDict = new {dictFullTypeName}();");
            deserializeBuilder.AppendLine($"                for (var i = 0; i < {tempDictCountName}; i++)");
            deserializeBuilder.AppendLine("                {");
            var keyTypeFullName = Utils.GetFullName(argumentTypes[0]);
            var valueTypeFullName = Utils.GetFullName(argumentTypes[1]);
            deserializeBuilder.AppendLine($"                    System.Collections.Generic.KeyValuePair<{keyTypeFullName}, {valueTypeFullName}> keyValueItem;");
            buildKeyValuePair(builder, deserializeBuilder, "keyValueItem", "keyValueItem", argumentTypes);
            deserializeBuilder.AppendLine($"                    tempDict[keyValueItem.Key] = keyValueItem.Value;");
            builder.AppendLine("                }");
            builder.AppendLine("            }");
            deserializeBuilder.AppendLine("                }");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = tempDict;");
            deserializeBuilder.AppendLine("            }");
        }
    }
}