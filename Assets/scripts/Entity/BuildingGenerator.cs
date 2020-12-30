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
        Debug.Log(wallContainer);
        Debug.Log("initialised world scripts");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Generate(string info)
    {
        building = JsonUtility.FromJson<BuildingType>(info);
        GameObject output = new GameObject("LivingQuarter", typeof(BoxCollider));

        //Debug.Log(new Vector3(building.x,building.y,building.z));

        //sets overall box collider of building
        output.GetComponent<BoxCollider>().size = new Vector3(building.x, building.y, building.z);
        output.AddComponent<Entity>();
        Debug.Log(output.GetComponent<BoxCollider>().size);

        //generates walls on 4 directions
        //NB: the default rotation of wall is "inside to the positive z direction".
        GameObject wall0 = new GameObject("Wall0");
        wall0.transform.SetParent(output.transform);
        for (int x = 0; x<building.x; x++)
        {
            for (int y = 0; y < building.y; y++)
            {
                //Debug.Log(wallContainer);
                GameObject newWall = Instantiate(wallContainer, wall0.transform);
                newWall.transform.position = new Vector3(x + 0.5f, y, 0);
                newWall.transform.rotation = Quaternion.Euler(0, 0, 0);
                //newWall.transform.SetParent(wall0.transform);

                //newWall.GetComponent<Wall>().Attach();
            }
            
        }
        
        wall0.transform.position = new Vector3((float)-building.x / 2, (float)-building.y / 2, (float)-building.z / 2);

        Debug.Log("generated building" + new Vector3(building.x, building.y, building.z));
        return output;
    }
}
