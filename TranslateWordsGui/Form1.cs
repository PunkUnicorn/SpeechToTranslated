using SpeechToTranslatedCommon;
using System.Drawing;
using System.Globalization;
using System.IO.Pipes;
using System.Text;
using TranslateWordsConsole;
using TranslateWordsProcess;

namespace TranslateWordsGui
{
    public partial class Form1 : Form
    {
        private class OffsetTranslation
        {
            public OffsetTranslation(ulong offset, string translation) { Offset = offset; Translation = translation; }

            public ulong Offset { get; private set; }
            public string Translation { get; private set; }
        }

        private TranslatorHelper translatorHelper;
        private NamedPipeClientStream client;
        private string languageCode;
        private Task listenerTask;
        private FunkyColours funkyColours = new FunkyColours();
        private BroadcastHelper broadcastHelper;

        public Form1()
        {
            InitializeComponent();

            splitContainer1.SplitterDistance = splitContainer1.Height;
            numericUpDown1.Value = (decimal)modelLabel.Font.Size;
            numericUpDown1.DecimalPlaces = 1;
            numericUpDown1.Increment = 0.1m;
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;

            previewNumericUpDown2.Value = (decimal)previewLabel.Font.Size;
            previewNumericUpDown2.DecimalPlaces = 1;
            previewNumericUpDown2.Increment = 0.1m;
            previewNumericUpDown2.ValueChanged += PreviewNumericUpDown1_ValueChanged;

            forcedRestartButton.Click += ControlsButton1_Click;

            messageFlower1.Font = modelLabel.Font;

            if (Program.Args.Contains("test"))
            {
                var testTimer = new System.Windows.Forms.Timer();
                testTimer.Interval = 1000;
                testTimer.Tick += TestTimer_Tick;
                testTimer.Start();
            }
        }

        private void TestTimer_Tick(object sender, EventArgs e)
        {
            var me = (System.Windows.Forms.Timer)sender;
            var sb = new StringBuilder();
            for (int i = 0; i < new Random().Next(100); i++)
                sb.Append($"ipsom lorum {i} ");

            sb.AppendLine();
            sb.AppendLine();

            UpdateFinalParagraph("", sb.ToString(), new Random().Next(int.MinValue, int.MaxValue));
        }

        private void ControlsButton1_Click(object sender, EventArgs e) => Close();

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            var me = (NumericUpDown)sender;

            modelLabel.Font = new Font(modelLabel.Font.FontFamily, (float)me.Value);

            messageFlower1.Font = modelLabel.Font;
            messageFlower1.Invalidate();
        }

        private void PreviewNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            var me = (NumericUpDown)sender;
            previewLabel.Font = new Font(previewLabel.Font.FontFamily, (float)me.Value);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var paramException = new ArgumentException("Need parameter of at least one language code e.g. es");
            if ((Program.Args?.Length ?? 0) == 0) throw paramException;
            languageCode = Program.Args?[0] ?? throw paramException;

            var appConfig = ConfigurationLoader.Load() ?? throw new InvalidProgramException("Unable to read appsettings.json");
            translatorHelper = new TranslatorHelper(appConfig, languageCode);
            broadcastHelper = new BroadcastHelper(appConfig, languageCode);
            if (broadcastHelper.IsBroadcastService)
                await broadcastHelper.WakeupAsync();

            this.Text = await translatorHelper.TranslateWordsAsync($"Translation into {new CultureInfo(languageCode).DisplayName}");

            listenerTask = Task.Factory.StartNew(() => BackgroundWorker1_DoWork(null, null!));

        }

        private async Task BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            client = new NamedPipeClientStream(".", string.Format(NamedMemoryPipe.PipeNameFormatString, languageCode), PipeDirection.In);

            this.Invoke(() => errorLabel.Text = $"\n\n{DateTime.Now.ToShortTimeString()} Connecting to server...\n\n");
            client.Connect();
            this.Invoke(() => errorLabel.Text = "");

            try
            {
                // Read data through the pipe
                var ss = new InterProcessMessageStreamer(client);

                string GetTicks()
                    => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);

                var otherLanguageFilenameFormatString = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_{{0}}.txt";

                for (var message = ss.ReadString(); true; message = ss.ReadString())
                {
                    var isTranslationMessage = InterProcessMessageStreamer.DecodeTranslationMessage(message, out var words,out var isIncremental, out var isFinalParagraph, out var offset, out int sharedRandom);

                    if (!isTranslationMessage)
                    {
                        ProcessLayoutMessage(message);
                        continue;
                    }

                    if (words == null || words.Length == 0)
                        continue;

                    string saveWords;
                    if (words == "\n\n" || words == "\n")
                    {
                        saveWords = words;
                    }
                    else
                    {
                        var translation = await translatorHelper.TranslateWordsAsync(words);

                        if (isFinalParagraph)
                        {
                            UpdateFinalParagraph(words, translation, sharedRandom);
                        }
                        else
                        {
                            if (isIncremental)
                                UpdateIncremental(words, translation);
                            else
                                UpdateAbsolute(words, translation);
                        }

                        saveWords = translation;
                    }

                    if (isFinalParagraph)
                        File.AppendAllText(string.Format($"{TranslatorHelper.logpath}{otherLanguageFilenameFormatString}", languageCode), $"{saveWords}\n\n");
                }
            }
            catch (Exception ex)
            {
                this.Invoke(() => errorLabel.Text = $"\n\n{ex.Message}\n\n");
            }
        }

        private bool ProcessLayoutMessage(string message)
        {
            if (!InterProcessMessageStreamer.DecodeLayoutMessage(message, out var count, out var index))
                return false;

            var sc = Screen.GetWorkingArea(this);
            this.Width = sc.Width / count;
            this.Height = sc.Height;
            this.Top = 0;
            this.Left = this.Width * (index - 1);
            return true;
        }

        private void UpdateIncremental(string words, string translation)
        {
            try
            {
                previewLabel.Invoke(() => previewLabel.Text += translation);
                debugPreviewLabel.Invoke(() => debugPreviewLabel.Text += words);
            }
            catch (Exception ex)
            {
                this.Invoke(() => errorLabel.Text = $"\n\n{ex.Message}\n\n");
            }
        }

        private void UpdateAbsolute(string words, string translation)
        {
            try
            {
                previewLabel.Invoke(() => previewLabel.Text = translation);
                debugPreviewLabel.Invoke(() => debugPreviewLabel.Text = words);
            }
            catch (Exception ex)
            {
                this.Invoke(() => errorLabel.Text = $"\n\n{ex.Message}\n\n");
            }
        }

        private void UpdateFinalParagraph(string words, string translation, int sharedRandom)
        {

            try
            {
                debugPreviewLabel.Invoke(() => debugPreviewLabel.Text = words);
                previewLabel.Invoke(() => previewLabel.Text = translation);

                var colour = funkyColours.MakeFunkyColour(modelLabel.ForeColor, sharedRandom);

                if (broadcastHelper.IsBroadcastService)
                    broadcastHelper.BroadcastFinalParagraph(translation, colour);

                messageFlower1.Invoke(() => messageFlower1.AddParagraph($"{translation}\n\n", colour));
            }
            catch (Exception ex)
            {
                this.Invoke(() => errorLabel.Text = $"\n\n{ex.Message}\n\n");
            }
        }

        private void hideButton_Click(object sender, EventArgs e)
            => this.WindowState = FormWindowState.Minimized;
    }
}
