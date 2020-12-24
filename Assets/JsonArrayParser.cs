using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JsonArrayParser
{
    public List<string> ar = new List<string>();
    public JsonArrayParser(string data)
    {
        string str = "";
        bool isArray = false;
        foreach (char c in data){
            if(c == ']'){
                isArray = false;
            }
            if(isArray){
                str+=c;
            }
            if(c == '['){
                isArray = true;
            }
        }
        ar = parse(str);
    }

    public List<string> parse(string data)
    {
        int subCount = 0;
        string substr = "";
        List<string> result = new List<string>();
        foreach (char c in data){
            
            if(c == '{'){
                subCount+=1;
            }
            if(subCount > 0){
                substr+=c;
            }
            if(c == '}'){
                subCount-=1;
                if(subCount == 0){
                    result.Add(substr);
                    substr = "";
                }
            }
            
        }
        return result;
    }
}