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
        /// 每次序列化传输的长度
        /// </summary>
        public const int bufferSize = 8192;
        public const int nullStringLength = (int)-1;
        public const byte nullByte = (byte)255;
        public const byte falseByte = (byte)0;
        public const byte trueByte = (byte)1;
    }
}