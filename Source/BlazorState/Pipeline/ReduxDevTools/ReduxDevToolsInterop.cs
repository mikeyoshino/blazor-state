namespace BlazorState.Pipeline.ReduxDevTools;

using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Threading.Tasks;

public class ReduxDevToolsInterop
{
  private const string JsFunctionName = "reduxDevTools.ReduxDevToolsDispatch";

  private readonly IJSRuntime JSRuntime;

  private readonly ILogger Logger;

  private readonly IReduxDevToolsStore Store;

  public bool IsEnabled { get; set; }

  public ReduxDevToolsInterop
  (
    ILogger<ReduxDevToolsInterop> aLogger,
    IReduxDevToolsStore aStore,
    IJSRuntime aJSRuntime
  )
  {
    Logger = aLogger;
    Store = aStore;
    JSRuntime = aJSRuntime;
  }

  public async Task DispatchAsync<TRequest>(TRequest aRequest, object aState)
  {
    if (IsEnabled)
    {
      Logger.LogDebug($"{GetType().Name}: {nameof(this.DispatchAsync)}");
      Logger.LogDebug($"{GetType().Name}: aRequest.GetType().FullName:{aRequest.GetType().FullName}");
      var reduxAction = new ReduxAction(aRequest);
      await JSRuntime.InvokeAsync<object>(JsFunctionName, reduxAction, aState);
    }
  }

  public async Task DispatchInitAsync(object aState)
  {
    if (IsEnabled)
      await JSRuntime.InvokeAsync<object>(JsFunctionName, "init", aState);
  }

  public async Task InitAsync()
  {
    Logger.LogDebug("Init ReduxDevToolsInterop");
    const string ReduxDevToolsFactoryName = "ReduxDevToolsFactory";
    IsEnabled = await JSRuntime.InvokeAsync<bool>(ReduxDevToolsFactoryName);

    if (IsEnabled)
      await DispatchInitAsync(Store.GetSerializableState());
  }
}
