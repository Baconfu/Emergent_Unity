using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Constants 
{
    public const int chunk_width = 16;

    public string data_path = Application.dataPath + "/data";

    enum spaceType: ushort {
        //space types 0-100 are non-collidable
        //space types 100-200 are collidable
        air = 0,
        rock =  100   
    }
}