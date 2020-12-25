using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldTex
{
    private Texture2D txr;
    public Texture2D GetTxr(){return txr;}

    private int width;
    private int height;

    private float addRule;
    public void SetAddRule(string rule)
    {
        if(rule == "add"){
            addRule = 1;
        }
        if(rule == "subtract"){
            addRule = -1;
        }
        if(rule == "set"){
            addRule = 0;
        }
    }

    private float strength = 1;
    public void SetStrength(float f)
    {
        strength = f;
    }

    private string drawRule = "Constant";
    public void SetDrawRule(string rule)
    {
        drawRule = rule;
    }


    private int radius = 1;
    public void SetRadius(int n)
    {
        radius = n;
    }

    public void UpdateBrushBuffer()
    {
        Color[] buffer = new Color[radius*radius*4];
        int i = 0;
        for(int y=0; y<radius*2; y++){
            for(int x=0; x<radius*2; x++){
                buffer[i] = new Color(0,0,0,1);
                i++;
            }
        }

        i=0;
        if(drawRule == "constant"){
            for(int y=0; y<radius*2; y++){
                for(int x=0; x<radius*2; x++){
                    float dist = (new Vector2(x,y) - new Vector2(radius,radius)).magnitude;
                    if(dist<radius){
                        buffer[i] = new Color(strength,strength,strength,1);
                    }
                    i++;
                }
            }
        }
        if(drawRule == "linear"){
            for(int y=0; y<radius*2; y++){
                for(int x=0; x<radius*2; x++){
                    float dist = (new Vector2(x,y) - new Vector2(radius,radius)).magnitude;
                    if(dist<radius){
                        float value = -1 * dist / radius + 1;
                       buffer[i] = new Color(value*strength,value*strength,value*strength,1);
                    }
                    i++;
                }
            }
        }
        if(drawRule == "sin_plus"){
            for(int y=0; y<radius*2; y++){
                for(int x=0; x<radius*2; x++){
                    float dist = (new Vector2(x,y) - new Vector2(radius,radius)).magnitude;
                    if(dist<radius){
                        float value = Mathf.Sin(dist*(float)3.1415/2/radius + (float)3.1415/2);
                        buffer[i] = new Color(value*strength,value*strength,value*strength,1);
                    }
                    i++;
                }
            }
        }
        if(drawRule == "sin_minus"){
            for(int y=0; y<radius*2; y++){
                for(int x=0; x<radius*2; x++){
                    float dist = (new Vector2(x,y) - new Vector2(radius,radius)).magnitude;
                    if(dist<radius){
                        float value = Mathf.Sin(dist*(float)3.1415/2/radius + (float)3.1415) + 1;
                        buffer[i] = new Color(value*strength,value*strength,value*strength,1);
                    }
                    i++;
                }
            }
        }

        brushBuffer = buffer;
    }

    private Color[] texBuffer;
    private Color[] brushBuffer;


    public WorldTex(Texture2D texture)
    {
        txr = texture;
        width = texture.width;
        height = texture.height;
        texBuffer = new Color[width * height];
        for(int i=0; i<texBuffer.Length; i++){
            texBuffer[i] = new Color(0,0,0,1);
        }
    }

    public double[] Get1D()
    {
        double[] output = new double[width * height];
        int i=0;
        for(int y=0; y<height; y++){
            for(int x=0; x<width; x++){
                output[i] = (double)texBuffer[i].grayscale;
                i++;
            }
        }
        return output;
    }

    public void DrawLine(Vector2 p1, Vector2 p2)
    {
        Vector2 t = p1;
        float frac = 1/Mathf.Sqrt (Mathf.Pow (p2.x - p1.x, 2) + Mathf.Pow (p2.y - p1.y, 2));
        float ctr = 0;
        if(p2.x + radius > txr.width || p2.y + radius > txr.height || p2.x - radius < 0 || p2.y - radius < 0
        || p1.x + radius > txr.width || p1.y + radius > txr.height || p1.x - radius < 0 || p1.y - radius < 0){
            return;
        }
        while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y) {
            t = Vector2.Lerp(p1, p2, ctr);
            ctr += frac;
            Draw(new Vector2((int)t.x,(int)t.y));
        }
    }
    
    public void Draw(Vector2 pos)
    {
        int i = 0;
        int j = 0;
        int localx = 0;
        int localy = 0;
        for(int y=(int)pos.y - radius; y<(int)pos.y + radius; y++){
            for(int x=(int)pos.x - radius; x<(int)pos.x + radius; x++){
                i = y * width + x;
                j = localy * radius * 2 + localx;
                if(texBuffer[i].grayscale < brushBuffer[j].grayscale){
                    texBuffer[i] = brushBuffer[j];
                }

                localx++;
            }
            localy++;
            localx = 0;
        }

        
    }

    public void ApplyTexBuffer()
    {
        txr.SetPixels(texBuffer);
    }
}