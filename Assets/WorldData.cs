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
                    int index = indexChunk((int)(x/Constants.chunkWidthTiles),(int)(y/Constants.chunkWidthTiles),(int)(z/Constants.chunkWidthTiles));
                    chunks[index].index(x%Constants.chunkWidthTiles,y%Constants.chunkWidthTiles,z%Constants.chunkWidthTiles).type = "terrain";

                }
                i++;
            }
        }

    }

    private int indexChunk(int x,int y,int z){
        return x + y*wc*hc + z*wc;
    }

    public void ToFile()
    {
        for(int i=0; i<chunks.Length; i++){
            string data = chunks[i].SaveToJson();
            string fileName = "/chunk_" + (i%wc).ToString() + "_" + ((int)(i/wc/dc)).ToString() + "_" + ((int)((i%(wc*dc))/wc)).ToString() + ".json";
            
            System.IO.File.WriteAllText(Application.dataPath + "/data" + fileName, data);
        }
        
    }
}

[System.Serializable]
public class ChunkData
{
    public TileData[] tiles;
    public ChunkData()
    {
        tiles = new TileData[30*30*30];
        for(int i=0; i<tiles.Length; i++){
            tiles[i] = new TileData();
            tiles[i].type = "air";
        }
    }

    public TileData index(int x,int y,int z)
    {
        return tiles[x + z*30 + y * 900];
    }

    public string SaveToJson()
    {
        return JsonUtility.ToJson(this);
    }


}

[System.Serializable]
public class TileData
{

    public string type;
    public TileData()
    {

    }
}