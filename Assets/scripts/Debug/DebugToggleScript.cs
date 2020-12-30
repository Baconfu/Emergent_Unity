using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugToggleScript : MonoBehaviour
{
    public int type;
    Toggle m_Toggle;

    GameObject debugText;
    Component debugTextScript;

    void Start()
    {
        debugText = GameObject.Find("DebugText");
        debugTextScript = GameObject.Find("DebugText").GetComponent<DebugTextScript>();
        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });
        m_Toggle.isOn = debugText.GetComponent<DebugTextScript>().getDebugDisplayStatus((DebugTextScript.debugCategory)type);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleValueChanged(Toggle change)
    {
        debugText.GetComponent<DebugTextScript>().setDebugDisplayStatus(type, m_Toggle.isOn);

        /*if (type == 0)
        {
            
        }

        if (type == 2)
        {
            debugText.GetComponent<DebugTextScript>().setDebugDisplayStatus(2, m_Toggle.isOn);
        }

        if (type == 5)
        {
            debugText.GetComponent<DebugTextScript>().setDebugDisplayStatus(5,m_Toggle.isOn);
        }

        if (type == 7)
        {
            debugText.GetComponent<DebugTextScript>().setDebugDisplayStatus(7,m_Toggle.isOn);
        }*/
    }

    
}
