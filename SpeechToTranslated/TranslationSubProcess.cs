using SpeechToTranslatedCommon;
using SpeechToTranslatorCommon;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace SpeechToTranslated
{
    public class TranslationSubProcess : IDoTranslation
    {
        private Process process;
        private NamedPipeServerStream namedPipeServer;
        private StreamString namedPipeServerWriter;
        private readonly string languageCode;
        private readonly bool wantEnglish;

        public TranslationSubProcess(string languageCode, bool wantEnglish) 
        { 
            this.languageCode = languageCode;
            this.wantEnglish = wantEnglish;
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

        internal void Kill() 
            => process.Kill();

        private void CreateProcess()
        {
            if (process is not null && !process.HasExited)
                process.Kill();

            var psi = new ProcessStartInfo("TranslateWordsProcess.exe")
            {
                UseShellExecute = true,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                Arguments = $"{languageCode} {wantEnglish}"
            };
            process = new Process { StartInfo = psi };
            process.Start();
        }

        private void CreateNamedPipe()
        {
            if (namedPipeServer is not null)
                namedPipeServer.Close();    

            namedPipeServer = new NamedPipeServerStream(string.Format(NamedMemoryPipe.PipeNameFormatString, languageCode), PipeDirection.Out);
            namedPipeServerWriter = new StreamString(namedPipeServer);
            Console.WriteLine($"Waiting for client connection for {languageCode}...");
            namedPipeServer.WaitForConnection();
            Console.WriteLine($"Client connected for {languageCode}.");
        }
    }
}
