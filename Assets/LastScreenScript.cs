using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LastScreenScript : MonoBehaviour
{
    private int prevSceneID;
    public Text uiText;

    private void Start()
    {
        prevSceneID = SceneManager.GetActiveScene().buildIndex - 1;
        uiText.text = PlayerPrefs.GetFloat(prevSceneID.ToString()).ToString();
    }


    void Update()
    {
        if (Input.GetKey("space"))
        {
            SceneManager.LoadScene("StartScreen");
        }

        if (Input.GetKey("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
