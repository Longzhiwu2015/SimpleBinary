using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleBinary
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Utils
    {
        #region 获取类型全名
        /// <summary>
        /// 确定指定类型的实例是否可以分配给当前类型的实例
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="parantType"></param>
        /// <returns></returns>
        public static bool IsAssignableFrom(Type testType, Type parantType)
        {
            return testType.FullName != "System.Object" && testType.IsAssignableFrom(parantType);
        }
        /// <summary>
        /// 确定指定类型的实例是否可以分配给当前类型的实例
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="parantType"></param>
        /// <returns></returns>
        public static bool IsAssignableFromForEmit(Type testType, Type parantType)
        {
            return testType.FullName != "System.Object" && parantType.IsAssignableFrom(testType);
        }
        /// <summary>
        /// 获取类型全名
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static string GetFullName(string typeFullName)
        {
            var builder = new System.Text.StringBuilder();
            builder.Append(typeFullName);
            builder.Replace('+', '.');
            return _getFullName(builder);
        }
        /// <summary>
        /// 获取类型全名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFullName(Type type)
        {
            if (type.IsNested || type.IsGenericType)
                return GetFullName(type.FullName);
            return type.FullName;
        }
        /// <summary>
        /// 获取类型全名
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        static string _getFullName(StringBuilder builder)
        {
            var genericTypeIndex = builder.ToString().IndexOf('`');
            if (genericTypeIndex == -1)
            {
                return _getClassRealName(builder.ToString());
            }
            var argumentIndex = builder.ToString().IndexOf('[');
            if (argumentIndex == -1)
            {
                argumentIndex = builder.ToString().IndexOf('`');
                if (argumentIndex != -1)
                {
                    builder.Remove(argumentIndex, builder.Length - argumentIndex);
                    return builder.ToString() + "<T>";
                }
                throw new Exception("类全名不符合规范=>" + builder.ToString());
            }
            var argumentBuilder = new System.Text.StringBuilder();
            argumentBuilder.Append(builder.ToString().Substring(argumentIndex));
            builder.Length = argumentIndex;
            var tempGenericArgumentIndex = 0;
            var genericCount = 0;
            do
            {
                //获取泛型参数个数List
                var numberText = "";
                var tempIndex = genericTypeIndex + 1;
                var tempString = builder.ToString();
                for (; tempIndex < builder.Length; tempIndex++)
                {
                    var c = tempString[tempIndex];
                    if (c == '`')
                    {
                        break;
                    }
                    short tempShort;
                    if (short.TryParse(c.ToString(), out tempShort))
                    {
                        numberText += c.ToString();
                    }
                    else
                    {
                        break;
                    }
                }
                byte tempByte;
                if (byte.TryParse(numberText, out tempByte))
                {
                    numberText = "<{";
                    for (var i = 0; i < tempByte; i++)
                    {
                        if (i != 0)
                            numberText += "}, {";
                        numberText += tempGenericArgumentIndex;
                        tempGenericArgumentIndex++;
                    }
                    builder.Remove(genericTypeIndex, tempByte.ToString().Length + 1);
                    numberText += "}>";
                    builder.Insert(genericTypeIndex, numberText);
                }
                genericCount += tempByte;
                if (tempByte == 0)
                {
                    //防止死循环
                    builder.Remove(genericTypeIndex, 1);
                }
                genericTypeIndex = builder.ToString().IndexOf('`');

            } while (genericTypeIndex != -1);
            //有几个参数
            return string.Format(builder.ToString(), _getArguments(argumentBuilder, genericCount).ToArray());
        }
        static IEnumerable<string> _getArguments(StringBuilder argumentBuilder, int argumentCount)
        {
            argumentBuilder.Remove(argumentBuilder.Length - 1, 1);
            argumentBuilder.Remove(0, 1);
            var argumentString = argumentBuilder.ToString();
            if (argumentString.IndexOf('[') == -1 || argumentString.IndexOf(']') == -1)
            {
                var argumentIndex = argumentBuilder.ToString().IndexOf('`');
                if (argumentIndex != -1)
                {
                    argumentBuilder.Remove(argumentIndex, argumentBuilder.Length - argumentIndex);
                    yield return argumentBuilder.ToString() + "<T>";
                    yield break;
                }
                throw new Exception("泛型类全名不符合规范");
            }
            var tempCount = 0;
            var tempStartIndex = 0;
            var thisArgumentCount = 0;
            for (var i = 0; i < argumentString.Length; i++)
            {
                var c = argumentString[i];
                if (c == '[')
                {
                    if (tempCount == 0)
                        tempStartIndex = i + 1;
                    tempCount++;
                    continue;
                }
                if (c == ']')
                {
                    tempCount--;
                    if (tempCount == 0)
                    {
                        if (i <= tempStartIndex)
                            throw new Exception("泛型类全名不符合规范");
                        thisArgumentCount += 1;
                        yield return _getFullName(new StringBuilder(_getClassRealName(argumentString.Substring(tempStartIndex, i - tempStartIndex))));
                    }
                }
            }
            if (thisArgumentCount != argumentCount)
            {
                throw new Exception($"泛型参数数量不符合要求 {thisArgumentCount}/{argumentCount}");
            }
        }
        static string _getClassRealName(string tempString)
        {
            var tokenIndex = tempString.LastIndexOf(", Version=", StringComparison.Ordinal);
            if (tokenIndex != -1)
            {
                tempString = tempString.Substring(0, tokenIndex);
                tokenIndex = tempString.LastIndexOf(',');
                tempString = tempString.Substring(0, tokenIndex);
            }
            return tempString;
        }
        #endregion 获取类型全名


        #region 定值哈希算法
        /// <summary>
        /// 计算定值哈希码
        /// <para>返回非负整数</para>
        /// </summary>
        /// <param name="content">自动去除首尾空格</param>
        /// <returns></returns>
        public static uint HashCodeUInt(string content)
        {
            return HashCodeUInt(Encoding.UTF8.GetBytes(content));
            //return HashCodeInt(content) & 0x7fffffff;
        }
        /// <summary>
        /// 计算定值哈希码
        /// <para>返回非负整数</para>
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static uint HashCodeUInt(IEnumerable<byte> bytes)
        {
            uint num = 0;
            uint num2 = 0;
            foreach (var c in bytes)
            {
                num = ((num << 4) * 0x81) + c;
                num2 = num & 0xf0000000;
                if (num2 != 0)
                {
                    num ^= num2 >> 0x18;
                    num &= ~num2;
                }
            }
            return num;
            //return HashCodeInt(bytes) & 0x7fffffff;
        }
        /// <summary>
        /// 计算定值哈希码
        /// </summary>
        /// <param name="content">自动去除首尾空格</param>
        /// <returns></returns>
        public static int HashCodeInt(string content)
        {
            return HashCodeInt(Encoding.UTF8.GetBytes(content));
        }
        /// <summary>
        /// 计算定值哈希码
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int HashCodeInt(IEnumerable<byte> bytes)
        {
            uint num = 0;
            uint num2 = 0;
            foreach (var c in bytes)
            {
                num = ((num << 4) * 0x81) + c;
                num2 = num & 0xf0000000;
                if (num2 != 0)
                {
                    num ^= num2 >> 0x18;
                    num &= ~num2;
                }
            }
            return (int)num;
        }
        /// <summary>
        /// 计算定值哈希码
        /// </summary>
        /// <param name="content">自动去除首尾空格</param>
        /// <returns></returns>
        public static long HashCodeLong(string content)
        {
            return HashCodeLong(Encoding.UTF8.GetBytes(content));
        }
        /// <summary>
        /// 计算定值哈希码
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static long HashCodeLong(IEnumerable<byte> bytes)
        {
            ulong num = 0;
            ulong num2 = 0;
            foreach (var c in bytes)
            {
                num = ((num << 8) * 0x101) + c;
                num2 = num & 0xf000000000000000;
                if (num2 != 0)
                {
                    num ^= num2 >> 0x30;
                    num &= ~num2;
                }
            }
            return (long)num;
        }
        /// <summary>
        /// 计算定值哈希码
        /// <para>返回非负整数</para>
        /// </summary>
        /// <param name="content">自动去除首尾空格</param>
        /// <returns></returns>
        public static ulong HashCodeULong(string content)
        {
            return HashCodeULong(Encoding.UTF8.GetBytes(content));
        }
        /// <summary>
        /// 计算定值哈希码
        /// <para>返回非负整数</para>
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ulong HashCodeULong(IEnumerable<byte> bytes)
        {
            ulong num = 0;
            ulong num2 = 0;
            foreach (var c in bytes)
            {
                num = ((num << 8) * 0x101) + c;
                num2 = num & 0xf000000000000000;
                if (num2 != 0)
                {
                    num ^= num2 >> 0x30;
                    num &= ~num2;
                }
            }
            return num;
        }
        #endregion 定值哈希算法


        #region 时间和Long类型相互转换
        /// <summary>
        /// 时间相互转换
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTimeOffset DateTimeToDateTimeOffSet(DateTime datetime)
        {
            //return new DateTimeOffset(datetime, TimeZoneInfo.Local.GetUtcOffset(datetime));
            return DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
        }
        /// <summary>
        /// 时间相互转换
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime DateTimeOffSetToDateTime(DateTimeOffset datetime)
        {
            if (datetime.Offset.Equals(TimeSpan.Zero))
                return datetime.UtcDateTime;
            else if (datetime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(datetime.DateTime)))
                return DateTime.SpecifyKind(datetime.DateTime, DateTimeKind.Local);
            else
                return datetime.DateTime;
        }
        ///<summary>
        /// 把C#时间转为long(时间戳)TotalMilliseconds
        /// <para>获取指定时间的时间戳</para>
        ///</summary>
        ///<param name="time">C#时间</param>
        ///<returns>转换后的long</returns>
        public static long TimeToLong(DateTime time)
        {
            return (long)(time - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalMilliseconds; // 当地时区
            //return (time.ToUniversalTime().Ticks - DateTime.Parse("1970-1-1 00:00:00").Ticks) / 10000000;
        }
        ///<summary>
        /// long(时间戳)转化为C#时间(毫秒)
        /// <para>时间戳转换为时间</para>
        ///</summary>
        ///<param name="timeTicks">毫秒</param>
        ///<returns>转化后的C#时间</returns>
        public static DateTime LongToTime(long timeTicks)
        {
            return new DateTime(1970, 1, 1).ToLocalTime().AddMilliseconds(timeTicks);
            //return DateTime.MinValue.AddTicks(timeTicks);
            //return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(timeStamp);
            //return new DateTime(1970, 1, 1).ToLocalTime().AddSeconds(time);
        }
        ///<summary>
        /// 把C#时间转为long(时间戳)TotalSeconds
        /// <para>获取指定时间的时间戳</para>
        ///</summary>
        ///<param name="time">C#时间</param>
        ///<returns>转换后的long</returns>
        public static long TimeToLongBySecond(DateTime time)
        {
            return (long)(time - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds; // 当地时区
        }
        ///<summary>
        /// long(时间戳)转化为C#时间(秒)
        /// <para>时间戳转换为时间</para>
        ///</summary>
        ///<param name="seconds">秒</param>
        ///<returns>转化后的C#时间</returns>
        public static DateTime LongToTimeBySecond(long seconds)
        {
            return new DateTime(1970, 1, 1).ToLocalTime().AddSeconds(seconds);
        }
        /// <summary>
        /// 是否是时间格式数据
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string content)
        {
            DateTime result;
            if (TryParse(content, out result))
                return result;
            return DateTime.MinValue;
        }
        /// <summary>
        /// 将字符串转换为DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out DateTime result)
        {
            if (DateTime.TryParse(value, out result))
            {
                return true;
            }
            long timeValue;
            if (long.TryParse(value, out timeValue))
            {
                result = Utils.LongToTime(timeValue);
                return true;
            }
            result = default(DateTime);
            return false;
        }
        ///<summary>
        /// 把C#TimeSpan转换为long
        ///</summary>
        ///<param name="time">C#时间</param>
        ///<returns>转换后的long</returns>
        public static long TimeSpanToLong(TimeSpan time)
        {
            return time.Ticks;
        }
        ///<summary>
        /// long转化为C#时间TimeSpan
        ///</summary>
        ///<param name="time">时间值</param>
        ///<returns>转化后的C#时间</returns>
        public static TimeSpan LongToTimeSpan(long time)
        {
            return new TimeSpan(time);
        }
        /// <summary>
        /// 判断时间是否小于默认值1753-01-01
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsDefaultTime(DateTime time)
        {
            return time.Ticks < DefaultMinTileTick;
        }
        /// <summary>
        /// 判断时间是否等于默认值1753-01-01
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool EqualDefaultTime(DateTime time)
        {
            return time.Ticks == DefaultMinTileTick;
        }
        /// <summary>
        /// 1753-01-01的时间Ticks
        /// </summary>
        public const long DefaultMinTileTick = 552877920000000000;
        /// <summary>
        /// 获取数据库支持的最小时间值
        /// <para>兼容所有数据库</para>
        /// </summary>
        /// <returns></returns>
        public static DateTime SqlMinDateTime()
        {
            return new DateTime(DefaultMinTileTick);
        }
        #endregion 时间和Long类型相互转换


        /// <summary>
        /// 可空类型Nullable
        /// </summary>
        public static readonly Type NullAbleType = typeof(Nullable<>);
        /// <summary>
        /// 是否可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(NullAbleType);
        }
        /// <summary>
        /// 匿名类型名关键字
        /// </summary>
        public const string AnonymousTypeKey = "<>f__AnonymousType";
        /// <summary>
        /// 是否Tuple类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTupleType(Type type)
        {
            return typeof(System.Runtime.CompilerServices.ITuple).IsAssignableFrom(type);
        }
        /// <summary>
        /// 判断类型是否匿名类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAnonymousType(Type type)
        {
            return IsAnonymousType(type.FullName);
        }
        /// <summary>
        /// 判断类型是否匿名类型
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static bool IsAnonymousType(string typeFullName)
        {
            return typeFullName.IndexOf(AnonymousTypeKey, StringComparison.Ordinal) != -1;
        }





        /// <summary>
        /// 分隔字符1(,)
        /// </summary>
        public const char SpiltChat1 = ',';
        /// <summary>
        /// 分隔字符2(|)
        /// </summary>
        public const char SpiltChat2 = '|';
        /// <summary>
        /// 将字符串转换为枚举
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse<T>(string value, out T result) where T : struct
        {
            try
            {
                string[] valueSplits;
                if (value.IndexOf(SpiltChat1) != -1)
                    valueSplits = value.Split(SpiltChat1);
                else if (value.IndexOf(SpiltChat2) != -1)
                    valueSplits = value.Split(SpiltChat2);
                else valueSplits = null;
                if (valueSplits != null && valueSplits.Length > 1)
                {
                    int tempInt = 0;
                    foreach (var num in valueSplits)
                    {
                        T tempResult;
                        if (Enum.TryParse(num, out tempResult))// && Enum.IsDefined(typeof(T), tempResult))
                            tempInt |= Convert.ToInt32(tempResult);
                    }
                    return Enum.TryParse(tempInt.ToString(), out result);
                }
                return Enum.TryParse(value, out result);// && Enum.IsDefined(typeof(T), tempResult))
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }
        /// <summary>
        /// 将int转换为枚举
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse<T>(int value, out T result) where T : struct
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                result = (T)Enum.ToObject(typeof(T), value);
                return true;
            }
            result = default(T);
            return false;
        }
        /// <summary>
        /// 将object转换为枚举
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse<T>(object value, out T result) where T : struct
        {
            if (value == null)
            {
                result = default(T);
                return false;
            }
            if (value.GetType().IsEnum)
            {
                result = (T)value;
                return true;
            }
            return TryParse<T>(value.ToString(), out result);
        }
        /// <summary>
        /// 将字符串转换为枚举?
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse<T>(string value, out T? result) where T : struct
        {
            if (value == null || value.Length == 0)
            {
                result = default(T);
                return true;
            }
            T tempResult;
            var resultBool = TryParse<T>(value, out tempResult);
            result = tempResult;
            return resultBool;
        }
        /// <summary>
        /// 将对象转换为枚举?
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse<T>(object value, out T? result) where T : struct
        {
            if (value == null)
            {
                result = default(T);
                return true;
            }
            T tempResult;
            var resultBool = TryParse<T>(value, out tempResult);
            result = tempResult;
            return resultBool;
        }








        #region 路径服务
        /// <summary>
        /// 将cshtml的Web形式路径转换为类名
        /// </summary>
        /// <param name="razorFileWebPath">Web路径形式</param>
        /// <returns></returns>
        public static string RazorPathToClassName(string razorFileWebPath)
        {
            var builder = new System.Text.StringBuilder();
            bool isNumberFirst = false;
            foreach (byte b in Encoding.UTF8.GetBytes(razorFileWebPath))
            {
                if (b >= (byte)'0' && b <= (byte)'9')
                {
                    if (builder.Length == 0) isNumberFirst = true;
                    builder.Append((char)b);
                }
                else if (b >= (byte)'a' && b <= (byte)'z')
                    builder.Append((char)b);
                else if (b >= (byte)'A' && b <= (byte)'Z')
                    builder.Append((char)b);
                else
                    builder.Append("_");
            }
            if (isNumberFirst) builder.Insert(0, "_");
            return builder.ToString();
        }
        /// <summary>
        /// 从RazorView视图根路径获取相对路径(从Views开始)
        /// </summary>
        /// <param name="razorFileWebPath">Web路径形式</param>
        /// <returns></returns>
        public static string GetRazorViewPathShortPath(string razorFileWebPath)
        {
            var viewRootPath = WebPath(razorFileWebPath);
            var tempIndex = viewRootPath.LastIndexOf(ViewsPath, StringComparison.OrdinalIgnoreCase);
            if (tempIndex != -1)
            {
                viewRootPath = viewRootPath.Substring(tempIndex + ViewsPath.Length);
            }
            tempIndex = viewRootPath.LastIndexOf(".cshtml", StringComparison.OrdinalIgnoreCase);
            if (tempIndex != -1) return viewRootPath.Substring(0, tempIndex);
            return viewRootPath;
        }
        /// <summary>
        /// 从RazorView视图根路径获取相对路径(从Views开始)
        /// </summary>
        /// <param name="razorFileWebPath">Web路径形式</param>
        /// <returns></returns>
        public static string GetRazorViewRootPath(string razorFileWebPath)
        {
            var viewRootPath = WebPath(razorFileWebPath);
            var tempIndex = viewRootPath.LastIndexOf(ViewsPath, StringComparison.OrdinalIgnoreCase);
            if (tempIndex != -1)
            {
                return viewRootPath.Substring(0, tempIndex + 7);
            }
            var fullPath = MapPath(razorFileWebPath);
            if (File.Exists(fullPath)) return WebPath(Path.GetDirectoryName(fullPath));
            return viewRootPath;
        }
        const string ViewsPath = "/Views/";


        static readonly string _VirtualPath = Path.AltDirectorySeparatorChar.ToString();
        /// <summary>
        /// 获取网站的虚拟目录
        /// <para>自动识别/或者~/</para>
        /// </summary>
        public static string VirtualPath => _VirtualPath;
        /// <summary>
        /// 根据文件完整路径，获取Web绝对路径
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static string WebPath(string fileFullPath)
        {
            string resultPath;
            if (fileFullPath == null || fileFullPath.Length == 0)
                resultPath = Path.AltDirectorySeparatorChar.ToString();
            //Linux平台，两个分隔字符一样
            else if (Path.AltDirectorySeparatorChar == Path.DirectorySeparatorChar)
            {
                //Linux下尚需修正
                resultPath = fileFullPath;
            }
            else
            {
                string localvirtualPath = VirtualPath;
                if (fileFullPath.IndexOf(_appStartPath, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (fileFullPath.IndexOf(Path.DirectorySeparatorChar) >= 0)
                        resultPath = string.Concat(localvirtualPath, fileFullPath.Substring(_appStartPath.Length).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                    else
                        resultPath = string.Concat(localvirtualPath, fileFullPath.Substring(_appStartPath.Length));
                }
                else
                {
                    string str = fileFullPath;
                    if (fileFullPath.IndexOf(Path.DirectorySeparatorChar) >= 0)
                        str = fileFullPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    if (str.IndexOf(localvirtualPath, StringComparison.Ordinal) == 0)
                        resultPath = str;
                    else
                    {
                        if (str.IndexOf(string.Concat("~", Path.AltDirectorySeparatorChar), StringComparison.Ordinal) == 0)
                            str = str.Substring(2);
                        else if (str.IndexOf(Path.AltDirectorySeparatorChar) == 0)
                            str = str.Substring(1);
                        resultPath = string.Concat(localvirtualPath, str);
                    }
                }
            }
            return resultPath;
        }
        /// <summary>
        /// Bin路径目录
        /// </summary>
        public static string BinPath => MapPath("/Bin/");
        /// <summary>
        /// 返回相对于站点路径的物理路径全路径
        /// <para>string.Format(@"{0}partPath", BaseConfigModel.AppStartPath)</para>
        /// <para>此方法不受异步影响System.Web.Hosting.HostingEnvironment.MapPath(partPath);</para>
        /// </summary>
        /// <param name="partPath">文件或目录的部分路径</param>
        /// <returns></returns>
        public static string MapPath(string partPath)
        {
            //return System.Web.Hosting.HostingEnvironment.MapPath(partPath);
            if (partPath == null || partPath.Length == 0)
                return partPath;
            if (partPath.IndexOf(':') != -1)
            {
                try
                {
                    if (partPath.IndexOf("://", StringComparison.Ordinal) != -1)
                        partPath = Uri.UnescapeDataString(new Uri(partPath).AbsolutePath);
                    else
                        return partPath;
                }
                catch
                {
                    return partPath;
                }
            }
            partPath = partPath.Trim();
            if (partPath.IndexOf(Path.VolumeSeparatorChar) == -1)
            {
                var builder = new System.Text.StringBuilder();
                builder.Append(partPath);
                if (builder.ToString().IndexOf('~') == 0)
                    builder.Remove(0, 1);
                if ('/' == Path.DirectorySeparatorChar)
                {
                    //Linux
                    if (builder.ToString().IndexOf('\\') != -1)
                        builder.Replace('\\', '/');
                    while (builder.ToString().IndexOf('/') == 0)
                        builder.Remove(0, 1);
                    //应该不允许用父路径
                    var tempIndex = builder.ToString().IndexOf("../", StringComparison.Ordinal);
                    if (tempIndex == 0)
                    {
                        var tempBasePath = _appStartPath;
                        if (tempBasePath.LastIndexOf('/') == tempBasePath.Length - 1)
                            tempBasePath = tempBasePath.Substring(0, tempBasePath.Length - 1);
                        do
                        {
                            var baseIndex = tempBasePath.LastIndexOf('/');
                            if (baseIndex < 1) throw new Exception("路径位置定位错误");
                            tempBasePath = tempBasePath.Substring(0, baseIndex);
                            builder.Remove(0, 3);
                            tempIndex = builder.ToString().IndexOf("../", StringComparison.Ordinal);
                        }
                        while (tempIndex != -1);
                        builder.Insert(0, "/");
                        builder.Insert(0, tempBasePath);
                        return builder.ToString();
                    }
                }
                else
                {
                    //Windows
                    if (builder.ToString().IndexOf('/') != -1)
                        builder.Replace('/', Path.DirectorySeparatorChar);
                    while (builder.ToString().IndexOf(Path.DirectorySeparatorChar) == 0)
                        builder.Remove(0, 1);
                    //应该不允许用父路径
                    var tempIndex = builder.ToString().IndexOf("..\\", StringComparison.Ordinal);
                    if (tempIndex == 0)
                    {
                        var tempBasePath = _appStartPath;
                        if (tempBasePath.LastIndexOf('\\') == tempBasePath.Length - 1)
                            tempBasePath = tempBasePath.Substring(0, tempBasePath.Length - 1);
                        do
                        {
                            var baseIndex = tempBasePath.LastIndexOf('\\');
                            if (baseIndex < 1) throw new Exception("路径位置定位错误");
                            tempBasePath = tempBasePath.Substring(0, baseIndex);
                            builder.Remove(0, 3);
                            tempIndex = builder.ToString().IndexOf("..\\", StringComparison.Ordinal);
                        }
                        while (tempIndex != -1);
                        builder.Insert(0, "\\");
                        builder.Insert(0, tempBasePath);
                        return builder.ToString();
                    }
                }
                builder.Insert(0, _appStartPath);
                return builder.ToString();
            }
            return partPath;
        }
        static readonly string _appStartPath = AppContext.BaseDirectory;
        #endregion 路径服务
    }
}