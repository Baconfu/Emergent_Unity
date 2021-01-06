using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class BuildingGenerator : MonoBehaviour
{
    private string buildingInfoString;
    private GameObject BuildingContainer;
    private BuildingType building;

    public GameObject wall;
    public GameObject door;

    //Not used for now. 
    //However, if checking spelling becomes an issue in hardcoded jsons later on
    //(yes, it's a real possibility proven by our previous performances)
    //then building types would be denoted by ints not strings.
    public enum type
    {
        item = 0,
        structure = 1,
        building = 2,
    }


    // Start is called before the first frame update
    void Start()
    {
        door = Resources.Load("Door") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Generate(string info)
    {
        building = JsonConvert.DeserializeObject<BuildingType>(info);
        GameObject output = new GameObject(building.name, typeof(BoxCollider));

        //Debug.Log(new Vector3(building.x,building.y,building.z));

        //sets overall box collider of building
        output.GetComponent<BoxCollider>().size = new Vector3(building.x, building.y, building.z);
        output.GetComponent<BoxCollider>().isTrigger = true;

        //lets the script know the bounding box of entity
        //also acting as a trigger to detect player's presence
        output.AddComponent<Metaentity>();
        output.GetComponent<Metaentity>().occupation = output.GetComponent<BoxCollider>();
        //Debug.Log(output.GetComponent<BoxCollider>().size);

        

        if (building.type == "Building")
        {
            //generates walls on 4 directions
            //NB: the default rotation of wall is "inside to the positive z direction".
            output.GetComponent<Metaentity>().buildingType = building.type;

            for (int x = 0; x < building.x; x++)
            {
                for (int y = 0; y < building.y; y++)
                {
                    GameObject newWallBottom = Instantiate(wall, 
                        new Vector3(x - (float)building.x / 2 + 0.5f, y - (float)building.y/2, -(float)building.z / 2), 
                        Quaternion.Euler(0, 0, 0), 
                        output.transform);
                    newWallBottom.name = new Vector3Int(2, x, y).ToString();

                    GameObject newWallTop = Instantiate(wall,
                        new Vector3(x - (float)building.x / 2 + 0.5f, y - (float)building.y / 2, (float)building.z / 2), 
                        Quaternion.Euler(0, 180, 0), 
                        output.transform);
                    newWallTop.name = new Vector3Int(0, x, y).ToString();
                }
            }

            for (int z = 0; z < building.z; z++)
            {
                for (int y = 0; y < building.y; y++)
                {
                    GameObject newWall3 = Instantiate(wall, 
                        new Vector3(-(float)building.x/2, y - (float)building.y / 2, z + 0.5f - (float)building.z / 2), 
                        Quaternion.Euler(0, 90, 0), 
                        output.transform);
                    newWall3.name = new Vector3Int(3, z, y).ToString();

                    GameObject newWall1 = Instantiate(wall, 
                        new Vector3((float)building.x/2, y - (float)building.y / 2, z + 0.5f - (float)building.z/2), 
                        Quaternion.Euler(0, -90, 0), 
                        output.transform);
                    newWall1.name = new Vector3Int(1, z, y).ToString();
                }
            }



            //generating door(s) by 'digging out' holes on walls and instantiating the door
            if (building.haveDoor)
            {
                foreach(string str in building.positionOfDoor)
                {
                    //'position' here is special. 
                    //the first element is the wall number, 
                    //the two elements after is the position on the wall, starting from the bottom-lower-left corner.
                    Vector3 position = StringToVector3(str);
                    Destroy(output.transform.Find("(" + Mathf.RoundToInt(position[0]) + ", " + position[1] + ", " + position[2] + ")").gameObject);
                    Destroy(output.transform.Find("(" + Mathf.RoundToInt(position[0]) + ", " + position[1] + ", " + (position[2]+1) + ")").gameObject);

                    Vector3 doorPosition = Vector3.zero;
                    //GameObject newDoor = Instantiate(door, new Vector3, Quaternion.identity, wallList[Mathf.RoundToInt(position[0])]);
                }
                
            }

            Debug.Log("generated building" + new Vector3(building.x, building.y, building.z));
            return output;
        }

        return null;
    }

    Vector3 StringToVector3(string s)
    {
        // Remove the parentheses
        if (s.StartsWith("(") && s.EndsWith(")"))
        {
            s = s.Substring(1, s.Length - 2);
        }

        // split the items
        string[] sArray = s.Split(',');

        // store as a Vector3
        Vector3 output = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return output;
    }
}
