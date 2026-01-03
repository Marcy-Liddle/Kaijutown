//using UnityEditor.PackageManager;
using UnityEngine;
using System.Collections;
using System;

public class Raycaster : MonoBehaviour
{

    [SerializeField] private float  m_RayDistance   = 10.0f;

    private bool        m_RayHit        = false;
    private Vector3     m_HitPoint      = Vector3.zero;
    private Vector3     m_HitNormal     = Vector3.zero;

    public GameObject explode;
    public GameObject laser;

    public GameObject head;

    // Update is called once per frame
    void Update()
    {
       
        
        if (Input.GetKeyDown("f"))
        {
       
                DoRaycast();
        }
       
    }


    void DoRaycast()
    {
        RaycastHit hitInfo; //gives information about the first ray hit if available
        Ray ray = new Ray(transform.position, -transform.up);

        //Do the raycast. Store the information in hitInfo
        m_RayHit = Physics.Raycast(ray, out hitInfo, m_RayDistance);

        //create a laser blast
        Instantiate(laser, parent:head.transform, false);

        //if the laser hits, create an explosion
        if (m_RayHit)
        {
            m_HitPoint = hitInfo.point;     //Stores the position that the ray collided with the object
            Instantiate(explode, m_HitPoint, transform.rotation);

        }
 
    }



    //draws the rayCast in scene view 
    private void OnDrawGizmos()
    {
        if (m_RayHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, m_HitPoint);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_HitPoint, 0.125f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(m_HitPoint, m_HitPoint + m_HitNormal);
        }
        else
        {
            //Draw a line to where the ray ends
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position - transform.up * m_RayDistance);

            Gizmos.DrawSphere(transform.position - transform.up * m_RayDistance, 0.125f);
        }
    }
}
