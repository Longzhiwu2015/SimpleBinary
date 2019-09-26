using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleBinary.Serializers
{
    /// <summary>
    /// 服务端接收上传文件流
    /// </summary>
    class ServerReceiveUploadFile : Stream// UploadFile
    {
        bool isInited;
        void init()
        {
            if (isInited) return;
            isInited = true;
            var dir = AppContext.BaseDirectory + "App_Data";
            dir = Path.Combine(dir, "Temp");
            Directory.CreateDirectory(dir);
            _filePath = Path.Combine(dir, Guid.NewGuid().ToString() + ".tmp");
            _fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.ReadWrite);
        }
        string _filePath;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath => _filePath;
        /// <summary>
        /// 是否null
        /// </summary>
        public bool IsNull => _fileStream == null;
        bool isReset;
        Stream _fileStream;
        /// <summary>
        /// 上传的文件内容
        /// </summary>
        //public override Stream Body { get { if (_fileStream != null && !isReset) { _fileStream.Seek(0, SeekOrigin.Begin); isReset = true; } return _fileStream; } set { if(_fileStream != null) throw new Exception("当前流已赋值"); _fileStream = value; } }

        void resetFileStream()
        {
            if (_fileStream != null && !isReset) { _fileStream.Seek(0, SeekOrigin.Begin); isReset = true; }
        }
        /// <summary>
        /// 打开读取(可能会报空引用异常)
        /// </summary>
        /// <returns></returns>
        public Stream OpenRead()
        {
            if (!isReset)
            {
                _fileStream.Seek(0, SeekOrigin.Begin);
                isReset = true;
            }
            return _fileStream;
        }
        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool SaveAs(string filePath)
        {
            if (_fileStream == null) return false;
            _fileStream.Dispose();
            _fileStream = null;
            if (filePath.Equals(_filePath, StringComparison.OrdinalIgnoreCase))
            {
                _filePath = null;
                return true;
            }
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.Move(_filePath, filePath);
            return true;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool isDisposing)
        {
            if (_fileStream != null) _fileStream.Dispose();
            if (_filePath != null) File.Delete(_filePath);
        }















        public override void Flush()
        {
            if (_fileStream != null) _fileStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            resetFileStream();
            if (_fileStream != null) return _fileStream.Read(buffer, offset, count);
            return -1;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (_fileStream != null) return _fileStream.Seek(offset, origin);
            return -1;
        }

        public override void SetLength(long value)
        {
            if (_fileStream != null) _fileStream.SetLength(value);
        }
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            init();
            return _fileStream.WriteAsync(buffer, cancellationToken);
        }
        public override bool CanRead => _fileStream != null;

        public override bool CanSeek => _fileStream != null;

        public override bool CanWrite => _fileStream != null;

        public override long Length { get { if (_fileStream == null) return -1; return _fileStream.Length; } }

        public override long Position { get { if (_fileStream == null) return -1; resetFileStream(); return _fileStream.Position; } set { if (_fileStream == null) return; _fileStream.Position = value; } }

        /// <summary>
        /// 写入文件数据
        /// </summary>
        /// <param name="b"></param>
        public override void WriteByte(byte b)
        {
            init();
            _fileStream.WriteByte(b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            init();
            await _fileStream.WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// 写入文件数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            init();
            _fileStream.Write(buffer, offset, count);
        }
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            resetFileStream();
            return _fileStream.BeginRead(buffer, offset, count, callback, state);
        }
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            init();
            return _fileStream.BeginWrite(buffer, offset, count, callback, state);
        }
        public override void Close()
        {
            resetFileStream();
            _fileStream.Close();
            //if (_fileStream == null) return;
        }
        public override void CopyTo(Stream destination, int bufferSize)
        {
            if (_fileStream == null) return;
            resetFileStream();
            _fileStream.CopyTo(destination, bufferSize);
        }
        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            if (_fileStream == null) return Task.CompletedTask;
            resetFileStream();
            return _fileStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }
        public override int EndRead(IAsyncResult asyncResult)
        {
            resetFileStream();
            return _fileStream.EndRead(asyncResult);
        }
        public override void EndWrite(IAsyncResult asyncResult)
        {
            if (_fileStream == null) return;
            _fileStream.EndWrite(asyncResult);
        }
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            if (_fileStream == null) return Task.CompletedTask;
            return _fileStream.FlushAsync(cancellationToken);
        }
        public override int Read(Span<byte> buffer)
        {
            resetFileStream();
            return _fileStream.Read(buffer);
        }
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            resetFileStream();
            return _fileStream.ReadAsync(buffer, cancellationToken);
        }
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            resetFileStream();
            return _fileStream.ReadAsync(buffer, offset, count, cancellationToken);
        }
        public override int ReadByte()
        {
            resetFileStream();
            return _fileStream.ReadByte();
        }
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            init();
            _fileStream.Write(buffer);
        }
    }
}