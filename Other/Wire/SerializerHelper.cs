using Wire.Internal;
using System.IO;
using System.Threading;

namespace Wire
{
    /// <summary>
    /// 序列化和反序列化帮助类(使用此类即可)
    /// </summary>
    public class SerializerHelper
    {
        /// <summary>
        /// 序列化和反序列化帮助类(使用此类即可)
        /// </summary>
        private SerializerHelper() { }
        static Serializer _serializerInstance;
        /// <summary>
        /// 静态对象
        /// </summary>
        static Serializer SerializerInstance
        {
            get
            {
                if (_serializerInstance == null)
                {
                    Interlocked.CompareExchange(ref _serializerInstance, new Serializer(new SerializerOptions(false)), null);
                }
                return _serializerInstance;
            }
        }
        /// <summary>
        /// 将对象序列化成字节数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Serialize(object obj)
        {
            return SerializerInstance.Serialize(obj);
        }
        /// <summary>
        /// 将对象序列化到文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        /// <param name="session"></param>
        /// <param name="writeMode">CreateNew默认(不覆盖不追加)/Create覆盖/Append追加</param>
        public static void Serialize(object obj, [NotNull] string filePath, SerializerSession session, FileMode writeMode = FileMode.Create)
        {
            SerializerInstance.Serialize(obj, filePath, session, writeMode);
        }
        /// <summary>
        /// 从文件反序列化到对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        /// <param name="writeMode">CreateNew默认(不覆盖不追加)/Create覆盖/Append追加</param>
        public static void Serialize(object obj, [NotNull] string filePath, FileMode writeMode = FileMode.Create)
        {
            SerializerInstance.Serialize(obj, filePath, writeMode);
        }
        /// <summary>
        /// 序列化对象到流
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="stream"></param>
        /// <param name="session"></param>
        public static void Serialize(object obj, [NotNull] Stream stream, SerializerSession session)
        {
            SerializerInstance.Serialize(obj, stream, session);
        }
        /// <summary>
        /// 序列化对象到流
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="stream"></param>
        public void Serialize(object obj, [NotNull] Stream stream)
        {
            SerializerInstance.Serialize(obj, stream);
        }
        /// <summary>
        /// 从指定文件反序列化对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static object Deserialize([NotNull] string filePath)
        {
            return SerializerInstance.Deserialize(filePath);
        }
        /// <summary>
        /// 从指定文件反序列化对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public static object Deserialize([NotNull]string filePath, DeserializerSession session)
        {
            return SerializerInstance.Deserialize(filePath, session);
        }
        /// <summary>
        /// 从字节数组反序列化对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object Deserialize([NotNull] byte[] bytes)
        {
            return SerializerInstance.Deserialize(bytes);
        }
        /// <summary>
        /// 从流反序列化到对象
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public object Deserialize([NotNull] Stream stream)
        {
            return SerializerInstance.Deserialize(stream);
        }
        /// <summary>
        /// 从流反序列化到对象
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public object Deserialize([NotNull] Stream stream, DeserializerSession session)
        {
            return SerializerInstance.Deserialize(stream, session);
        }
        /// <summary>
        /// 释放序列化对象
        /// </summary>
        public static void Dispose()
        {
            Interlocked.Exchange(ref _serializerInstance, null);
        }
    }
}