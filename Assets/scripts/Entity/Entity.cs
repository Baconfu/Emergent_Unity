using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    

    public bool proposed;

    public Color startColor;

    

    public Player player ;
    public GameObject worldObject;

    public event EventHandler OnEnterProposed;
    public event EventHandler OnExitProposed;



    // Start is called before the first frame update
    public virtual void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        worldObject = GameObject.FindWithTag("World");
        GameObject.FindWithTag("World").GetComponent<EntityPlacement>().OnEnterPlacing += EnterPlacing;
        GameObject.FindWithTag("World").GetComponent<EntityPlacement>().OnExitPlacing += ExitPlacing;
    }


    private void OnDisable()
    {
        GameObject.FindWithTag("World").GetComponent<EntityPlacement>().OnEnterPlacing -= EnterPlacing;
    }

  

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void SetProposed(bool desired)
    {
        if (desired)
        {
            proposed = true;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                col.isTrigger = true;
            }
        }
        else
        {
            proposed = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                col.isTrigger = false;
                Debug.Log(col.ToString() + "istrigger false");
            }
        }
    }

    private void EnterPlacing(object sender, EventArgs e)
    {
        Debug.Log("EnterPlacing evnet function called");
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        foreach (Transform trans in GetComponentsInChildren<Transform>())
        {
            trans.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }    
    private void ExitPlacing(object sender, EventArgs e)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (Transform trans in GetComponentsInChildren<Transform>())
        {
            trans.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

}
