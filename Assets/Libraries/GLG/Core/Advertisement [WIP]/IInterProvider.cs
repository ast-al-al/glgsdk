public interface IInterProvider
{
    public string InterAdapterName { get; }
    public string InterNetworkName { get; }
    public bool IsInterReady { get; }
    public IInterProvider InitializeInter();
    public IInterProvider ShowInter(string placement);
    public IInterProvider HideInter();
    /// <summary>
    /// Устанавливает однократный обратный вызов на закрытие интера после показа или ошибки.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IInterProvider OnInterClosed(System.Action callback);
    /// <summary>
    /// Устанавливает однократный обратный вызов на успешный показ интера.
    /// </summary>
    /// <param name="callback">Обратный вызов. 1 параметр - имя сетки</param>
    public IInterProvider OnInterSuccess(System.Action<string> callback);
    /// <summary>
    /// Устанавливает однократный обратный вызов на ошибку показа интера.
    /// </summary>
    /// <param name="callback">Обратный вызов. 1 параметр - имя сетки, 2 параметр - код ошибки</param>
    public IInterProvider OnInterError(System.Action<string, string> callback);
}
