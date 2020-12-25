using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TectonicPlate
{
    private Vector2 realPos;
    private Vector2 pos;

    public void SetPos(Vector2 p){pos = p;}
    public Vector2 GetPos(){return pos;}
    public void SetRealPos(Vector2 p){realPos = p;}
    public Vector2 GetRealPos(){return realPos;}

    private int hexRadius = 500;

    private Vector3 drift;


    private TectonicPlate[] neighbours = new TectonicPlate[6];
    private Hex[] border = new Hex[6];
    public Hex GetHex(int index){return border[index];}

    private Hex center;
    private Ridge[] edges = new Ridge[6];
    public Ridge GetRidge(int index){return edges[index];}


    public TectonicPlate(Vector2 RealPos,Vector2 Pos)
    {
        realPos = RealPos;
        pos = Pos;
        
    }


    public void setNeighbours(TectonicPlate[] adjacent)
    {
        neighbours = adjacent;
        for(int i=0; i<adjacent.Length; i++){
            if(adjacent[i] != null){
                adjacent[i].setNeighbour((i+3)%6,this);
                edges[i] = adjacent[i].GetRidge((i+3)%6);
                border[i] = adjacent[i].GetHex((i+2)%6);
                if(i == 0){
                    border[5] = adjacent[0].GetHex(3);
                }
                else{
                    border[i-1] = adjacent[i].GetHex((i+3)%6);
                }
            }
        }
    }


    public void setNeighbour(int index, TectonicPlate plate)
    {
        neighbours[index] = plate;
    }


    public void populateRidges()
    {
        for(int i=0; i<edges.Length; i++){
        if(edges[i] == null){
            if(i== 0){
                
                edges[i] = new Ridge(border[5],border[0]);
            }else{
                edges[i] = new Ridge(border[i-1],border[i]);
            }
            edges[i].CreatePath();

        }

        }
    }

    
    public void populateHexes()
    {
        for(int i=0; i<6; i++){
            if(border[i] == null){
                float angle = (float)3.1415926/6 + i*(float)3.1415926/3;
                border[i] = new Hex(realPos + new Vector2(2*hexRadius * Mathf.Cos(angle), 2*hexRadius * Mathf.Sin(angle)));
            }
        }
    }
}