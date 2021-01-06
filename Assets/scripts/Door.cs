using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Subentity
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.Find("Flip").GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    new void Update()
    {
        
    }
}
