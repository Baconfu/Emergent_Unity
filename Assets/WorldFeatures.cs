using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IndexedValue
{
    public int index = 0;
    public float value = 0;
    public IndexedValue(int i,float v)
    {
        index = i;
        value = v;
    }
    

}

public class WorldFeatures
{
    public WorldFeatures()
    {

    }

    


    public List<IndexedValue> Sort(List<IndexedValue> ar)
    {
       
        float pivot = ar[0].value;
        List<IndexedValue> rhs = new List<IndexedValue>();
        List<IndexedValue> lhs = new List<IndexedValue>();
        List<IndexedValue> equal = new List<IndexedValue>();

        for(int i=0; i<ar.Count; i++){
            if(ar[i].value < pivot){
                lhs.Add(ar[i]);
            }
            if(ar[i].value > pivot){
                rhs.Add(ar[i]);
            }
            if(ar[i].value == pivot){
                equal.Add(ar[i]);
            }
        }
        if(lhs.Count > 1){
            lhs = Sort(lhs);
        }
        if(rhs.Count > 1){
            rhs = Sort(rhs);
        }
        for(int i=0; i<equal.Count; i++){
            lhs.Add(equal[i]);
        }
        for(int i=0; i<rhs.Count; i++){
            lhs.Add(rhs[i]);
        }
        return lhs;

    }

    public float[] RidgeAccentStencil(WorldTex txr, WorldGenerator generator)
    {
        float worldScale = 10;

        List<Ridge> allRidges = generator.GetAllRidges();

        txr.SetDrawRule("sin_minus");
        txr.SetRadius(350);
        
        txr.SetStrength((float)0.05);
        txr.setRatio();
        txr.SetAddRule("add");
        txr.UpdateBrushBuffer();


        for(int i=0; i<allRidges.Count; i++){
            if(allRidges[i].finalized){
                List<Vector3> ridge = allRidges[i].GetPath();
                int highest = 0;
                float highestAltitude = 0;
                for (int j = 0; j < ridge.Count - 1; j++)
                {
                    if (ridge[j].z > highestAltitude)
                    {
                        highest = j;
                        highestAltitude = ridge[j].z;
                    }
                }
                if (highestAltitude > 0.005)
                {
                    /*
                    if(allRidges[i].getFoldCharacter() > 1){
                        txr.DrawAddStencil(scaledStencil,w/10,h/10,new Vector2(ridge[highest].x/worldScale,ridge[highest].y / worldScale));
                    }*/
                    txr.Draw(new Vector2(ridge[highest].x / worldScale, ridge[highest].y / worldScale), (int)(ridge[highest].z * 10000 / worldScale), ridge[highest].z / worldScale);
                }
                /*
                for(int j=0; j<ridge.Count-1; j++){
                    if(ridge[j].z>0.02 && ridge[j].z < 0.025){
                        //Debug.Log("drawing");
                        txr.Draw(new Vector2(ridge[j].x/10,ridge[j].y/10),60,0.01f);
                    }
                }*/
            }
            
        }
        return txr.GetTexBuffer();
    }

    public float[] MountainRangeStencil(WorldTex txr)
    {
        WorldGenerator generator = new WorldGenerator();

        List<Vector2> plates = new List<Vector2>();
        
        for(int i=-5; i<5; i++){
            for(int j=0; j<10; j++){
                plates.Add(new Vector2(i,j));
            }
        }

        for(int i=0; i<plates.Count; i++){
            generator.GeneratePlate(plates[i]);
        }


        float worldScale = 10;

        List<Hex> allHexes = generator.GetAllHexes();
        List<TectonicPlate> allPlates = generator.GetAllPlates();
        List<Ridge> allRidges = generator.GetAllRidges();


        txr.SetDrawRule("sin_minus");
        txr.SetRadius(350);
        
        txr.SetStrength((float)0.05);
        txr.setRatio();
        txr.SetAddRule("add");
        txr.UpdateBrushBuffer();



        for(int i=0; i<allRidges.Count; i++){
            if(allRidges[i].finalized){
                List<Vector3> ridge = allRidges[i].GetPath();
                for(int j=0; j<ridge.Count-1; j++){
                    txr.DrawLine(new Vector3(ridge[j].x/worldScale,ridge[j].y/worldScale,ridge[j].z/worldScale),new Vector3(ridge[j+1].x/worldScale,ridge[j+1].y/worldScale,ridge[j+1].z/worldScale));
                }
            }
            
        }

                for(int i=0; i<allRidges.Count; i++){
            if(allRidges[i].finalized){
                List<Vector3> ridge = allRidges[i].GetPath();
                int highest = 0;
                float highestAltitude = 0;
                for (int j = 0; j < ridge.Count - 1; j++)
                {
                    if (ridge[j].z > highestAltitude)
                    {
                        highest = j;
                        highestAltitude = ridge[j].z;
                    }
                }
                if (highestAltitude > 0.005)
                {
                    /*
                    if(allRidges[i].getFoldCharacter() > 1){
                        txr.DrawAddStencil(scaledStencil,w/10,h/10,new Vector2(ridge[highest].x/worldScale,ridge[highest].y / worldScale));
                    }*/
                    txr.Draw(new Vector2(ridge[highest].x / worldScale, ridge[highest].y / worldScale), (int)(ridge[highest].z * 10000 / worldScale), ridge[highest].z / worldScale);
                }
                /*
                for(int j=0; j<ridge.Count-1; j++){
                    if(ridge[j].z>0.02 && ridge[j].z < 0.025){
                        //Debug.Log("drawing");
                        txr.Draw(new Vector2(ridge[j].x/10,ridge[j].y/10),60,0.01f);
                    }
                }*/
            }
            
        }

        for(int i=0; i<allHexes.Count; i++){

            //Debug.Log(allHexes[i].GetHexElevation());
            
            txr.Draw(allHexes[i].GetPos()/worldScale,(int)(allHexes[i].GetHexElevation() * 600/worldScale), allHexes[i].GetHexElevation()/10/worldScale);
            
        }

        return txr.GetTexBuffer();
    }

