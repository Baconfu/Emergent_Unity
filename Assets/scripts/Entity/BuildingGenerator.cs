using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    private string buildingInfoString;
    private GameObject BuildingContainer;
    private BuildingType building;

    public GameObject wallContainer;


    // Start is called before the first frame update
    void Start()
    {
        //wall = Resources.Load("Wall");
        //Debug.Log(wallContainer);
        //Debug.Log("initialised world scripts");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Generate(string info)
    {
        building = JsonUtility.FromJson<BuildingType>(info);
        GameObject output = new GameObject(building.name, typeof(BoxCollider));

        //Debug.Log(new Vector3(building.x,building.y,building.z));

        //sets overall box collider of building
        output.GetComponent<BoxCollider>().size = new Vector3(building.x, building.y, building.z);
        output.GetComponent<BoxCollider>().isTrigger = true;
        output.AddComponent<Entity>();
        Debug.Log(output.GetComponent<BoxCollider>().size);

        //generates walls on 4 directions
        //NB: the default rotation of wall is "inside to the positive z direction".


        GameObject wall0 = new GameObject("Wall0");
        GameObject wall1 = new GameObject("Wall1");
        GameObject wall2 = new GameObject("Wall2");
        GameObject wall3 = new GameObject("Wall3");

        wall0.transform.SetParent(output.transform);
        wall1.transform.SetParent(output.transform);
        wall2.transform.SetParent(output.transform);
        wall3.transform.SetParent(output.transform);


        
        
        for (int x = 0; x<building.x; x++)
        {
            for (int y = 0; y < building.y; y++)
            {
                //Debug.Log(wallContainer);
                GameObject newWall2 = Instantiate(wallContainer, wall2.transform);
                newWall2.transform.position = new Vector3(x + 0.5f, y, 0);
                newWall2.transform.rotation = Quaternion.Euler(0, 0, 0);
                //newWall.transform.SetParent(wall0.transform);

                //newWall.GetComponent<Wall>().Attach();

                GameObject newWall0 = Instantiate(wallContainer, wall0.transform);
                newWall0.transform.position = new Vector3(x + 0.5f, y, building.z);
                newWall0.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            
        }

        for (int z = 0; z < building.z; z++)
        {
            for (int y = 0; y < building.y; y++)
            {
                GameObject newWall3 = Instantiate(wallContainer, wall3.transform);
                newWall3.transform.position = new Vector3(0, y, z + 0.5f);
                newWall3.transform.rotation = Quaternion.Euler(0, 90, 0);

                GameObject newWall1 = Instantiate(wallContainer, wall1.transform);
                newWall1.transform.position = new Vector3(building.x, y, z + 0.5f);
                newWall1.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }



        wall0.transform.position = new Vector3((float)-building.x / 2, (float)-building.y / 2, (float)-building.z / 2);
        wall1.transform.position = new Vector3((float)-building.x / 2, (float)-building.y / 2, (float)-building.z / 2);
        wall2.transform.position = new Vector3((float)-building.x / 2, (float)-building.y / 2, (float)-building.z / 2);
        wall3.transform.position = new Vector3((float)-building.x / 2, (float)-building.y / 2, (float)-building.z / 2);

        Debug.Log("generated building" + new Vector3(building.x, building.y, building.z));
        return output;
    }
}
