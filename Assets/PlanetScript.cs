using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{

    int iterator = 0;
    GameObject planet;
    // Start is called before the first frame update
    void Start()
    {
        planet = GameObject.Find("PlanetObject");
    }

    // Update is called once per frame
    void Update()
    {
        iterator++;


        float angle = (float)iterator * Mathf.PI/18000;
        planet.transform.position = new Vector3(Mathf.Cos(angle) * 80,0,Mathf.Sin(angle) * 80);


        planet.transform.Rotate(new Vector3(0,0.1f,0));
    }
}
