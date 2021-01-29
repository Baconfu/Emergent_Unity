using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metaentity : Entity
{
    public int hp;

    public string buildingType;

    public Collider occupation;

    public string originalName;

    // Start is called before the first frame update
    public override void Awake()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }


    public override void SetProposed(bool desired)
    {
        base.SetProposed(desired);

        if (desired)
        {
            Debug.Log("name changed");
            originalName = gameObject.name;
            gameObject.name += "(Proposed)";
        }
        else
        {
            if (originalName.Length != 0)
            {
                gameObject.name = originalName;
            }
            
        }

        

    }
}
