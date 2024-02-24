
public interface IBannerProvider
{
    public string BannerAdapterName { get; }
    public string BannerNetworkName { get; }
    public bool IsBannerReady { get; }
    public IBannerProvider InitializeBanner();
    public IBannerProvider ShowBanner(string placement);
    public IBannerProvider HideBanner();
    /// <summary>
    /// Устанавливает однократный обратный вызов на успешный показ баннера.
    /// </summary>
    /// <param name="callback">Обратный вызов. 1 параметр - имя сетки</param>
    public IBannerProvider OnBannerSuccess(System.Action<string> callback);
    /// <summary>
    /// Устанавливает однократный обратный вызов на ошибку показа баннера.
    /// </summary>
    /// <param name="callback">Обратный вызов. 1 параметр - имя сетки, 2 параметр - код ошибки</param>
    public IBannerProvider OnBannerError(System.Action<string, string> callback);
}
