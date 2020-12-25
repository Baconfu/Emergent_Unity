using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;

    public Vector3 m_Velocity;

    Quaternion m_Rotation;
    public float RotationSpeed;
    public float MovementSpeed;
    public float JumpHeight;

    public List<bool> ContextList;


    public enum Context
    {
        InAir = 0,
        JumpDisabled = 1,
        Walking = 2,
        Placing = 100,



        ///don't delete NumberOfContexts: it is necessary to find the total number of contexts
        NumberOfContexts

    }


    // Start is called before the first frame update
    void Start()
    {
        Quaternion initialRotation = Quaternion.identity;
        Vector3 initialPosition = new Vector3(0f, 2f, 0f);

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        transform.SetPositionAndRotation(initialPosition, initialRotation);
        m_Rotation = initialRotation;

        for (int i = 0; i < (int)Context.NumberOfContexts; i++)
        {
            ContextList.Add(false);
        }

        
        
    }

    // Update is called once per frame
    void Update()
    {
        

        float HorizontalMovement = Input.GetAxisRaw("Horizontal");
        float VerticalMovement = Input.GetAxisRaw("Vertical");

        
        //IsWalking will be used later for animation stuff
        if ((HorizontalMovement != 0 || VerticalMovement != 0) && !GetContext(Context.InAir))
        {
            SetContext(Context.Walking, true);
        }
        else
        {
            SetContext(Context.Walking, false);
        }


        //Debug.Log(IsWalking);

        m_Velocity = new Vector3(HorizontalMovement, 0, VerticalMovement);
        m_Velocity.Normalize();
        if (GetContext(Context.InAir))
        {
            m_Velocity = m_Velocity * 0.5f;
        }

        //Rotating the direction of movement according to camera orientation
        float CameraYDirectionRadian = GameObject.Find("Camera").transform.rotation.eulerAngles[1];
        m_Velocity = Quaternion.AngleAxis(CameraYDirectionRadian, Vector3.up) * m_Velocity;

        if (Mathf.Approximately(Input.GetAxis("Jump"), 1) && GetContext(Context.InAir)==false && !GetContext(Context.JumpDisabled))
        {
            m_Rigidbody.AddForce(new Vector3(0, JumpHeight * 4, 0),ForceMode.Impulse);
            SetContext(Context.InAir, true);
            SetContext(Context.JumpDisabled, true);
            StartCoroutine("JumpDisabler");
        }

        m_Rotation = Quaternion.Euler(transform.forward);


        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Velocity * Time.deltaTime * MovementSpeed / 10);
        m_Rigidbody.MoveRotation(Quaternion.RotateTowards(m_Rotation, Quaternion.Euler(m_Velocity), RotationSpeed * Time.deltaTime));

        

        //Debug.Log(m_Collider.bounds);
    }

    private void LateUpdate()
    {
        UpdateContext();
    }

    private void OnCollisionStay(Collision collision)
    {
        //SetContext(Context.InAir, false);
    }

    bool GetContext(Context target)
    {
        return ContextList[(int)target];
    }

    void SetContext(Context target, bool desired)
    {
        ContextList[(int)target] = desired;
    }

    void UpdateContext()
    {

        if (!GetContext(Context.JumpDisabled))
        {
            List<Vector3> BottomCorners = new List<Vector3>();


            BottomCorners.Add(m_Collider.bounds.min + new Vector3(-0.01f, 0, -0.01f));
            BottomCorners.Add(m_Collider.bounds.min + new Vector3(m_Collider.bounds.size[0]+0.01f, 0, -0.01f));
            BottomCorners.Add(m_Collider.bounds.min + new Vector3(-0.01f, 0, m_Collider.bounds.size[2]+0.01f));
            BottomCorners.Add(m_Collider.bounds.min + new Vector3(m_Collider.bounds.size[0]+0.01f, 0, m_Collider.bounds.size[2]+0.01f));



            for (int i = 0; i < 4; i++)
            {
                RaycastHit Hit;
                Ray DownRay = new Ray(BottomCorners[i]+new Vector3(0,0.01f,0), Vector3.down);
                Physics.Raycast(DownRay, out Hit);
                //Debug.Log(i.ToString() + BottomCorners[i].ToString() + ":" + Hit.distance.ToString());

                if (Hit.distance < 0.1)
                {
                    SetContext(Context.InAir, false);
                    break;
                }

                SetContext(Context.InAir, true);
                
                
            }
        
            
        }

    }

    IEnumerator JumpDisabler()
    {
        yield return new WaitForSeconds(0.5f);
        SetContext(Context.JumpDisabled, false);
    }

    IEnumerator JumpEnabler()
    {
        yield return new WaitForSeconds(0.5f);
        SetContext(Context.JumpDisabled, true);
    }
}
