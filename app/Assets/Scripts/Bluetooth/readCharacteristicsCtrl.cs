using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readCharacteristicsCtrl : MonoBehaviour {

    controller read;

    public void onClick()
    {
        read = FindObjectOfType<controller>();
        read.callReadCharacteristics(); // Setting this parameter to true means the update loop is entered and the 
    }

    void Start() { }

    void Update() { }
}
