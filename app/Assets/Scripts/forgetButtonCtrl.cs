using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forgetButtonCtrl : MonoBehaviour {

    controller forget;

    public void onClick()
    {
        forget = FindObjectOfType<controller>();
        forget.forgetDevice();
    }

    void Start() { }

    void Update() { }
}
