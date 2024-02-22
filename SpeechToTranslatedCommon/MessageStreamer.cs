using System.Text;
using System.Text.RegularExpressions;

namespace SpeechToTranslatedCommon
{
    public static class NamedMemoryPipe
    {
        public const string PipeNameFormatString = "SpeechToTranslated\\{0}";
    }

    // https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-use-named-pipes-for-network-interprocess-communication
    // Defines the data protocol for reading and writing strings on our stream.
    public class MessageStreamer
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;
        private static Regex offsetRegex = new Regex("^([i|f|p]):offset:(\\d*):([-|0-9]\\d*):(.*)$", RegexOptions.Compiled);

        public MessageStreamer(Stream ioStream)
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
                len = (int)UInt16.MaxValue;

            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }

        public void EncodeTranslationMessage(bool isFinalParagraph, bool isAddTo, ulong offset, string englishWords, int sharedRandom)
        {
            var codeChar = isAddTo
                ? "i"
                : isFinalParagraph
                    ? "p"
                    : "f";

            WriteString($"{codeChar}:offset:{offset}:{sharedRandom}:{englishWords}");
        }

        public static bool DecodeTranslationMessage(string message, out string words, out bool isIncremental, out bool isFinalParagraph, out ulong offset, out int sharedRandom)
        {
            var offsetCapture = offsetRegex.Match(message);
            if (!offsetCapture.Success)
            {
                words = "";
                offset = 0;
                isFinalParagraph = false;
                isIncremental = false;
                sharedRandom = 0;
                return false;
            }
            isIncremental = offsetCapture.Groups[1].ToString() == "i";
            isFinalParagraph = offsetCapture.Groups[1].ToString() == "p";
            offset = ulong.Parse(offsetCapture.Groups[2].Value);
            sharedRandom = int.Parse(offsetCapture.Groups[3].Value);
            words = offsetCapture.Groups[4].Value;
            return true;
        }

        public void EncodeLayoutMessage(int count, int index)
        {
            WriteString($":layout:{count}:{index}");
        }

        public static bool DecodeLayoutMessage(string message, out int count, out int index)
        {
            var layoutRegex = new Regex("^:layout:(\\d*):(\\d*)", RegexOptions.Compiled);
            var layoutCapture = layoutRegex.Match(message);
            if (!layoutCapture.Success)
            {
                count = -1;
                index = -1;
                return false;
            }

            count = int.Parse(layoutCapture.Groups[1].Value);
            index = int.Parse(layoutCapture.Groups[2].Value);

            return true;
        }
    }
}
