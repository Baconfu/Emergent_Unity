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



public class Fractal 
{
    public Fractal()
    {
        heightMap = new float[0];
        intensity = 1;
        w = 0;
        h = 0;
        iterator = 1;
    }

    public float[] heightMap
    {
        get;set;
    }

    public float intensity
    {
        get;set;
    }

    public int w
    {
        get;set;
    }
    public int h
    {
        get;set;
    }

    public int iterator
    {
        get;private set;
    }

    public void IterateTo(int target)
    {
        while(iterator <= target){
            Iterate();
        }
    }

    public void Iterate(float intensityCoefficient)
    {
        float[,] temp = new float[w,h];
        int i = 0;
        for(int y =0; y<h; y++){
            for(int x=0; x<w; x++){
                temp[x,y] = heightMap[i];
                i++;
            }
        }

        int fw = w*2-1;
        int fh = h*2-1;

        float[,] output = new float[fw,fh];

        
        for(int y=0; y<h; y++){
            for(int x=0; x<w; x++){
                output[x*2,y*2] = temp[x,y];
            }
        }
        

        for(int y=0; y<h; y++){
            for(int x=0; x<w; x++){
                int lx = x*2;
                int ly = y*2;
                if(x!=w-1){
                    
                    output[lx + 1,ly] = (output[lx,ly] + output[lx+2,ly]) / 2;
                }
                if(y!=h-1){
                    output[lx,ly+1] = (output[lx,ly] + output[lx,ly+2]) / 2;
                }
            }
        }

        
        for(int y=0; y<h-1; y++){
            for(int x=0; x<w-1; x++){

                int lx = x*2;
                int ly = y*2;
                
                output[lx+1,ly+1] = (output[lx+1,ly] + output[lx+1,ly+2] + output[lx,ly+1] + output[lx+2, ly+1] + output[lx,ly] + output[lx+2,ly] + output[lx,ly+2] + output[lx+2,ly+2])/8;
                
                float amp = intensityCoefficient * Mathf.Pow(2f, -1 * iterator);

                float rand = Random.Range(-amp,amp);
                output[lx+1,ly+1] += rand;

            }
        }
        

        float[] oneD = new float[(w*2-1)*(h*2-1)];

        i=0;
        for(int y=0; y<h*2-1; y++){
            for(int x=0; x<w*2-1; x++){
                oneD[i] = output[x,y];
                i++;
            }
        }

        heightMap = oneD;
        w = fw;
        h = fh;
        iterator++;
    }

    public void Iterate()
    {
        Iterate(intensity);
    }

}

public class Erosion
{
    public float[,] heightMap 
    {
        get;set;
    }
    public int w{get;set;}
    public int h{get;set;}
    public Erosion(float[,] HeightMap,int W,int H,float SedimentLossRate)
    {
        heightMap = HeightMap;
        w=W;
        h=H;
        sedimentLoss = SedimentLossRate;
    }

    public float sedimentLoss{get;set;}

    public float[] Get1D()
    {
        float[] output = new float[w*h];
        int i=0;
        for(int y=0; y<h; y++){
            for(int x=0; x<w; x++){
                output[i] = heightMap[x,y];
                i++;
            }
        }


        return output;
    }

