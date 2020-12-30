using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private float cameraYRotation;
    private float yRotation;
    private Player player;

    public Entity root;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraYRotation = GameObject.FindWithTag("MainCamera").transform.rotation.eulerAngles[1];
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        root = transform.root.parent.GetComponent<Entity>();
        Debug.Log(root);
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
        
    }

    public void Attach()
    {
        
    }
}
