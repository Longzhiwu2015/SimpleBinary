using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    class OtherSerialize
    {
        public static byte[] ToBytesProtoBuf<T>(T models)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, models);
                return stream.ToArray();
            }
        }
        public static T ToModelProtoBuf<T>(System.IO.MemoryStream stream)
        {
            return ProtoBuf.Serializer.Deserialize<T>(stream);
        }


        public static byte[] ToBytesMsg<T>(T models)
        {
            return MessagePack.MessagePackSerializer.Serialize(models);
        }
        public static T ToModelMsg<T>(System.IO.MemoryStream stream)
        {
            return MessagePack.MessagePackSerializer.Deserialize<T>(stream);
        }


        public static byte[] ToBytesWire<T>(T models)
        {
            return Wire.SerializerHelper.Serialize(models);
        }
        public static T ToModelWire<T>(System.IO.MemoryStream stream)
        {
            return (T)Wire.SerializerHelper.Deserialize(stream.ToArray());
        }
        public static byte[] ToBytesJson<T>(T models)
        {
            return Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(models));
        }
        public static T ToModelJson<T>(System.IO.MemoryStream stream)
        {
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(stream.ToArray()));
        }
    }
}
