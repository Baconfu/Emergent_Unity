using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;



public class WorldData
{
    private ChunkData[] chunks;
    public WorldData()
    {
        
    }

    private int wc;
    private int hc;
    private int dc;
    public void FromHeightMap(float[] heightMap,int w,int h,int d)
    {
        
        wc = (int)(w/Constants.chunkWidthTiles); hc = (int)(h/Constants.chunkWidthTiles); dc = (int)(d/Constants.chunkWidthTiles);
        chunks = new ChunkData[wc*hc*dc];
        for(int n = 0; n<chunks.Length; n++){
            chunks[n] = new ChunkData();
        }
        int i=0;
        for(int x=0; x<w; x++){
            for(int z=0; z<d; z++){

                for(int y=0; y<(int)(h*heightMap[i]); y++){
                    //chunks[indexChunk((int)(x/Constants.chunkWidthTiles),(int)(y/Constants.chunkWidthTiles),(int)(z/Constants.chunkWidthTiles))];
                }
                i++;
            }
        }
    }

    private int indexChunk(int x,int y,int z){
        return x + y*wc + z*wc*hc;
    }
}

public class ChunkData
{
    //public TileData
    public ChunkData()
    {

    }


}

public class TileData
{

    public string type;
    public TileData()
    {

    }
}