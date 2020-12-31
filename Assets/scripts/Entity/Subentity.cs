using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subentity : Entity
{

    public Player player;

    public Entity root;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        root = transform.root.GetComponent<Entity>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (player.GetContext(Player.Context.Placing))
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
