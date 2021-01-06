using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunScript : MonoBehaviour
{

    private GameObject sun;
    // Start is called before the first frame update
    void Start()
    {
        sun = GameObject.Find("SunObject");
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
