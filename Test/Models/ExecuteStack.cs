using System.Collections.Generic;
using System.Net;

namespace Test.Models
{
    /// <summary>
    /// 请求处理堆栈
    /// </summary>
    public struct ExecuteStack
    {
        /// <summary>
        /// 此次请求的唯一处理Id
        /// </summary>
        public string TagId { get; set; }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 执行时间(毫秒)
        /// </summary>
        public int ExecuteTime { get; set; }
        /// <summary>
        /// 响应码
        /// </summary>
        public HttpStatusCode Code { get; set; }
        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 子处理请求
        /// </summary>
        //public List<ExecuteStack> Childs { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess() => Code == HttpStatusCode.OK;
    }
}