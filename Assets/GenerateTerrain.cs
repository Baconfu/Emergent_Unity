using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class GenerateTerrain : MonoBehaviour
{


    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    public Vector3[] PopulateVerts(float[] heights,int width,int height)
    {
        float highest = 0;
        for(int a=0; a<heights.Length; a++){
            if(heights[a]>highest){
                highest = heights[a];
            }
        }
        if(highest<1){
            highest = 1;
        }
        Vector3[] output = new Vector3[width * height];
        int i = 0;
        for(int z=0; z<height; z++){
            for(int x=0; x<width; x++){
                output[i] = new Vector3((float)x / width,heights[i]/highest,(float)z / height) - new Vector3((float)0.5,(float)0.5,(float)0.5);

                i++;
            }
        }
        return output;
    }

    public Vector2[] PopulateUV(int width, int height)
    {
        Vector2[] output = new Vector2[width * height];
        int i=0; 
        for(int z=0; z<height; z++){
            for(int x=0; x<width; x++){
                output[i] =  new Vector2(x,z);
                i++;
            }
        }
        return output;
    }

    public int[] PopulateTriangles(int width,int height)
    {
        int[] output = new int[width* height * 6];
        int i = 0;
        for(int z=0; z<height; z++){
            for(int x=0; x<width; x++){
                if(x == width-1 || z == height-1){
                    
                }
                else{
                    output[i*6] = i;
                    output[i*6+1] = i+1+width;
                    output[i*6+2] = i+1;
                    output[i*6+3] = i;
                    output[i*6+4] = i+width;
                    output[i*6+5] = i+width+1;
                    
                }
                i++;
                
            }
        }
        return output;
    }

    public double[] PopulateHeights(int width,int height)
    {
        double[] output = new double[width * height];
        int i=0;
        for(int z=0; z<height; z++){
            for(int x=0; x<width; x++){
                output[i] = x*Random.Range(0.3f,1.0f);
                i++;
            }
        }
        return output;
    }

    public Vector3[] PopulateNormals(int width, int height, Vector3[] vertices) 
    {
        Vector3[] output = new Vector3[width*height];
        int i=0;

        for(int z=0; z<height; z++){
            for(int x=0; x<width; x++){
                output[i] = new Vector3(0,10,0).normalized;
                i++;
            }
        }
        return output;
    }

    public Texture2D DrawLine(Texture2D tex, Vector2 p1, Vector2 p2, Color col)
    {
        Vector2 t = p1;
        float frac = 1/Mathf.Sqrt (Mathf.Pow (p2.x - p1.x, 2) + Mathf.Pow (p2.y - p1.y, 2));
        float ctr = 0;
        if(p2.x>tex.width || p2.y > tex.height || p2.x < 0 || p2.y < 0
        || p1.x> tex.width || p1.y > tex.height || p1.x < 0 || p1.y < 0){
            return tex;
        }
        while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y) {
            t = Vector2.Lerp(p1, p2, ctr);
            ctr += frac;
            tex.SetPixel((int)t.x, (int)t.y, col);
        }
        return tex;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Random.InitState(5);
        int w = 1000;
        int h = 1000;
        Texture2D texture = new Texture2D(w,h);
        for(int i=0; i<w; i++){
            for(int j=0; j<h; j++){
                texture.SetPixel(i,j,new Color((float)0,(float)0,(float)0,1));
            }
        }

        float worldScale = 10;

        WorldTex txr = new WorldTex(texture);
        
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

        /*
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
                    
                    if(allRidges[i].getFoldCharacter() > 1){
                        txr.DrawAddStencil(scaledStencil,w/10,h/10,new Vector2(ridge[highest].x/worldScale,ridge[highest].y / worldScale));
                    }
                    txr.Draw(new Vector2(ridge[highest].x / worldScale, ridge[highest].y / worldScale), (int)(ridge[highest].z * 10000 / worldScale), ridge[highest].z / worldScale);
                }
                
                for(int j=0; j<ridge.Count-1; j++){
                    if(ridge[j].z>0.02 && ridge[j].z < 0.025){
                        //Debug.Log("drawing");
                        txr.Draw(new Vector2(ridge[j].x/10,ridge[j].y/10),60,0.01f);
                    }
                }
            }
            
        }
        

        txr.fillAdd(0.5f);
        for(int i=0; i<allHexes.Count; i++){

            //Debug.Log(allHexes[i].GetHexElevation());
            
            txr.Draw(allHexes[i].GetPos()/worldScale,(int)(allHexes[i].GetHexElevation() * 300/worldScale), allHexes[i].GetHexElevation()/20/worldScale);
            
        }

        */
        


        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        

        
        
        //float[] heightMap = txr.GetTexBuffer();
        WorldFeatures features = new WorldFeatures();

        WorldTex txr2 = new WorldTex(new Texture2D(w,h));

        float[] heightMap = new float[w*h];

        float[] export = new float[30*30];

        /*
        heightMap = features.HexElevationStencil(txr2,generator);
        float[] ridges = features.TectonicPlateStencil(new WorldTex(new Texture2D(w,h)),generator);
        float[] ridgeAccent = features.RidgeAccentStencil(new WorldTex(new Texture2D(w,h)),generator);


        List<float[]> layers = new List<float[]>();
        layers.Add(heightMap);
        layers.Add(ridges);
        layers.Add(ridgeAccent);

        
        //heightMap = txr.GetTexBuffer();
        //heightMap = AddArray(ridges,heightMap);
        heightMap = AddArray(layers);

        */

        heightMap = features.MountainRangeStencil(new WorldTex(new Texture2D(w,h)));
        for(int i=0; i<heightMap.Length; i++){
            heightMap[i]+=0.5f;
        }


        for(int y=0; y<30; y++){
            for(int x=0; x<30; x++){
                float avg = 0;
                for(int i = y*10+300; i<(y+1)*10 + 300; i++){
                    for(int j=x*10+300; j<(x+1)*10 + 300; j++){
                        avg+= heightMap[i*w + j];
                    }
                }
                avg/=100;
                export[x + y*30] = avg;

            }
        }

        string exportString = "";
        for(int i=0; i<export.Length; i++){
            exportString += export[i].ToString() + ",";
        }
        System.IO.File.WriteAllText(Application.dataPath + "/export.txt", exportString);
        

        /*
        float[] river = new float[heightMap.Length];
        river = features.RiverStencil(heightMap,w);

        float high = 0;
        float low = 1000000;
        for(int i=0; i<river.Length; i++){
            if(river[i]<low){
                low = river[i];
            }
            if(river[i] > high){
                high = river[i];
            }
        }
        Debug.Log(low);
        Debug.Log(high);

        for(int i=0; i<river.Length; i++){
            river[i]/=high;
        }

        Texture2D stencilTex = new Texture2D(w,h);
        float[] stencil = river;
        float[] scaledStencil = stencil;

        
        Color[] stencilMap = new Color[scaledStencil.Length];
        for(int i=0; i<scaledStencil.Length; i++){
            stencilMap[i] = new Color(scaledStencil[i],scaledStencil[i],scaledStencil[i],1);
        }
        stencilTex.SetPixels(stencilMap);
        stencilTex.Apply();
        byte[] StencilBytes = stencilTex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Stencil.png",StencilBytes);

        */
        /*
        WorldFeatures features = new WorldFeatures();

        WorldTex txr2 = new WorldTex(new Texture2D(w,h));

        float[] heightMap = features.HexElevationStencil(txr2,generator);
        float[] ridges = features.TectonicPlateStencil(new WorldTex(new Texture2D(w,h)),generator);

        heightMap = AddArray(ridges,heightMap);*/

        

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = PopulateVerts(heightMap,w,h);
        mesh.normals = PopulateNormals(w,h,mesh.vertices);
        mesh.uv = PopulateUV(w,h);
        mesh.triangles = PopulateTriangles(w,h);
        mesh.RecalculateNormals();


      
    }

    public float[] Scale(float[] input,int w,int h,int factor)
    {
        float[] output = new float[(int)(w/factor) * (int)(h/factor)];
        int i=0;
        for(int y=0; y<(int)(h/factor); y++){
            for(int x=0; x<(int)(w/factor); x++){
                
                float avg = 0;
                for(int ly = y*factor; ly<(y+1)*factor; ly++){
                    for(int lx = x*factor; lx<(x+1)*factor; lx++){
                        avg += input[lx + ly*w];
                    }
                }
                avg/=factor*factor;

                avg/=(factor/2);
            

                output[i] = avg;

                i++;
            }
        }
        return output;
    }

    public float[] AddArray(List<float[]> input)
    {
        float[] output = new float[input[0].Length];
        for(int i=0; i<input[0].Length; i++){
            for(int j=0; j<input.Count; j++){
                output[i] += input[j][i];
            }
            
        }
        return output;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
