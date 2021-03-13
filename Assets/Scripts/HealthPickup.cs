using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healAmount;
    public bool isFullHeal;

    public int soundToPlay;

    public GameObject healthPickupEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            Instantiate(healthPickupEffect, gameObject.transform.position + new Vector3(0f, -0.2f, 0f), gameObject.transform.rotation);
            Destroy(gameObject);
            AudioManager.instance.playSFX(soundToPlay);

            if (isFullHeal)
            {
                HealthManager.instance.ResetHealth();
            }else
            {
                HealthManager.instance.AddHealth(healAmount);
            }
        }
    }
}
