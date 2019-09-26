using System;

namespace SimpleBinary
{
    /// <summary>
    /// 根据类型生成二进制序列化反序列化的接口
    /// </summary>
    public interface ISerializerCode
    {
        /// <summary>
        /// 根据类型生成接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ISerializer<T> Build<T>();
        /// <summary>
        /// 根据类型生成接口
        /// </summary>
        /// <param name="type"></param>
        object Build(Type type);
    }
}