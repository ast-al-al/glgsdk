public interface IRewardedProvider
{
    public string RewardedAdapterName { get; }
    public string RewardedNetworkName { get; }
    public bool IsRewardedReady { get; }
    public IInterProvider InitializeRewarded();
    public IRewardedProvider ShowRewarded(string placement);
    public IRewardedProvider HideRewraded();
    /// <summary>
    /// Устанавливает однократный обратный вызов на успешный показ реварда.
    /// </summary>
    /// <param name="callback">Обратный вызов. 1 параметр - имя сетки</param>
    public IRewardedProvider OnRewardedSuccess(System.Action<string> callback);
    /// <summary>
    /// Устанавливает однократный обратный вызов на получение награды.
    /// </summary>
    /// <param name="callback">Обратный вызов. 1 параметр - имя сетки</param>
    public IRewardedProvider OnReward(System.Action<string> callback);
    /// <summary>
    /// Устанавливает однократный обратный вызов на ошибку показа реварда.
    /// </summary>
    /// <param name="callback">Обратный вызов. 1 параметр - имя сетки, 2 параметр - код ошибки</param>
    public IRewardedProvider OnRewardedError(System.Action<string, string> callback);
}
