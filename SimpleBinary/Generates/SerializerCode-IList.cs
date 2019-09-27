using System;
using System.Text;

namespace SimpleBinary.Generates
{
    partial class SerializerCode
    {
        /// <summary>
        /// ISet[T]
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="propertyType"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        void buildISet(StringBuilder builder, StringBuilder deserializeBuilder,
            Type propertyType, string serializeFieldName, string deserializeFieldName)
        {
            builder.AppendLine($"            if ({serializeFieldName} == default({Utils.GetFullName(propertyType)}))");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.SerializerNull(stream);");
            builder.AppendLine("            }");
            builder.AppendLine($"            else");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.Serialize({serializeFieldName}.Count, stream);");
            builder.AppendLine($"               foreach (var setItem in {serializeFieldName})");
            builder.AppendLine("                {");
            var tempListCountName = $"listCount_{index++}";

            var modelType = propertyType.GetGenericArguments()[0];
            var itemModelFullName = Utils.GetFullName(modelType);
            deserializeBuilder.AppendLine($"            SimpleBinary.Serializers.SerializerHelper.Deserialize(stream, out int {tempListCountName});");
            deserializeBuilder.AppendLine($"            if ({tempListCountName} == -1)");
            deserializeBuilder.AppendLine("            {");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = null;");
            deserializeBuilder.AppendLine("            }");
            deserializeBuilder.AppendLine($"            else");
            deserializeBuilder.AppendLine("            {");
            if(propertyType.IsInterface)
            {
                deserializeBuilder.AppendLine($"                var setItems = new System.Collections.Generic.HashSet<{itemModelFullName}>();");
            }
            else
            {
                deserializeBuilder.AppendLine($"                var setItems = new {Utils.GetFullName(propertyType)}();");
            }
            deserializeBuilder.AppendLine($"                for (var i = 0; i < {tempListCountName}; i++)");
            deserializeBuilder.AppendLine("                {");

            deserializeBuilder.AppendLine($"                    var setItem = default({itemModelFullName});");
            generateCodeByType(modelType, builder, deserializeBuilder, "setItem", "setItem");


            deserializeBuilder.AppendLine($"                    setItems.Add(setItem);");
            builder.AppendLine("                }");
            builder.AppendLine("            }");
            deserializeBuilder.AppendLine("                }");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = setItems;");
            deserializeBuilder.AppendLine("            }");
        }
        /// <summary>
        /// IEnumerable[T]
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="propertyType"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        void buildEnumerable(StringBuilder builder, StringBuilder deserializeBuilder,
            Type propertyType, string serializeFieldName, string deserializeFieldName)
        {
            builder.AppendLine($"            if ({serializeFieldName} == default({Utils.GetFullName(propertyType)}))");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.SerializerNull(stream);");
            builder.AppendLine("            }");
            builder.AppendLine($"            else");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.Serialize({serializeFieldName}.Count(), stream);");
            builder.AppendLine($"               foreach (var listItem in {serializeFieldName})");
            builder.AppendLine("                {");
            var tempListCountName = $"listCount_{index++}";

            var modelType = propertyType.GetGenericArguments()[0];
            var itemModelFullName = Utils.GetFullName(modelType);
            deserializeBuilder.AppendLine($"            SimpleBinary.Serializers.SerializerHelper.Deserialize(stream, out int {tempListCountName});");
            deserializeBuilder.AppendLine($"            if ({tempListCountName} == -1)");
            deserializeBuilder.AppendLine("            {");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = null;");
            deserializeBuilder.AppendLine("            }");
            deserializeBuilder.AppendLine($"            else");
            deserializeBuilder.AppendLine("            {");
            if (propertyType.IsInterface)
            {
                deserializeBuilder.AppendLine($"                var listItems = new System.Collections.Generic.List<{itemModelFullName}>();");
            }
            else
            {
                deserializeBuilder.AppendLine($"                var listItems = new {Utils.GetFullName(propertyType)}();");
            }
            deserializeBuilder.AppendLine($"                for (var i = 0; i < {tempListCountName}; i++)");
            deserializeBuilder.AppendLine("                {");
            deserializeBuilder.AppendLine($"                    var listItem = default({itemModelFullName});");
            generateCodeByType(modelType, builder, deserializeBuilder, "listItem", "listItem");
            deserializeBuilder.AppendLine($"                    listItems.Add(listItem);");
            builder.AppendLine("                }");
            builder.AppendLine("            }");
            deserializeBuilder.AppendLine("                }");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = listItems;");
            deserializeBuilder.AppendLine("            }");
        }
        /// <summary>
        /// List[T]
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="propertyType"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        void buildList(StringBuilder builder, StringBuilder deserializeBuilder,
            Type propertyType, string serializeFieldName, string deserializeFieldName)
        {
            if (!propertyType.IsGenericType)
            {
                throw new Exception("不支持Object类型的二进制序列化");
            }
            builder.AppendLine($"            if ({serializeFieldName} == default({Utils.GetFullName(propertyType)}))");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.SerializerNull(stream);");
            builder.AppendLine("            }");
            builder.AppendLine($"            else");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.Serialize({serializeFieldName}.Count, stream);");
            builder.AppendLine($"               foreach (var listItem in {serializeFieldName})");
            builder.AppendLine("                {");
            var tempListCountName = $"listCount_{index++}";

            var modelType = propertyType.GetGenericArguments()[0];
            var itemModelFullName = Utils.GetFullName(modelType);
            deserializeBuilder.AppendLine($"            SimpleBinary.Serializers.SerializerHelper.Deserialize(stream, out int {tempListCountName});");
            deserializeBuilder.AppendLine($"            if ({tempListCountName} == -1)");
            deserializeBuilder.AppendLine("            {");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = null;");
            deserializeBuilder.AppendLine("            }");
            deserializeBuilder.AppendLine($"            else");
            deserializeBuilder.AppendLine("            {");
            if (propertyType.IsInterface)
            {
                deserializeBuilder.AppendLine($"                var listItems = new System.Collections.Generic.List<{itemModelFullName}>();");
            }
            else
            {
                deserializeBuilder.AppendLine($"                var listItems = new {Utils.GetFullName(propertyType)}();");
            }
            deserializeBuilder.AppendLine($"                for (var i = 0; i < {tempListCountName}; i++)");
            deserializeBuilder.AppendLine("                {");
            deserializeBuilder.AppendLine($"                    {itemModelFullName} listItem;");
            generateCodeByType(modelType, builder, deserializeBuilder, "listItem", "listItem");
            deserializeBuilder.AppendLine($"                    listItems.Add(listItem);");
            builder.AppendLine("                }");
            builder.AppendLine("            }");
            deserializeBuilder.AppendLine("                }");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = listItems;");
            deserializeBuilder.AppendLine("            }");
        }
        /// <summary>
        /// 数组T[]
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="propertyType"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        void buildArray(StringBuilder builder, StringBuilder deserializeBuilder,
            Type propertyType, string serializeFieldName, string deserializeFieldName)
        {
            var arrayTypeFullName = Utils.GetFullName(propertyType);
            builder.AppendLine($"            if ({serializeFieldName} == default({arrayTypeFullName}))");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.SerializerNull(stream);");
            builder.AppendLine("            }");
            builder.AppendLine($"            else");
            builder.AppendLine("            {");
            builder.AppendLine($"               SimpleBinary.Serializers.SerializerHelper.Serialize({serializeFieldName}.Length, stream);");
            builder.AppendLine($"               foreach (var arrayItem in {serializeFieldName})");
            builder.AppendLine("                {");
            var tempArrayName = $"arrayLength_{index++}";
            deserializeBuilder.AppendLine($"            SimpleBinary.Serializers.SerializerHelper.Deserialize(stream, out int {tempArrayName});");
            deserializeBuilder.AppendLine($"            if ({tempArrayName} == -1)");
            deserializeBuilder.AppendLine("            {");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = null;");
            deserializeBuilder.AppendLine("            }");
            deserializeBuilder.AppendLine($"            else");
            deserializeBuilder.AppendLine("            {");
            deserializeBuilder.AppendLine($"                var arrayItems = new {arrayTypeFullName}[{tempArrayName}];");
            deserializeBuilder.AppendLine($"                for (var i = 0; i < {tempArrayName}; i++)");
            deserializeBuilder.AppendLine("                {");
            var modelType = propertyType.GetElementType();
            deserializeBuilder.AppendLine($"                    {Utils.GetFullName(modelType)} arrayItem;");
            generateCodeByType(modelType, builder, deserializeBuilder, "arrayItem", "arrayItem");
            deserializeBuilder.AppendLine($"                    arrayItems[i] = arrayItem;");
            builder.AppendLine("                }");
            builder.AppendLine("            }");
            deserializeBuilder.AppendLine("                }");
            deserializeBuilder.AppendLine($"                {deserializeFieldName} = arrayItems;");
            deserializeBuilder.AppendLine("            }");
        }
    }
}