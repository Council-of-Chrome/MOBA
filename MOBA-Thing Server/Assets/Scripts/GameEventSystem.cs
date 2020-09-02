using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventSystem
{
    private delegate void GameEventListener(EventInfo _info);
    private static Dictionary<Type, List<GameEventListener>> gameEventListeners = new Dictionary<Type, List<GameEventListener>>();

    public static void Sub<T>(Action<T> _eventListener) where T : EventInfo
    {
        Type eventType = typeof(T);

        if (!gameEventListeners.ContainsKey(eventType) || gameEventListeners[eventType] == null)
            gameEventListeners[eventType] = new List<GameEventListener>();

        GameEventListener wrapper = (info) => { _eventListener((T)info); };

        gameEventListeners[eventType].Add(wrapper);
    }

    public static void Unsub<T>(Action<T> _eventListener) where T : EventInfo
    {
        Type eventType = typeof(T);

        if (gameEventListeners.ContainsKey(eventType))
        {
            GameEventListener wrapper = (info) => { _eventListener((T)info); };
            gameEventListeners[eventType].Remove(wrapper);
        }
    }

    public static void TriggerGameEvent(EventInfo _info)
    {
        Type infoType = _info.GetType();

        if (gameEventListeners[infoType] == null || gameEventListeners[infoType].Count == 0)
            return;

        foreach (GameEventListener eventListener in gameEventListeners[infoType])
        {
            eventListener(_info);
        }
    }
}

public abstract class EventInfo
{
    public abstract int TargetID { get; }
}

public class AffectHPInfo : EventInfo
{
    public override int TargetID { get; }
    public int CallerID { get; }
    public bool IgnoreShield { get; }
    public ResourceEffector Effector { get; }

    public AffectHPInfo(int _targetID, int _callerID, bool _ignoreShield, ResourceEffector _effector)
    {
        TargetID = _targetID;
        CallerID = _callerID;
        IgnoreShield = _ignoreShield;
        Effector = _effector;
    }
}
