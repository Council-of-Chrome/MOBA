using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public bool Active { get; private set; }
    public float TotalDuration { get; private set; }

    private float currentTime;

    private Action OnStart { get; }
    private Action OnUpdate { get; }
    private Action OnEnd { get; }

    public void Start(float _duration)
    {
        TotalDuration = _duration;
        OnStart?.Invoke();
        GameClock.AddEventToUpdate(UpdateTick);
        Active = true;
    }

    private void UpdateTick(float _timeStep)
    {
        OnUpdate?.Invoke();

        currentTime += _timeStep;
        if (currentTime >= TotalDuration)
            Stop();
    }

    public void Stop()
    {
        currentTime = 0f;
        OnEnd?.Invoke();
        GameClock.RemoveEventFromUpdate(UpdateTick);
        Active = false;
    }

    public void AbruptReset()
    {
        currentTime = 0f;
    }

    public void Reset(float _duration)
    {
        Stop();
        Start(_duration);
    }

    public Timer(Action _start = null, Action _update = null, Action _end = null)
    {
        OnStart = _start;
        OnUpdate = _update;
        OnEnd = _end;
    }
}