    public void Iterate()
    {
        Vector2 init = new Vector2(Random.Range(0,w-1),Random.Range(0,h-1));
        
        WaterDrop drop = new WaterDrop(init);
        
        int lifeSpan = 200;

        bool moving = true;
        while(moving){
            
            
            int x = (int)drop.pos.x;
            int y = (int)drop.pos.y;

            lifeSpan-=1;
            if(lifeSpan < 0){
                heightMap[x,y]+=drop.sediment;
                moving = false;
                //Debug.Log("dead");
                break;
            }

            Vector2 lowest = drop.pos;
            float lowestVal = heightMap[x,y];
            if(y!=0){
                if(heightMap[x,y-1] < lowestVal){
                    lowest = new Vector2(x,y-1);
                    lowestVal = heightMap[x,y-1];
                }
            }
            if(x!=w-1){
                if(heightMap[x+1,y] < lowestVal){
                    lowest = new Vector2(x+1,y);
                    lowestVal = heightMap[x+1,y];
                }
            }
            if(y!=h-1){
                if(heightMap[x,y+1] < lowestVal){
                    lowest = new Vector2(x,y+1);
                    lowestVal = heightMap[x,y+1];
                }
            }
            if(x!=0){
                if(heightMap[x-1,y] < lowestVal){
                    lowest = new Vector2(x-1,y);
                    lowestVal = heightMap[x-1,y];
                }
            }
            if(drop.pos == lowest){
                heightMap[x,y] += drop.sediment;
                moving = false;
            }
            float newV = heightMap[x,y] - heightMap[(int)lowest.x,(int)lowest.y];

            
            float sedimentGain = newV/2f;
            sedimentLoss = drop.sediment/2f;
            float sedimentChange = sedimentGain-sedimentLoss;
            //Debug.Log(sedimentGain);
            if(drop.sediment < -1 * sedimentChange){
                sedimentChange = -1 * drop.sediment;
            }
            //Debug.Log("gain: " + sedimentGain + " sediment change: " + sedimentChange);
            drop.sediment += sedimentChange;
            heightMap[x,y] += -1 * sedimentChange;


            drop.pos = lowest;



        }



    }

}
public class WaterDrop
{
    public Vector2 pos{get;set;}
    public float sediment{get;set;}

    public WaterDrop(Vector2 Pos)
    {
        pos = Pos;
    }
}

public class WorldFeatures
{
    public WorldFeatures()
    {

    }

    public float worldScale = 40;

    

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
                    txr.Draw(new Vector2(ridge[highest].x / worldScale, ridge[highest].y / worldScale), (int)(ridge[highest].z * 10000 / worldScale), ridge[highest].z *2 / worldScale);
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
                    txr.DrawLine(new Vector3(ridge[j].x/worldScale,ridge[j].y/worldScale,ridge[j].z*2/worldScale),new Vector3(ridge[j+1].x/worldScale,ridge[j+1].y/worldScale,ridge[j+1].z*2/worldScale));
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
                    txr.Draw(new Vector2(ridge[highest].x / worldScale, ridge[highest].y / worldScale), (int)(ridge[highest].z * 10000 / worldScale), ridge[highest].z *2 / worldScale);
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
            
            txr.Draw(allHexes[i].GetPos()/worldScale,(int)(allHexes[i].GetHexElevation() * 600/worldScale), allHexes[i].GetHexElevation()/5/worldScale);
            
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

    public float[] FractalIteration(float[] heightMap,int w,int h,float intensity)
    {
        float[,] temp = new float[w,h];
        int i = 0;
        for(int y =0; y<h; y++){
            for(int x=0; x<w; x++){
                temp[x,y] = heightMap[i];
                i++;
            }
        }

        float[,] output = new float[w*2-1,h*2-1];

        int fw = w*2-1;
        int fh = h*2-1;
        for(int y=0; y<h; y++){
            for(int x=0; x<w; x++){
                output[x*2,y*2] = temp[x,y];
            }
        }
        

        for(int y=0; y<h; y++){
            for(int x=0; x<w; x++){
                int lx = x*2;
                int ly = y*2;
                if(x!=w-1){
                    
                    output[lx + 1,ly] = (output[lx,ly] + output[lx+2,ly]) / 2;
                }
                if(y!=h-1){
                    output[lx,ly+1] = (output[lx,ly] + output[lx,ly+2]) / 2;
                }
            }
        }

        
        for(int y=0; y<h-1; y++){
            for(int x=0; x<w-1; x++){

                int lx = x*2;
                int ly = y*2;
                
                output[lx+1,ly+1] = (output[lx+1,ly] + output[lx+1,ly+2] + output[lx,ly+1] + output[lx+2, ly+1] + output[lx,ly] + output[lx+2,ly] + output[lx,ly+2] + output[lx+2,ly+2])/8;
                

                float rand = Random.Range(-intensity,intensity);
                output[lx+1,ly+1] += rand;

            }
        }
        

        float[] oneD = new float[(w*2-1)*(h*2-1)];

        i=0;
        for(int y=0; y<h*2-1; y++){
            for(int x=0; x<w*2-1; x++){
                oneD[i] = output[x,y];
                i++;
            }
        }

        return oneD;
    }

    




}