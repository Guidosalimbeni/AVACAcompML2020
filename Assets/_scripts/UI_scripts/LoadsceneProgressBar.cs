using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadsceneProgressBar : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image slider;
    public Text text;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));

    }

    IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            
            slider.fillAmount = progress;
            text.text = progress * 100 + " %";

            yield return null;
        }

    }
}
