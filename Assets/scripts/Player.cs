using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    public Collider m_Collider;

    public Collider inAirCollider;

    public Vector3 m_Velocity;

    Quaternion m_Rotation;
    public float RotationSpeed;
    public float MovementSpeed;
    public float JumpHeight;

    public List<bool> ContextList;

    public Animator m_Animator;

    public event EventHandler onInside;
    public event EventHandler onOutside;


    public enum Context
    {
        InAir = 0,
        JumpDisabled = 1,
        Walking = 2,
        Inside = 10,
        Placing = 100,



        ///don't delete NumberOfContexts: it is necessary to find the total number of contexts
        NumberOfContexts

    }


    // Start is called before the first frame update
    void Start()
    {
        Quaternion initialRotation = Quaternion.identity;
        Vector3 initialPosition = new Vector3(0f, 20f, 0f);

        m_Rigidbody = GetComponent<Rigidbody>();
        //m_Collider = GetComponent<Collider>();

        transform.SetPositionAndRotation(initialPosition, initialRotation);
        m_Rotation = initialRotation;

        for (int i = 0; i < (int)Context.NumberOfContexts; i++)
        {
            ContextList.Add(false);
        }

        //GetComponent<Projector>().transform = new Vector3(0.5f, 8, 0.5f);

        m_Animator = transform.Find("Ethan").GetComponent<Animator>();
        //SetContext(Context.Inside, true);
        
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
        
        

        //Rotating the direction of movement according to camera orientation
        float CameraYDirectionRadian = GameObject.FindWithTag("MainCamera").transform.rotation.eulerAngles[1];
        m_Velocity = Quaternion.AngleAxis(CameraYDirectionRadian, Vector3.up) * m_Velocity;

        m_Velocity.Normalize();

        
        Vector3 temp = m_Velocity * MovementSpeed * Time.fixedDeltaTime * 10;
        m_Rigidbody.velocity = new Vector3(temp[0], m_Rigidbody.velocity[1], temp[2]);

        

        

        if (!GetContext(Context.InAir))
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity[0], 0, m_Rigidbody.velocity[2]);
        }

        if (GetContext(Context.InAir))
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity[0] * 0.5f, m_Rigidbody.velocity[1], m_Rigidbody.velocity[2] * 0.5f);
        }

        if (Mathf.Approximately(Input.GetAxis("Jump"), 1) && GetContext(Context.InAir) == false && GetContext(Context.JumpDisabled) == false)
        {
            m_Rigidbody.AddForce(new Vector3(0, JumpHeight * 10, 0), ForceMode.Impulse);
            SetContext(Context.InAir, true);
            SetContext(Context.JumpDisabled, true);
            StartCoroutine("JumpDisabler");
            //Debug.Log("Jump");
        }

        //m_Rigidbody.MovePosition(m_Rigidbody.position + (m_Velocity * Time.deltaTime * MovementSpeed / 10));

        //float velocityClamp = 8;
        //if (Mathf.Abs(m_Rigidbody.velocity[0]) > velocityClamp) { m_Rigidbody.velocity = new Vector3(velocityClamp, m_Rigidbody.velocity[1], m_Rigidbody.velocity[2]); }
        //if (Mathf.Abs(m_Rigidbody.velocity[2]) > velocityClamp) { m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity[0], m_Rigidbody.velocity[1], velocityClamp); }

        if (Mathf.Approximately(m_Velocity.normalized.magnitude, 1))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(m_Velocity.normalized, Vector3.up), RotationSpeed * Time.fixedDeltaTime * 3);
        }
        
        //m_Rigidbody.MoveRotation(Quaternion.RotateTowards(m_Rotation, Quaternion.Euler(m_Velocity), RotationSpeed * Time.deltaTime));

        

        //Debug.Log(m_Collider.bounds);
    }

    private void LateUpdate()
    {
        UpdateContext();
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Metaentity container))
        {
            //Debug.Log("collision entered");
            if (container.buildingType == "Building")
            {
                SetContext(Context.Inside, true);
                onInside?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        //SetContext(Context.InAir, false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Metaentity container))
        {
            if (container.buildingType == "Building")
            {
                SetContext(Context.Inside, false);
                onOutside?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public bool GetContext(Context target)
    {
        return ContextList[(int)target];
    }

    public void SetContext(Context target, bool desired)
    {
        ContextList[(int)target] = desired;
    }

    private void UpdateContext()
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
        m_Animator.SetBool("walking", GetContext(Context.Walking));
        m_Animator.SetBool("inAir", GetContext(Context.InAir));

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
