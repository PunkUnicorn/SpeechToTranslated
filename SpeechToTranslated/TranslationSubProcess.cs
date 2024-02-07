using SpeechToTranslatedCommon;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace SpeechToTranslated
{
    public class TranslationSubProcess : ITranslate
    {
        private Process process;
        private NamedPipeServerStream namedPipeServer;
        private MessageStreamer namedPipeServerWriter;
        private readonly string languageCode;

        public TranslationSubProcess(string languageCode)
        {
            this.languageCode = languageCode;
            CreateProcess();
            CreateNamedPipe();
        }

        public void OutputLineBreak()
        {
            try
            {
                namedPipeServerWriter.WriteString("\n\n");
            }
            catch (Exception e)
            {
                var fg = Console.ForegroundColor;
                try
                {
                    Console.Error.WriteLine($"{e.Message} for {languageCode}. Recreating...");
                    CreateProcess();
                    CreateNamedPipe();
                }
                finally
                {
                    Console.ForegroundColor = fg;
                }
            }
        }

        public void TranslateWords(bool isFinalParagraph, bool isAddTo, ulong offset, string englishWords)
        {
            try
            {
                namedPipeServerWriter.EncodeMessage(isFinalParagraph, isAddTo, offset, englishWords);
            }
            catch (Exception e)
            {
                var fg = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"{e.Message} for {languageCode}. Recreating...");
                    CreateProcess();
                    CreateNamedPipe();
                }
                finally
                {
                    Console.ForegroundColor = fg;
                }
            }
        }

        public void TranslateWords(string englishWords)
        {
            try
            {
                namedPipeServerWriter.WriteString(englishWords);
            }
            catch (Exception e)
            {
                var fg = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"{e.Message} for {languageCode}. Recreating...");
                    CreateProcess();
                    CreateNamedPipe();
                }
                finally
                {
                    Console.ForegroundColor = fg;
                }
            }
        }

        public void TranslateGreenWords(string words)
            => TranslateWords($"green:{words}");

        internal void Kill()
            => process.Kill();

        private void CreateProcess()
        {
            if (process is not null && !process.HasExited)
                process.Kill();

            var psi = new ProcessStartInfo("TranslateWordsGui.exe")
            {
                UseShellExecute = true,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                Arguments = $"{languageCode}"
            };
            process = new Process { StartInfo = psi };
            process.Start();
        }

        private void CreateNamedPipe()
        {
            if (namedPipeServer is not null)
                namedPipeServer.Close();

            namedPipeServer = new NamedPipeServerStream(string.Format(NamedMemoryPipe.PipeNameFormatString, languageCode), PipeDirection.Out);
            namedPipeServerWriter = new MessageStreamer(namedPipeServer);
            Console.WriteLine($"Waiting for client connection for {languageCode}...");
            namedPipeServer.WaitForConnection();
            Console.WriteLine($"Client connected for {languageCode}.");
        }
    }
}
