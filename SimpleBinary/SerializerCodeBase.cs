using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleBinary
{
    /// <summary>
    /// 快速二进制序列化帮助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FastSerialize<T> : SerializerCodeBase
    {
        private FastSerialize() { }
        static FastSerialize()
        {
            _serializer = Create<T>();
        }
        static ISerializer<T> _serializer;
        /// <summary>
        /// 序列化到流
        /// </summary>
        /// <param name="model"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static void Serialize(T model, Stream stream)
        {
            _serializer.Serialize(model, stream);
        }
        /// <summary>
        /// 返回序列化结果流
        /// </summary>
        /// <param name="model"></param>
        public static MemoryStream Serialize(T model)
        {
            return _serializer.Serialize(model);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>返回序列化的长度</returns>
        public static T Deserialize(Stream stream)
        {
            return _serializer.Deserialize(stream);
        }
    }
    /// <summary>
    /// 生成二进制序列化代码的基类
    /// </summary>
    public abstract class SerializerCodeBase : ISerializerCode
    {
        /// <summary>
        /// 根据类型生成接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual ISerializer<T> Build<T>() => throw new NotImplementedException();
        /// <summary>
        /// 根据类型生成接口
        /// </summary>
        /// <param name="type"></param>
        public virtual object Build(Type type) => throw new NotImplementedException();
        /// <summary>
        /// 初始化接口
        /// </summary>
        /// <param name="serializerCode"></param>
        protected static void Init(ISerializerCode serializerCode)
        {
            _serializerCode = serializerCode;
        }
        /// <summary>
        /// 根据类型生成二进制序列化反序列化的接口
        /// </summary>
        static ISerializerCode _serializerCode;
        /// <summary>
        /// 创建序列化接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static ISerializer<T> Create<T>()
        {
            return _serializerCode.Build<T>();
        }
    }
}