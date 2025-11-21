using UnityEditor.PackageManager;
using UnityEngine;
using System.Collections;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private float  m_RayDistance   = 10.0f;

    private bool        m_RayHit        = false;
    private Vector3     m_HitPoint      = Vector3.zero;
    private Vector3     m_HitNormal     = Vector3.zero;
    private bool        m_Grounded      = false;

    public GameObject explode;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space pressed");
                DoRaycast();
        }
       
    }

    void DoRaycast()
    {
        RaycastHit hitInfo; //gives us information about what we hit (if anything)
        Ray ray = new Ray(transform.position, -transform.up);

        //Do the raycast. Store the information in hitInfo
        m_RayHit = Physics.Raycast(ray, out hitInfo, m_RayDistance);

        if (m_RayHit)
        {
            m_HitPoint = hitInfo.point;     //Store the position that our ray collided with the object
            m_HitNormal = hitInfo.normal;   //Store the surface normal of the object
            m_Grounded = Vector3.Dot(Vector3.up, hitInfo.normal) > 0.5f; //Bit of a magic number here. Just trust me on this one.

            Instantiate(explode, m_HitPoint, transform.rotation);

        }
 
    }



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
