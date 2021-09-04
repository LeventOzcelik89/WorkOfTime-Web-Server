namespace Infoline.Framework
{
    public interface IByteArrayConvertable
    {
        int Size { get; }

        byte[] GetBytes();
        byte[] GetBytes(int bufferSize);
        void SetData(byte[] data);
    }
}
