using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlacement : MonoBehaviour
{
    Vector3 cursorHitPoint;
    Vector3 cursorEmptyUnitSpace;

    public GameObject proposedEntity;
    public string proposedEntityName;

    public int rotation = 0;

    public float yValue;


    Ray ray;

    World world;
    Player player;
    BuildingGenerator buildingGenerator;

    public event EventHandler OnEnterPlacing;
    public event EventHandler OnExitPlacing;

    void Start()
    {
        //Debug.Log(System.IO.File.ReadAllText(Application.dataPath + "/buildings/LivingQuarter.json"));
        proposedEntity = null;

        //temporary line (test entity):
        //proposedEntityName = "LivingQuarter";
        proposedEntityName = "TemperatureProbe";

        //probably remove this AFTER the inventory system is mostly complete.
        //i.e. when the player can successfully choose what entity to place down

        world = GameObject.FindWithTag("World").GetComponent<World>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        buildingGenerator = GameObject.FindGameObjectWithTag("World").GetComponent<BuildingGenerator>();

    }

    void Update()
    {
        RaycastHit hit;
        ray = GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit);
        //Debug.Log(hit.collider.gameObject.GetComponent<UnitSpaceScript>().type);
        cursorHitPoint = hit.point;
        cursorEmptyUnitSpace = World.EmptyUnitSpaceOnCursor();


        if (player.GetContext(Player.Context.Placing) == true)
        {
            //if placing mode persists from last frame...
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                //exiting placing mode
                player.SetContext(Player.Context.Placing, false);
                
                Destroy(proposedEntity);
                //Destroy(GameObject.Find(proposedEntityName + "(Proposed)"));
                //Debug.Log(GameObject.Find(proposedEntityName + "(Proposed)"));
                proposedEntity = null;
                Debug.Log("object destroyed");
                //OnExitPlacing?.Invoke(this, EventArgs.Empty);
                return;
            }

            Debug.Log("placing updated");
            proposedEntity.transform.position = GetProposedPositionFromCursor(proposedEntity);
            Debug.Log(proposedEntity);
            proposedEntity.transform.rotation = Quaternion.identity;
            proposedEntity.transform.Rotate(transform.up, 90f * rotation);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("placed building");
                GameObject newObject = Instantiate(proposedEntity, GetProposedPositionFromCursor(proposedEntity), Quaternion.Euler(0, 90 * rotation, 0));
                newObject.GetComponent<Metaentity>().SetProposed(false);
                foreach (Subentity sub in newObject.GetComponentsInChildren<Subentity>())
                {
                    sub.SetProposed(false);
                }

            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                yValue += 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                yValue += -1;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                rotation += 1;
                if (rotation > 3) rotation = 0;
                if (rotation < 0) rotation = 3;
            }

            return;
        }

        if (player.GetContext(Player.Context.Placing) == false)
        {
            //if the player is not placing...
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                //entering placing mode
                Debug.Log("entering placing mode");
                player.SetContext(Player.Context.Placing, true);
                //OnEnterPlacing?.Invoke(this, EventArgs.Empty);


                yValue = Mathf.Round(cursorHitPoint[1]);
                proposedEntity = buildingGenerator.Generate(System.IO.File.ReadAllText(Application.dataPath + "/Resources/Buildings/" + proposedEntityName + ".json"));
                proposedEntity.GetComponent<Metaentity>().SetProposed(true);

                foreach (Subentity sub in proposedEntity.GetComponentsInChildren<Subentity>())
                {
                    sub.SetProposed(true);
                }

                

                //Instantiate(proposedEntity);

                //proposedEntity = GameObject.Find(proposedEntityName + "(Proposed)");

                




                //Debug.Log("original name:" + proposedEntity.GetComponent<Entity>().originalName);
            }
            return;

        }

    }

    Vector3 GetDimension(GameObject target)
    {
        //calculating bounds first based on colliders 
        //(box collider is most priortised), and if no colliders are found, 
        //then calculated using mesh bounds

        //MOST PRIORTISED GO TO BOTTOM
        
        Vector3 dimension;
       
        

        if (target.TryGetComponent(out BoxCollider box))
        {
            dimension = box.bounds.size;
            //Debug.Log(box.bounds);
            goto a;
        }

        if (target.TryGetComponent(out Component collider))
        {
            dimension = target.GetComponent<Collider>().bounds.size;
            Debug.Log(target.GetComponent<Collider>().bounds);
            goto a;
        }

        dimension = target.GetComponent<Mesh>().bounds.size; goto a;
    
    a:
        
        return dimension;


    }

    Vector3 GetProposedPositionFromCursor(GameObject proposed)
    {
        //you HAVE TO feed this function INSTANTIATED STUFF or the dimension thing will not work


        float enter;
        //this is cursor position's projection on the y-value plane
        //Vector2 cursorProjection = new Vector2(cursorHitPoint[0], cursorHitPoint[2]);
        Plane yValuePlane = new Plane(Vector3.up, new Vector3(0, yValue, 0));
        Vector2 cursorProjection = Vector2.zero;


        if (yValuePlane.Raycast(ray, out enter))
        {
            cursorProjection = new Vector2(ray.GetPoint(enter)[0], ray.GetPoint(enter)[2]);
            //Debug.Log(cursorProjection);
        }

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
        Vector3 output = new Vector3(result[0], yValue + GetDimension(proposed)[1] / 2, result[1]);


        if (proposed.TryGetComponent(out Metaentity ent))
        {
            if (ent.buildingType == "Item")
            {
                output += new Vector3(0, -GetDimension(proposedEntity)[1] / 2, 0);
            }
        }


        return output;
        
    }


}
