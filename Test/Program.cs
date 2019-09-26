using SimpleBinary;
using SimpleBinary.Generates;
using Test.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Google.Protobuf;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            //首先初始化序列化编译器
            SerializerCodeHelper.Init();
            Console.WriteLine("开始序列化...");
            testSerialze();
            tupleTest();

            speedTest();


            Console.ReadLine();
        }
        static void speedTest()
        {
            Console.WriteLine("正在准备数据...");
            var dataList = new List<ClassF>();
            for (var i = 0; i < 5000; i++)
            {
                dataList.Add(new ClassF
                {
                    Age = 30,
                    //KeyValues = new System.Collections.Generic.Dictionary<string, string> { { "k", "123_"+i }, { "l", "789_"+i } },
                    Address = "上海路_" + i,
                    Name = "雷老虎_" + i,
                    Time = DateTime.Now
                });
            }
            Directory.CreateDirectory(Utils.MapPath("/111"));
            var SimpleBinaryPath = Utils.MapPath("/111/333.SimpleBinary");
            if (!File.Exists(SimpleBinaryPath)) System.IO.File.WriteAllBytes(SimpleBinaryPath, FastSerialize<List<ClassF>>.Serialize(dataList).ToArray());
            var MessagePackPath = Utils.MapPath("/111/333.MessagePack");
            if (!File.Exists(MessagePackPath)) System.IO.File.WriteAllBytes(MessagePackPath, OtherSerialize.ToBytesMsg(dataList));
            var WirePath = Utils.MapPath("/111/333.Wire");
            if (!File.Exists(WirePath)) System.IO.File.WriteAllBytes(WirePath, OtherSerialize.ToBytesWire(dataList));
            var protobufnetPath = Utils.MapPath("/111/333.protobuf-net");
            if (!File.Exists(protobufnetPath)) System.IO.File.WriteAllBytes(protobufnetPath, OtherSerialize.ToBytesProtoBuf(dataList));
            var NewtonsoftJsonPath = Utils.MapPath("/111/333.Newtonsoft-Json");
            if (!File.Exists(NewtonsoftJsonPath)) System.IO.File.WriteAllBytes(NewtonsoftJsonPath, OtherSerialize.ToBytesJson(dataList));
            var googleprotobufPath = Utils.MapPath("/111/333.googleprotobufPath");
            if (!File.Exists(googleprotobufPath))
            {
                using (var output = File.Create(googleprotobufPath))
                {
                    for (var i = 0; i < 5000; i++)
                    {
                        new ClassFPuf
                        {
                            Age = 30,
                            //KeyValues = new System.Collections.Generic.Dictionary<string, string> { { "k", "123_"+i }, { "l", "789_"+i } },
                            Address = "上海路_" + i,
                            Name = "雷老虎_" + i,
                            Time = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(Utils.DateTimeToDateTimeOffSet(DateTime.Now))
                        }.WriteTo(output);
                    }
                }
            }
            Console.WriteLine("参与测试速度的库...");
            /*
            //*/
            System.IO.Directory.CreateDirectory(Utils.MapPath("/111"));
            //
            var sortItems = new List<SortedItem>();

            Console.WriteLine("5000个集合对象文件大小比较...");
            sortItems.Add(new SortedItem { Name = "SimpleBinary", Text = "文件大小", Sort = new System.IO.FileInfo(SimpleBinaryPath).Length });

            sortItems.Add(new SortedItem { Name = "MessagePack", Text = "文件大小", Sort = new System.IO.FileInfo(MessagePackPath).Length });

            sortItems.Add(new SortedItem { Name = "Wire", Text = "文件大小", Sort = new System.IO.FileInfo(WirePath).Length });

            sortItems.Add(new SortedItem { Name = "protobuf-net", Text = "文件大小", Sort = new System.IO.FileInfo(protobufnetPath).Length });

            sortItems.Add(new SortedItem { Name = "Newtonsoft.Json", Text = "文件大小", Sort = new System.IO.FileInfo(NewtonsoftJsonPath).Length });
            
            sortItems.Add(new SortedItem { Name = "GoogleProtobuf", Text = "文件大小", Sort = new System.IO.FileInfo(googleprotobufPath).Length });
            sortItems.Sort();
            for (var i = 1; i <= sortItems.Count; i++)
            {
                Console.WriteLine(i + "." + sortItems[i - 1].ToString());
            }

            sortItems.Clear();
            //for (var k = 0; k < 3; k++)
            {
                Console.WriteLine($"开始序列化对象测试...");
                Console.WriteLine();
                const int count = 1000;
                var stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(SimpleBinaryPath));
                //var testModel = (List<Lw.Framework.Configs.ClassF>)new Lw.Serializers.Binary.GenCodes.Serialize_2ceb6e51ce64429caacda368df1a9d08().Deserialize(stream);
                var testModel = FastSerialize<List<ClassF>>.Deserialize(stream);
                //System.IO.File.WriteAllBytes(Lw.Framework.SystemTools.MapPath("/111/333.msg"), Lw.Framework.Configs.ClassF.ToBytesMsg(testModel));


                //Framework.Transport.ISerializer<System.Collections.Generic.List<Lw.Framework.Configs.ClassF>> lwTest = new Lw.Serializers.Binary.GenCodes.Serialize_2ceb6e51ce64429caacda368df1a9d08();
                var sw = System.Diagnostics.Stopwatch.StartNew();
                for (var i = 0; i < count; i++)
                {
                    var tempStream = FastSerialize<List<ClassF>>.Serialize(testModel);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "SimpleBinary", Text = "序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });
                //Console.WriteLine($"SimpleBinary序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");
                sw.Reset();
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    var bytes = OtherSerialize.ToBytesMsg<List<ClassF>>(testModel);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "MessagePack", Text = "序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });
                //Console.WriteLine($"MessagePack序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");
                sw.Reset();
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    var bytes = OtherSerialize.ToBytesWire(testModel);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "Wire", Text = "序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });
                //Console.WriteLine($"Wire序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");
                sw.Reset();
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    var bytes = OtherSerialize.ToBytesProtoBuf(testModel);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "ProtobufNet", Text = "序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });
                //Console.WriteLine($"ProtobufNet序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");
                sw.Reset();
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    var jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(testModel);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "Newtonsoft.Json", Text = "序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });
                //Console.WriteLine($"Newtonsoft.Json序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");
                //Console.WriteLine($"ProtobufNet序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");

                sw.Reset();
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    using (var output = new MemoryStream())
                    {
                        for (var k = 0; k < 5000; k++)
                        {
                            new ClassFPuf
                            {
                                Age = 30,
                                //KeyValues = new System.Collections.Generic.Dictionary<string, string> { { "k", "123_"+i }, { "l", "789_"+i } },
                                Address = "上海路_" + i,
                                Name = "雷老虎_" + i,
                                Time = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(Utils.DateTimeToDateTimeOffSet(DateTime.Now))
                            }.WriteTo(output);
                        }
                    }
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "GoogleProtobuf", Text = "序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });

                Console.WriteLine("序列化5000个对象耗时(毫秒)，循环1000次...");
                sortItems.Sort();
                for (var i = 1; i <= sortItems.Count; i++)
                {
                    Console.WriteLine(i + "." + sortItems[i - 1].ToString());
                }
                sortItems.Clear();


                Console.WriteLine();
                Console.WriteLine($"开始反序列化对象测试...");
                Console.WriteLine();

                stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(SimpleBinaryPath));
                sw.Reset();
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    var model = FastSerialize<List<ClassF>>.Deserialize(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "SimpleBinary", Text = "反序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });
                //Console.WriteLine($"SimpleBinary反序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");
                sw.Reset();
                stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(MessagePackPath));
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    var model = OtherSerialize.ToModelMsg<List<ClassF>>(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "MessagePack", Text = "反序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });
                //Console.WriteLine($"MessagePack反序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");
                sw.Reset();
                stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(WirePath));
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    var model = OtherSerialize.ToModelWire<List<ClassF>>(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "Wire", Text = "反序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });
                //Console.WriteLine($"Wire反序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");
                stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(protobufnetPath));
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    var model = OtherSerialize.ToModelProtoBuf<List<ClassF>>(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "ProtobufNet", Text = "反序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });
                //Console.WriteLine($"ProtobufNet反序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");
                sw.Reset();
                stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(NewtonsoftJsonPath));
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    var model = OtherSerialize.ToModelJson<List<ClassF>>(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "Newtonsoft.Json", Text = "反序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });


                sw.Reset();
                stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(googleprotobufPath));
                sw.Start();
                for (var i = 0; i < count; i++)
                {
                    for (var k = 0; k < 5000; k++)
                    {
                        var model = ClassFPuf.Parser.ParseFrom(stream);
                    }
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                }
                sw.Stop();
                sortItems.Add(new SortedItem { Name = "GoogleProtobuf", Text = "反序列化5000个对象耗时(毫秒)", Sort = sw.ElapsedMilliseconds });


                //Console.WriteLine($"Newtonsoft-Json反序列化5000个对象，共执行了{count}次，耗时{sw.ElapsedMilliseconds}毫秒");
                Console.WriteLine("反序列化5000个对象耗时(毫秒)，循环1000次...");
                sortItems.Sort();
                for (var i = 1; i <= sortItems.Count; i++)
                {
                    Console.WriteLine(i + "." + sortItems[i - 1].ToString());
                }
                sortItems.Clear();
            }



        }
        const string Name = "SimpleBinary";
        static void tupleTest()
        {
            Console.WriteLine($"{Name}=>测试Tuple...");
            var itemTuple = Tuple.Create<DateTime, string, byte[], ClassB>(DateTime.Now, "二进制序列化测试=>" + DateTime.Now.ToString(),
                Encoding.UTF8.GetBytes("看不见看不见..."),
                new ClassB()
                {
                    Age = 18,
                    Dict = new System.Collections.Generic.Dictionary<string, string> { { "c", "1" }, { "d", "3" } },
                    Name = "关山月",
                    Time = DateTime.Now,
                    Some = new ClassC
                    {
                        Age = 30,
                        Name = "沧海蓝",
                        Time = DateTime.Now,
                        Dict = new System.Collections.Generic.Dictionary<string, string> { { "e", "2" }, { "f", "4" } },
                        Some = new ClassD
                        {
                            Age = 30,
                            Name = "豪客迈",
                            Time = DateTime.Now,
                            Dict = new System.Collections.Generic.Dictionary<string, string> { { "g", "3" }, { "h", "5" } },
                            Some = new ClassE
                            {
                                Age = 30,
                                Name = "杰克逊",
                                Time = DateTime.Now,
                                Dict = new System.Collections.Generic.Dictionary<string, string> { { "i", "4" }, { "j", "6" } },
                                Some = new ClassF
                                {
                                    Age = 30,
                                    Name = "俱往矣",
                                    Time = DateTime.Now
                                }
                            }
                        }
                    }
                }
                );
            System.IO.Directory.CreateDirectory(Utils.MapPath("/111"));
            System.IO.File.WriteAllBytes(Utils.MapPath("/111/222.bin"),
                FastSerialize<Tuple<DateTime, string, byte[], ClassB>>
            .Serialize(itemTuple).ToArray());


            var model2 = FastSerialize<Tuple<DateTime, string, byte[], ClassB>>.Deserialize(new System.IO.MemoryStream(System.IO.File.ReadAllBytes(Utils.MapPath("/111/222.bin"))));
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(model2));
        }

        static void testSerialze()
        {
            Console.WriteLine($"{Name}=>测试复杂对象...");
            System.IO.Directory.CreateDirectory(Utils.MapPath("/111"));
            System.IO.File.WriteAllBytes(Utils.MapPath("/111/111.bin"),
                FastSerialize<ClassA>.Serialize(new ClassA()
                {
                    BTGirl = FiveBTGirl.乔碧罗,
                    CurrentStep = new ExecuteStack
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        ServiceName = "不会被序列化的服务",
                        Message = "不会被序列化",
                        ExecuteTime = 100,
                        TagId = "a1"
                    },
                    Age = 20,
                    Dict = new System.Collections.Generic.Dictionary<string, string> { { "a", "123" }, { "b", "789" } },
                    Name = "SimpleBinary",
                    Time = DateTime.Now,
                    NextStep = new ExecuteStack { Code = System.Net.HttpStatusCode.OK, ExecuteTime = 3000, Message = "测试", ServiceName = "111a", TagId = "123" },
                    Some = new ClassB()
                    {
                        Age = 18,
                        Dict = new System.Collections.Generic.Dictionary<string, string> { { "c", "123" }, { "d", "789" } },
                        Name = "帅哥2011",
                        Time = DateTime.Now,
                        Some = new ClassC
                        {
                            Age = 30,
                            Name = "美女A",
                            Time = DateTime.Now,
                            Dict = new System.Collections.Generic.Dictionary<string, string> { { "e", "123" }, { "f", "789" } },
                            Some = new ClassD
                            {
                                Age = 30,
                                Name = "美女B",
                                Time = DateTime.Now,
                                Dict = new System.Collections.Generic.Dictionary<string, string> { { "g", "123" }, { "h", "789" } },
                                Some = new ClassE
                                {
                                    Age = 30,
                                    Name = "美女C",
                                    Time = DateTime.Now,
                                    Dict = new System.Collections.Generic.Dictionary<string, string> { { "i", "123" }, { "j", "789" } },
                                    Some = new ClassF
                                    {
                                        Age = 30,
                                        //KeyValues = new System.Collections.Generic.Dictionary<string, string> { { "k", "123" }, { "l", "789" } },
                                        Name = "美女D",
                                        Time = DateTime.Now
                                    }
                                }
                            }
                        }
                    },
                    TupleValues = Tuple.Create<DateTime, string, byte[], ClassA>(
                    DateTime.Now, "EasyWorkFlow即将登场", Encoding.UTF8.GetBytes("你装作看不见看不见..."), new ClassA
                    {
                        Age = 18,
                        Name = "帅哥2013",
                        Time = DateTime.Now,
                        CurrentStep = new ExecuteStack
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            ExecuteTime = 3000000,
                            Message = "干什么",
                            ServiceName = "哈哈哈",
                            TagId = "b"
                        },
                        NextStep = new ExecuteStack
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            ServiceName = "初恋时",
                            Message = "很幸福",
                            ExecuteTime = 300000000,
                            TagId = "a"
                        },
                        Some = new ClassB
                        {
                            Age = 17,
                            Name = "我的初恋美女",
                            Time = DateTime.Now,

                        }
                    }
                )
                }).ToArray());



            var model = FastSerialize<ClassA>.Deserialize(new System.IO.MemoryStream(System.IO.File.ReadAllBytes(Utils.MapPath("/111/111.bin"))));
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(model));
        }
    }
}