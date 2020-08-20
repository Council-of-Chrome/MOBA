using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//plug these into monobehaviour controller classes to isolate work
public class ExperienceManager
{
    public int Level { get; private set; }

    public int MaxExperience { get; private set; }
    public int CurrentExperience { get; private set; }

    public delegate void ExperienceHandler(int _entityID, int _value); //<-- TODO: make more verbose than just 'id', contain team etc
    public static ExperienceHandler OnChangeExperience; //invoke this delegate to modify experience from some system event

    private int EntityID { get; }
    private Func<int, int> ExperienceScaler { get; }

    public ExperienceManager(int _entityID, int _baseXP, Func<int, int> _xpScaler)
    {
        EntityID = _entityID;

        MaxExperience = _baseXP;
        CurrentExperience = 0;

        ExperienceScaler = _xpScaler;

        OnChangeExperience += Modify;
    }
    //destructor incase something weird terminates the class
    ~ExperienceManager()
    {
        OnChangeExperience -= Modify;
    }

    private void Modify(int _entityID, int _xpValue)
    {
        if (_entityID != EntityID)
            return;

        TestLevelRecursive(
            Mathf.Clamp(CurrentExperience + _xpValue, 0, MaxExperience)
            );
    }
    //levelup,  passing remainder into next level and test again
    private void TestLevelRecursive(int _xpValue)
    {
        if (_xpValue >= MaxExperience)
        {
            Level = Mathf.Clamp(Level++, 0, 21);
            ExperienceScaler(Level);
            CurrentExperience = (CurrentExperience + _xpValue) - MaxExperience;
            TestLevelRecursive(CurrentExperience);
        }
    }
}
