using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    /// <summary>
    /// 排序对象
    /// </summary>
    public class SortedItem : IComparable<SortedItem>
    {
        string _name;
        /// <summary>
        /// 序列器名称
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public long Sort { get; set; }

        const int maxSize = 20;
        public override string ToString()
        {
            var tempName = Name ?? "";
            var leftLength = maxSize - tempName.Length;
            if (leftLength > 0)
            {
                for(var i=0;i< leftLength; i++)
                {
                    tempName += " ";
                }
            }
            return $"{tempName}，{Text}，{Sort}";
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(SortedItem other)
        {
            return this.Sort.CompareTo(other.Sort);
        }
    }
}
