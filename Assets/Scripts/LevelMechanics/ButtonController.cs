using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    public bool isPressed, isOnOff;
    public Transform button, buttonDown;
    private Vector3 buttonUp;
    

    // Start is called before the first frame update
    void Start()
    {
        buttonUp = button.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (isOnOff)
            {


                if (isPressed)
                {
                    button.position = buttonUp;
                    isPressed = false;

                }
                else
                {
                    isPressed = true;
                    button.position = buttonDown.position;

                }
            }else
            {
                if(!isPressed)
                {
                    isPressed = true;
                    button.position = buttonDown.position;
                }
            }
            
        }
    }
}
