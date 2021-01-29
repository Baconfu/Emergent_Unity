using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Linq;


public class Chunk : MonoBehaviour
{

    Mesh mesh;
    GameObject meshObject;

    Vector3[] vertices;
    Vector3[] rawVertices;
    int[] triangles;

    private GameObject[] spaces = new GameObject[2000];
    private Vector3Int position;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3Int(Mathf.RoundToInt(transform.position[0]),
            Mathf.RoundToInt(transform.position[1]),
            Mathf.RoundToInt(transform.position[2]));

        Constants constant = new Constants();

        string path = constant.data_path + "/chunks" + "/chunk_" + 
            ((int)position.x).ToString() + "_" + 
            ((int)position.y).ToString() + 
            ".json";
        // + "_" + ((int)v.z).ToString();

        File chunkFile = new File(path);

        string data = chunkFile.Data();

        //Debug.Log(data);

        JsonArrayParser parser = new JsonArrayParser(data);

        List<UnitSpaceInfo> buffer = new List<UnitSpaceInfo>();

        //Debug.Log(parser.ar.Count);
        foreach (string s in parser.ar)
        {
            buffer.Add(JsonUtility.FromJson<UnitSpaceInfo>(s));
        }

        //Debug.Log(buffer.Count);
        //Debug.Log(buffer[0].type);

        GameObject root = new GameObject(position.ToString());
        root.transform.position = position;

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<Renderer>();
        mesh = GetComponent<MeshFilter>().mesh;

        vertices = new Vector3[(int)Mathf.Pow(Constants.chunk_width_tiles + 1, 2)];
        //without the 'sides' (the extra row & column of vertices to make chunk meshes connect together)
        rawVertices = new Vector3[(int)Mathf.Pow(Constants.chunk_width_tiles, 2)];

        CreateMesh();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateMesh()
    {
        
        mesh.Clear();
        
        float[] heightArray = new float[(int)Mathf.Pow(Constants.chunk_width_tiles, 2)];

        for(int i=0; i<heightArray.Length; i++)
        {
            heightArray[i] = float.Parse(
                System.IO.File.ReadAllText(Application.dataPath + "/Resources/export.txt").
                Split(',')[i]);
        }

        //meshObject = new GameObject("Ground", typeof(MeshFilter), typeof(Renderer));

        for (int x = 0; x < Constants.chunk_width_tiles; x++)
        {
            for (int z = 0; z < Constants.chunk_width_tiles; z++)
            {
                rawVertices[x + z * Constants.chunk_width_tiles].x = x;
                rawVertices[x + z * Constants.chunk_width_tiles].z = z;


                rawVertices[x + z * Constants.chunk_width_tiles].y = heightArray[x + z * Constants.chunk_width_tiles];
            }
        }


        //set 'mesh' symbol's vertices and triangles to be lists generated above




        //meshObject.GetComponent<MeshFilter>().mesh = mesh;


        /*int iterator = 0;
        for (int z = 0; z < 5; z++)
        {
            for (int y = 0; y < Constants.chunk_width_tiles; y++)
            {
                for (int x = 0; x < Constants.chunk_width_tiles; x++)
                {
                    spaces[iterator] = Instantiate(Resources.Load("UnitSpace") as GameObject, new Vector3(x, z, y), Quaternion.identity, root.transform);
                    spaces[iterator].name = "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";

                    if (buffer[iterator].type == "terrain")
                    {
                        spaces[iterator].GetComponent<UnitSpace>().SetUnitSpaceType(UnitSpace.UnitSpaceType.rock);
                    }
                    if (buffer[iterator].type == "air")
                    {
                        spaces[iterator].GetComponent<UnitSpace>().SetUnitSpaceType(UnitSpace.UnitSpaceType.air);
                    }
                    iterator++;
                }
            }
        }
        root.transform.SetParent(GameObject.Find("UnitSpaceCollection").transform);
        */
    }
}
