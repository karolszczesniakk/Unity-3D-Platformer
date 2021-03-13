using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;
    public int currentHealth, maxHealth;
    public float invicibleLength = 2f;
    private float invicibleCounter;

    public Sprite[] healthBarImages;

    public int soundToPlay;


    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        ResetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if(invicibleCounter>0)
        {
            invicibleCounter -= Time.deltaTime;

            for (int i = 0; i < PlayerController.instance.playerPieces.Length; i++)
            {
                if (Mathf.Floor(invicibleCounter * 5f) % 2 == 0)
                {
                    PlayerController.instance.playerPieces[i].SetActive(true);
                }
                else
                {
                    PlayerController.instance.playerPieces[i].SetActive(false);
                }

                if(invicibleCounter<=0)
                {
                    PlayerController.instance.playerPieces[i].SetActive(true);
                }
            }

        }
    }

    public void Hurt()
    {
        if (invicibleCounter <= 0)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {

                currentHealth = 0;
                GameManager.instance.Respawn();
                AudioManager.instance.playSFX(soundToPlay);
            }
            else
            {
                PlayerController.instance.Knockback();
                invicibleCounter = invicibleLength;
                AudioManager.instance.playSFX(soundToPlay);

            }
            UpdateUI();
        }
    }

    public void ResetHealth()
    {
        UIManager.instance.healthImage.enabled = true;
        currentHealth = maxHealth;
        UpdateUI();
    }


    public void AddHealth(int amountToHeal)
    {
        currentHealth += amountToHeal;
        if(currentHealth>maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateUI();
    }


    public void UpdateUI()
    {
        UIManager.instance.healthText.text = currentHealth.ToString();

        switch(currentHealth)
        {
            case 5:
                UIManager.instance.healthImage.sprite = healthBarImages[4];
                break;
            case 4:
                UIManager.instance.healthImage.sprite = healthBarImages[3];
                break;
            case 3:
                UIManager.instance.healthImage.sprite = healthBarImages[2];
                break;
            case 2:
                UIManager.instance.healthImage.sprite = healthBarImages[1];
                break;
            case 1:
                UIManager.instance.healthImage.sprite = healthBarImages[0];
                break;
            case 0:
                UIManager.instance.healthImage.enabled = false;
                break;


        }

       
    }
    public void PlayerKilled()
    {
        currentHealth = 0;
        UpdateUI();
        
    }
}


