using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Subentity
{
    private float cameraYRotation;
    private float yRotation;

    public bool hidden;
    public bool shouldBeHidden;
    
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        
        cameraYRotation = GameObject.FindWithTag("MainCamera").transform.rotation.eulerAngles[1];
        //Debug.Log(gameObject.name + "rotation: " + transform.rotation.eulerAngles[1]);

        hidden = false;
        
    }



    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (player.GetContext(Player.Context.Inside))
        {
            if (Quaternion.Angle(GameObject.FindWithTag("MainCamera").transform.rotation, transform.rotation) <= 90)
            {
                shouldBeHidden = true;
            }
            else
            {
                shouldBeHidden = false;
            }

        }
        else
        {
            shouldBeHidden = false;
        }

        if (hidden)
        {
            if (shouldBeHidden)
            {
                return;
            }
            else
            {
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                {
                    r.enabled = true;
                }
                hidden = false;
            }
        }
        else
        {
            if (shouldBeHidden)
            {
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                {
                    r.enabled = false;
                }
                hidden = true;
            }
            else
            {
                return;
            }
        }




    }

    void UpdateOcclusion()
    {
        

        /*yRotation = transform.rotation.eulerAngles[1];
        if (player.GetContext(Player.Context.Inside))
        {
            //put wall-hiding mechanics here
            //(hide wall when the player is inside AND camera is pointing at "outside" of wall)

            if (Quaternion.Angle(GameObject.FindWithTag("MainCamera").transform.rotation, transform.rotation) >= 90)
            {
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                {
                    r.enabled = true;
                }
            }
            if (Quaternion.Angle(GameObject.FindWithTag("MainCamera").transform.rotation, transform.rotation) <= 90)
            {
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                {
                    r.enabled = false;
                }
            }
        }*/
    }

}
