using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpace : MonoBehaviour
{
    public int type;

    public Collider physicsCollider;
    //public Collider detectionCollider;

    protected Player player;

    Material buildingMaterial;

    public enum UnitSpaceType{
        air = 0,

        rock = 100,

        numberOfUnitSpaceTypes
    }

    /*public bool isCollidable()
    {
        if(type < 100){
            return false;
        }
        return true;
        
    }*/

    // Start is called before the first frame update
    private void Awake()
    {
        //buildingMaterial = GetComponent<Renderer>().material;
    }

    void Start()
    {
        gameObject.layer = 0;
        if(type < 100){
            physicsCollider.isTrigger = true;
        }else{
            physicsCollider.isTrigger = false;
        }
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        if(type == (int)UnitSpaceType.air){
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            gameObject.layer = 2;
        }

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.onInside += OnPlayerInside;
        player.onOutside += OnPlayerOutside;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UnitSpaceType GetUnitSpaceType(){
        return (UnitSpaceType)type;
    }

    public void SetUnitSpaceType(UnitSpaceType desired){
        type = (int)desired;
    }

    void OnPlayerInside(object sender, EventArgs e)
    {
        Debug.Log("Inside!");
    }
    
    void OnPlayerOutside(object sender, EventArgs e)
    {

    }

    /*public void Darken(float percent)
    {
        percent = Mathf.Clamp01(percent);
        buildingMaterial.color = new Color(
            buildingMaterial.color.r * (1 - percent), 
            buildingMaterial.color.g * (1 - percent), 
            buildingMaterial.color.b * (1 - percent), 
            buildingMaterial.color.a);
    }*/

}
