using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpaceScript : MonoBehaviour
{
    private int type;
    public int Type { get => type; set => type = value; }

    public bool isCollidable()
    {
        if(type < 100){
            return false;
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
