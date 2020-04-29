using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleLevelSelect : MonoBehaviour
{
    private bool m_Running = false;

    public void LoadLevel(int levelSelect)
    {
        Debug.Log("LoadLevel");
        StartCoroutine(LoadYourAsyncScene(levelSelect));
    }

    private IEnumerator LoadYourAsyncScene(int levelSelect)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = new AsyncOperation();

        if (!m_Running)
        {
            if (levelSelect == 1)
            {
                m_Running = true;
                asyncLoad = SceneManager.LoadSceneAsync("Assets/Scenes/Scenarios/Scenario 2/Level 1.unity");
            }
            else if (levelSelect == 2)
            {
                m_Running = true;
                asyncLoad = SceneManager.LoadSceneAsync("Assets/Scenes/Scenarios/Scenario 3/Level 2.unity");
            }
            else if (levelSelect == 3)
            {
                m_Running = true;
                asyncLoad = SceneManager.LoadSceneAsync("Assets/Scenes/Main Menu.unity");
            }
        }

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            m_Running = false;
            yield return null;
        }
    }
}
