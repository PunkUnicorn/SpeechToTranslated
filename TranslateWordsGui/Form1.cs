using SpeechToTranslatedCommon;
using System.ComponentModel;
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

        public Form1()
        {
            InitializeComponent();
            labels = new List<Label>() { label1, label2, label3 };
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

        private async void BackgroundWorker1_DoWork(object? sender, System.ComponentModel.DoWorkEventArgs e)
        {
            client = new NamedPipeClientStream(".", string.Format(NamedMemoryPipe.PipeNameFormatString, languageCode), PipeDirection.In);

            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Connecting to server...");
            client.Connect();
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Connected to server!");

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

                        Label useLabel;
                        if (labelIndex >= labels.Count)
                        {
                            --labelIndex;
                            var updateLabel = labels.First();
                            foreach (var label in labels.Skip(1))
                            {
                                this.Invoke(() => updateLabel.Text = label.Text);
                                updateLabel = label;
                            }
                            this.Invoke(() => labels[labelIndex].Text = "");
                            useLabel = labels[labelIndex];
                        }
                        useLabel = labels[labelIndex];
                        //var isLabelInvisible = Parent == null
                        //    ? false
                        //    : useLabel.Bottom > Parent.Height;

                        //if (isLabelInvisible)
                        //    this.Invoke(() => useLabel.Text = "CANT SEE IT");

                        if (isFinalParagraph)
                        {
                            this.Invoke(() => useLabel.Text = translation);
                            this.Invoke(() => label1b.Text = words);

                            this.Invoke(() => useLabel.Text += "\n\n");

                            labelIndex++;
                        }
                        else
                        {
                            if (!isIncremental)
                            {
                                this.Invoke(() => useLabel.Text = translation);
                                this.Invoke(() => label1b.Text = words);
                            }
                            else
                            {
                                this.Invoke(() => useLabel.Text += translation);
                                this.Invoke(() => label1b.Text += words);
                            }
                        }

                        saveWords = translation;
                    }

                    if (isFinalParagraph)
                        File.AppendAllText(string.Format($"{TranslatorHelper.logpath}{otherLanguageFilenameFormatString}", languageCode), $"{saveWords}\n\n");
                }
            }
            catch (Exception ex)
            {
                this.Invoke(() => label1b.Text = ex.Message);
            }
        }


    }
}