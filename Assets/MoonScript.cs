using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject moon;

    int iterator = 0;
    void Start()
    {
        moon = GameObject.Find("MoonObject");
    }

    // Update is called once per frame
    void Update()
    {
        iterator++;

        float angle = (float)iterator * Mathf.PI/1800;
        moon.transform.localPosition = new Vector3(Mathf.Cos(angle) * 5,0,Mathf.Sin(angle) * 5);
    }
}
