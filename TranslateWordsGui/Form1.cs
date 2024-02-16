using SpeechToTranslatedCommon;
using System.Globalization;
using System.IO.Pipes;
using System.Text;
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

        public Form1()
        {
            InitializeComponent();

            splitContainer1.SplitterDistance = splitContainer1.Height;
            numericUpDown1.Value = (decimal)modelLabel.Font.Size;
            numericUpDown1.DecimalPlaces = 1;
            numericUpDown1.Increment = 0.5m;
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;

            previewNumericUpDown2.Value = (decimal)previewLabel.Font.Size;
            previewNumericUpDown2.DecimalPlaces = 1;
            previewNumericUpDown2.Increment = 0.5m;
            previewNumericUpDown2.ValueChanged += PreviewNumericUpDown1_ValueChanged;

            forcedRestartButton.Click += ControlsButton1_Click;

            MakeDebugContents(false);
        }

        private void MakeDebugContents(bool debug)
        {
            if (!debug)
                return;

            var sb = new StringBuilder();
            for (int i = 0; i < 100; i++)
                sb.Append($"ipsom lorum {i} ");
            translationFlowLayoutPanel.Controls.Add(new Label { Text = sb.ToString(), Font = modelLabel.Font, AutoSize = true });
        }

        private void ControlsButton1_Click(object sender, EventArgs e) => Close();

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            var me = (NumericUpDown)sender;

            modelLabel.Font = new Font(modelLabel.Font.FontFamily, (float)me.Value);
            translationFlowLayoutPanel.SuspendLayout();
            foreach (var label in translationFlowLayoutPanel.Controls)
            {
                if (label is Label)
                    ((Label)label).Font = new Font(((Label)label).Font.FontFamily, (float)me.Value);
            }
            translationFlowLayoutPanel.ResumeLayout();
        }

        private void PreviewNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            var me = (NumericUpDown)sender;
            previewLabel.Font = new Font(previewLabel.Font.FontFamily, (float)me.Value);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            if ((Program.Args?.Length ?? 0) == 0) throw new ArgumentException("Need parameter of the language code e.g. es"); ;
            languageCode = Program.Args?[0] ?? throw new ArgumentException("Need parameter of the language code e.g. es"); ;

            var appConfig = ConfigurationLoader.Load() ?? throw new InvalidProgramException("Unable to read appsettings.json");
            translatorHelper = new TranslatorHelper(appConfig, languageCode);

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
                var ss = new MessageStreamer(client);

                string GetTicks()
                    => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);

                var otherLanguageFilenameFormatString = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_{{0}}.txt";

                for (var message = ss.ReadString(); true; message = ss.ReadString())
                {
                    var isTranslationMessage = MessageStreamer.DecodeTranslationMessage(message, out var words,out var isIncremental, out var isFinalParagraph, out var offset);

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
                            UpdateFinalParagraph(words, translation);
                        }
                        else
                        {
                            if (!isIncremental)
                                UpdateAbsolute(words, translation);
                            else
                                UpdateIncremental(words, translation);
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
            if (!MessageStreamer.DecodeLayoutMessage(message, out var count, out var index))
                return false;
            //var myScreen = Screen.FromControl(this);
            ////var otherScreen = Screen.AllScreens.FirstOrDefault(s => s.Equals(myScreen));
            //this.Left = myScreen.WorkingArea.Left + 120;
            //this.Top = myScreen.WorkingArea.Top + 120;
            //this.Width = myScreen.WorkingArea.Width / count;
            //this.Height = myScreen.WorkingArea.Height;
            //return true;
            var screen = Screen.FromControl(this);
            var sc = screen.WorkingArea;
            Point p = new Point(sc.Location.X + (sc.Width * (index - 1)), sc.Location.Y);
            this.Location = p;
            this.Width = sc.Width/count;
            this.Height = sc.Height;
            //this.Top = sc.Top;
            //this.Left = sc.Width*(index-1);
            return true;
        }

        private void UpdateIncremental(string words, string translation)
        {
            this.Invoke(() => previewLabel.Text += translation);
            this.Invoke(() => debugPreviewLabel.Text += words);
        }

        private void UpdateAbsolute(string words, string translation)
        {
            this.Invoke(() => previewLabel.Text = translation);
            this.Invoke(() => debugPreviewLabel.Text = words);
        }

        private void UpdateFinalParagraph(string words, string translation)
        {
            this.Invoke(() => debugPreviewLabel.Text = words);
            this.Invoke(() => previewLabel.Text = translation);
            this.Invoke(() =>
            {
                var label = new Label() { Text = $"{translation}\n\n" };
                label.Font = modelLabel.Font;

                funkyColours.SetSeed(words.GetHashCode());
                if (checkBox1.Checked)
                    label.ForeColor = funkyColours.MakeFunkyColour(modelLabel.ForeColor);

                label.AutoSize = true;
                translationFlowLayoutPanel.Controls.Add(label);
                translationFlowLayoutPanel.ScrollControlIntoView(label);
            });
        }

        private void hideButton_Click(object sender, EventArgs e)
            => this.WindowState = FormWindowState.Minimized;
    }
}