    public float[] RiverStencil(float[] heightMap,int w)
    {

        List<IndexedValue> map = new List<IndexedValue>();
        for(int i=0; i<heightMap.Length; i++){
            map.Add(new IndexedValue(i,heightMap[i]));
        }
        float[] water = new float[heightMap.Length];
        for(int i=0; i<water.Length; i++){
            water[i] = 0.1f;
        }
        List<IndexedValue> mem = map;

        map = Sort(map);
        map.Reverse();

        for(int a=0; a<map.Count; a++){
            int i = map[a].index;
            int y = (int)(i/w);
            int x = i%w;
            float move = water[i] / 2;
            water[i]-=move;
            int factor = 0;
            bool b1 = false; bool b2 = false; bool b3 = false; bool b4 = false;
            if(x!=w-1){
                
                if(mem[i+1].value<mem[i].value){
                    factor+=1;
                    b1 = true;
                }
            }
            
            if(x!=0){
                if(mem[i-1].value<mem[i].value){
                    factor+=1;
                    b2 = true;
                }
            }
            
            if(y!=0){
                if(mem[(y-1)*w+x].value<mem[i].value){
                    factor+=1;
                    b3 = true;
                }
            }
            
            if(y!=w-1){
                    if(mem[(y+1)*w+x].value<mem[i].value){
                        factor+=1;
                        b4 = true;
                    }
                
            }
            
            move/=factor;
            if(b1){water[i+1]+=move;}
            if(b2){water[i-1]+=move;}
            if(b3){water[(y-1)*w + x]+=move;}
            if(b4){water[(y+1)*w + x]+=move;}

        }


        return water;
    }

    public int FindIndex(List<IndexedValue> ar,int i)
    {
        for(int a=0; a<ar.Count; a++){
            if(ar[a].index == i){
                return a;
            }
        }
        return -1;
    }

    public float[] HexElevationStencil(WorldTex txr,WorldGenerator generator)
    {
        float worldScale = 10;

        List<Hex> allHexes = generator.GetAllHexes();

        txr.SetDrawRule("sin_minus");
        txr.SetRadius(350);
        
        txr.SetStrength((float)0.05);
        txr.setRatio();
        txr.SetAddRule("add");
        txr.UpdateBrushBuffer();

        for(int i=0; i<allHexes.Count; i++){

            //Debug.Log(allHexes[i].GetHexElevation());
            
            txr.Draw(allHexes[i].GetPos()/worldScale,(int)(allHexes[i].GetHexElevation() * 300/worldScale), allHexes[i].GetHexElevation()/20/worldScale);
            
        }

        return txr.GetTexBuffer();
    }

    public float[] TectonicPlateStencil(WorldTex txr,WorldGenerator generator)
    {
        
        float worldScale = 10;

        List<Ridge> allRidges = generator.GetAllRidges();

        txr.SetDrawRule("sin_minus");
        txr.SetRadius(350);
        
        txr.SetStrength((float)0.05);
        txr.setRatio();
        txr.SetAddRule("add");
        txr.UpdateBrushBuffer();


        for(int i=0; i<allRidges.Count; i++){
            if(allRidges[i].finalized){
                List<Vector3> ridge = allRidges[i].GetPath();
                for(int j=0; j<ridge.Count-1; j++){
                    txr.DrawLine(new Vector3(ridge[j].x/worldScale,ridge[j].y/worldScale,ridge[j].z/worldScale),new Vector3(ridge[j+1].x/worldScale,ridge[j+1].y/worldScale,ridge[j+1].z/worldScale));
                }
            }
            
        }



        return txr.GetTexBuffer();

    }



}