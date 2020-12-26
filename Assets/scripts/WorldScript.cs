using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class WorldScript : MonoBehaviour
{

    private GameObject[] spaces = new GameObject[5];
    
    

    // Start is called before the first frame update
    void Start()
    {
        /*
        for(int i=0; i<5; i++){
            spaces[i] = UnityEngine.Object.Instantiate(GameObject.Find("UnitSpace"));
            spaces[i].transform.position = new Vector3(i*2+2,0,0);
        }*/

        ChunkScript c = new ChunkScript(new Vector3(0,0,0));


        UnityEngine.Object.Instantiate(Resources.Load("Ladder"), new Vector3(0,1,0), Quaternion.identity);

        

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public static Vector3 EmptyUnitSpaceOnCursor()
    {
        //this one only works on terrain blocks, NOT ON ENTITIES
        RaycastHit hit;
        Physics.Raycast(GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit);
        Vector3 hitColliderPosition = hit.transform.gameObject.GetComponent<Transform>().position;

        if(hit.transform.gameObject.GetComponent<UnitSpaceScript>().type >= 100)
        {
            if (hit.point.y < hit.transform.gameObject.transform.position.y + 1)
            {

                if (hit.point.x == hit.transform.gameObject.transform.position.x)
                {
                    //west
                    return hitColliderPosition + new Vector3(-1, 0, 0);
                }

                if (hit.point.x == hit.transform.gameObject.transform.position.x + 1)
                {
                    //east
                    return hitColliderPosition + new Vector3(1, 0, 0);
                }

                if (hit.point.z == hit.transform.gameObject.transform.position.z)
                {
                    //south
                    return hitColliderPosition + new Vector3(0, 0, -1);
                }

                if (hit.point.z == hit.transform.gameObject.transform.position.z + 1)
                {
                    //north
                    return hitColliderPosition + new Vector3(0, 0, 1);
                }

            }
            else
            {
                //top
                return hitColliderPosition + new Vector3(0, 1, 0);
            }
        }       

        return Vector3.zero;
    }




    void loadChunk()
    {

    }
}
