using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex
{
    private Vector2 pos;
    public Vector2 GetPos(){return pos;}
    public void SetPos(Vector2 p){pos = p;}

    public bool finalized;

    private List<TectonicPlate> adjacent;
    public void RegisterAdjacent(TectonicPlate plate)
    {
        
        adjacent.Add(plate);
        if(adjacent.Count == 3){
            FinalizeHex();
        }
        
    }

    public float GetVectorAngle(Vector2 vector)
    {
        float angle = Mathf.Atan(vector.y/vector.x);
        if(vector.x<0){
            angle += (float)3.1415926;
        }else{
            if(vector.y<0){
                angle += 2*(float)3.1415926;
            }
        }
        return angle;
    }

    private void FinalizeHex()
    {
        finalized = true;

        float[] collisionCoefficients = new float[3];

        Vector2 vector = adjacent[1].GetRealPos() - adjacent[0].GetRealPos();

        collisionCoefficients[0] = 0;

        collisionCoefficients[0] += Vector2.Dot(adjacent[0].GetDrift(),vector.normalized);
        collisionCoefficients[0] -= Vector2.Dot(adjacent[1].GetDrift(),vector.normalized);

        vector = adjacent[2].GetRealPos() - adjacent[1].GetRealPos();
        
        collisionCoefficients[1] = 0;

        collisionCoefficients[1] += Vector2.Dot(adjacent[1].GetDrift(),vector.normalized);
        collisionCoefficients[1] -= Vector2.Dot(adjacent[2].GetDrift(),vector.normalized);

        vector = adjacent[0].GetRealPos() - adjacent[2].GetRealPos();
        
        collisionCoefficients[2] = 0;

        collisionCoefficients[2] += Vector2.Dot(adjacent[2].GetDrift(),vector.normalized);
        collisionCoefficients[2] -= Vector2.Dot(adjacent[0].GetDrift(),vector.normalized);
        
        for(int i=0; i<3; i++){
            //Debug.Log("Ridge: " +  i  + " collision coefficient: " + collisionCoefficients[i]);
        }

        hexElevation = collisionCoefficients[0] + collisionCoefficients[1] + collisionCoefficients[2];

    }

    private float focalPointElevation;

    private float hexElevation = 0;
    public float GetHexElevation(){return hexElevation;}
    public float GetFocalPointElevation(){return focalPointElevation;}
    public void SetFocalPointElevation(float f){focalPointElevation = f;}


    private Vector2 focalPoint;
    public Vector2 GetFocalPoint(){return focalPoint + pos;}

    private float radius = 500;


    public Hex(Vector2 Pos)
    {
        adjacent = new List<TectonicPlate>();

        pos = Pos;
        float angle = Random.Range(0.000f, 6.283f);
        float dist = Random.Range(0.0f,500.0f);
        
        focalPoint = new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);

        SetFocalPointElevation(Random.Range(0.001f,0.04f));
    }
}