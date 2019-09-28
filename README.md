# SimpleBinary
二进制快速序列化反序列化类库
简体中文 | [English](./README.en.md)


## 介绍
[SimpleBinary] 是一个基于.net core 2.2开源的二进制序列化反序列化库
支持的数据类型
Primitives(int, string, etc...), Enum, Nullable<>, 
TimeSpan, DateTime, DateTimeOffset, Guid, Uri, Version, StringBuilder, 
BitArray, ArraySegment<>, BigInteger, Complext, Array[],
KeyValuePair<,>, Tuple<,...>, ValueTuple<,...>,Stream,MemoryStream,
 List<>, LinkedList<>, Queue<>, Stack<>, HashSet<>, ReadOnlyCollection<>, 
 IList<>, ICollection<>, IEnumerable<>, Dictionary<,>, IDictionary<,>, 
 SortedDictionary<,>, SortedList<,>, ILookup<,>, IGrouping<,>, ObservableCollection<>,
 ReadOnlyOnservableCollection<>, IReadOnlyList<>, IReadOnlyCollection<>, ISet<>,
 ConcurrentBag<>, ConcurrentQueue<>, ConcurrentStack<>, ReadOnlyDictionary<,>, 
 IReadOnlyDictionary<,>, ConcurrentDictionary<,>, Lazy<>, Task<>, 
 custom inherited ICollection<> or IDictionary<,> etc
 
## 使用过程中对Model类型可以做到0侵入(如果不使用忽略字段/属性的特性)
## 安全性方面，有前置根据类成员计算的唯一标识long值校验，可以防止误解码的情况出现
## 使用示例
    public class ClassF
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
    }
	//序列化
	var stream = new System.IO.MemoryStream();
	List<ClassF> list = new List<ClassF>();
	var model = new ClassF();
	model.Address = "测试地址";
	model.Age = 20;
	model.Name = "张三";
	model.Time = DateTime.Now;
	list.Add(model);
	SimpleBinary.FastSerialize<List<ClassF>>.Serialize(list, stream);
	//反序列化
	var newList = SimpleBinary.FastSerialize<List<ClassF>>.Deserialize(stream);
## 对于不想序列化的字段/属性，可以使用System.Runtime.Serialization.IgnoreDataMemberAttribute特性进行标记

## 性能对比测试

![性能测试对比](https://github.com/Longzhiwu2015/SimpleBinary/blob/master/test.png)

## License

[MIT](https://github.com/mgbq/nx-admin/blob/master/LICENSE)
Copyright (c) 2018-present nxmin
