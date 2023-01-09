using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace TelegramSoundboardBot.Frontend.Localization;

public interface ILocalizationService
{
    void SetLocale(CultureInfo cultureInfo);
    string Localize(string str);
    string Localize(string str, params object[] args);
}

public class LocalizationService : ILocalizationService
{
    private static readonly ResourceManager ResourceManager =
        new("TelegramSoundboardBot.Frontend.Localization.Resources.TelegramBot", typeof(LocalizationService).Assembly);

    private CultureInfo _cultureInfo = CultureInfo.InvariantCulture;

    public void SetLocale(CultureInfo cultureInfo)
    {
        _cultureInfo = cultureInfo;
    }

    public string Localize(string str)
    {
        var res = ResourceManager.GetString(str, _cultureInfo);
        Debug.Assert(res is not null);
        return res;
    }

    public string Localize(string str, params object[] args)
    {
        return string.Format(_cultureInfo, Localize(str), args);
    }
}