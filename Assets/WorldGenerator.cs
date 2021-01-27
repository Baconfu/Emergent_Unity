using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator
{

    List<TectonicPlate> world = new List<TectonicPlate>();

    private float hexRadius = 500;
    private Vector2 plateXVector;
    private Vector2 plateYVector;


    private TectonicPlate[] GetAdjacentPlates(Vector2 pos)
    {
        TectonicPlate[] output = new TectonicPlate[6];
        output[0] = GetPlate(pos + new Vector2(1,0));
        output[1] = GetPlate(pos + new Vector2(0,1));
        output[2] = GetPlate(pos + new Vector2(-1,1));
        output[3] = GetPlate(pos + new Vector2(-1,0));
        output[4] = GetPlate(pos + new Vector2(0,-1));
        output[5] = GetPlate(pos + new Vector2(1,-1));
        return output;
    }

    private TectonicPlate GetPlate(Vector2 pos)
    {
        for(int i=0; i<world.Count; i++){
            if(world[i].GetPos() == pos){
                return world[i];
            }
        }
        return null;
    }

    public TectonicPlate GeneratePlate(Vector2 p)
    {
        Vector2 pos = p.x * plateXVector + p.y * plateYVector;
        TectonicPlate plate = new TectonicPlate(pos,p);

        plate.setNeighbours(GetAdjacentPlates(p));
        plate.populateHexes();
        plate.populateRidges();
        world.Add(plate);
        return plate;
    }

    public List<Ridge> GetAllRidges()
    {
        List<Ridge> output = new List<Ridge>();
        for(int i=0; i<world.Count; i++){
            for(int j=0; j<6; j++){
                Ridge ridge = world[i].GetRidge(j);
                if(!output.Contains(ridge)){
                    output.Add(ridge);
                }
            }
        }
        return output;  
    }

    public List<Hex> GetAllHexes()
    {
        List<Hex> output = new List<Hex>();
        for(int i=0; i<world.Count; i++){
            for(int j=0; j<6; j++){
                Hex hex = world[i].GetHex(j);
                if(!output.Contains(hex)){
                    output.Add(hex);
                }
            }
        }
        return output;
    }

    public List<TectonicPlate> GetAllPlates()
    {
        return world;
    }

    public WorldGenerator()
    {
        plateXVector = new Vector2(2 * hexRadius * Mathf.Pow(2*(1-Mathf.Cos(2*(float)3.1415926/3)),(float)0.5),0);
        plateYVector = new Vector2(hexRadius * Mathf.Pow(2*(1 - Mathf.Cos(2*(float)3.1415926/3)),(float)0.5),3*hexRadius);
    }



}