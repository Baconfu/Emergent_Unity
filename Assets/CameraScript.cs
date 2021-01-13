using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    GameObject follow;
    GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        follow = GameObject.Find("moon");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
