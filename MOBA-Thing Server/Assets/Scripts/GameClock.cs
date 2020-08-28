using System;
using System.Collections.Generic;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    public static float MatchTimeMilliseconds { get; private set; } = 0f;

    public static GameTime ClockTime { get { return new GameTime(MatchTimeMilliseconds); } }
    public struct GameTime
    {
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public GameTime(float _time)
        {
            Minutes = Mathf.RoundToInt(_time * 0.016f); //0.016f ~= 1f / 60f, faster for compiler
            Seconds = Mathf.RoundToInt(_time % 60f);
        }
    }

    private static List<Action<float>> toAdd = new List<Action<float>>();
    private static List<Action<float>> onUpdateMethods = new List<Action<float>>();
    private static List<Action<float>> toRemove = new List<Action<float>>();

    public static void AddEventToUpdate(Action<float> _toUpdate)
    {
        toAdd.Add(_toUpdate);
    }
    public static void RemoveEventFromUpdate(Action<float> _toUpdate)
    {
        toRemove.Add(_toUpdate);
    }

    void Update()
    {
        if (toAdd.Count > 0)
        {
            foreach (Action<float> action in toAdd)
            {
                onUpdateMethods.Add(action);
            }
            toAdd.Clear();
        }

        if (onUpdateMethods.Count > 0)
            foreach (Action<float> action in onUpdateMethods)
            {
                action(Time.fixedDeltaTime);
            }

        if (toRemove.Count > 0)
        {
            foreach (Action<float> action in toRemove)
            {
                onUpdateMethods.Remove(action);
            }
            toRemove.Clear();
        }
        MatchTimeMilliseconds += Time.fixedDeltaTime;
    }
}
