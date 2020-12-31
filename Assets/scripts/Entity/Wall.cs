using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Subentity
{
    private float cameraYRotation;
    private float yRotation;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        cameraYRotation = GameObject.FindWithTag("MainCamera").transform.rotation.eulerAngles[1];
        
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        
        yRotation = transform.rotation.eulerAngles[1];
        if (player.GetContext(Player.Context.Inside))
        {
            //put wall-hiding mechanics here
            //(hide wall when the player is inside AND camera is pointing at "outside" of wall)
        }

        if (root.proposed)
        {
            startColor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.color = Color.yellow;
            Debug.Log("tried to change color");
        }
        
    }

}
