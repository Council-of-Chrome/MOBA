using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;
    private void OnEnable()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            throw new System.Exception("GameManager Instance already claimed.");
        }
        Instance = this;
    }
    #endregion   //may not actually need this

    //public GameTeam BlueTeam;
    //public GameTeam RedTeam;

    public static float MatchTimeMilliseconds { get; private set; } = 0f;

    public static GameClock GameTime { get { return new GameClock(MatchTimeMilliseconds); } }
    public struct GameClock
    {
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public GameClock(float _time)
        {
            Minutes = Mathf.RoundToInt(_time * 0.016f); //0.016f ~= 1f / 60f, faster for compiler
            Seconds = Mathf.RoundToInt(_time % 60f);
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        //do shit here probably

        MatchTimeMilliseconds += Time.unscaledDeltaTime;
    }
}
