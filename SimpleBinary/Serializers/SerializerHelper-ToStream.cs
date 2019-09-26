using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SimpleBinary.Serializers
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public partial class SerializerHelper
    {
        /// <summary>
        /// 写入-1，表示null值
        /// </summary>
        /// <param name="stream"></param>
        public static void SerializerNull(Stream stream)
        {
            stream.WriteByte(nullByte);
            stream.WriteByte(nullByte);
            stream.WriteByte(nullByte);
            stream.WriteByte(nullByte);
        }
        /// <summary>
        /// 写入0，表示长度为0
        /// </summary>
        /// <param name="stream"></param>
        public static void SerializerZeroLength(Stream stream)
        {
            stream.WriteByte(falseByte);
            stream.WriteByte(falseByte);
            stream.WriteByte(falseByte);
            stream.WriteByte(falseByte);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<byte> bytes, Stream stream)
        {
            if (bytes == null)
            {
                SerializerNull(stream);
                return;
            }
            var length = bytes.Count();
            stream.Write(BitConverter.GetBytes(length), 0, 4);
            foreach (var b in bytes)
            {
                stream.WriteByte(b);
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<sbyte> bytes, Stream stream)
        {
            if (bytes == null)
            {
                SerializerNull(stream);
                return;
            }
            var length = bytes.Count();
            stream.Write(BitConverter.GetBytes(length), 0, 4);
            foreach (var b in bytes)
            {
                stream.WriteByte((byte)b);
            }
        }
        /// <summary>
        /// 序列化流
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="stream"></param>
        public static void Serialize(Stream sourceStream, Stream stream)
        {
            if (sourceStream == null)
            {
                SerializerNull(stream);
                return;
            }
            else
            {
                stream.Write(BitConverter.GetBytes((int)sourceStream.Length), 0, 4);
                int tempByte;
                while ((tempByte = sourceStream.ReadByte()) != -1)
                {
                    stream.WriteByte((byte)tempByte);
                }
            }
        }
        /// <summary>
        /// 序列化流(返回是否需要立即输出)
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="stream"></param>
        public static void Serialize(Stream sourceStream, MemoryStream stream)
        {
            if (sourceStream == null)
            {
                SerializerNull(stream);
            }
            else
            {
                stream.Write(BitConverter.GetBytes((int)sourceStream.Length), 0, 4);
                int tempByte;
                while ((tempByte = sourceStream.ReadByte()) != -1)
                {
                    stream.WriteByte((byte)tempByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="content"></param>
        /// <param name="stream"></param>
        public static void Serialize(string content, Stream stream)
        {
            if (content == null)
            {
                SerializerNull(stream);
                return;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            stream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            stream.Write(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="b"></param>
        /// <param name="stream"></param>
        public static void Serialize(bool b, Stream stream)
        {
            if (b)
                stream.WriteByte(trueByte);
            else
                stream.WriteByte(falseByte);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="b"></param>
        /// <param name="stream"></param>
        public static void Serialize(bool? b, Stream stream)
        {
            if (b.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            if (b.Value)
                stream.WriteByte(trueByte);
            else
                stream.WriteByte(falseByte);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="b"></param>
        /// <param name="stream"></param>
        public static void Serialize(byte b, Stream stream)
        {
            stream.WriteByte(b);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="b"></param>
        /// <param name="stream"></param>
        public static void Serialize(byte? b, Stream stream)
        {
            if (b.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.WriteByte(b.Value);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="b"></param>
        /// <param name="stream"></param>
        public static void Serialize(sbyte b, Stream stream)
        {
            stream.WriteByte((byte)b);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="b"></param>
        /// <param name="stream"></param>
        public static void Serialize(sbyte? b, Stream stream)
        {
            if (b.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.WriteByte((byte)b.Value);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c">长度为2</param>
        /// <param name="stream"></param>
        public static void Serialize(char c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(c), 0, 2);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c">长度为2</param>
        /// <param name="stream"></param>
        public static void Serialize(char? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.Write(BitConverter.GetBytes(c.Value), 0, 2);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c">长度为2</param>
        /// <param name="stream"></param>
        public static void Serialize(short c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(c), 0, 2);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c">长度为2</param>
        /// <param name="stream"></param>
        public static void Serialize(short? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.Write(BitConverter.GetBytes(c.Value), 0, 2);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c">长度为2</param>
        /// <param name="stream"></param>
        public static void Serialize(ushort c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(c), 0, 2);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c">长度为2</param>
        /// <param name="stream"></param>
        public static void Serialize(ushort? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.Write(BitConverter.GetBytes(c.Value), 0, 2);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(int c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(c), 0, 4);
            /*
            yield return (byte)(c & 0xff);
            yield return (byte)((c >> 8) & 0xff);
            yield return (byte)((c >> 16) & 0xff);
            yield return (byte)((c >> 24) & 0xff);
            //*/
            //foreach (var b in BitConverter.GetBytes(c)) yield return b;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(int? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.Write(BitConverter.GetBytes(c.Value), 0, 4);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(uint c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(c), 0, 4);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(uint? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.Write(BitConverter.GetBytes(c.Value), 0, 4);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(long c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(c), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(long? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.Write(BitConverter.GetBytes(c.Value), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(ulong c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(c), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(ulong? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.Write(BitConverter.GetBytes(c.Value), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(DateTime c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(Utils.TimeToLong(c)), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(DateTime? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.Write(BitConverter.GetBytes(Utils.TimeToLong(c.Value)), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(DateTimeOffset c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(Utils.TimeToLong(Utils.DateTimeOffSetToDateTime(c))), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(DateTimeOffset? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.Write(BitConverter.GetBytes(Utils.TimeToLong(Utils.DateTimeOffSetToDateTime(c.Value))), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(SqlDateTime c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(Utils.TimeToLong(c.Value)), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(SqlDateTime? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            stream.Write(BitConverter.GetBytes(Utils.TimeToLong(c.Value.Value)), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(decimal c, Stream stream)
        {
            int[] bits = decimal.GetBits(c);
            byte[] bytes = new byte[bits.Length * 4];
            for (int i = 0; i < bits.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    bytes[i * 4 + j] = (byte)(bits[i] >> (j * 8));
                }
            }
            stream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            stream.Write(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(decimal? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            Serialize(c.Value, stream);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(double c, Stream stream)
        {
            byte[] bytes = BitConverter.GetBytes(c);
            stream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            stream.Write(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(double? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            Serialize(c.Value, stream);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(float c, Stream stream)
        {
            byte[] bytes = BitConverter.GetBytes(c);
            stream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            stream.Write(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(float? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            Serialize(c.Value, stream);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void SerializeEnum<Enum>(Enum c, Stream stream) where Enum : struct
        {
            stream.Write(BitConverter.GetBytes(Convert.ToInt32(c)), 0, 4);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void SerializeEnum<Enum>(Enum? c, Stream stream) where Enum : struct
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            SerializeEnum(c.Value, stream);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(Guid c, Stream stream)
        {
            stream.Write(c.ToByteArray(), 0, 16);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(Guid? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            Serialize(c.Value, stream);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(TimeSpan c, Stream stream)
        {
            stream.Write(BitConverter.GetBytes(Utils.TimeSpanToLong(c)), 0, 8);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="c"></param>
        /// <param name="stream"></param>
        public static void Serialize(TimeSpan? c, Stream stream)
        {
            if (c.HasValue == false)
            {
                stream.WriteByte(falseByte);
                return;
            }
            stream.WriteByte(trueByte);
            Serialize(c.Value, stream);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<string> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount), 0, 4);
            foreach (var content in contents)
            {
                //字符串长度
                if (content == null)
                {
                    SerializerNull(stream);
                    continue;
                }
                var bytes = Encoding.UTF8.GetBytes(content);
                stream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
                stream.Write(bytes, 0, bytes.Length);
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<bool> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount), 0, 4);
            foreach (var content in contents)
            {
                //字符串长度
                stream.WriteByte(content ? trueByte : falseByte);
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<bool?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount), 0, 4);
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.WriteByte(content.Value ? trueByte : falseByte);
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<byte?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount), 0, 4);
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.WriteByte(content.Value);
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<sbyte?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount), 0, 4);
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.WriteByte((byte)content.Value);
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<char> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount), 0, 4);
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(content), 0, 2);
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<char?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(content.Value));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<short> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(content));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<short?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount), 0, 4);
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(content.Value));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<ushort> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount), 0, 4);
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(content));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<ushort?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount), 0, 4);
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(content.Value));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<int> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount), 0, 4);
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(content));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<int?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(content.Value));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<uint> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(content));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<uint?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(content.Value));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<long> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(content));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<long?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(content.Value));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<ulong> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(content));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<ulong?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(content.Value));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<DateTime> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(Utils.TimeToLong(content)));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<DateTime?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(Utils.TimeToLong(content.Value)));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<DateTimeOffset> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(Utils.TimeToLong(Utils.DateTimeOffSetToDateTime(content))));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<DateTimeOffset?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(Utils.TimeToLong(Utils.DateTimeOffSetToDateTime(content.Value))));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<SqlDateTime> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(Utils.TimeToLong(content.Value)));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<SqlDateTime?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(Utils.TimeToLong(content.Value.Value)));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<decimal> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                int[] bits = decimal.GetBits(content);
                byte[] bytes = new byte[bits.Length * 4];
                for (int i = 0; i < bits.Length; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bytes[i * 4 + j] = (byte)(bits[i] >> (j * 8));
                    }
                }
                stream.Write(BitConverter.GetBytes(bytes.Length));
                stream.Write(bytes);
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<decimal?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    int[] bits = decimal.GetBits(content.Value);
                    byte[] bytes = new byte[bits.Length * 4];
                    for (int i = 0; i < bits.Length; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            bytes[i * 4 + j] = (byte)(bits[i] >> (j * 8));
                        }
                    }
                    stream.Write(BitConverter.GetBytes(bytes.Length));
                    stream.Write(bytes);
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<double> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                byte[] bytes = BitConverter.GetBytes(content);
                stream.Write(BitConverter.GetBytes(bytes.Length));
                stream.Write(bytes);
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<double?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    byte[] bytes = BitConverter.GetBytes(content.Value);
                    stream.Write(BitConverter.GetBytes(bytes.Length));
                    stream.Write(bytes);
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<float> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                byte[] bytes = BitConverter.GetBytes(content);
                stream.Write(BitConverter.GetBytes(bytes.Length));
                stream.Write(bytes);
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<float?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    byte[] bytes = BitConverter.GetBytes(content.Value);
                    stream.Write(BitConverter.GetBytes(bytes.Length));
                    stream.Write(bytes);
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void SerializeEnum<Enum>(IEnumerable<Enum> contents, Stream stream) where Enum : struct
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(Convert.ToInt32(content)));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void SerializeEnum<Enum>(IEnumerable<Enum?> contents, Stream stream) where Enum : struct
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(Convert.ToInt32(content.Value)));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<Guid> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                stream.Write(content.ToByteArray());
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<Guid?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(content.Value.ToByteArray());
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<TimeSpan> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                stream.Write(BitConverter.GetBytes(Utils.TimeSpanToLong(content)));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<TimeSpan?> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
                return;
            }
            int arrayCount = contents.Count();
            //数组长度
            stream.Write(BitConverter.GetBytes(arrayCount));
            foreach (var content in contents)
            {
                if (content.HasValue)
                {
                    stream.WriteByte(trueByte);
                    stream.Write(BitConverter.GetBytes(Utils.TimeSpanToLong(content.Value)));
                }
                else
                {
                    stream.WriteByte(falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<KeyValuePair<string, string>> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
            }
            else
            {
                int arrayCount = contents.Count();
                //数组长度
                stream.Write(BitConverter.GetBytes(arrayCount));
                foreach (var content in contents)
                {
                    Serialize(content.Key, stream);
                    Serialize(content.Value, stream);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
            }
            else
            {
                int arrayCount = contents.Count();
                //数组长度
                stream.Write(BitConverter.GetBytes(arrayCount));
                foreach (var content in contents)
                {
                    Serialize(content.Key, stream);
                    Serialize(content.Value.ToString(), stream);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<KeyValuePair<bool, bool>> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
            }
            else
            {
                int arrayCount = contents.Count();
                //数组长度
                stream.Write(BitConverter.GetBytes(arrayCount));
                foreach (var content in contents)
                {
                    stream.WriteByte(content.Key ? trueByte : falseByte);
                    stream.WriteByte(content.Value ? trueByte : falseByte);
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<KeyValuePair<bool?, bool?>> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
            }
            else
            {
                int arrayCount = contents.Count();
                //数组长度
                stream.Write(BitConverter.GetBytes(arrayCount));
                foreach (var content in contents)
                {
                    if (content.Key.HasValue)
                    {
                        stream.WriteByte(trueByte);
                        stream.WriteByte(content.Key.Value ? trueByte : falseByte);
                    }
                    else
                    {
                        stream.WriteByte(falseByte);
                    }
                    if (content.Value.HasValue)
                    {
                        stream.WriteByte(trueByte);
                        stream.WriteByte(content.Value.Value ? trueByte : falseByte);
                    }
                    else
                    {
                        stream.WriteByte(falseByte);
                    }
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<KeyValuePair<short, short>> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
            }
            else
            {
                int arrayCount = contents.Count();
                //数组长度
                stream.Write(BitConverter.GetBytes(arrayCount));
                foreach (var content in contents)
                {
                    stream.Write(BitConverter.GetBytes(content.Key));
                    stream.Write(BitConverter.GetBytes(content.Value));
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(IEnumerable<KeyValuePair<short?, short?>> contents, Stream stream)
        {
            if (contents == null)
            {
                SerializerNull(stream);
            }
            else
            {
                int arrayCount = contents.Count();
                //数组长度
                stream.Write(BitConverter.GetBytes(arrayCount));
                foreach (var content in contents)
                {
                    if (content.Key.HasValue)
                    {
                        stream.WriteByte(trueByte);
                        stream.Write(BitConverter.GetBytes(content.Key.Value));
                    }
                    else
                    {
                        stream.WriteByte(falseByte);
                    }
                    if (content.Value.HasValue)
                    {
                        stream.WriteByte(trueByte);
                        stream.Write(BitConverter.GetBytes(content.Value.Value));
                    }
                    else
                    {
                        stream.WriteByte(falseByte);
                    }
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(Uri contents, Stream stream)
        {
            if (contents == null || contents.OriginalString == null)
            {
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
            }
            else
            {
                //数组长度
                stream.Write(BitConverter.GetBytes(contents.OriginalString.Length));
                stream.Write(Encoding.UTF8.GetBytes(contents.OriginalString));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(Version contents, Stream stream)
        {
            if (contents == null)
            {
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
            }
            else
            {
                var content = contents.ToString();
                stream.Write(BitConverter.GetBytes(content.Length));
                stream.Write(Encoding.UTF8.GetBytes(content));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(StringBuilder contents, Stream stream)
        {
            if (contents == null)
            {
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
            }
            else
            {
                var content = contents.ToString();
                stream.Write(BitConverter.GetBytes(content.Length));
                stream.Write(Encoding.UTF8.GetBytes(content));
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(BigInteger contents, Stream stream)
        {
            var content = contents.ToByteArray();
            stream.Write(BitConverter.GetBytes(content.Length));
            stream.Write(content);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(Complex contents, Stream stream)
        {
            Serialize(contents.Real, stream);
            Serialize(contents.Imaginary, stream);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="stream"></param>
        public static void Serialize(BitArray contents, Stream stream)
        {
            if (contents == null)
            {
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
                stream.WriteByte(nullByte);
            }
            else
            {
                stream.Write(BitConverter.GetBytes(contents.Count));
                for (var i = 0; i < contents.Count; i++)
                {
                    stream.WriteByte(contents[i] ? trueByte : falseByte);
                }
            }
        }
    }
}