using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public float m_GamePoints = 0.0f;
    public bool m_DidPlayerWin = false;
    public bool m_IsGameOver = false;

    private void OnDisable()
    {
        m_GamePoints = 0.0f;
        m_DidPlayerWin = false;
        m_IsGameOver = false;
    }

    public virtual string timerFormat (int timePassed)
    {
        string time;

        if (timePassed <=9)
        {
            time = "00:0" + timePassed.ToString();
        }
        else
        {
            time = "00:" + timePassed.ToString();
        }
        
        return time;
    }

    public void ResetGameState()
    {
        m_GamePoints = 0.0f;
        m_DidPlayerWin = false;
        m_IsGameOver = false;
    }
}
