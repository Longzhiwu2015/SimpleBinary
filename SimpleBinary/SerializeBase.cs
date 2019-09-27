using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleBinary
{
    /// <summary>
    /// 序列化反序列化基类
    /// </summary>
    public abstract class SerializeBase<T> : ISerializer<T>
    {
        /// <summary>
        /// 处理的类型
        /// </summary>
        public virtual Type Type => typeof(T);
        /// <summary>
        /// 类型全名
        /// </summary>
        public virtual string ClassFullName => Utils.GetFullName(Type);
        /// <summary>
        /// 类型Id
        /// </summary>
        public abstract long TypeId { get; }
        /// <summary>
        /// 序列化的成员Hash
        /// </summary>
        public abstract long MemberHash { get; }
        /// <summary>
        /// 获取成员和类型描述Json
        /// </summary>
        public abstract string MemberJson { get; }
        /// <summary>
        /// 序列化到流
        /// </summary>
        /// <param name="model"></param>
        public MemoryStream Serialize(T model)
        {
            var stream = new MemoryStream();
            Serialize(model, stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
        /// <summary>
        /// 序列化到流
        /// </summary>
        /// <param name="model"></param>
        /// <param name="stream"></param>
        public abstract void Serialize(T model, Stream stream);
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>返回反序列化后的对象</returns>
        public abstract T Deserialize(Stream stream);
    }
}