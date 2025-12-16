using UnityEngine;
using UnityEngine.Rendering;


public class brickHealth : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private bool core;
    [SerializeField] private bool final;

    public objective objective;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (core)
        {
            health = 15;

        }
        else if (final)
        {
            health = 1;
        }
        else
        {
            health = 10f;
        }
       
    }

    private void Update()
    {
        if (health <= 0)
        {
            if (core)
            {
                objective.cores -= 1;
                Destroy(gameObject);
            }
            else if (final)
            {
                objective.win();
            }
            

        }
    }

    private void OnCollisionEnter(Collision attack)
    {
        if (attack.gameObject.tag == "attack")
        {
            health -= 2f;
        }

    }

    private void OnCollisionStay(Collision attack)
    {
        if (attack.gameObject.tag == "attack")
        {
            health -= 2f;
        }

    }

    private void OnCollisionLeave(Collision attack)
    {
       if (attack.gameObject.tag == "attack")
        {
             health -= 2f;
        }
       
    }
}
