using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float TotalDuration { get; private set; }
    public float CurrentTime { get; private set; }

    private Action OnStart { get; }
    private Action OnUpdate { get; }
    private Action OnEnd { get; }

    public void Start(float _duration)
    {
        TotalDuration = _duration;
        OnStart?.Invoke();
        GameClock.AddEventToUpdate(UpdateTick);
    }

    private void UpdateTick(float _timeStep)
    {
        OnUpdate?.Invoke();

        CurrentTime += _timeStep;
        if (CurrentTime >= TotalDuration)
            Stop();
    }

    public void Stop()
    {
        CurrentTime = 0f;
        OnEnd?.Invoke();
        GameClock.RemoveEventFromUpdate(UpdateTick);
    }

    public void Reset(float _castTime)
    {
        Stop();
        Start(_castTime);
    }

    public Timer(Action _start = null, Action _update = null, Action _end = null)
    {
        OnStart = _start;
        OnUpdate = _update;
        OnEnd = _end;
    }
}
