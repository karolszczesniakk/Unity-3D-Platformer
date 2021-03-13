using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingObjects : MonoBehaviour
{
    public ButtonController theButton;
    public GameObject disappearingObject;
    public bool revealWhenPressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(theButton.isPressed)
        {
            disappearingObject.SetActive(revealWhenPressed);

        }else
        {
            disappearingObject.SetActive(!revealWhenPressed);
        }
    }
}
