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
        private readonly bool forceConsole;

        public TranslationSubProcess(string languageCode, bool forceConsole)
        {
            this.languageCode = languageCode;
            this.forceConsole = forceConsole;
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
                namedPipeServerWriter.EncodeTranslationMessage(isFinalParagraph, isAddTo, offset, englishWords);
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

        //public void TranslateWords(string englishWords)
        //{
        //    try
        //    {
        //        namedPipeServerWriter.WriteString(englishWords);
        //    }
        //    catch (Exception e)
        //    {
        //        var fg = Console.ForegroundColor;
        //        try
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.Error.WriteLine($"{e.Message} for {languageCode}. Recreating...");
        //            CreateProcess();
        //            CreateNamedPipe();
        //        }
        //        finally
        //        {
        //            Console.ForegroundColor = fg;
        //        }
        //    }
        //}

        //public void TranslateGreenWords(string words)
        //    => TranslateWords($"green:{words}");

        internal void Kill()
            => process.Kill();

        private void CreateProcess()
        {
            if (process is not null && !process.HasExited)
                process.Kill();

            var program = forceConsole || !OperatingSystem.IsWindows()
                ? "TranslateWordsConsole.exe"
                : "TranslateWordsGui.exe";

            var psi = new ProcessStartInfo(program)
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

        internal void LayoutShift(int count, int index)
        {
            try
            {
                namedPipeServerWriter.EncodeLayoutMessage(count, index);
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
    }
}
