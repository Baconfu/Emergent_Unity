using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ridge
{

    private Hex originHex;
    private Hex destinationHex;

    private TectonicPlate[] neighbours;

    private List<Vector3> path;

    private float foldCharacter;
    private float blockCharacter;
    private float volcanicCharacter;

    public bool finalized = false;

    public void SetCharacter(float[] character)
    {
        foldCharacter = character[0];
        blockCharacter = character[1];
        volcanicCharacter = character[2];
    }

    public Vector2 RotateVector(Vector2 vector, float angle)
    {
        return new Vector2(vector.magnitude * Mathf.Cos(GetVectorAngle(vector) + angle),
        vector.magnitude * Mathf.Sin(GetVectorAngle(vector) + angle));
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


    public void RegisterNeighbour(TectonicPlate plate)
    {
        if(neighbours[0] == null){
            neighbours[0] = plate;
        }else{
            neighbours[1] = plate;
            FinalizeRidge();
        }
    }



    private void FinalizeRidge()
    {
        finalized = true;
        float collisionCoefficient = 0;

        Vector2 vector = neighbours[1].GetRealPos() - neighbours[0].GetRealPos();

        collisionCoefficient += Vector2.Dot(neighbours[0].GetDrift(),vector.normalized);
        collisionCoefficient -= Vector2.Dot(neighbours[1].GetDrift(),vector.normalized);

        float slideCoefficient = 0;

        Vector2 vector2 = new Vector2(-vector.y,vector.x);

        slideCoefficient += Vector2.Dot(neighbours[0].GetDrift(),vector2.normalized);
        slideCoefficient -= Vector2.Dot(neighbours[1].GetDrift(),vector2.normalized);


        if(collisionCoefficient>0){
            foldCharacter = collisionCoefficient;
            volcanicCharacter = 0;
        }else{
            volcanicCharacter = Mathf.Abs(collisionCoefficient);
            foldCharacter = 0;
        }

        blockCharacter = Mathf.Abs(slideCoefficient);

        CreatePath();
    }

    public Ridge(Hex Origin,Hex Destination)
    {
        originHex = Origin;
        destinationHex = Destination;
        neighbours = new TectonicPlate[2];
    }

    public void CreatePath()
    {
        List<Vector3> ridge = new List<Vector3>();
        Vector2 origin = originHex.GetFocalPoint();
        Vector2 destination = destinationHex.GetFocalPoint();

        float initialElevation = originHex.GetFocalPointElevation();
        float endElevation = destinationHex.GetFocalPointElevation();

        ridge.Add(new Vector3(origin.x,origin.y,initialElevation));

        float randomAngle = Random.Range(-0.1666f,0.1666f);

        Vector2 vector = RotateVector(destination - origin, randomAngle);

        float initialLength = (destination-origin).magnitude;
        float cumulativeLength = 0;


        float elevation = initialElevation;


        int i=0;

        while(true)
        {
            if(i>1000){
                break;
            }

            float angle = Random.Range(-0.25f,0.25f);
            vector = vector.normalized;
            Vector2 dest = destination - new Vector2(ridge[i].x,ridge[i].y);
            float deltaElevation = Random.Range(-0.002f,0.002f);
            if(foldCharacter>0){
                deltaElevation = Random.Range(-0.002f,0.002f);
            }
            if(volcanicCharacter>0){
                deltaElevation = Random.Range(-0.02f,0.02f);
            }

            

            

            if(dest.magnitude<initialLength / 6){
                //as dest gets smaller, the factor increases, thus angle is reduced
                angle /= ((initialLength/5 - dest.magnitude) / 10);

            }
            if(dest.magnitude<10){
                ridge.Add(new Vector3(destination.x,destination.y,endElevation));
                break;
            }
            if(dest != null){

                float elevationTendency = (endElevation - ridge[i].z);
                
                float tendency = GetVectorAngle(dest) - GetVectorAngle(vector);
                if(tendency > 3.1415926){
                    tendency -= (float)3.1415926 * 2;
                }
                if(tendency <-3.1415926){
                    tendency += (float)3.1415926 * 2;
                }

                if(cumulativeLength < initialLength/2){
                    tendency/=10;
                    elevationTendency/=10;
                    
                }
                angle += tendency/3;
                deltaElevation += elevationTendency / 5;

            }
            vector = RotateVector(vector,angle);

            float length = Random.Range(2.0f,4.0f);
            length *= initialLength/565;
            cumulativeLength += length;

            vector*=length;

            ridge.Add(ridge[i] + new Vector3(vector.x,vector.y,deltaElevation));
            i+=1;
        }
        
        path = ridge;


    }

    public List<Vector3> GetPath()
    {
        return path;
    }
}