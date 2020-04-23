using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    private Button button;
    public int scene;

    static public string username;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TryLoadScene);
    }

    private void TryLoadScene()
    {
        SceneManager.LoadSceneAsync(scene);
    }
}
