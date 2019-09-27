using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBinary.Serializers
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public partial class SerializerHelper
    {
        static byte[] readBytes(Stream stream, int length)
        {
            var bytes = new byte[length];
            int readLength;
            if ((readLength = stream.Read(bytes, 0, length)) > 0 && readLength == length)
            {
                return bytes;
            }
            throw new Exception("解释协议时出错，读取预定长度数据失败");
        }
        /// <summary>
        /// 读取一个字节，并判断是否到流结尾
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte readByte(Stream stream)
        {
            var tempInt = stream.ReadByte();
            if (tempInt == -1) throw new Exception("解释协议时出错，意料之外的流长度");
            return (byte)tempInt;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out byte[] result)
        {
            int templength = BitConverter.ToInt32(readBytes(stream, 4), 0);
            if (templength == -1)
            {
                result = null;
            }
            else if (templength == 0)
            {
                result = new byte[0];
            }
            else result = readBytes(stream, templength);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out Stream result)
        {
            int templength = (int)((readByte(stream) & 0xFF)
                    | ((readByte(stream) & 0xFF) << 8)
                    | ((readByte(stream) & 0xFF) << 16)
                    | ((readByte(stream) & 0xFF) << 24));
            if (templength == -1)
            {
                result = null;
                return;
            }
            result = new MemoryStream();
            int readLength = 0;
            var bytes = new byte[bufferSize];
            while (templength > 0 && (readLength = stream.ReadAsync(bytes, 0, templength >= bufferSize ? bufferSize : templength).ConfigureAwait(false).GetAwaiter().GetResult()) > 0)
            {
                result.Write(bytes, 0, readLength);
                templength -= readLength;
            }
            result.Seek(0, SeekOrigin.Begin);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out MemoryStream result)
        {
            int templength = (int)((readByte(stream) & 0xFF)
                    | ((readByte(stream) & 0xFF) << 8)
                    | ((readByte(stream) & 0xFF) << 16)
                    | ((readByte(stream) & 0xFF) << 24));
            if (templength == -1)
            {
                result = null;
                return;
            }
            result = new MemoryStream();
            int readLength = 0;
            var bytes = new byte[bufferSize];
            while (templength > 0 && (readLength = stream.ReadAsync(bytes, 0, templength >= bufferSize ? bufferSize : templength).ConfigureAwait(false).GetAwaiter().GetResult()) > 0)
            {
                result.Write(bytes, 0, readLength);
                templength -= readLength;
            }
            result.Seek(0, SeekOrigin.Begin);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out string result)
        {
            int templength = BitConverter.ToInt32(readBytes(stream, 4));
            if (templength == -1)
            {
                result = null;
                return;
            }
            if (templength == 0)
            {
                result = string.Empty;
                return;
            }
            result = Encoding.UTF8.GetString(readBytes(stream, templength));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out bool result)
        {
            var tempInt = stream.ReadByte();
            if (tempInt == -1) throw new Exception("解释协议时出错，意料之外的流长度");
            result = tempInt == 1;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out bool? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            var tempInt = stream.ReadByte();
            if (tempInt == -1) throw new Exception("解释协议时出错，意料之外的流长度");
            result = tempInt == 1;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out byte result)
        {
            var tempInt = stream.ReadByte();
            if (tempInt == -1) throw new Exception("解释协议时出错，意料之外的流长度");
            result = (byte)tempInt;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out byte? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            var tempInt = stream.ReadByte();
            if (tempInt == -1) throw new Exception("解释协议时出错，意料之外的流长度");
            result = (byte)tempInt;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out sbyte result)
        {
            var tempInt = stream.ReadByte();
            if (tempInt == -1) throw new Exception("解释协议时出错，意料之外的流长度");
            result = (sbyte)tempInt;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out sbyte? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            var tempInt = stream.ReadByte();
            if (tempInt == -1) throw new Exception("解释协议时出错，意料之外的流长度");
            result = (sbyte)tempInt;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out char result)
        {
            result = BitConverter.ToChar(readBytes(stream, 2));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out char? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            result = BitConverter.ToChar(readBytes(stream, 2));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out short result)
        {
            result = BitConverter.ToInt16(readBytes(stream, 2));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out short? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            result = BitConverter.ToInt16(readBytes(stream, 2));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ushort result)
        {
            result = BitConverter.ToUInt16(readBytes(stream, 2));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ushort? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            result = BitConverter.ToUInt16(readBytes(stream, 2));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out int result)
        {
            /*
            result = (int)((readByte(stream) & 0xFF)
                    | ((readByte(stream) & 0xFF) << 8)
                    | ((readByte(stream) & 0xFF) << 16)
                    | ((readByte(stream) & 0xFF) << 24));
                    //*/
            result = BitConverter.ToInt32(readBytes(stream, 4));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out int? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            /*
            result = (int)((readByte(stream) & 0xFF)
                    | ((readByte(stream) & 0xFF) << 8)
                    | ((readByte(stream) & 0xFF) << 16)
                    | ((readByte(stream) & 0xFF) << 24));
            //*/
            result = BitConverter.ToInt32(readBytes(stream, 4));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out uint result)
        {
            result = BitConverter.ToUInt32(readBytes(stream, 4));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out uint? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            result = BitConverter.ToUInt32(readBytes(stream, 4));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out long result)
        {
            result = BitConverter.ToInt64(readBytes(stream, 8));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out long? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            result = BitConverter.ToInt64(readBytes(stream, 8));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ulong result)
        {
            result = BitConverter.ToUInt64(readBytes(stream, 8));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ulong? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            result = BitConverter.ToUInt64(readBytes(stream, 8));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out DateTime result)
        {
            long tempLong = BitConverter.ToInt64(readBytes(stream, 8));
            result = Utils.LongToTime(tempLong);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out DateTime? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            long tempLong = BitConverter.ToInt64(readBytes(stream, 8));
            result = Utils.LongToTime(tempLong);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out DateTimeOffset result)
        {
            long tempLong = BitConverter.ToInt64(readBytes(stream, 8));
            result = Utils.DateTimeToDateTimeOffSet(Utils.LongToTime(tempLong));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out DateTimeOffset? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            long tempLong = BitConverter.ToInt64(readBytes(stream, 8));
            result = Utils.DateTimeToDateTimeOffSet(Utils.LongToTime(tempLong));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out SqlDateTime result)
        {
            long tempLong = BitConverter.ToInt64(readBytes(stream, 8));
            result = new SqlDateTime(Utils.LongToTime(tempLong));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out SqlDateTime? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            long tempLong = BitConverter.ToInt64(readBytes(stream, 8));
            result = new SqlDateTime(Utils.LongToTime(tempLong));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out decimal result)
        {
            byte[] resultArray;
            Deserialize(stream, out resultArray);
            int[] bits = new int[resultArray.Length / 4];
            for (int i = 0; i < bits.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    bits[i] |= resultArray[i * 4 + j] << j * 8;
                }
            }
            result = new decimal(bits);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out decimal? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            decimal tempResult;
            Deserialize(stream, out tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out double result)
        {
            byte[] resultArray;
            Deserialize(stream, out resultArray);
            result = BitConverter.ToDouble(resultArray, 0);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out double? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            byte[] resultArray;
            Deserialize(stream, out resultArray);
            result = BitConverter.ToDouble(resultArray, 0);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out float result)
        {
            byte[] resultArray;
            Deserialize(stream, out resultArray);
            result = BitConverter.ToSingle(resultArray, 0);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out float? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            byte[] resultArray;
            Deserialize(stream, out resultArray);
            result = BitConverter.ToSingle(resultArray, 0);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out Enum result) where Enum : struct
        {
            int value;
            Deserialize(stream, out value);
            Utils.TryParse<Enum>(value, out result);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out Enum? result) where Enum : struct
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            int value;
            Deserialize(stream, out value);
            Utils.TryParse<Enum>(value, out result);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out Guid result)
        {
            result = new Guid(readBytes(stream, 16));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out Guid? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            result = new Guid(readBytes(stream, 16));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out TimeSpan result)
        {
            long value;
            Deserialize(stream, out value);
            result = Utils.LongToTimeSpan(value);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out TimeSpan? result)
        {
            byte hasValue = readByte(stream);
            if (hasValue == 0)
            {
                result = null;
                return;
            }
            long value;
            Deserialize(stream, out value);
            result = Utils.LongToTimeSpan(value);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<byte> result)
        {
            Deserialize(stream, out byte[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<byte> result)
        {
            Deserialize(stream, out byte[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<byte>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<byte> result)
        {
            Deserialize(stream, out byte[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<byte>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<byte> result)
        {
            Deserialize(stream, out byte[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<byte>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<byte> result)
        {
            Deserialize(stream, out byte[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<byte>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out string[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new string[0];
                return;
            }
            result = new string[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                var tempLength = BitConverter.ToInt32(readBytes(stream, 4));
                if (tempLength == -1)
                {
                    continue;
                }
                if (tempLength == 0)
                {
                    result[i] = string.Empty;
                    continue;
                }
                result[i] = Encoding.UTF8.GetString(readBytes(stream, tempLength));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<string> result)
        {
            Deserialize(stream, out string[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<string> result)
        {
            Deserialize(stream, out string[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<string>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<string> result)
        {
            Deserialize(stream, out string[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<string>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<string> result)
        {
            Deserialize(stream, out string[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<string>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<string> result)
        {
            Deserialize(stream, out string[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<string>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out bool[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new bool[0];
                return;
            }
            result = new bool[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = readByte(stream) == 1;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<bool> result)
        {
            Deserialize(stream, out bool[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<bool> result)
        {
            Deserialize(stream, out bool[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<bool>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<bool> result)
        {
            Deserialize(stream, out bool[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<bool>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<bool> result)
        {
            Deserialize(stream, out bool[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<bool>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<bool> result)
        {
            Deserialize(stream, out bool[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<bool>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out sbyte[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new sbyte[0];
                return;
            }
            result = new sbyte[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = (sbyte)readByte(stream);
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<sbyte> result)
        {
            Deserialize(stream, out sbyte[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<sbyte> result)
        {
            Deserialize(stream, out sbyte[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<sbyte>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<sbyte> result)
        {
            Deserialize(stream, out sbyte[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<sbyte>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<sbyte> result)
        {
            Deserialize(stream, out sbyte[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<sbyte>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<sbyte> result)
        {
            Deserialize(stream, out sbyte[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<sbyte>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out byte?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new byte?[0];
                return;
            }
            result = new byte?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = readByte(stream);
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<byte?> result)
        {
            Deserialize(stream, out byte?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<byte?> result)
        {
            Deserialize(stream, out byte?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<byte?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<byte?> result)
        {
            Deserialize(stream, out byte?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<byte?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<byte?> result)
        {
            Deserialize(stream, out byte?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<byte?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<byte?> result)
        {
            Deserialize(stream, out byte?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<byte?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out sbyte?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new sbyte?[0];
                return;
            }
            result = new sbyte?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = (sbyte)readByte(stream);
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<sbyte?> result)
        {
            Deserialize(stream, out sbyte?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<sbyte?> result)
        {
            Deserialize(stream, out sbyte?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<sbyte?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<sbyte?> result)
        {
            Deserialize(stream, out sbyte?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<sbyte?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<sbyte?> result)
        {
            Deserialize(stream, out sbyte?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<sbyte?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<sbyte?> result)
        {
            Deserialize(stream, out sbyte?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<sbyte?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out char[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new char[0];
                return;
            }
            result = new char[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = BitConverter.ToChar(readBytes(stream, 2));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<char> result)
        {
            Deserialize(stream, out char[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<char> result)
        {
            Deserialize(stream, out char[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<char>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<char> result)
        {
            Deserialize(stream, out char[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<char>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<char> result)
        {
            Deserialize(stream, out char[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<char>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<char> result)
        {
            Deserialize(stream, out char[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<char>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out char?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new char?[0];
                return;
            }
            result = new char?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = BitConverter.ToChar(readBytes(stream, 2));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<char?> result)
        {
            Deserialize(stream, out char?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<char?> result)
        {
            Deserialize(stream, out char?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<char?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<char?> result)
        {
            Deserialize(stream, out char?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<char?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<char?> result)
        {
            Deserialize(stream, out char?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<char?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<char?> result)
        {
            Deserialize(stream, out char?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<char?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out short[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new short[0];
                return;
            }
            result = new short[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = BitConverter.ToInt16(readBytes(stream, 2));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<short> result)
        {
            Deserialize(stream, out short[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<short> result)
        {
            Deserialize(stream, out short[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<short>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<short> result)
        {
            Deserialize(stream, out short[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<short>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<short> result)
        {
            Deserialize(stream, out short[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<short>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<short> result)
        {
            Deserialize(stream, out short[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<short>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out short?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new short?[0];
                return;
            }
            result = new short?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = BitConverter.ToInt16(readBytes(stream, 2));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<short?> result)
        {
            Deserialize(stream, out short?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<short?> result)
        {
            Deserialize(stream, out short?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<short?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<short?> result)
        {
            Deserialize(stream, out short?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<short?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<short?> result)
        {
            Deserialize(stream, out short?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<short?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<short?> result)
        {
            Deserialize(stream, out short?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<short?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ushort[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new ushort[0];
                return;
            }
            result = new ushort[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = BitConverter.ToUInt16(readBytes(stream, 2));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<ushort> result)
        {
            Deserialize(stream, out ushort[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<ushort> result)
        {
            Deserialize(stream, out ushort[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<ushort>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<ushort> result)
        {
            Deserialize(stream, out ushort[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<ushort>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<ushort> result)
        {
            Deserialize(stream, out ushort[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<ushort>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<ushort> result)
        {
            Deserialize(stream, out ushort[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<ushort>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ushort?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new ushort?[0];
                return;
            }
            result = new ushort?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = BitConverter.ToUInt16(readBytes(stream, 2));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<ushort?> result)
        {
            Deserialize(stream, out ushort?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<ushort?> result)
        {
            Deserialize(stream, out ushort?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<ushort?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<ushort?> result)
        {
            Deserialize(stream, out ushort?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<ushort?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<ushort?> result)
        {
            Deserialize(stream, out ushort?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<ushort?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<ushort?> result)
        {
            Deserialize(stream, out ushort?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<ushort?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out int[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new int[0];
                return;
            }
            result = new int[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = BitConverter.ToInt32(readBytes(stream, 4));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<int> result)
        {
            Deserialize(stream, out int[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<int> result)
        {
            Deserialize(stream, out int[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<int>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<int> result)
        {
            Deserialize(stream, out int[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<int>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<int> result)
        {
            Deserialize(stream, out int[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<int>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<int> result)
        {
            Deserialize(stream, out int[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<int>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out int?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new int?[0];
                return;
            }
            result = new int?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = BitConverter.ToInt32(readBytes(stream, 4));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<int?> result)
        {
            Deserialize(stream, out int?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<int?> result)
        {
            Deserialize(stream, out int?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<int?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<int?> result)
        {
            Deserialize(stream, out int?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<int?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<int?> result)
        {
            Deserialize(stream, out int?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<int?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<int?> result)
        {
            Deserialize(stream, out int?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<int?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out uint[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new uint[0];
                return;
            }
            result = new uint[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = BitConverter.ToUInt32(readBytes(stream, 4));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<uint> result)
        {
            Deserialize(stream, out uint[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<uint> result)
        {
            Deserialize(stream, out uint[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<uint>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<uint> result)
        {
            Deserialize(stream, out uint[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<uint>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<uint> result)
        {
            Deserialize(stream, out uint[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<uint>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<uint> result)
        {
            Deserialize(stream, out uint[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<uint>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out uint?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new uint?[0];
                return;
            }
            result = new uint?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = BitConverter.ToUInt32(readBytes(stream, 4));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<uint?> result)
        {
            Deserialize(stream, out uint?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<uint?> result)
        {
            Deserialize(stream, out uint?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<uint?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<uint?> result)
        {
            Deserialize(stream, out uint?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<uint?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<uint?> result)
        {
            Deserialize(stream, out uint?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<uint?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<uint?> result)
        {
            Deserialize(stream, out uint?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<uint?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out long[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new long[0];
                return;
            }
            result = new long[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = BitConverter.ToInt64(readBytes(stream, 8));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<long> result)
        {
            Deserialize(stream, out long[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<long> result)
        {
            Deserialize(stream, out long[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<long>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<long> result)
        {
            Deserialize(stream, out long[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<long>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<long> result)
        {
            Deserialize(stream, out long[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<long>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<long> result)
        {
            Deserialize(stream, out long[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<long>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out long?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new long?[0];
                return;
            }
            result = new long?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = BitConverter.ToInt64(readBytes(stream, 8));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<long?> result)
        {
            Deserialize(stream, out long?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<long?> result)
        {
            Deserialize(stream, out long?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<long?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<long?> result)
        {
            Deserialize(stream, out long?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<long?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<long?> result)
        {
            Deserialize(stream, out long?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<long?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<long?> result)
        {
            Deserialize(stream, out long?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<long?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ulong[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new ulong[0];
                return;
            }
            result = new ulong[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = BitConverter.ToUInt64(readBytes(stream, 8));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<ulong> result)
        {
            Deserialize(stream, out ulong[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<ulong> result)
        {
            Deserialize(stream, out ulong[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<ulong>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<ulong> result)
        {
            Deserialize(stream, out ulong[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<ulong>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<ulong> result)
        {
            Deserialize(stream, out ulong[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<ulong>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<ulong> result)
        {
            Deserialize(stream, out ulong[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<ulong>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ulong?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new ulong?[0];
                return;
            }
            result = new ulong?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = BitConverter.ToUInt64(readBytes(stream, 8));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<ulong?> result)
        {
            Deserialize(stream, out ulong?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<ulong?> result)
        {
            Deserialize(stream, out ulong?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<ulong?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<ulong?> result)
        {
            Deserialize(stream, out ulong?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<ulong?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<ulong?> result)
        {
            Deserialize(stream, out ulong?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<ulong?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<ulong?> result)
        {
            Deserialize(stream, out ulong?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<ulong?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out DateTime[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new DateTime[0];
                return;
            }
            result = new DateTime[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = Utils.LongToTime(BitConverter.ToInt64(readBytes(stream, 8)));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<DateTime> result)
        {
            Deserialize(stream, out DateTime[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<DateTime> result)
        {
            Deserialize(stream, out DateTime[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<DateTime>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<DateTime> result)
        {
            Deserialize(stream, out DateTime[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<DateTime>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<DateTime> result)
        {
            Deserialize(stream, out DateTime[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<DateTime>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<DateTime> result)
        {
            Deserialize(stream, out DateTime[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<DateTime>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out DateTime?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new DateTime?[0];
                return;
            }
            result = new DateTime?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = Utils.LongToTime(BitConverter.ToInt64(readBytes(stream, 8)));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<DateTime?> result)
        {
            Deserialize(stream, out DateTime?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<DateTime?> result)
        {
            Deserialize(stream, out DateTime?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<DateTime?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<DateTime?> result)
        {
            Deserialize(stream, out DateTime?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<DateTime?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<DateTime?> result)
        {
            Deserialize(stream, out DateTime?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<DateTime?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<DateTime?> result)
        {
            Deserialize(stream, out DateTime?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<DateTime?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out DateTimeOffset[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new DateTimeOffset[0];
                return;
            }
            result = new DateTimeOffset[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = Utils.DateTimeToDateTimeOffSet(Utils.LongToTime(BitConverter.ToInt64(readBytes(stream, 8))));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<DateTimeOffset> result)
        {
            Deserialize(stream, out DateTimeOffset[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<DateTimeOffset> result)
        {
            Deserialize(stream, out DateTimeOffset[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<DateTimeOffset>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<DateTimeOffset> result)
        {
            Deserialize(stream, out DateTimeOffset[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<DateTimeOffset>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<DateTimeOffset> result)
        {
            Deserialize(stream, out DateTimeOffset[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<DateTimeOffset>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<DateTimeOffset> result)
        {
            Deserialize(stream, out DateTimeOffset[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<DateTimeOffset>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out DateTimeOffset?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new DateTimeOffset?[0];
                return;
            }
            result = new DateTimeOffset?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = Utils.DateTimeToDateTimeOffSet(Utils.LongToTime(BitConverter.ToInt64(readBytes(stream, 8))));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<DateTimeOffset?> result)
        {
            Deserialize(stream, out DateTimeOffset?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<DateTimeOffset?> result)
        {
            Deserialize(stream, out DateTimeOffset?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<DateTimeOffset?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<DateTimeOffset?> result)
        {
            Deserialize(stream, out DateTimeOffset?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<DateTimeOffset?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<DateTimeOffset?> result)
        {
            Deserialize(stream, out DateTimeOffset?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<DateTimeOffset?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<DateTimeOffset?> result)
        {
            Deserialize(stream, out DateTimeOffset?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<DateTimeOffset?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out SqlDateTime[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new SqlDateTime[0];
                return;
            }
            result = new SqlDateTime[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = new SqlDateTime(Utils.LongToTime(BitConverter.ToInt64(readBytes(stream, 8))));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<SqlDateTime> result)
        {
            Deserialize(stream, out SqlDateTime[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<SqlDateTime> result)
        {
            Deserialize(stream, out SqlDateTime[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<SqlDateTime>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<SqlDateTime> result)
        {
            Deserialize(stream, out SqlDateTime[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<SqlDateTime>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<SqlDateTime> result)
        {
            Deserialize(stream, out SqlDateTime[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<SqlDateTime>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<SqlDateTime> result)
        {
            Deserialize(stream, out SqlDateTime[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<SqlDateTime>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out SqlDateTime?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new SqlDateTime?[0];
                return;
            }
            result = new SqlDateTime?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = new SqlDateTime(Utils.LongToTime(BitConverter.ToInt64(readBytes(stream, 8))));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<SqlDateTime?> result)
        {
            Deserialize(stream, out SqlDateTime?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<SqlDateTime?> result)
        {
            Deserialize(stream, out SqlDateTime?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<SqlDateTime?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<SqlDateTime?> result)
        {
            Deserialize(stream, out SqlDateTime?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<SqlDateTime?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<SqlDateTime?> result)
        {
            Deserialize(stream, out SqlDateTime?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<SqlDateTime?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<SqlDateTime?> result)
        {
            Deserialize(stream, out SqlDateTime?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<SqlDateTime?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out decimal[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new decimal[0];
                return;
            }
            result = new decimal[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                byte[] resultArray;
                Deserialize(stream, out resultArray);
                int[] bits = new int[resultArray.Length / 4];
                for (int k = 0; k < bits.Length; k++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bits[k] |= resultArray[k * 4 + j] << j * 8;
                    }
                }
                result[i] = new decimal(bits);
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<decimal> result)
        {
            Deserialize(stream, out decimal[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<decimal> result)
        {
            Deserialize(stream, out decimal[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<decimal>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<decimal> result)
        {
            Deserialize(stream, out decimal[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<decimal>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<decimal> result)
        {
            Deserialize(stream, out decimal[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<decimal>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<decimal> result)
        {
            Deserialize(stream, out decimal[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<decimal>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out decimal?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new decimal?[0];
                return;
            }
            result = new decimal?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    byte[] resultArray;
                    Deserialize(stream, out resultArray);
                    int[] bits = new int[resultArray.Length / 4];
                    for (int k = 0; k < bits.Length; k++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            bits[k] |= resultArray[k * 4 + j] << j * 8;
                        }
                    }
                    result[i] = new decimal(bits);
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<decimal?> result)
        {
            Deserialize(stream, out decimal?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<decimal?> result)
        {
            Deserialize(stream, out decimal?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<decimal?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<decimal?> result)
        {
            Deserialize(stream, out decimal?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<decimal?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<decimal?> result)
        {
            Deserialize(stream, out decimal?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<decimal?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<decimal?> result)
        {
            Deserialize(stream, out decimal?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<decimal?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out double[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new double[0];
                return;
            }
            result = new double[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                byte[] resultArray;
                Deserialize(stream, out resultArray);
                result[i] = BitConverter.ToDouble(resultArray, 0);
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<double> result)
        {
            Deserialize(stream, out double[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<double> result)
        {
            Deserialize(stream, out double[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<double>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<double> result)
        {
            Deserialize(stream, out double[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<double>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<double> result)
        {
            Deserialize(stream, out double[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<double>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<double> result)
        {
            Deserialize(stream, out double[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<double>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out double?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new double?[0];
                return;
            }
            result = new double?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    byte[] resultArray;
                    Deserialize(stream, out resultArray);
                    result[i] = BitConverter.ToDouble(resultArray, 0);
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<double?> result)
        {
            Deserialize(stream, out double?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<double?> result)
        {
            Deserialize(stream, out double?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<double?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<double?> result)
        {
            Deserialize(stream, out double?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<double?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<double?> result)
        {
            Deserialize(stream, out double?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<double?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<double?> result)
        {
            Deserialize(stream, out double?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<double?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out float[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new float[0];
                return;
            }
            result = new float[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                byte[] resultArray;
                Deserialize(stream, out resultArray);
                result[i] = BitConverter.ToSingle(resultArray, 0);
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<float> result)
        {
            Deserialize(stream, out float[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<float> result)
        {
            Deserialize(stream, out float[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<float>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<float> result)
        {
            Deserialize(stream, out float[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<float>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<float> result)
        {
            Deserialize(stream, out float[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<float>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<float> result)
        {
            Deserialize(stream, out float[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<float>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out float?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new float?[0];
                return;
            }
            result = new float?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    byte[] resultArray;
                    Deserialize(stream, out resultArray);
                    result[i] = BitConverter.ToSingle(resultArray, 0);
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<float?> result)
        {
            Deserialize(stream, out float?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<float?> result)
        {
            Deserialize(stream, out float?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<float?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<float?> result)
        {
            Deserialize(stream, out float?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<float?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<float?> result)
        {
            Deserialize(stream, out float?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<float?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<float?> result)
        {
            Deserialize(stream, out float?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<float?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out Enum[] result) where Enum : struct
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new Enum[0];
                return;
            }
            result = new Enum[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                int value;
                Deserialize(stream, out value);
                Enum tempResult;
                Utils.TryParse<Enum>(value, out tempResult);
                result[i] = tempResult;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out IEnumerable<Enum> result) where Enum : struct
        {
            DeserializeEnum(stream, out Enum[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out List<Enum> result) where Enum : struct
        {
            DeserializeEnum(stream, out Enum[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<Enum>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out IList<Enum> result) where Enum : struct
        {
            DeserializeEnum(stream, out Enum[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<Enum>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out ArraySegment<Enum> result) where Enum : struct
        {
            DeserializeEnum(stream, out Enum[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<Enum>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out ReadOnlyMemory<Enum> result) where Enum : struct
        {
            DeserializeEnum(stream, out Enum[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<Enum>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out Enum?[] result) where Enum : struct
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new Enum?[0];
                return;
            }
            result = new Enum?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    int value;
                    Deserialize(stream, out value);
                    Enum tempResult;
                    Utils.TryParse<Enum>(value, out tempResult);
                    result[i] = tempResult;
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out IEnumerable<Enum?> result) where Enum : struct
        {
            DeserializeEnum(stream, out Enum?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out List<Enum?> result) where Enum : struct
        {
            DeserializeEnum(stream, out Enum?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<Enum?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out IList<Enum?> result) where Enum : struct
        {
            DeserializeEnum(stream, out Enum?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<Enum?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out ArraySegment<Enum?> result) where Enum : struct
        {
            DeserializeEnum(stream, out Enum?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<Enum?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Enum"></typeparam>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void DeserializeEnum<Enum>(Stream stream, out ReadOnlyMemory<Enum?> result) where Enum : struct
        {
            DeserializeEnum(stream, out Enum?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<Enum?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out Guid[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new Guid[0];
                return;
            }
            result = new Guid[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = new Guid(readBytes(stream, 16));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<Guid> result)
        {
            Deserialize(stream, out Guid[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<Guid> result)
        {
            Deserialize(stream, out Guid[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<Guid>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<Guid> result)
        {
            Deserialize(stream, out Guid[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<Guid>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<Guid> result)
        {
            Deserialize(stream, out Guid[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<Guid>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<Guid> result)
        {
            Deserialize(stream, out Guid[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<Guid>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out Guid?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new Guid?[0];
                return;
            }
            result = new Guid?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = new Guid(readBytes(stream, 16));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<Guid?> result)
        {
            Deserialize(stream, out Guid?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<Guid?> result)
        {
            Deserialize(stream, out Guid?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<Guid?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<Guid?> result)
        {
            Deserialize(stream, out Guid?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<Guid?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<Guid?> result)
        {
            Deserialize(stream, out Guid?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<Guid?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<Guid?> result)
        {
            Deserialize(stream, out Guid?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<Guid?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out TimeSpan[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new TimeSpan[0];
                return;
            }
            result = new TimeSpan[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = Utils.LongToTimeSpan(BitConverter.ToInt64(readBytes(stream, 8)));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<TimeSpan> result)
        {
            Deserialize(stream, out TimeSpan[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<TimeSpan> result)
        {
            Deserialize(stream, out TimeSpan[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<TimeSpan>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<TimeSpan> result)
        {
            Deserialize(stream, out TimeSpan[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<TimeSpan>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<TimeSpan> result)
        {
            Deserialize(stream, out TimeSpan[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<TimeSpan>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<TimeSpan> result)
        {
            Deserialize(stream, out TimeSpan[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<TimeSpan>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out TimeSpan?[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new TimeSpan?[0];
                return;
            }
            result = new TimeSpan?[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                if (readByte(stream) == 1)
                {
                    result[i] = Utils.LongToTimeSpan(BitConverter.ToInt64(readBytes(stream, 8)));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<TimeSpan?> result)
        {
            Deserialize(stream, out TimeSpan?[] tempResult);
            result = tempResult;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out List<TimeSpan?> result)
        {
            Deserialize(stream, out TimeSpan?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<TimeSpan?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IList<TimeSpan?> result)
        {
            Deserialize(stream, out TimeSpan?[] tempResult);
            if (tempResult == null) result = null;
            else result = new List<TimeSpan?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ArraySegment<TimeSpan?> result)
        {
            Deserialize(stream, out TimeSpan?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ArraySegment<TimeSpan?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out ReadOnlyMemory<TimeSpan?> result)
        {
            Deserialize(stream, out TimeSpan?[] tempResult);
            if (tempResult == null) result = null;
            else result = new ReadOnlyMemory<TimeSpan?>(tempResult);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out KeyValuePair<string, string>[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new KeyValuePair<string, string>[0];
                return;
            }
            var resultItems = new KeyValuePair<string, string>[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                string item1;
                Deserialize(stream, out item1);
                string item2;
                Deserialize(stream, out item2);
                resultItems[i] = new KeyValuePair<string, string>(item1, item2);
            }
            result = resultItems;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>[0];
                return;
            }
            var resultItems = new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                string item1;
                Deserialize(stream, out item1);
                string item2;
                Deserialize(stream, out item2);
                resultItems[i] = new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>(item1, item2);
            }
            result = resultItems;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out IEnumerable<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> result)
        {
            Deserialize(stream, out KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>[] _result);
            result = _result;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out KeyValuePair<bool, bool>[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new KeyValuePair<bool, bool>[0];
                return;
            }
            var resultItems = new KeyValuePair<bool, bool>[arrayLength];
            //result = new KeyValuePair<string, string>[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                resultItems[i] = new KeyValuePair<bool, bool>(readByte(stream) == 1, readByte(stream) == 1);
            }
            result = resultItems;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out KeyValuePair<bool?, bool?>[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new KeyValuePair<bool?, bool?>[0];
                return;
            }
            var resultItems = new KeyValuePair<bool?, bool?>[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                bool? item1;
                Deserialize(stream, out item1);
                bool? item2;
                Deserialize(stream, out item2);
                resultItems[i] = new KeyValuePair<bool?, bool?>(item1, item2);
            }
            result = resultItems;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out KeyValuePair<short, short>[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new KeyValuePair<short, short>[0];
                return;
            }
            var resultItems = new KeyValuePair<short, short>[arrayLength];
            //result = new KeyValuePair<string, string>[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                short item1;
                Deserialize(stream, out item1);
                short item2;
                Deserialize(stream, out item2);
                resultItems[i] = new KeyValuePair<short, short>(item1, item2);
            }
            result = resultItems;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out KeyValuePair<short?, short?>[] result)
        {
            int arrayLength = BitConverter.ToInt32(readBytes(stream, 4));
            if (arrayLength == -1)
            {
                result = null;
                return;
            }
            if (arrayLength == 0)
            {
                result = new KeyValuePair<short?, short?>[0];
                return;
            }
            var resultItems = new KeyValuePair<short?, short?>[arrayLength];
            for (var i = 0; i < arrayLength; i++)
            {
                short? item1;
                Deserialize(stream, out item1);
                short? item2;
                Deserialize(stream, out item2);
                resultItems[i] = new KeyValuePair<short?, short?>(item1, item2);
            }
            result = resultItems;
        }












        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out Version result)
        {
            Deserialize(stream, out string _version);
            if (_version == null)
            {
                result = null;
                return;
            }
            result = new Version(_version);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out StringBuilder result)
        {
            Deserialize(stream, out string _version);
            if (_version == null)
            {
                result = null;
                return;
            }
            result = new StringBuilder(_version);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out BigInteger result)
        {
            Deserialize(stream, out byte[] _version);
            if (_version == null)
            {
                result = new BigInteger();
                return;
            }
            result = new BigInteger(_version);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out Complex result)
        {
            Deserialize(stream, out double _real);
            Deserialize(stream, out double _imaginary);
            result = new Complex(_real, _imaginary);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="result"></param>
        public static void Deserialize(Stream stream, out BitArray result)
        {
            Deserialize(stream, out int _length);
            if (_length == -1)
            {
                result = null;
                return;
            }
            result = new BitArray(_length);
            for (var i = 0; i < _length; i++)
            {
                var tempByte = stream.ReadByte();
                if (tempByte == -1) throw new Exception("意料之外的流长度");
                result[i] = tempByte == 1;
            }            
        }
        //Complext,Lazy<>
        /*
 Array[],
 Array[,], Array[,,], Array[,,,], 
 LinkedList<>, Queue<>, Stack<>, HashSet<>, ReadOnlyCollection<>, 
 ICollection<>, IEnumerable<>
 SortedDictionary<,>, ILookup<,>, IGrouping<,>, ObservableCollection<>,
 ReadOnlyOnservableCollection<>, IReadOnlyList<>, IReadOnlyCollection<>,
 ConcurrentBag<>, ConcurrentQueue<>, ConcurrentStack<>, ReadOnlyDictionary<,>, 
 IReadOnlyDictionary<,>, ConcurrentDictionary<,>, Lazy<>, Task<>, 
 custom inherited ICollection<> or IDictionary<,> with paramterless constructor, IList,
        */
    }
}