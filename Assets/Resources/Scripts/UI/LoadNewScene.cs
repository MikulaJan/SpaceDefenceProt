using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNewScene : MonoBehaviour
{
    public GameObject LoadingPanel;
    public Image LoadingBar;

    public void LoadANewScene(string scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(string scene)
    {
        LoadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;
        float targetTime = 2f; // 2 seconds to load

        while (elapsedTime < targetTime)
        {
            elapsedTime += Time.deltaTime;
            float progressValue = Mathf.Clamp01(elapsedTime / targetTime);

            LoadingBar.fillAmount = progressValue;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
