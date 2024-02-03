using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Google.Protobuf;
using Microsoft.Extensions.Configuration;
using NAudio.Wave;
using SpeechToTranslated.SpeechRecognition;

public class NAudioToGoogleSpeechToText : ISpeechToText, IDisposable
{
    private readonly WaveInEvent waveSource = new WaveInEvent();
    private readonly SpeechClient speech;
    private readonly RecognitionConfig config;

    public event WordsEventArgs.WordsReadyHandler? WordsReady;
    public event WordsEventArgs.SentanceReadyHandler? SentanceReady;

    public NAudioToGoogleSpeechToText(IConfiguration appConfig)
    {
        ////GoogleCredential googleCredential;
        ////using (Stream m = new MemoryStream("{")
        ////    googleCredential = GoogleCredential.FromStream(m);
        ////var channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.Host,
        ////    googleCredential.ToChannelCredentials());
        ////var speech = SpeechClient.Create(channel);

        //Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", appConfig["translate.google.key"]);
        //var speech = new SpeechClientBuilder();
        //speech.GoogleCredential.

        //config = new RecognitionConfig
        //{
        //    Encoding = RecognitionConfig.Types.AudioEncoding.Flac,
        //    SampleRateHertz = 16000,
        //    LanguageCode = LanguageCodes.English.SouthAfrica
        //};
    }

    public Task RunSpeechToTextForeverAsync()
    {
        Console.WriteLine("Now recording...");
        
        waveSource.WaveFormat = new WaveFormat(16000, 1);

        waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);

        return Task.CompletedTask;
    }

    private void waveSource_DataAvailable(object? sender, WaveInEventArgs e)
    {
        var audio = RecognitionAudio.FromBytes(e.Buffer, 0, e.BytesRecorded);

        var response = speech.Recognize(config, audio);

        //var words = new List<string>();
        //foreach (var result in response.Results)
        //{
        //    words.Add(result.Alternatives.First().Transcript);
        //    foreach (var alternative in result.Alternatives)
        //    {
        //        Console.WriteLine($"{alternative.Transcript}, {alternative.Confidence}");
        //    }
        //}
        var topResult = response.Results.FirstOrDefault()?.Alternatives?.FirstOrDefault();
        WordsReady?.Invoke(new WordsEventArgs(false, (ulong)DateTime.Now.Ticks, topResult?.Transcript ?? ""));
    }


    public void Dispose()
    {
        waveSource.StartRecording();
        waveSource.StopRecording();

    }
}