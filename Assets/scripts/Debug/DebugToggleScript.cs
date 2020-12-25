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
        if (type == 0){
            debugText.GetComponent<DebugTextScript>().setDebugDisplayStatus(0,m_Toggle.isOn);
        }
        if (type == 5){
            Debug.Log("trying to change list in menu controler");
            debugText.GetComponent<DebugTextScript>().setDebugDisplayStatus(5,m_Toggle.isOn);
        }
    }

    
}
