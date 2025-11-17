using UnityEditor.PackageManager;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private bool   m_RaycastAll    = false;
    [SerializeField] private float  m_RayDistance   = 10.0f;

    [SerializeField] UnityEngine.UI.Text m_Text;

    private bool        m_RayHit        = false;
    private Vector3     m_HitPoint      = Vector3.zero;
    private Vector3     m_HitNormal     = Vector3.zero;
    private bool        m_Grounded      = false;

    // Update is called once per frame
    void Update()
    {
        if (!m_RaycastAll)
        {
            DoRaycast();
        }
        else
        {
            DoRaycastAll();
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
            
            m_Text.text = $"The ray hit {hitInfo.collider.name}.";
            m_Text.text += m_Grounded ? "\nThis is the ground!" : "\nThis is NOT the ground.";
        }
        else
        {
            m_Text.text = $"The ray hit nothing!";
        }
    }

    void DoRaycastAll()
    {
        RaycastHit[] hitInfos; //gives us information about what we hit (if anything)
        Ray ray = new Ray(transform.position, -transform.up);

        //Do the raycast. Store the information in hitInfos
        hitInfos = Physics.RaycastAll(ray, m_RayDistance);

        //We hit something if there are 1 or more results in hitInfos
        m_RayHit = hitInfos.Length > 0;

        if (m_RayHit)
        {
            //We're setting the "hit point" to be the first item in the array
            //This is NOT necessarily the closest item
            m_HitPoint = hitInfos[0].point;
            m_HitNormal = hitInfos[0].normal;

            //Some string fun in here, can you make sense of it?
            string printString = "The Ray hit ";
            printString += hitInfos[0].collider.name;
            for (int i = 1; i < hitInfos.Length; i++)
            {
                printString += $" and {hitInfos[i].collider.name}";
            }
            printString += ".";
            m_Text.text = printString;
        }
        else
        {
            m_Text.text = $"The ray hit nothing!";
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
