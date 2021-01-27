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
        
        Random.InitState(23);
        int w = 256;
        int h = 256;
        Texture2D texture = new Texture2D(w,h);
        for(int i=0; i<w; i++){
            for(int j=0; j<h; j++){
                texture.SetPixel(i,j,new Color((float)0,(float)0,(float)0,1));
            }
        }

        float worldScale = 20;

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

       
        


        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        

        
        
        //float[] heightMap = txr.GetTexBuffer();
        WorldFeatures features = new WorldFeatures();

        WorldTex txr2 = new WorldTex(new Texture2D(w,h));

        float[] heightMap = new float[w*h];

        float[] export = new float[30*30];

        
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

        heightMap = ScaleHeight(heightMap,1000/w);




        
        float[] init = new float[4];
        init[0] = 0.3f;
        init[1] = 0.5f;
        init[2] = 0.6f;
        init[3] = 0.7f;

        Fractal fractal = new Fractal();
        fractal.heightMap = init;
        fractal.w = 2;
        fractal.h = 2;
        fractal.intensity = 0.6f;

        fractal.IterateTo(8);
        
        int debugCount = 100;

        float[,] fractal2D = new float[fractal.w,fractal.h];
        int it = 0;
        for(int y=0; y<fractal.h; y++){
            for(int x=0; x<fractal.w; x++){
                fractal2D[x,y] = fractal.heightMap[it];
                it++;
            }
        }
        erosion = new Erosion(fractal2D,fractal.w,fractal.h,0.00000005f);

        float[,,] voxelMap = new GenerateVoxel().FromHeightMap(erosion.Get1D(),fractal.w,fractal.h,fractal.w);
        
        float[] alt =  new float[fractal.w*fractal.h];

        int iterator = 0;
        for(int z=0; z<fractal.h; z++){
            for(int x=0; x<fractal.w; x++){
                float surface = 0;
                for(int y=0; y<fractal.h; y++){
                    if(voxelMap[x,y,z] > 0){
                        surface = y;
                        break;
                    }
                }
                alt[iterator] = (float)surface / (float)fractal.h;
                iterator++;
            }
        }
        

        float[] map = alt;
        int width = fractal.w;
        int height = fractal.h;
        
        
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = PopulateVerts(map,width,height);
        mesh.normals = PopulateNormals(width,height,mesh.vertices);
        mesh.uv = PopulateUV(width,height);
        mesh.triangles = PopulateTriangles(width,height);
        mesh.RecalculateNormals();


      
    }


    Erosion erosion;
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

    public float[] ScaleHeight(float[] input,float factor)
    {
        float[] output = new float[input.Length];
        for(int i=0; i<input.Length; i++){
            output[i] = input[i] * factor;
        }
        return output;
    }
    public int iterator=1;


    private int fw = 2;
    private int fh = 2;

    private float[] fractal;
    // Update is called once per frame
    void Update()
    {
        /*
        if(iterator<100000){

            if(iterator%1000 == 0){
                Debug.Log("iterant");
                float[] map = erosion.Get1D();
                int width = erosion.w;
                int height = erosion.h;
                Mesh mesh = new Mesh();
                GetComponent<MeshFilter>().mesh = mesh;

                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                mesh.vertices = PopulateVerts(map,width,height);
                mesh.normals = PopulateNormals(width,height,mesh.vertices);
                mesh.uv = PopulateUV(width,height);
                mesh.triangles = PopulateTriangles(width,height);
                mesh.RecalculateNormals();
            }
            erosion.Iterate();

            

            
            iterator++;

        }else{
            Debug.Log("done");
        }*/
    }

    
}

public class GenerateVoxel
{
    public GenerateVoxel()
    {

    }
    
    public float[,,] FromHeightMap(float[] heightMap,int w,int h,int d)
    {
        int iterator = 0;
        float[,] temp = new float[w,h];
        for(int z=0; z<d; z++){
            for(int x=0; x<w; x++){
                temp[x,z] = heightMap[iterator];
                iterator++;
            }
        }
        return FromHeightMap(temp,w,h,d);
    }
    
    public float[,,] FromHeightMap(float[,] heightMap,int w,int h,int d)
    {
        float[,,] output = new float[w,h,d];
        for(int z=0; z<d; z++){
            for(int x=0; x<w; x++){
                float height = heightMap[x,z] * (float)d;
                for(int y=0; y<h; y++){
                    output[x,y,z] = (float)y - height;
                }
            }
        }

        return output;
    }
}
