using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSResetPosition : MonoBehaviour
{

    public static LSResetPosition instance;
    public Vector3 respawnPosition;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        PlayerController.instance.gameObject.SetActive(false);
        PlayerController.instance.transform.position = respawnPosition;
        PlayerController.instance.gameObject.SetActive(true);
    }    
}


