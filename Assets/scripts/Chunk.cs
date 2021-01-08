using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Linq;


public class Chunk : MonoBehaviour
{
    private GameObject[] spaces = new GameObject[4096];
    private Vector3Int position;
    private World world;

    // Start is called before the first frame update
    void Awake()
    {
        world = GameObject.Find("World").GetComponent<World>();
        
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

        gameObject.name = position.ToString();
        transform.position = position;
        UnityEngine.Random.InitState(3);

        int iterator = 0;
        for (int z = 0; z < Constants.chunk_width; z++)
        {
            for (int y = 0; y < Constants.chunk_width; y++)
            {
                for (int x = 0; x < Constants.chunk_width; x++)
                {

                    spaces[iterator] = Instantiate(Resources.Load("UnitSpace") as GameObject, new Vector3(x, z, y), Quaternion.identity, transform);
                    spaces[iterator].name = "(" + x.ToString() + ", " + z.ToString() + ", " + y.ToString() + ")";
                    if (z <= 8)
                    {
                        spaces[iterator].GetComponent<UnitSpace>().SetUnitSpaceType(UnitSpace.UnitSpaceType.rock);
                    }

                    if (z < 12 && z > 8)
                    {
                        if (spaces[(z - 1) * Constants.chunk_width * Constants.chunk_width + y * Constants.chunk_width + x].GetComponent<UnitSpace>().GetUnitSpaceType() == UnitSpace.UnitSpaceType.rock
                            && UnityEngine.Random.Range(0, 9) < 5)
                        {
                            spaces[iterator].GetComponent<UnitSpace>().SetUnitSpaceType(UnitSpace.UnitSpaceType.rock);
                        }
                        else
                        {
                            spaces[iterator].GetComponent<UnitSpace>().SetUnitSpaceType(UnitSpace.UnitSpaceType.air);

                        }
                    }
                    if (z >= 12)
                    {
                        spaces[iterator].GetComponent<UnitSpace>().SetUnitSpaceType(UnitSpace.UnitSpaceType.air);

                    }
                    /*
                    if (buffer[iterator].type == "terrain")
                    {
                        spaces[iterator].GetComponent<UnitSpace>().SetUnitSpaceType(UnitSpace.UnitSpaceType.rock);
                    }
                    if (buffer[iterator].type == "air")
                    {
                        spaces[iterator].GetComponent<UnitSpace>().SetUnitSpaceType(UnitSpace.UnitSpaceType.air);
                    }*/
                    iterator++;
                }
            }
        }
        transform.SetParent(GameObject.Find("UnitSpaceCollection").transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load(Vector3 v)
    {
        
    }

    GameObject GetUnitSpace (int index)
    {
        return spaces[index];
    }

    GameObject GetUnitSpace(Vector3Int pos)
    {
        return spaces[pos[0] + pos[1] * Constants.chunk_width + pos[2] * (int)Mathf.Pow(Constants.chunk_width, 2)];
    }


}
