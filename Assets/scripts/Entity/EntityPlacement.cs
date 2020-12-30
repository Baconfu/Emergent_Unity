using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlacement : MonoBehaviour
{
    Vector3 cursorHitPoint;
    Vector3 cursorEmptyUnitSpace;

    GameObject proposedEntity;
    string proposedEntityName;

    int rotation = 0;

    float yValue;

    bool placing;
    bool delay;

    Ray ray;

    GameObject world;
    BuildingGenerator buildingGenerator;

    void Start()
    {
        //Debug.Log(System.IO.File.ReadAllText(Application.dataPath + "/buildings/LivingQuarter.json"));

        //temporary line (test entity):

        //proposedEntity = Resources.Load("TestEntity3_4_3");

        //probably remove this AFTER the inventory system is mostly complete.
        //i.e. when the player can successfully choose what entity to place down

        world = GameObject.FindGameObjectWithTag("World");
        buildingGenerator = GameObject.FindGameObjectWithTag("World").GetComponent<BuildingGenerator>();

    }

    void Update()
    {
        //Debug.Log("update ");

        RaycastHit hit;
        ray = GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit);
        //Debug.Log(hit.collider.gameObject.GetComponent<UnitSpaceScript>().type);
        cursorHitPoint = hit.point;
        cursorEmptyUnitSpace = World.EmptyUnitSpaceOnCursor();

        //the TAB key trigger is also temporary. change ASAP when infrastructure and overall ideas are complete;
        if (World.KeyPressed(KeyCode.Tab))
        {
            //Debug.Log("TAB");
            if (placing)
            {
                placing = false;
                Destroy(GameObject.Find("LivingQuarter"));
                
            }
            else
            {
                //this is what happens when you ENTER placing mode
                placing = true;
                yValue = Mathf.Round(cursorHitPoint[1]);
                proposedEntity = buildingGenerator.Generate(System.IO.File.ReadAllText(Application.dataPath + "/buildings/LivingQuarter.json"));
                proposedEntity = GameObject.Find(proposedEntity.name);
                //ignore raycast layer (number 2)
                proposedEntity.layer = 2;
                proposedEntity.GetComponent<BoxCollider>().isTrigger = true;
            }
   
        }

        if (World.KeyPressed(KeyCode.UpArrow))
        {
            yValue += 1;
        }
        if (World.KeyPressed(KeyCode.DownArrow))
        {
            yValue += -1;
        }
        if (World.KeyPressed(KeyCode.R))
        {
            rotation += 1;
            if (rotation > 3) { rotation = 0; }
            if (rotation < 0) { rotation = 3; }
        }

        //Debug.Log("placing: " + placing);

        //this is what happens if placing mode PERSISTS through a frame
        if (placing)
        {
            Debug.Log("placing updated");
            proposedEntity.transform.position = GetProposedPositionFromCursor(proposedEntity);
            proposedEntity.transform.rotation = Quaternion.identity;
            proposedEntity.transform.Rotate(transform.up, 90f * rotation);
            //Debug.Log(GetProposedPositionFromCursor(GameObject.Find("TestEntity3_4_3(Clone)")));

            if (World.KeyPressed(KeyCode.Mouse0))
            {
                GameObject newObject = Instantiate(proposedEntity, GetProposedPositionFromCursor(proposedEntity), Quaternion.Euler(0, 90 * rotation, 0));
            }
        }
    }

    Vector3 GetDimension(GameObject target)
    {
        //calculating bounds first based on colliders 
        //(box collider is most priortised), and if no colliders are found, 
        //then calculated using mesh bounds

        //MOST PRIORTISED GO TO BOTTOM
        
        Vector3 dimension;
       
        if (target.TryGetComponent(typeof(Collider), out Component collider))
        {
            dimension = target.GetComponent<Collider>().bounds.size;
            //Debug.Log(target.GetComponent<Collider>().bounds);
            goto a;
        }

        if (target.TryGetComponent(typeof(BoxCollider), out Component box))
        {
            dimension = target.GetComponent<BoxCollider>().bounds.size;
            
            goto a;
        }

        dimension = target.GetComponent<Mesh>().bounds.size; goto a;
    
    a:

        return dimension;


    }

    Vector3 GetProposedPositionFromCursor(GameObject proposed)
    {
        //you HAVE TO feed this function INSTANTIATED STUFF or the dimension thing will not work


        float enter = 0.0f;
        //this is cursor position's projection on the y-value plane
        //Vector2 cursorProjection = new Vector2(cursorHitPoint[0], cursorHitPoint[2]);
        Plane yValuePlane = new Plane(Vector3.up, new Vector3(0, yValue, 0));
        Vector2 cursorProjection = Vector2.zero;


        if (yValuePlane.Raycast(ray, out enter))
        {
            cursorProjection = new Vector2(ray.GetPoint(enter)[0], ray.GetPoint(enter)[2]);
            //Debug.Log(cursorProjection);
        }
        
        //Vector2 cursorProjection = ray.GetPoint(Mathf.Abs(ray.origin[1] - yValue) / Mathf.Cos(Vector3.Angle(ray.direction, Vector3.down)));
        

        Vector2 result = Vector2.zero;

        Vector2 dimensionProjection = new Vector2(Mathf.Round(GetDimension(proposed)[0]), Mathf.Round(GetDimension(proposed)[2]));
        
        /*if (rotation % 2 == 1)
        {
            dimensionProjection = new Vector2(Mathf.Round(GetDimension(proposed)[2]), Mathf.Round(GetDimension(proposed)[0]));
        }*/
 


        
        for (int i=0; i<2; i++)
        {
            //if the x/y-length of the proposed entity is even... 
            if (dimensionProjection[i] % 2 == 0)
            {
                result[i] = Mathf.Round(cursorProjection[i]); 

            }
            //if odd...
            else
            {
                result[i] = Mathf.Floor(cursorProjection[i]) + 0.5f;
            }
        }

        Debug.Log("dimension projection" + dimensionProjection);
        Debug.Log("rotation state" + rotation % 2);

        return new Vector3(result[0], yValue + GetDimension(proposed)[1] / 2, result[1]);
        
    }

    IEnumerator Delayer()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        delay = true;
    }

}
