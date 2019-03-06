using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disconnectCtrl : MonoBehaviour
{
    controller disconnect;

    public void onClick()
    {
        disconnect = FindObjectOfType<controller>();
        disconnect.disconnectBluetooth();
    }

    void Start () {}
	
	void Update () {}
}
