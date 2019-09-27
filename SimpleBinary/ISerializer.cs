using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleBinary
{
    /// <summary>
    /// 序列化接口
    /// </summary>
    public interface ISerializer<T> : ISerializer
    {
        /// <summary>
        /// 序列化到流
        /// </summary>
        /// <param name="model"></param>
        MemoryStream Serialize(T model);
        /// <summary>
        /// 序列化到流
        /// </summary>
        /// <param name="model"></param>
        /// <param name="stream"></param>
        /// <returns>返回序列化的长度</returns>
        void Serialize(T model, Stream stream);
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>返回反序列化后的对象</returns>
        T Deserialize(Stream stream);
    }
    /// <summary>
    /// 序列化接口
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 处理的类型
        /// </summary>
        Type Type { get; }
        /// <summary>
        /// 类型全名
        /// </summary>
        string ClassFullName { get; }
        /// <summary>
        /// 类型Id
        /// </summary>
        long TypeId { get; }
        /// <summary>
        /// 序列化的成员Hash
        /// </summary>
        long MemberHash { get; }
        /// <summary>
        /// 获取成员和类型描述Json
        /// </summary>
        string MemberJson { get; }
    }
}