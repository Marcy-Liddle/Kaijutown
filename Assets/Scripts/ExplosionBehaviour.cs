using System.Collections;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{

    public bool scalable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //explosionEffect();
        if (scalable)
        {
            transform.localScale +=  new Vector3 (0.001f , 0.001f, 0.001f);
        }
        
        Destroy(gameObject, 1f);
    }

    IEnumerator explosionEffect()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject,1f);
    }
}
