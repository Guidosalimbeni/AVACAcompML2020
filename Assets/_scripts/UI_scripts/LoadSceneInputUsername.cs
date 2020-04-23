using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LoadSceneInputUsername : MonoBehaviour
{
    public int scene;
    public TMP_InputField usernameInputField;
    static public string username;

    private void Awake()
    {
        //usernameInputField = GetComponent<TMP_InputField>();
        //usernameInputField.onEndEdit.AddListener(TryLoadScene);
    }

    public void SubmitAndStartGame()
    {
        usernameInputField.text = usernameInputField.text;
        //Debug.Log(usernameInputField.text);
        username = usernameInputField.text;
        if (username != "")
        {
            SceneManager.LoadSceneAsync(scene);
        }

    }

    /*
    private void TryLoadScene(string username)
    {
        Debug.Log(username);
        SceneManager.LoadSceneAsync(scene);
    }
    */
}
