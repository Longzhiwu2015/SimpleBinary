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