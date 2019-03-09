using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.


public class gsrClass
{
    public string bodySensorLocation;
    public int scl;
    public int timestamp;
    public int yValue; 
    

}


public class testGraph : MonoBehaviour
{
    Vector3 graphdata;
    int[] xval = new int[] { 1551987459, 1551988459, 1551989459, 1551990459, 1551991459, 1551992459, 1551993459, 1551994459, 1551995459, 1551996459 };
    int[] yval = new int[] { 66, 70, 39, 69, 28, 59, 60, 60, 70, 79 };
    public Vector3[] vertext;
    List<Vector3> mydata = new List<Vector3>();
    private LineRenderer line; 


    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Sprites/Default")); 
        lineSmoother(); 
    }
    public void lineSmoother()
        {
            var arraylength = xval.Length;

            for (var i = 0; i < arraylength; i++)
            {
                mydata.Add(new Vector3(xval[i], yval[i], 0));
                Debug.Log(mydata);
            }

        vertext = mydata.ToArray(); 
        //Vector3[] smoothed = SmoothLine(vertext, 10);
        //print(smoothed);

        }

    private void Update()
    {
        line.SetPositions(vertext);
    }

}








