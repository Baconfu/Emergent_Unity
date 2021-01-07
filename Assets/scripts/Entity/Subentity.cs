using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subentity : Entity
{
    public Material highlight;
    public Material original;
    

    public Entity root;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        root = transform.root.GetComponent<Entity>();

        highlight = Resources.Load("Materials/ProposedMaterial") as Material;
        //Debug.Log(Resources.Load("Materials/ProposedMaterial"));
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();



    }

    public override void SetProposed(bool desired)
    {
        base.SetProposed(desired);
        /*if (desired)
        {
            foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
            {
                Material[] previousMaterials = r.materials;
                Material[] newMaterials = new Material[previousMaterials.Length + 1];
                previousMaterials.CopyTo(newMaterials, 0);
                newMaterials[previousMaterials.Length] = highlight;
                r.materials = newMaterials;
            }
    }
        else
        {
            foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
            {
                Material[] newMaterials = r.materials;
                Material[] previousMaterials = new Material[newMaterials.Length - 1];
                Array.Copy(newMaterials, previousMaterials, newMaterials.Length - 1);
                r.materials = previousMaterials;
            }
        }*/
    }




}
