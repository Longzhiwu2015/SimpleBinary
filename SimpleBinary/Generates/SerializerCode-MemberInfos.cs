using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleBinary.Generates
{
    partial class SerializerCode
    {
        const byte splitByte = (byte)'>';
        const byte equalByte = (byte)'=';
        char nibbleToHex(byte nibble)
        {
            return (char)((nibble < 10) ? (nibble + '0') : (nibble - 10 + 'A'));
        }
        string binaryToHex(byte nibble)
        {
            return nibbleToHex((byte)(nibble >> 4)) + "" + nibbleToHex((byte)(nibble & 0xf));
        }
        string getMemberJson(string classFullName, IEnumerable<PropertyInfo> properties, IEnumerable<FieldInfo> fields)
        {
            var builder = new StringBuilder("{");
            builder.Append("\\\"");
            builder.Append(classFullName);
            builder.Append("\\\":{");

            bool hasSome = false;
            foreach (var p in properties)
            {
                if (hasSome)
                {
                    builder.Append(",");
                }
                else
                {
                    hasSome = true;
                }
                builder.Append("\\\"");
                builder.Append(p.Name);
                builder.Append("\\\"");
                builder.Append(":");
                builder.Append("\\\"");
                builder.Append(Utils.GetFullName(p.PropertyType));
                builder.Append("\\\"");
                buildClassJson(builder, p.PropertyType);
            }
            foreach (var p in fields)
            {
                if (hasSome)
                {
                    builder.Append(",");
                }
                else
                {
                    hasSome = true;
                }
                builder.Append("\\\"");
                builder.Append(p.Name);
                builder.Append("\\\"");
                builder.Append(":");
                builder.Append("\\\"");
                builder.Append(Utils.GetFullName(p.FieldType));
                builder.Append("\\\"");
                buildClassJson(builder, p.FieldType);
            }
            builder.Append("}");
            builder.Append("}");
            return builder.ToString();
        }
        void buildClassJson(StringBuilder builder, Type type)
        {

        }
        StringBuilder memberBuilder = new StringBuilder("{");
        /// <summary>
        /// 开始一个成员访问
        /// </summary>
        void startMember()
        {
            if(memberCount > 0) memberBuilder.Append(",");
            memberBuilder.Append("\\\"members\\\":{");
            memberCount = 0;
        }
        int memberCount;
        /// <summary>
        /// 结束一个成员访问
        /// </summary>
        void endMember()
        {
            memberBuilder.Append("}");
        }
        /// <summary>
        /// 添加一个成员
        /// </summary>
        /// <param name="classFullName">成员类型</param>
        void addMember(string classFullName)
        {
            if (memberCount > 0) memberBuilder.Append(",");
            memberCount += 1;
            memberBuilder.Append("\\\"");
            memberBuilder.Append("root");
            memberBuilder.Append("\\\"");
            memberBuilder.Append(":");
            memberBuilder.Append("\\\"");
            memberBuilder.Append(classFullName);
            memberBuilder.Append("\\\"");
        }
        /// <summary>
        /// 添加一个成员
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <param name="classFullName">成员类型</param>
        void addMember(string memberName, string classFullName)
        {
            if (memberCount > 0) memberBuilder.Append(","); 
            memberCount += 1;
            memberBuilder.Append("\\\"");
            memberBuilder.Append(memberName);
            memberBuilder.Append("\\\"");
            memberBuilder.Append(":");
            memberBuilder.Append("\\\"");
            memberBuilder.Append(classFullName);
            memberBuilder.Append("\\\"");
        }
    }
}