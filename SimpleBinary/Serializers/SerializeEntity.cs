using System;
using System.Text;

namespace SimpleBinary.Serializers
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public class SerializeEntity
    {
        readonly string codeText1;
        readonly string codeText2;
        const string codeText3 = @"
            return model;
        }
";
        /// <summary>
        /// 序列化帮助类
        /// </summary>
        /// <param name="type"></param>
        public SerializeEntity(Type type)
        {
            var classFullName = Utils.GetFullName(type);
            var isCanNull = type.IsClass || Utils.IsNullable(type);
            var tempGuid = Guid.NewGuid().ToString("N");
            string newModelString;
            if(type.IsClass && type.GetConstructor(Type.EmptyTypes) != null)
            {
                newModelString = $"var model = new {classFullName}();";
            }
            else
            {
                newModelString = $"var model = default({classFullName});";
            }
            if (isCanNull)
            {
                codeText1 = $@"
        /// <summary>
        /// 序列化{classFullName}
        /// </summary>
        /// <param name=""model""></param>
        /// <param name=""stream""></param>
        /// <returns></returns>
        void serialize_{tempGuid}({classFullName} model, Stream stream)
        {{
            if (model == null) {{ stream.WriteByte(SimpleBinary.Serializers.SerializerHelper.falseByte); return; }}
            else {{ stream.WriteByte(SimpleBinary.Serializers.SerializerHelper.trueByte); }}
";
                codeText2 = $@"
        }}
        /// <summary>
        /// 反序列化{classFullName}
        /// </summary>
        /// <param name=""stream""></param>
        /// <returns></returns>
        {classFullName} deSerialize_{tempGuid}(Stream stream)
        {{
            if (SimpleBinary.Serializers.SerializerHelper.readByte(stream) != 1) return null;
            {newModelString}
";
            }
            else
            {
                codeText1 = $@"
        /// <summary>
        /// 序列化{classFullName}
        /// </summary>
        /// <param name=""model""></param>
        /// <param name=""stream""></param>
        /// <returns></returns>
        void serialize_{tempGuid}({classFullName} model, Stream stream)
        {{
";
                codeText2 = $@"
        }}
        /// <summary>
        /// 反序列化{classFullName}
        /// </summary>
        /// <param name=""stream""></param>
        /// <returns></returns>
        {classFullName} deSerialize_{tempGuid}(Stream stream)
        {{
            {newModelString}
";
            }
            SerializeName = $"serialize_{tempGuid}";
            DerializeName = $"deSerialize_{tempGuid}";
            SerializeBuilder = new StringBuilder();
            DerializeBuilder = new StringBuilder();
        }
        /// <summary>
        /// 序列化方法名
        /// </summary>
        public string SerializeName { get; private set; }
        /// <summary>
        /// 反序列化方法名
        /// </summary>
        public string DerializeName { get; private set; }
        /// <summary>
        /// 序列化Builder
        /// </summary>
        public StringBuilder SerializeBuilder { get; private set; }
        /// <summary>
        /// 反序列化Builder
        /// </summary>
        public StringBuilder DerializeBuilder { get; private set; }
        /// <summary>
        /// 生成最后的Code
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Concat(codeText1, SerializeBuilder.ToString(), codeText2, DerializeBuilder.ToString(), codeText3);
        }
    }
}