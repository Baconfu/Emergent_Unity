using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Entity
{
    float cameraYRotation;
    float yRotation;
    Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraYRotation = GameObject.FindWithTag("MainCamera").transform.rotation.eulerAngles[1];
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        yRotation = transform.rotation.eulerAngles[1];
        if (player.GetContext(Player.Context.Inside))
        {
            //put wall-hiding mechanics here
            //(hide wall when the player is inside AND camera is pointing at "outside" of wall)
        }
        
        if (proposed)
        {

        }
    }
}
