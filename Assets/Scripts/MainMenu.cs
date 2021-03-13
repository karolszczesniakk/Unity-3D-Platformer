using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public string[] levelNames;

    public GameObject continueButton;

    public string firstLevel, levelSelect;
    void Start()
    {
        if(PlayerPrefs.HasKey("Continue"))
        {
            continueButton.SetActive(true);
        }else
        {
            ResetProgress();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(firstLevel);
        PlayerPrefs.SetString("CurrentLevel", firstLevel);
        PlayerPrefs.SetInt("Continue", 0);
        ResetProgress();
    }

    public void Continue()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetProgress()
    {
        for(int i = 0; i < levelNames.Length; i++)
        {
            PlayerPrefs.SetInt(levelNames[i] + "_unlocked", 0);
            PlayerPrefs.SetInt(levelNames[i] + "_coins", 0);
        }
        PlayerPrefs.SetInt("LevelBoss_unlocked", 0);

    }

}


