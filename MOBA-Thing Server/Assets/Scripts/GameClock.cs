using System;
using System.Collections.Generic;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    private const float MINUTE_MULT = 1f / 60f;

    public static float MatchTimeMilliseconds { get; private set; } = 0f;

    public static GameTime ClockTime { get { return new GameTime(MatchTimeMilliseconds); } }
    public struct GameTime
    {
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public GameTime(float _time)
        {
            Minutes = Mathf.FloorToInt(_time * MINUTE_MULT);
            Seconds = Mathf.FloorToInt(_time % 60f);
        }

        public override string ToString()
        {
            return $"Game time: {Minutes.ToString("00")}:{Seconds.ToString("00")}";
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

    void FixedUpdate()
    {
        MatchTimeMilliseconds += Time.fixedDeltaTime;
        //Debug.Log(MatchTimeMilliseconds);
        Debug.Log(ClockTime.ToString() + ": " + MatchTimeMilliseconds);

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
    }
}
