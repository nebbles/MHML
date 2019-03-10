using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disconnectCtrl : MonoBehaviour
{
    controller disconnect;

    public void onClick()
    {
        disconnect = FindObjectOfType<controller>();
        disconnect.disconnectBluetooth(false); // Not called through device forget, so don't delete the name & address
    }

    void Start () {}
	
	void Update () {}
}
