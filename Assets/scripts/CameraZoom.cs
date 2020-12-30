using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float minZoom;
    public float maxZoom;
    
    public float sensitivity;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float zoom = GetComponent<Cinemachine.CinemachineFreeLook>().m_Lens.OrthographicSize;
            zoom += -Input.GetAxis("Mouse ScrollWheel") * sensitivity / 3;
            //Debug.Log("zoom:"+zoom);
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            //Debug.Log("zoom after clamp:" + zoom);
            GetComponent<Cinemachine.CinemachineFreeLook>().m_Lens.OrthographicSize = zoom;
            
        }
        //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
    }
}
