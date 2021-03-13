using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LSLevelEntry : MonoBehaviour
{

    public string levelName, levelToCheck, displayName;

    public GameObject mapPointActive, mapPointInactive;

    private bool canLoadLevel, levelUnlocked, levelLoading;
    // Start is called before the first frame update




    void Start()
    {
        if (PlayerPrefs.GetInt(levelToCheck + "_unlocked") == 1 || levelToCheck == "")
        {
            mapPointActive.SetActive(true);
            mapPointInactive.SetActive(false);
            levelUnlocked = true;

        }
        else
        {
            mapPointActive.SetActive(false);
            mapPointInactive.SetActive(true);
            levelUnlocked=false;
        }
        if(PlayerPrefs.GetString("CurrentLevel") == levelName)
        {
            LSResetPosition.instance.respawnPosition = transform.position;
            LSResetPosition.instance.ResetPosition();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && canLoadLevel && levelUnlocked && !levelLoading)
        {
            levelLoading = true;
            StartCoroutine(LevelLoadCo());


        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canLoadLevel = true;

            LSUIManager.instance.lNamePanel.SetActive(true);
            LSUIManager.instance.lNameText.text = displayName;

            if(PlayerPrefs.HasKey(levelName + "_coins") && PlayerPrefs.GetInt(levelName + "_coins") != 0)
            {
                LSUIManager.instance.coinsText.text = PlayerPrefs.GetInt(levelName + "_coins").ToString();
            }else
            {
                LSUIManager.instance.coinsText.text = "???";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            canLoadLevel = false;
            LSUIManager.instance.lNamePanel.SetActive(false);

        }
    }

   public IEnumerator LevelLoadCo()
    {
        
        PlayerController.instance.bStopMove = true;
        UIManager.instance.fadeToBlack = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(levelName);
        
        PlayerPrefs.SetString("CurrentLevel", levelName);

    }
}

