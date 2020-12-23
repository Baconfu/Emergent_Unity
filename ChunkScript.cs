using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Linq;


public class ChunkScript : MonoBehaviour
{

    private GameObject[] spaces = new GameObject[(int)Math.Pow(Constants.chunk_width_tiles,3)];


    public ChunkScript(Vector3 v)
    {
        Constants constant = new Constants();

        string path = constant.data_path + "chunk_" + ((int)v.x).ToString() + "_" + ((int)v.y).ToString() + ".json";// + "_" + ((int)v.z).ToString();


        File chunkFile = new File(path);

        string data = chunkFile.Data();

        Debug.Log(data);

        var temp = JsonUtility.FromJson<List<UnitSpaceLoader>>(data);
        Debug.Log(temp.ToList().Count);

        GameObject root = GameObject.Find("UnitSpace");
        for(int z=0; z<Constants.chunk_width_tiles; z++){
            for(int y=0; y<Constants.chunk_width_tiles; y++){
                for(int x=0; x<Constants.chunk_width_tiles; x++){
                    
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
