using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private int hp;

    public bool proposed;

    public Color startColor;

    public string originalName;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (player.GetContext(Player.Context.Placing))
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void setProposed(bool desiredProposed)
    {
        if (desiredProposed)
        {
            proposed = true;

            //ignore raycast layer (number 2)
            gameObject.layer = 2;
            foreach(Transform trans in GetComponentsInChildren<Transform>())
            {
                trans.gameObject.layer = 2;
            }

            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                col.isTrigger = true;
            }

            originalName = gameObject.name;
            gameObject.name += "(Proposed)";
        }

        if (!desiredProposed)
        {
            proposed = false;

            foreach (Transform trans in GetComponentsInChildren<Transform>())
            {
                trans.gameObject.layer = 0;
            }

            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                col.isTrigger = false;
                Debug.Log("setted colliders");
            }

            if (originalName.Length != 0)
            {
                gameObject.name = originalName;
            }
            
        }

        foreach(Entity entity in GetComponentsInChildren<Entity>())
        {
            entity.proposed = desiredProposed;
        }

    }

    public void onParentSetProposed(bool desiredProposed)
    {

    }
}
