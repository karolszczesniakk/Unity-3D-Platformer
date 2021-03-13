using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Vector3 respawnPosition;

    public GameObject deathEffect;

    public int currentCoins;

    public int levelEndMusic = 8;

    public string levelToLoad;

    public bool isRespawning;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        respawnPosition = PlayerController.instance.transform.position;
        addCoins(0);
 
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCo());
        HealthManager.instance.PlayerKilled();


    }

    public IEnumerator RespawnCo()
    {   
        
        PlayerController.instance.gameObject.SetActive(false);

        CameraController.instance.theCMBrain.enabled = false;


        UIManager.instance.fadeToBlack = true;

        Instantiate(deathEffect, PlayerController.instance.transform.position + new Vector3(0f, 1f, 0f), PlayerController.instance.transform.rotation);

        yield return new WaitForSeconds(2f);

        isRespawning = true;

        UIManager.instance.fadeFromBlack = true;

        HealthManager.instance.ResetHealth();

        PlayerController.instance.transform.position = respawnPosition;
        CameraController.instance.theCMBrain.enabled = true;
        
        PlayerController.instance.gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint;
    }

    public void addCoins(int coinsToAdd)
    {
        currentCoins += coinsToAdd;
        UIManager.instance.coinText.text = currentCoins.ToString();
    }

    public void PauseUnpause()
    {
        if(UIManager.instance.pauseScreen.activeInHierarchy)
        {
            UIManager.instance.pauseScreen.SetActive(false);
            Time.timeScale = 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            UIManager.instance.pauseScreen.SetActive(true);
            UIManager.instance.CloseOptions();
            Time.timeScale = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }



    }
    public IEnumerator LevelEndCo()
    {
        AudioManager.instance.PlayMusic(levelEndMusic);
        PlayerController.instance.bStopMove = true;
        yield return new WaitForSeconds(1f);
        UIManager.instance.fadeToBlack = true;

        yield return new WaitForSeconds(2.5f);
        Debug.Log("Level Ended");

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked",1);
        if(PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_coins"))
        {
            if(currentCoins > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_coins"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_coins", currentCoins);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_coins", currentCoins);
        }

        SceneManager.LoadScene(levelToLoad);
    
    }
}
