using System;
using System.Collections.Generic;
using System.IO;

namespace Infoline.Framework
{
    public class LongMemoryStream : Stream
    {
        long _position;
        long _length;
        List<byte[]> _data = new List<byte[]>();
        int _maxArrayLength = 512 * 1024 * 1024;

        public override bool CanRead
        {
            get { return true; }
        }
        public override bool CanSeek
        {
            get { return true; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }
        public override long Length
        {
            get { return _length; }
        }
        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public LongMemoryStream()
        {

        }
        public LongMemoryStream(long length)
        {
            _position = 0;
            _length = length;
        }
        public LongMemoryStream(string file)
        {
            _position = 0;
            using (FileStream f = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                var fileSize = f.Length;
                while (fileSize > 0)
                {
                    var size = (int)Math.Min(_maxArrayLength, fileSize);
                    byte[] bytes = new byte[_maxArrayLength];
                    f.Read(bytes, 0, size);
                    //Write(bytes, 0, size);
                    _data.Add(bytes);
                    fileSize -= size;
                }
                _length = f.Length;
                _position = f.Length;
            }
        }


        public override void Write(byte[] buffer, int offset, int count)
        {
            long newLength = Math.Max(_position + count, _length);
            int newArrayCount = (int)(Math.Ceiling((double)newLength / _maxArrayLength));
            int extraArrayCount = newArrayCount - _data.Count;
            for (int i = 0; i < extraArrayCount; i++)
                _data.Add(new byte[_maxArrayLength]);


            int firstPartIndex = (int)((double)_position / _maxArrayLength);
            int lastPartIndex = (int)((double)(_position + count) / _maxArrayLength);

            long globalStart = _position;
            long globalCount = count;
            long destStart = 0;
            for (int i = firstPartIndex; i <= lastPartIndex; i++)
            {
                var localStart = globalStart % _maxArrayLength;
                var localCount = Math.Min(_maxArrayLength - localStart, globalCount);
                if (localCount == 0) break;
                Array.Copy(buffer, destStart, _data[i], localStart, localCount);
                globalStart += localCount;
                globalCount -= localCount;
                destStart += localCount;
            }
            _length = newLength;
            _position = newLength;
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_position + count > _length)
                throw new IndexOutOfRangeException();
            if (buffer == null)
                throw new NullReferenceException();
            if (buffer.Length < offset + count)
                throw new IndexOutOfRangeException();

            int firstPartIndex = (int)((double)_position / _maxArrayLength);
            int lastPartIndex = (int)((double)(_position + count) / _maxArrayLength);

            long globalStart = _position;
            long globalCount = count;
            long destStart = 0;
            for (int i = firstPartIndex; i <= lastPartIndex; i++)
            {
                var localStart = globalStart % _maxArrayLength;
                var localCount = Math.Min(_maxArrayLength - localStart, globalCount);
                if (localCount <= 0) break;
                Array.Copy(_data[i], localStart, buffer, destStart, localCount);
                globalStart += localCount;
                globalCount -= localCount;
                destStart += localCount;
            }
            return (int)destStart;

        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    _position = offset;
                    return _position;
                case SeekOrigin.Current:
                    _position = _position + offset;
                    return _position;
                case SeekOrigin.End:
                    _position = _length + _position;
                    return _position;
                default:
                    break;
            }
            return -1;
        }
        public override void SetLength(long value)
        {
            var newLength = value;
            if (_position > newLength)
                _position = newLength;

            if (newLength < _length)
            {
                int exArrayCount = _data.Count;
                int newArrayCount = (int)(Math.Ceiling((double)newLength / _maxArrayLength));
                int extraArrayCount = _data.Count - newArrayCount;
                for (int i = 0; i < extraArrayCount; i++)
                {
                    var idx = exArrayCount - i - 1;
                    _data.RemoveAt(idx);
                }
                var s = newLength % _maxArrayLength;
                var ss = _maxArrayLength - s;
                Array.Copy(_data[newArrayCount - 1], s, new byte[ss], 0, ss);
            }
            else if (newLength > _length)
            {
                int newArrayCount = (int)(Math.Ceiling((double)newLength / _maxArrayLength));
                int extraArrayCount = newArrayCount - _data.Count;
                for (int i = 0; i < extraArrayCount; i++)
                    _data.Add(new byte[_maxArrayLength]);
            }

            _length = newLength;
        }
        public override void Flush()
        {

        }
        protected override void Dispose(bool disposing)
        {
            _data = null;
            base.Dispose(disposing);
        }

    }
}
