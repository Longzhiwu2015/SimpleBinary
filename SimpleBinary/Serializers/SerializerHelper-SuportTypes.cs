using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleBinary.Serializers
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public partial class SerializerHelper
    {
        /// <summary>
        /// 是否可以SerializerHelper直接支持序列化的枚举
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumSuportSerializerHelper(Type type)
        {
            if (Utils.IsNullable(type))
            {
                var tempType = type.GetGenericArguments()[0];
                if (tempType.IsEnum) return true;
                return false;
            }
            if (type.IsEnum) return true;
            if(type.IsGenericType)
            {
                var arguments = type.GetGenericArguments();
                if(arguments.Length == 1)
                {
                    if(arguments[0].IsEnum)
                    {
                        if (typeof(IList<>).MakeGenericType(arguments).IsAssignableFrom(type))
                            return true;
                        if (typeof(IEnumerable<>).MakeGenericType(arguments).IsAssignableFrom(type))
                            return true;
                    }
                    return IsEnumSuportSerializerHelper(arguments[0]);
                }
            }
            //数组
            if (type.IsArray && type.HasElementType && type.GetElementType().IsEnum) return true;
            return false;
        }
        /// <summary>
        /// 获取支持序列化的所有类型全名
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetSuportSerializerTypeNames()
        {
            yield return "System.Byte";
            yield return "System.Byte[]";
            yield return "System.Collections.Generic.IEnumerable<System.Byte>";
            yield return "System.Collections.Generic.List<System.Byte>";
            yield return "System.Collections.Generic.IList<System.Byte>";
            yield return "System.ArraySegment<System.Byte>";
            yield return "System.ReadOnlyMemory<System.Byte>";
            yield return "System.IO.Stream";
            yield return "System.IO.MemoryStream";
            yield return "System.Nullable<System.Byte>";
            yield return "System.Nullable<System.Byte>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Byte>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Byte>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Byte>>";
            yield return "System.ArraySegment<System.Nullable<System.Byte>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Byte>>";
            yield return "System.SByte";
            yield return "System.SByte[]";
            yield return "System.Collections.Generic.IEnumerable<System.SByte>";
            yield return "System.Collections.Generic.List<System.SByte>";
            yield return "System.Collections.Generic.IList<System.SByte>";
            yield return "System.ArraySegment<System.SByte>";
            yield return "System.ReadOnlyMemory<System.SByte>";
            yield return "System.Nullable<System.SByte>";
            yield return "System.Nullable<System.SByte>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.SByte>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.SByte>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.SByte>>";
            yield return "System.ArraySegment<System.Nullable<System.SByte>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.SByte>>";
            yield return "System.String";
            yield return "System.String[]";
            yield return "System.Collections.Generic.IEnumerable<System.String>";
            yield return "System.Collections.Generic.List<System.String>";
            yield return "System.Collections.Generic.IList<System.String>";
            yield return "System.ArraySegment<System.String>";
            yield return "System.ReadOnlyMemory<System.String>";
            yield return "System.Boolean";
            yield return "System.Boolean[]";
            yield return "System.Collections.Generic.IEnumerable<System.Boolean>";
            yield return "System.Collections.Generic.List<System.Boolean>";
            yield return "System.Collections.Generic.IList<System.Boolean>";
            yield return "System.ArraySegment<System.Boolean>";
            yield return "System.ReadOnlyMemory<System.Boolean>";
            yield return "System.Nullable<System.Boolean>";
            yield return "System.Nullable<System.Boolean>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Boolean>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Boolean>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Boolean>>";
            yield return "System.ArraySegment<System.Nullable<System.Boolean>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Boolean>>";
            yield return "System.Char";
            yield return "System.Char[]";
            yield return "System.Collections.Generic.IEnumerable<System.Char>";
            yield return "System.Collections.Generic.List<System.Char>";
            yield return "System.Collections.Generic.IList<System.Char>";
            yield return "System.ArraySegment<System.Char>";
            yield return "System.ReadOnlyMemory<System.Char>";
            yield return "System.Nullable<System.Char>";
            yield return "System.Nullable<System.Char>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Char>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Char>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Char>>";
            yield return "System.ArraySegment<System.Nullable<System.Char>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Char>>";
            yield return "System.Int16";
            yield return "System.Int16[]";
            yield return "System.Collections.Generic.IEnumerable<System.Int16>";
            yield return "System.Collections.Generic.List<System.Int16>";
            yield return "System.Collections.Generic.IList<System.Int16>";
            yield return "System.ArraySegment<System.Int16>";
            yield return "System.ReadOnlyMemory<System.Int16>";
            yield return "System.Nullable<System.Int16>";
            yield return "System.Nullable<System.Int16>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Int16>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Int16>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Int16>>";
            yield return "System.ArraySegment<System.Nullable<System.Int16>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Int16>>";
            yield return "System.Int32";
            yield return "System.Int32[]";
            yield return "System.Collections.Generic.IEnumerable<System.Int32>";
            yield return "System.Collections.Generic.List<System.Int32>";
            yield return "System.Collections.Generic.IList<System.Int32>";
            yield return "System.ArraySegment<System.Int32>";
            yield return "System.ReadOnlyMemory<System.Int32>";
            yield return "System.Nullable<System.Int32>";
            yield return "System.Nullable<System.Int32>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Int32>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Int32>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Int32>>";
            yield return "System.ArraySegment<System.Nullable<System.Int32>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Int32>>";
            yield return "System.Int64";
            yield return "System.Int64[]";
            yield return "System.Collections.Generic.IEnumerable<System.Int64>";
            yield return "System.Collections.Generic.List<System.Int64>";
            yield return "System.Collections.Generic.IList<System.Int64>";
            yield return "System.ArraySegment<System.Int64>";
            yield return "System.ReadOnlyMemory<System.Int64>";
            yield return "System.Nullable<System.Int64>";
            yield return "System.Nullable<System.Int64>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Int64>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Int64>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Int64>>";
            yield return "System.ArraySegment<System.Nullable<System.Int64>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Int64>>";
            yield return "System.UInt16";
            yield return "System.UInt16[]";
            yield return "System.Collections.Generic.IEnumerable<System.UInt16>";
            yield return "System.Collections.Generic.List<System.UInt16>";
            yield return "System.Collections.Generic.IList<System.UInt16>";
            yield return "System.ArraySegment<System.UInt16>";
            yield return "System.ReadOnlyMemory<System.UInt16>";
            yield return "System.Nullable<System.UInt16>";
            yield return "System.Nullable<System.UInt16>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.UInt16>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.UInt16>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.UInt16>>";
            yield return "System.ArraySegment<System.Nullable<System.UInt16>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.UInt16>>";
            yield return "System.UInt32";
            yield return "System.UInt32[]";
            yield return "System.Collections.Generic.IEnumerable<System.UInt32>";
            yield return "System.Collections.Generic.List<System.UInt32>";
            yield return "System.Collections.Generic.IList<System.UInt32>";
            yield return "System.ArraySegment<System.UInt32>";
            yield return "System.ReadOnlyMemory<System.UInt32>";
            yield return "System.Nullable<System.UInt32>";
            yield return "System.Nullable<System.UInt32>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.UInt32>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.UInt32>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.UInt32>>";
            yield return "System.ArraySegment<System.Nullable<System.UInt32>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.UInt32>>";
            yield return "System.UInt64";
            yield return "System.UInt64[]";
            yield return "System.Collections.Generic.IEnumerable<System.UInt64>";
            yield return "System.Collections.Generic.List<System.UInt64>";
            yield return "System.Collections.Generic.IList<System.UInt64>";
            yield return "System.ArraySegment<System.UInt64>";
            yield return "System.ReadOnlyMemory<System.UInt64>";
            yield return "System.Nullable<System.UInt64>";
            yield return "System.Nullable<System.UInt64>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.UInt64>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.UInt64>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.UInt64>>";
            yield return "System.ArraySegment<System.Nullable<System.UInt64>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.UInt64>>";
            yield return "System.DateTime";
            yield return "System.DateTime[]";
            yield return "System.Collections.Generic.IEnumerable<System.DateTime>";
            yield return "System.Collections.Generic.List<System.DateTime>";
            yield return "System.Collections.Generic.IList<System.DateTime>";
            yield return "System.ArraySegment<System.DateTime>";
            yield return "System.ReadOnlyMemory<System.DateTime>";
            yield return "System.Nullable<System.DateTime>";
            yield return "System.Nullable<System.DateTime>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.DateTime>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.DateTime>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.DateTime>>";
            yield return "System.ArraySegment<System.Nullable<System.DateTime>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.DateTime>>";
            yield return "System.DateTimeOffset";
            yield return "System.DateTimeOffset[]";
            yield return "System.Collections.Generic.IEnumerable<System.DateTimeOffset>";
            yield return "System.Collections.Generic.List<System.DateTimeOffset>";
            yield return "System.Collections.Generic.IList<System.DateTimeOffset>";
            yield return "System.ArraySegment<System.DateTimeOffset>";
            yield return "System.ReadOnlyMemory<System.DateTimeOffset>";
            yield return "System.Nullable<System.DateTimeOffset>";
            yield return "System.Nullable<System.DateTimeOffset>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.DateTimeOffset>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.DateTimeOffset>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.DateTimeOffset>>";
            yield return "System.ArraySegment<System.Nullable<System.DateTimeOffset>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.DateTimeOffset>>";
            yield return "System.Data.SqlTypes.SqlDateTime";
            yield return "System.Data.SqlTypes.SqlDateTime[]";
            yield return "System.Collections.Generic.IEnumerable<System.Data.SqlTypes.SqlDateTime>";
            yield return "System.Collections.Generic.List<System.Data.SqlTypes.SqlDateTime>";
            yield return "System.Collections.Generic.IList<System.Data.SqlTypes.SqlDateTime>";
            yield return "System.ArraySegment<System.Data.SqlTypes.SqlDateTime>";
            yield return "System.ReadOnlyMemory<System.Data.SqlTypes.SqlDateTime>";
            yield return "System.Nullable<System.Data.SqlTypes.SqlDateTime>";
            yield return "System.Nullable<System.Data.SqlTypes.SqlDateTime>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Data.SqlTypes.SqlDateTime>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Data.SqlTypes.SqlDateTime>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Data.SqlTypes.SqlDateTime>>";
            yield return "System.ArraySegment<System.Nullable<System.Data.SqlTypes.SqlDateTime>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Data.SqlTypes.SqlDateTime>>";
            yield return "System.Decimal";
            yield return "System.Decimal[]";
            yield return "System.Collections.Generic.IEnumerable<System.Decimal>";
            yield return "System.Collections.Generic.List<System.Decimal>";
            yield return "System.Collections.Generic.IList<System.Decimal>";
            yield return "System.ArraySegment<System.Decimal>";
            yield return "System.ReadOnlyMemory<System.Decimal>";
            yield return "System.Nullable<System.Decimal>";
            yield return "System.Nullable<System.Decimal>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Decimal>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Decimal>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Decimal>>";
            yield return "System.ArraySegment<System.Nullable<System.Decimal>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Decimal>>";
            yield return "System.Double";
            yield return "System.Double[]";
            yield return "System.Collections.Generic.IEnumerable<System.Double>";
            yield return "System.Collections.Generic.List<System.Double>";
            yield return "System.Collections.Generic.IList<System.Double>";
            yield return "System.ArraySegment<System.Double>";
            yield return "System.ReadOnlyMemory<System.Double>";
            yield return "System.Nullable<System.Double>";
            yield return "System.Nullable<System.Double>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Double>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Double>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Double>>";
            yield return "System.ArraySegment<System.Nullable<System.Double>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Double>>";
            yield return "System.Single";
            yield return "System.Single[]";
            yield return "System.Collections.Generic.IEnumerable<System.Single>";
            yield return "System.Collections.Generic.List<System.Single>";
            yield return "System.Collections.Generic.IList<System.Single>";
            yield return "System.ArraySegment<System.Single>";
            yield return "System.ReadOnlyMemory<System.Single>";
            yield return "System.Nullable<System.Single>";
            yield return "System.Nullable<System.Single>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Single>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Single>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Single>>";
            yield return "System.ArraySegment<System.Nullable<System.Single>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Single>>";
            yield return "System.Guid";
            yield return "System.Guid[]";
            yield return "System.Collections.Generic.IEnumerable<System.Guid>";
            yield return "System.Collections.Generic.List<System.Guid>";
            yield return "System.Collections.Generic.IList<System.Guid>";
            yield return "System.ArraySegment<System.Guid>";
            yield return "System.ReadOnlyMemory<System.Guid>";
            yield return "System.Nullable<System.Guid>";
            yield return "System.Nullable<System.Guid>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.Guid>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.Guid>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.Guid>>";
            yield return "System.ArraySegment<System.Nullable<System.Guid>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.Guid>>";
            yield return "System.TimeSpan";
            yield return "System.TimeSpan[]";
            yield return "System.Collections.Generic.IEnumerable<System.TimeSpan>";
            yield return "System.Collections.Generic.List<System.TimeSpan>";
            yield return "System.Collections.Generic.IList<System.TimeSpan>";
            yield return "System.ArraySegment<System.TimeSpan>";
            yield return "System.ReadOnlyMemory<System.TimeSpan>";
            yield return "System.Nullable<System.TimeSpan>";
            yield return "System.Nullable<System.TimeSpan>[]";
            yield return "System.Collections.Generic.IEnumerable<System.Nullable<System.TimeSpan>>";
            yield return "System.Collections.Generic.List<System.Nullable<System.TimeSpan>>";
            yield return "System.Collections.Generic.IList<System.Nullable<System.TimeSpan>>";
            yield return "System.ArraySegment<System.Nullable<System.TimeSpan>>";
            yield return "System.ReadOnlyMemory<System.Nullable<System.TimeSpan>>";
            yield return "System.Uri";
            yield return "System.Version";
            yield return "System.Text.StringBuilder";
            yield return "System.Numerics.BigInteger";
            yield return "System.Numerics.Complex";
            yield return "System.Collections.BitArray";

            yield return "System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, Microsoft.Extensions.Primitives.StringValues>>";
            yield return "System.Collections.Generic.KeyValuePair<System.String, Microsoft.Extensions.Primitives.StringValues>[]";
        }
        /// <summary>
        /// 是否可以SerializerHelper直接支持序列化
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static bool IsSuportSerializerHelper(string typeFullName)
        {
            foreach(var name in GetSuportSerializerTypeNames())
            {
                if (name == typeFullName) return true;
            }
            return false;
        }
    }
}