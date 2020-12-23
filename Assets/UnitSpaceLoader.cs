using UnityEngine;


[System.Serializable]
public class UnitSpaceLoader
{
    public string type;

    public static UnitSpaceLoader CreateFromJson(string str)
    {
        return JsonUtility.FromJson<UnitSpaceLoader>(str);
    }
}