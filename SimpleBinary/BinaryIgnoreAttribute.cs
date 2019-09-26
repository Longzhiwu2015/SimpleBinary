using System;

namespace SimpleBinary
{
    /// <summary>
    /// 处理二进制序列化时忽略此字段/属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class BinaryIgnoreAttribute : Attribute
    {
        /// <summary>
        /// 处理二进制序列化时忽略此字段/属性
        /// </summary>
        public BinaryIgnoreAttribute()
        {
        }
    }
}