
namespace TranslateWordsProcess
{
    public interface ITranslateResult
    {
        string Words { get; }
        dynamic Result { get; }
    }

    public interface ITranslateService
    {
        Task<ITranslateResult> TranslateWordsAsync(string words);
    }
}