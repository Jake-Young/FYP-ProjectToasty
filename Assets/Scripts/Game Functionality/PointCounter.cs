using UnityEngine;
using TMPro;

public class PointCounter : MonoBehaviour
{
    public MachineLearningManager m_MachineLearningManager;
    public TMP_Text m_GamePointsUI;

    private float m_PreviosScore = 0;

    // Update is called once per frame
    private void Update()
    {
        float points = m_MachineLearningManager.m_GameState.m_GamePoints;

        if (points > m_PreviosScore)
        {
            m_GamePointsUI.text = points + " Points";
            m_PreviosScore = points;
        }
    }
}
