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


    Ray ray;

    GameObject world;
    Player player;
    BuildingGenerator buildingGenerator;

    void Start()
    {
        //Debug.Log(System.IO.File.ReadAllText(Application.dataPath + "/buildings/LivingQuarter.json"));

        //temporary line (test entity):

        //proposedEntity = Resources.Load("TestEntity3_4_3");
        proposedEntityName = "LivingQuarter";

        //probably remove this AFTER the inventory system is mostly complete.
        //i.e. when the player can successfully choose what entity to place down

        world = GameObject.FindWithTag("World");
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
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

        if (player.GetContext(Player.Context.Placing))
        {
            Debug.Log("placing updated");
            proposedEntity.transform.position = GetProposedPositionFromCursor(proposedEntity);
            proposedEntity.transform.rotation = Quaternion.identity;
            proposedEntity.transform.Rotate(transform.up, 90f * rotation);
            //Debug.Log(GetProposedPositionFromCursor(GameObject.Find("TestEntity3_4_3(Clone)")));

            if (World.KeyPressed(KeyCode.Tab))
            {
                //exiting placing mode
                player.SetContext(Player.Context.Placing, false);
                Destroy(GameObject.Find(proposedEntityName + "(Proposed)"));
                proposedEntity = null;
            }


            if (World.KeyPressed(KeyCode.Mouse0))
            {
                Debug.Log("placed building");
                GameObject newObject = Instantiate(proposedEntity, GetProposedPositionFromCursor(proposedEntity), Quaternion.Euler(0, 90 * rotation, 0));
                newObject.GetComponent<Entity>().setProposed(false);
            }
        }
        else
        {
            if (World.KeyPressed(KeyCode.Tab))
            {
                //entering placing mode
                Debug.Log("entering placing mode");
                player.SetContext(Player.Context.Placing, true);
                yValue = Mathf.Round(cursorHitPoint[1]);
                proposedEntity = buildingGenerator.Generate(System.IO.File.ReadAllText(Application.dataPath + "/buildings/" + proposedEntityName + ".json"));
                proposedEntity.GetComponent<Entity>().setProposed(true);
                proposedEntity = GameObject.Find(proposedEntityName + "(Proposed)");
                
                //Debug.Log("original name:" + proposedEntity.GetComponent<Entity>().originalName);
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


}
