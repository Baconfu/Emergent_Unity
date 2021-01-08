using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpaceCollection : MonoBehaviour
{
    public Chunk[,,] chunks;

    private Chunk c;

    void Start()
    {
        c = Resources.Load("Chunk") as Chunk;
        chunks[0,0,0] = Instantiate(c, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Chunk GetChunk (Vector3Int pos)
    {
        return chunks[pos[0], pos[1], pos[2]];
    }
}
