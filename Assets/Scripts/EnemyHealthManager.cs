using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{


    public int maxHealth = 1;
    private int currentHealth;

    public int deathSound;

    public GameObject deathEffect, itemToDrop;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            AudioManager.instance.playSFX(deathSound);

            Destroy(gameObject);

            PlayerController.instance.Bounce();

            Instantiate(deathEffect, transform.position + new Vector3(0f, 1f, 0f), transform.rotation);
            Instantiate(itemToDrop, transform.position + new Vector3(0f, 0.5f, 0f), transform.rotation);

        }
    }

}

