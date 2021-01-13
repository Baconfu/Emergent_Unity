using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldTex
{
    private Texture2D txr;

    private float[] texBuffer;
    private float[] brushBuffer;

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

    private float ratio;


    private int radius = 1;





    public void SetRadius(int n)
    {
        radius = n;
    }

    public void setRatio(){ratio = radius / strength;}

    public void UpdateBrushBuffer()
    {
        float[] buffer = new float[radius*radius*4];
        int i = 0;
        for(int y=0; y<radius*2; y++){
            for(int x=0; x<radius*2; x++){
                buffer[i] = 0;
                i++;
            }
        }

        i=0;
        if(drawRule == "constant"){
            for(int y=0; y<radius*2; y++){
                for(int x=0; x<radius*2; x++){
                    float dist = (new Vector2(x,y) - new Vector2(radius,radius)).magnitude;
                    if(dist<radius){
                        buffer[i] = strength;
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
                       buffer[i] = value * strength;
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
                        buffer[i] = value * strength;
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
                        buffer[i] = value * strength;
                    }
                    i++;
                }
            }
        }

        brushBuffer = buffer;
    }

    public WorldTex(Texture2D texture)
    {
        txr = texture;
        width = texture.width;
        height = texture.height;
        texBuffer = new float[width * height];
        for(int i=0; i<texBuffer.Length; i++){
            texBuffer[i] = 0;
        }
    }

    public float[] GetTexBuffer()
    {
        return texBuffer;
    }

    private void ApplyElevation(float f)
    {
        SetStrength(f);
        SetRadius((int)(f * ratio));
    }

    public void fillAdd(float f)
    {
        for(int i=0; i<texBuffer.Length; i++){
            texBuffer[i]+=f;
        }
    }

    public void DrawLine(Vector3 pt1, Vector3 pt2)
    {

        Vector2 p1  = new Vector2(pt1.x,pt1.y);
        Vector2 p2 = new Vector2(pt2.x,pt2.y);
        Vector2 t = p1;
        float frac = 1/Mathf.Sqrt (Mathf.Pow (p2.x - p1.x, 2) + Mathf.Pow (p2.y - p1.y, 2));
        float ctr = 0;

        float deltaElevation = frac * (pt2.z-pt1.z);
        float elevation = pt1.z;

        if(pt1.z>pt2.z){
            ApplyElevation(pt1.z);
        }else{
            ApplyElevation(pt2.z);
        }
        if(p1.x + radius > txr.width || p1.x - radius < 0 || p1.y+radius > txr.height || p1.y - radius<0 || p2.x + radius > txr.width || p2.x - radius < 0 || p2.y + radius>txr.height || p2.y - radius<0){
            return;
        }
        UpdateBrushBuffer();

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
                if(texBuffer[i] < brushBuffer[j]){
                    texBuffer[i] = brushBuffer[j];
                }

                localx++;
            }
            localy++;
            localx = 0;
        }

        
    }

    public void DrawAddStencil(float[] data,int w,int h,Vector2 pos)
    {
        if(pos.x + w > txr.width || pos.x - w < 0 || pos.y + h > txr.height || pos.y - h < 0){
            return;
        }
        int i=0;
        int j = 0;
        int localx = 0;
        int localy = 0;
        for(int y=(int)pos.y - h/2; y<(int)pos.y + h/2; y++){
            for(int x=(int)pos.x - w/2; x<(int)pos.x + w/2; x++){
                i = y * width + x;
                j = localy * w + localx;
                
                texBuffer[i]  = texBuffer[i] + data[j];
                /*if(texBuffer[i].grayscale < buffer[j].grayscale){
                    texBuffer[i] = buffer[j];
                }*/

                localx++;
            }
            localy++;
            localx = 0;
        }

    }

    public void Draw(Vector2 pos,int r,float s)
    {
        r = Mathf.Abs(r);
        if(pos.x + r > txr.width || pos.x - r < 0 || pos.y + r > txr.height || pos.y - r < 0){
            return;
        }
        

        
        

        //Debug.Log("drawn, r: " + r + " s: " + s);
        float[] buffer = new float[r*r*4];
        int i=0;
        for(int y=0; y<r*2; y++){
            
            for(int x=0; x<r*2; x++){
                buffer[i] = 0;
                i++;
            }
        }
        i=0;
        
     

        for (int y = 0; y < r * 2; y++)
        {
            for (int x = 0; x < r * 2; x++)
            {
                float dist = (new Vector2(x, y) - new Vector2(r, r)).magnitude;
                if (dist < r)
                {
                    //float value = Mathf.Sin((dist-(0.5f*r/2)) * 3.1415926f*2 / r) + 1;
                    float value = Mathf.Sin(dist * (float)3.1415 / 2 / r + (float)3.1415) + 1;
                    //float value = -1 * dist / r + 1;
                    
                    buffer[i] = value * s;
                    
                }
                i++;
            }
        }

        i=0;
        int j = 0;
        int localx = 0;
        int localy = 0;
        for(int y=(int)pos.y - r; y<(int)pos.y + r; y++){
            for(int x=(int)pos.x - r; x<(int)pos.x + r; x++){
                i = y * width + x;
                j = localy * r * 2 + localx;
                
                texBuffer[i]  = texBuffer[i] + buffer[j];
                /*if(texBuffer[i].grayscale < buffer[j].grayscale){
                    texBuffer[i] = buffer[j];
                }*/

                localx++;
            }
            localy++;
            localx = 0;
        }
    }

    public void ApplyTexBuffer()
    {
        Color[] colorMap = new Color[texBuffer.Length];
        for(int i=0; i<texBuffer.Length; i++){
            colorMap[i] = new Color(texBuffer[i],texBuffer[i],texBuffer[i],1);
        }
        txr.SetPixels(colorMap);
    }
}