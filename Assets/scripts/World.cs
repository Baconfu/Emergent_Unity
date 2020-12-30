using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class World : MonoBehaviour
{

    private Chunk c;

    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log(Application.dataPath);


        c = Resources.Load("Chunk") as Chunk;
        Instantiate(c, new Vector3(0, 0, 0), Quaternion.identity);
        Debug.Log(c);
        Camera.onPostRender += OnPostRenderCallback;

        Instantiate(Resources.Load("Ladder"), new Vector3(0,1,0), Quaternion.identity);

        

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
    public static Vector3 EmptyUnitSpaceOnCursor(bool returnBottomCenter)
    {
        //this method returns the BOTTOM centre point.
        if (returnBottomCenter)
        {
            return EmptyUnitSpaceOnCursor() + new Vector3(0.5f, 0, 0.5f);
        }
        return Vector3.zero;
    }
    public static bool KeyPressed(KeyCode target)
    {
        return Input.GetKeyDown(target);
    }
    void OnPostRenderCallback(Camera cam)
    {
        if (Application.isEditor)
        {
            //"Temp" tag means destroyed after each rendering i.e. only appearing for one frame
            GameObject[] toBeDestroyed = GameObject.FindGameObjectsWithTag("Temp");
            for (int i=0; i < toBeDestroyed.Length; i++)
            {
                Destroy(GameObject.FindGameObjectsWithTag("Temp")[i]);
            }
            //Debug.Log("tried to delete yes!!");
        }
        //Debug.Log("tried to delete");
    }


    void loadChunk()
    {

    }
}
