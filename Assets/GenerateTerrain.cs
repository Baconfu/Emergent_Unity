using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class GenerateTerrain : MonoBehaviour
{


    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    
    public Vector3[] PopulateVerts(double[] heights,int width,int height)
    {
        float highest = 0;
        for(int a=0; a<heights.Length; a++){
            if(heights[a]>highest){
                highest = (float)heights[a];
            }
        }
        if(highest<1){
            highest = 1;
        }
        Vector3[] output = new Vector3[width * height];
        int i = 0;
        for(int z=0; z<height; z++){
            for(int x=0; x<width; x++){
                output[i] = new Vector3((float)x / width,(float)heights[i]/highest,(float)z / height) - new Vector3((float)0.5,(float)0.5,(float)0.5);

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
        int w = 512;
        int h = 512;
        Texture2D texture = new Texture2D(w,h);
        for(int i=0; i<w; i++){
            for(int j=0; j<h; j++){
                texture.SetPixel(i,j,new Color((float)0,(float)0,(float)0,1));
            }
        }

        WorldTex txr = new WorldTex(texture);
        
        WorldGenerator generator = new WorldGenerator();

        List<Vector2> plates = new List<Vector2>();
        
        for(int i=0; i<7; i++){
            for(int j=0; j<7; j++){
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
        txr.SetRadius(70);
        txr.SetStrength((float)0.025);
        txr.SetAddRule("add");
        txr.UpdateBrushBuffer();

        for(int i=0; i<allRidges.Count; i++){
            List<Vector2> ridge = allRidges[i].GetPath();
            for(int j=0; j<ridge.Count-1; j++){
                txr.DrawLine(ridge[j]/15,ridge[j+1]/15);
            }
        }
        txr.ApplyTexBuffer();
        Texture2D tex = txr.GetTxr();
        tex.Apply();


        
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Saved.png",bytes);
            



        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        

        
        double[] heightMap = txr.Get1D();

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = PopulateVerts(heightMap,w,h);
        mesh.normals = PopulateNormals(w,h,mesh.vertices);
        mesh.uv = PopulateUV(w,h);
        mesh.triangles = PopulateTriangles(w,h);
        mesh.RecalculateNormals();



    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
