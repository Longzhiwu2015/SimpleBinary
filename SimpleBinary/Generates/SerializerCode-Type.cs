using SimpleBinary.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBinary.Generates
{
    partial class SerializerCode
    {
        /// <summary>
        /// 生成序列化反序列化接口代码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="properties"></param>
        /// <param name="fields"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        /// <returns></returns>
        void generateCode(Type type, StringBuilder builder, StringBuilder deserializeBuilder, IEnumerable<PropertyInfo> properties, IEnumerable<FieldInfo> fields, string serializeFieldName, string deserializeFieldName)
        {
            //1.从缓存获取处理类型的方法，此方法不检查此处
            //2.从支持类型获取处理方法
            //3.解释并缓存
            bool hasSomeField = properties.Count() > 0 || fields.Count() > 0;
            if (!hasSomeField)
            {
                throw new Exception($"类型 {Utils.GetFullName(type)} 不包含任何可序列化的元素");
            }
            else
            {
                //序列化代码
                foreach (var p in properties)
                {
                    var propertyFullName = Utils.GetFullName(p.PropertyType);
                    addMember(p.Name, propertyFullName);
                    generateCodeByType(p.PropertyType, builder, deserializeBuilder, serializeFieldName + "." + p.Name, deserializeFieldName + "." + p.Name, propertyFullName);
                }
                foreach (var p in fields)
                {
                    var propertyFullName = Utils.GetFullName(p.FieldType);
                    addMember(p.Name, propertyFullName);
                    generateCodeByType(p.FieldType, builder, deserializeBuilder, serializeFieldName + "." + p.Name, deserializeFieldName + "." + p.Name, propertyFullName);
                }
            }
        }
        /// <summary>
        /// 生成序列化反序列化接口代码(类型拆分分解)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        /// <param name="propertyFullName"></param>
        /// <returns></returns>
        void generateCodeByType(Type type, StringBuilder builder, StringBuilder deserializeBuilder, string serializeFieldName, string deserializeFieldName, string propertyFullName = null)
        {
            //1.从缓存获取处理类型的方法
            //2.从支持类型获取处理方法
            //3.解释并缓存
            if (propertyFullName == null)
            {
                propertyFullName = Utils.GetFullName(type);
                addMember(propertyFullName);
            }
            SerializeEntity serializeEntity;
            //ITuple
            if (typeof(ITuple).IsAssignableFrom(type))
            {
                buildTuple(builder, deserializeBuilder, type, serializeFieldName, deserializeFieldName);
            }
            //已缓存的方法
            else if (!isTopReadOnlyType(propertyFullName) && genCodeDict.TryGetValue(propertyFullName, out serializeEntity))
            {
                builder.AppendLine($"            {serializeEntity.SerializeName}({serializeFieldName}, stream);");
                deserializeBuilder.AppendLine($"            {deserializeFieldName} = {serializeEntity.DerializeName}(stream);");
            }
            //基本类型
            else if (SerializerHelper.IsSuportSerializerHelper(propertyFullName))
            {
                builder.AppendLine($"            SimpleBinary.Serializers.SerializerHelper.Serialize({serializeFieldName}, stream);");
                var tempBaseName = $"baseSuport_{index++}";
                deserializeBuilder.AppendLine($"            SimpleBinary.Serializers.SerializerHelper.Deserialize(stream, out {propertyFullName} {tempBaseName});");
                deserializeBuilder.AppendLine($"            {deserializeFieldName} = {tempBaseName};");
            }
            //枚举
            else if (SerializerHelper.IsEnumSuportSerializerHelper(type))
            {
                builder.AppendLine($"            SimpleBinary.Serializers.SerializerHelper.SerializeEnum({serializeFieldName}, stream);");
                var tempBaseEnumName = $"baseEnumSuport_{index++}";
                deserializeBuilder.AppendLine($"            SimpleBinary.Serializers.SerializerHelper.DeserializeEnum(stream, out {propertyFullName} {tempBaseEnumName});");
                deserializeBuilder.AppendLine($"            {deserializeFieldName} = {tempBaseEnumName};");
            }
            //可空类型
            else if (Utils.IsNullable(type))
            {
                buildNullable(builder, deserializeBuilder, type, serializeFieldName, deserializeFieldName);
            }
            else if (type.IsArray)
            {
                buildArray(builder, deserializeBuilder, type, serializeFieldName, deserializeFieldName);
            }
            else if (type.IsGenericType)
            {
                var arguments = type.GetGenericArguments();
                if(arguments.Length == 1)
                {
                    if (typeof(ISet<>).MakeGenericType(arguments).IsAssignableFrom(type))
                    {
                        buildISet(builder, deserializeBuilder, type, serializeFieldName, deserializeFieldName);
                    }
                    else if (typeof(Task).IsAssignableFrom(type))
                    {
                        buildTask(builder, deserializeBuilder, type, serializeFieldName, deserializeFieldName);
                    }
                    else if (typeof(Lazy<>).MakeGenericType(arguments).IsAssignableFrom(type))
                    {
                        buildLazy(builder, deserializeBuilder, type, serializeFieldName, deserializeFieldName);
                    }
                    else if (typeof(IList<>).MakeGenericType(arguments).IsAssignableFrom(type))
                    {
                        buildList(builder, deserializeBuilder, type, serializeFieldName, deserializeFieldName);
                    }
                    else if (typeof(IEnumerable<>).MakeGenericType(arguments).IsAssignableFrom(type))
                    {
                        buildEnumerable(builder, deserializeBuilder, type, serializeFieldName, deserializeFieldName);
                    }
                    else
                    {
                        //最后走成员分析
                        goto toProperties;
                    }
                }
                //字典
                else if (arguments.Length == 2)
                {
                    if (typeof(KeyValuePair<,>).MakeGenericType(arguments).IsAssignableFrom(type))
                    {
                        buildKeyValuePair(builder, deserializeBuilder, serializeFieldName, deserializeFieldName, arguments);
                    }
                    else if (typeof(SortedList<,>).MakeGenericType(arguments).IsAssignableFrom(type))
                    {
                        buildSortedList(builder, deserializeBuilder, type, serializeFieldName, deserializeFieldName, arguments);
                    }
                    else if (typeof(IDictionary<,>).MakeGenericType(arguments).IsAssignableFrom(type))
                    {
                        buildDictionary(builder, deserializeBuilder, type, serializeFieldName, deserializeFieldName, arguments);
                    }
                    else
                    {
                        //最后走成员分析
                        goto toProperties;
                    }
                }
                else
                {
                    //SortedList
                    //最后走成员分析
                    goto toProperties;
                }
            }
            else
            {
                //最后走成员分析
                goto toProperties;
            }
            return;
            toProperties:
            generateCodeByProperties(type, builder, deserializeBuilder, serializeFieldName, deserializeFieldName);
        }
        /// <summary>
        /// 生成序列化反序列化接口代码(类型拆分分解)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <param name="deserializeBuilder"></param>
        /// <param name="serializeFieldName"></param>
        /// <param name="deserializeFieldName"></param>
        /// <returns></returns>
        void generateCodeByProperties(Type type, StringBuilder builder, StringBuilder deserializeBuilder, string serializeFieldName, string deserializeFieldName)
        {
            startMember();
            var (properties, fields) = getTypeMembers(type);
            var propertyFullName = Utils.GetFullName(type);
            SerializeEntity serializeEntity = new SerializeEntity(type);
            genCodeDict[propertyFullName] = serializeEntity;
            generateCode(type, serializeEntity.SerializeBuilder, serializeEntity.DerializeBuilder, properties, fields, "model", "model");

            builder.AppendLine($"            {serializeEntity.SerializeName}({serializeFieldName}, stream);");

            deserializeBuilder.AppendLine($"            {deserializeFieldName} = {serializeEntity.DerializeName}(stream);");
            endMember();
        }
    }
}