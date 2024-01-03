﻿namespace SampleDotNet8.Client.Features.Counter;

using BlazorState;

public partial class CounterState
{
  public static class IncrementCount
  {
    public class Action : IAction
    {
      public int Amount { get; set; }
    }

    public class Handler : ActionHandler<Action>
    {
      public Handler(IStore aStore) : base(aStore) { }
      
      CounterState CounterState => Store.GetStateAsync<CounterState>();

      public override Task Handle(Action aAction, CancellationToken aCancellationToken)
      {
        CounterState.Count += aAction.Amount;
        return Task.CompletedTask;
      }
    }
  }
}
