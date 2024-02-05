using System.Text;

namespace SpeechToTranslatedCommon
{
    public static class NamedMemoryPipe
    {
        public const string PipeNameFormatString = "SpeechToTranslated\\{0}";
    }

    // https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-use-named-pipes-for-network-interprocess-communication
    // Defines the data protocol for reading and writing strings on our stream.
    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int len;
            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            var inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }

        public void WriteStringWithOffset(bool isFinalParagraph, bool isAddTo, ulong offset, string englishWords)
        {
            var codeChar = isAddTo
                ? "i"
                : isFinalParagraph
                    ? "p"
                    : "f";

            WriteString($"{codeChar}:offset:{offset}:{englishWords}");
        }
    }
}
