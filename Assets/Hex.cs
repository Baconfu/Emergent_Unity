using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex
{
    private Vector2 pos;
    public Vector2 GetPos(){return pos;}
    public void SetPos(Vector2 p){pos = p;}

    private float focalPointElevation;
    public float GetFocalPointElevation(){return focalPointElevation;}
    public void SetFocalPointElevation(float f){focalPointElevation = f;}


    private Vector2 focalPoint;
    public Vector2 GetFocalPoint(){return focalPoint + pos;}

    private float radius = 500;


    public Hex(Vector2 Pos)
    {
        pos = Pos;
        float angle = Random.Range(0.000f, 6.283f);
        float dist = Random.Range(0.0f,500.0f);

        focalPoint = new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);

    }
}