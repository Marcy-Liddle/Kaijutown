using UnityEngine;
using UnityEngine.Rendering;


public class brickHealth : MonoBehaviour
{
    [SerializeField] private float health = 10f;
    [SerializeField] private bool core;
    [SerializeField] private bool final;


    private void Update()
    {
        if (health <= 0)
        {
   
                Destroy(gameObject);

        }
    }

    private void OnCollisionEnter(Collision attack) 
    {
        if (attack.gameObject.CompareTag("attack"))
        {
            health -= 3f;
        }

    }

    private void OnCollisionStay(Collision attack)
    {
        if (attack.gameObject.CompareTag("attack"))
        {
            health -= 3f;
        }

    }

    private void OnCollisionLeave(Collision attack)
    {
       if (attack.gameObject.tag == "attack")
        {
             health -= 3f;
        }
       
    }
}
