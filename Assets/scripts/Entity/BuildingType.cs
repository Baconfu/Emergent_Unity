using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

[Serializable]
public class BuildingType
{
    public int x;
    public int y;
    public int z;

    public string name;
    public string type;
    
    /*
    types:
    "Building" is an entity with an interior. 
    "Structure" is a supportive entity (like pillars, struts etc.). 
    "Item" is a small entity, more often only 1x1 in its area occupied. 
     
     
     */



    //"Building" specific parameters
    public bool haveDoor;
    public List<string> positionOfDoor;
    public List<Vector3> positionOfDoorVector;

}

