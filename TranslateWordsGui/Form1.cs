using SpeechToTranslatedCommon;
using System.Globalization;
using System.IO.Pipes;
using System.Text.RegularExpressions;
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
        private List<Label> labels;
        private int labelIndex = 0;
        private bool verticalScrollbarAdded = false;
        private int colourAdd = 0;
        public Form1()
        {
            InitializeComponent();
            labels = new List<Label>() { };//label1, label2, label3, label4, label5 };
            splitContainer1.SplitterDistance = splitContainer1.Height;// - splitContainer1.Size.Height;
            numericUpDown1.Value = (decimal)modelLabel.Font.Size;
            numericUpDown1.DecimalPlaces = 1;
            numericUpDown1.Increment = 0.5m;
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;

            previewNumericUpDown2.Value = (decimal)previewLabel.Font.Size;
            previewNumericUpDown2.DecimalPlaces = 1;
            previewNumericUpDown2.Increment = 0.5m;
            previewNumericUpDown2.ValueChanged += PreviewNumericUpDown1_ValueChanged;


            controlsButton1.Click += ControlsButton1_Click;

            flowLayoutPanel8.ControlAdded += FlowLayoutPanel8_ControlAdded;
            flowLayoutPanel8.AutoScroll = true;
            flowLayoutPanel8.VerticalScroll.Enabled = true;
            flowLayoutPanel8.HorizontalScroll.Enabled = true;
            //webView21.CoreWebView2InitializationCompleted += WebView21_CoreWebView2InitializationCompleted;
            //_ = InitializeAsync();
            //webView21.CoreWebView2.OpenDevToolsWindow();

            this.Resize += Form1_Resize;

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            HideScrollbar();
        }

        private void FlowLayoutPanel8_ControlAdded(object sender, ControlEventArgs e)
        {
            HideScrollbar();
        }

        private void HideScrollbar()
        {
            if (verticalScrollbarAdded)
            {
                if (!flowLayoutPanel8.VerticalScroll.Visible)
                {
                    verticalScrollbarAdded = false;
                    flowLayoutPanel8.Size = new Size(flowLayoutPanel8.Size.Width - SystemInformation.VerticalScrollBarWidth, flowLayoutPanel8.Size.Height - SystemInformation.HorizontalScrollBarHeight);
                    flowLayoutPanel8.Invalidate();
                }
                return;
            }


            if (flowLayoutPanel8.VerticalScroll.Visible)
            {
                verticalScrollbarAdded = true;
                flowLayoutPanel8.Size = new Size(flowLayoutPanel8.Size.Width + SystemInformation.VerticalScrollBarWidth, flowLayoutPanel8.Size.Height + SystemInformation.HorizontalScrollBarHeight);
                flowLayoutPanel8.Invalidate();
            }
        }

        //private void WebView21_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        //{
        //    webView21.Source = new Uri($"file://{Path.Combine(Directory.GetCurrentDirectory(), "index.html")}");

        //}

        private async Task InitializeAsync()
        {
            //await webView21.EnsureCoreWebView2Async(null);
        }

        private void ControlsButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            var me = (NumericUpDown)sender;
            foreach (var label in labels)
                label.Font = new Font(label.Font.FontFamily, (float)me.Value);

            modelLabel.Font = new Font(modelLabel.Font.FontFamily, (float)me.Value);
            flowLayoutPanel8.SuspendLayout();
            foreach (var label in flowLayoutPanel8.Controls)
            {
                if (label is Label)
                    ((Label)label).Font = new Font(((Label)label).Font.FontFamily, (float)me.Value);
            }
            flowLayoutPanel8.ResumeLayout();
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

            this.Text = await translatorHelper.TranslateWords($"Translation into {new CultureInfo(languageCode).DisplayName}");

            listenerTask = Task.Factory.StartNew(() => BackgroundWorker1_DoWork(null, null!));
        }

        private async Task BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            client = new NamedPipeClientStream(".", string.Format(NamedMemoryPipe.PipeNameFormatString, languageCode), PipeDirection.In);

            this.Invoke(() => label2b.Text = $"\n\n{DateTime.Now.ToShortTimeString()} Connecting to server...\n\n");
            client.Connect();
            this.Invoke(() => label2b.Text = "");

            try
            {
                // Read data through the pipe
                var ss = new StreamString(client);

                string GetTicks()
                    => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);

                var otherLanguageFilenameFormatString = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_{{0}}.txt";
                var offsetRegex = new Regex("^([i|f|p]):offset:(\\d*):(.*$)", RegexOptions.Compiled);

                for (var words = ss.ReadString(); true; words = ss.ReadString())
                {
                    var offsetCapture = offsetRegex.Match(words);
                    var offset = ulong.Parse(offsetCapture.Groups[2].Value);
                    words = offsetCapture.Groups[3].Value;
                    var isIncremental = offsetCapture.Groups[1].ToString() == "i";
                    var isFinalParagraph = offsetCapture.Groups[1].ToString() == "p";

                    if (words == null || words.Length == 0)
                        continue;

                    string saveWords;
                    if (words == "\n\n" || words == "\n")
                    {
                        saveWords = words;
                    }
                    else
                    {
                        var translation = await translatorHelper.TranslateWords(words);

                        if (isFinalParagraph)
                        {
                            //////Label useLabel;
                            //////if (labelIndex >= labels.Count)
                            //////{
                            //////    --labelIndex;
                            //////    var updateLabel = labels.First();
                            //////    foreach (var label in labels.Skip(1))
                            //////    {
                            //////        this.Invoke(() => updateLabel.Text = label.Text);
                            //////        updateLabel = label;
                            //////    }
                            //////    this.Invoke(() => labels[labelIndex].Text = "");
                            //////    //useLabel = labels[labelIndex];
                            //////}
                            //////useLabel = labels[labelIndex];
                            UpdateFinalParagraph(words, translation);//, useLabel);
                            labelIndex++;
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
                this.Invoke(() => label2b.Text = $"\n\n{ex.Message}\n\n");
            }
        }

        private void UpdateIncremental(string words, string translation)
        {
            this.Invoke(() => previewLabel.Text += translation);
            this.Invoke(() => label1b.Text += words);
        }

        private void UpdateAbsolute(string words, string translation)
        {
            this.Invoke(() => previewLabel.Text = translation);
            this.Invoke(() => label1b.Text = words);
        }

        private void UpdateFinalParagraph(string words, string translation)//, Label useLabel)
        {
            //this.Invoke(() => useLabel.Text = translation);
            this.Invoke(() => label1b.Text = words);

            //this.Invoke(() => useLabel.Text += "\n\n");
            this.Invoke(() => previewLabel.Text = translation);

            this.Invoke(() =>
            {
                var label = new Label() { Text = $"{translation}\n\n" };
                label.Font = modelLabel.Font;

                if (checkBox1.Checked)
                {
                    colourAdd += 10;
                    if (colourAdd > 100)
                        colourAdd = 0;

                    byte[] bytes = BitConverter.GetBytes(modelLabel.ForeColor.ToArgb());
                    byte aVal = bytes[0];
                    byte rVal = bytes[1];
                    byte gVal = bytes[2];
                    byte bVal = bytes[3];
                    var wildcardR = new Random().Next(3) - 1;
                    var wildcardG = new Random().Next(3) - 1;
                    var wildcardB = new Random().Next(3) - 1;
                    var r = Math.Max(0, Math.Min(255, wildcardR * colourAdd + rVal + new Random().Next(80) - 40));
                    var g = Math.Max(0, Math.Min(255, wildcardG * colourAdd + gVal + new Random().Next(80) - 40));
                    var b = Math.Max(0, Math.Min(255, wildcardB * colourAdd + bVal + new Random().Next(80) - 40));
                    label.ForeColor = Color.FromArgb(aVal, r, g, b);
                }

                label.AutoSize = true;
                flowLayoutPanel8.Controls.Add(label);
                //flowLayoutPanel8.VerticalScroll.Visible = false;
                flowLayoutPanel8.ScrollControlIntoView(label);
            });
        }
    }
}