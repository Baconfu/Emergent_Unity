using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ridge
{

    private Hex originHex;
    private Hex destinationHex;

    private List<Vector2> path;

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




    public Ridge(Hex Origin,Hex Destination)
    {
        originHex = Origin;
        destinationHex = Destination;


    }

    public void CreatePath()
    {
        List<Vector2> ridge = new List<Vector2>();
        Vector2 origin = originHex.GetFocalPoint();
        Vector2 destination = destinationHex.GetFocalPoint();

        ridge.Add(origin);

        float randomAngle = Random.Range(-0.1666f,0.1666f);


        Vector2 vector = RotateVector(destination - origin, randomAngle);

        float initialLength = (destination-origin).magnitude;
        float cumulativeLength = 0;

        int i=0;

        while(true)
        {
            if(i>1000){
                break;
            }

            float angle = Random.Range(-0.25f,0.25f);
            vector = vector.normalized;
            Vector2 dest = destination - ridge[i];

            if(dest.magnitude<initialLength / 6){
                //as dest gets smaller, the factor increases, thus angle is reduced
                angle /= ((initialLength/5 - dest.magnitude) / 10);
            }
            if(dest.magnitude<10){
                ridge.Add(destination);
                break;
            }
            if(dest != null){
                float tendency = GetVectorAngle(dest) - GetVectorAngle(vector);
                if(tendency > 3.1415926){
                    tendency -= (float)3.1415926 * 2;
                }
                if(tendency <-3.1415926){
                    tendency += (float)3.1415926 * 2;
                }
                if(cumulativeLength < initialLength/2){
                    tendency/=10;
                }
                angle += tendency/5;

            }
            vector = RotateVector(vector,angle);

            float length = Random.Range(2.0f,4.0f);
            length *= initialLength/565;
            cumulativeLength += length;

            ridge.Add(ridge[i] + length * vector);
            i+=1;
        }
        
        path = ridge;


    }

    public List<Vector2> GetPath()
    {
        return path;
    }
}