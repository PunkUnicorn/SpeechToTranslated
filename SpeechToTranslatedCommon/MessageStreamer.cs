﻿using System.Text;
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

        public void EncodeMessage(bool isFinalParagraph, bool isAddTo, ulong offset, string englishWords)
        {
            var codeChar = isAddTo
                ? "i"
                : isFinalParagraph
                    ? "p"
                    : "f";

            WriteString($"{codeChar}:offset:{offset}:{englishWords}");
        }

        public static string DecodeMessage(string message, out bool isIncremental, out bool isFinalParagraph, out ulong offset)
        {
            var offsetRegex = new Regex("^([i|f|p]):offset:(\\d*):(.*$)", RegexOptions.Compiled);
            var offsetCapture = offsetRegex.Match(message);
            offset = ulong.Parse(offsetCapture.Groups[2].Value);
            var words = offsetCapture.Groups[3].Value;
            isIncremental = offsetCapture.Groups[1].ToString() == "i";
            isFinalParagraph = offsetCapture.Groups[1].ToString() == "p";
            return words;
        }
    }
}