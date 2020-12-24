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
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void loadChunk()
    {

    }
}
