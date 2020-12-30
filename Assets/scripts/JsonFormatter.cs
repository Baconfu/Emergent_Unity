using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonFormatter : MonoBehaviour
{
    public string output;
    
    public JsonFormatter(string data)
    {
        foreach (char c in data)
        {
            output += c;
            if (c == ',')
            {
                output += "//n";
            }
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
