using Test.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Test.Models
{
    public class ClassA
    {
        /// <summary>
        /// 五大美女
        /// </summary>
        public FiveBTGirl BTGirl { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Time { get; set; }
        public ClassB Some { get; set; }
        public Dictionary<string, string> Dict { get; set; }
        /// <summary>
        /// 当前执行步骤
        /// </summary>
        [IgnoreDataMember]//二进制序列化反序列化时不处理这个属性
        public virtual ExecuteStack CurrentStep { get; set; }
        /// <summary>
        /// 当前执行步骤
        /// </summary>
        public virtual ExecuteStack? NextStep { get; set; }
        /// <summary>
        /// Tuple
        /// </summary>
        public Tuple<DateTime, string, byte[], ClassA> TupleValues { get; set; }
        //, ClassA
    }
    public class ClassB
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Time { get; set; }
        public ClassC Some { get; set; }
        public Dictionary<string, string> Dict { get; set; }

    }
    public class ClassC
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Time { get; set; }
        public ClassD Some { get; set; }
        public Dictionary<string, string> Dict { get; set; }
    }
    public class ClassD
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Time { get; set; }
        public ClassE Some { get; set; }
        public Dictionary<string, string> Dict { get; set; }
    }
    public class ClassE
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Time { get; set; }
        public ClassF Some { get; set; }
        public Dictionary<string, string> Dict { get; set; }

    }
    [ProtoBuf.ProtoContract]
    [MessagePack.MessagePackObject]
    //[System.Runtime.Serialization.DataContract]
    public class ClassF
    {
        [ProtoBuf.ProtoMember(1)]
        [MessagePack.Key(1)]
        public string Name { get; set; }
        [ProtoBuf.ProtoMember(2)]
        //[MessagePack.Key(2)]
        public int Age { get; set; }
        [ProtoBuf.ProtoMember(3)]
        //[MessagePack.Key(3)]
        public DateTime Time { get; set; }
        [ProtoBuf.ProtoMember(4)]
        //[MessagePack.Key(4)]
        public string Address { get; set; }
    }
}