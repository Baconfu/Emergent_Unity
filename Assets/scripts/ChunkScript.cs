using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Linq;


public class ChunkScript : MonoBehaviour
{

    private GameObject[] spaces = new GameObject[2000];


    public ChunkScript(Vector3 v)
    {
        Constants constant = new Constants();

        string path = constant.data_path + "chunk_" + ((int)v.x).ToString() + "_" + ((int)v.y).ToString() + ".json";// + "_" + ((int)v.z).ToString();



        File chunkFile = new File(path);

        string data = chunkFile.Data();

        //Debug.Log(data);

        JsonArrayParser parser = new JsonArrayParser(data);

        List<UnitSpaceInfo> buffer = new List<UnitSpaceInfo>();
        
        //Debug.Log(parser.ar.Count);
        foreach(string s in parser.ar){
            buffer.Add(JsonUtility.FromJson<UnitSpaceInfo>(s));
        }

        //Debug.Log(buffer.Count);
        //Debug.Log(buffer[0].type);

        GameObject root = GameObject.Find("UnitSpace");
        int iterator = 0;
        for(int z=0; z<5; z++){
            for(int y=0; y<Constants.chunk_width_tiles; y++){
                for(int x=0; x<Constants.chunk_width_tiles; x++){
                    spaces[iterator] = UnityEngine.Object.Instantiate(GameObject.Find("UnitSpace"));
                    spaces[iterator].transform.position = new Vector3(x,z,y);
                    spaces[iterator].transform.SetParent(GameObject.Find("UnitSpaceCollection").transform, false);
                    if(buffer[iterator].type == "terrain"){
                        spaces[iterator].GetComponent<UnitSpaceScript>().SetUnitSpaceType(UnitSpaceScript.UnitSpaceType.rock);                                                
                    }
                    if(buffer[iterator].type == "air"){
                        spaces[iterator].GetComponent<UnitSpaceScript>().SetUnitSpaceType(UnitSpaceScript.UnitSpaceType.air);                                                
                    }
                    iterator++;
                }
            }
        }


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
