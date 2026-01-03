using System.Collections;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    //explosion noise
    private AudioSource source;
    public AudioClip[] kaboom;

    //explosion scales before being destroyed whereas the laser does not
    public bool scalable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();

        AudioClip boom = kaboom[(int)Random.Range(0, kaboom.Length)];
        source.clip = boom;
        source.PlayOneShot(boom, 1.0f);

    }

    // Update is called once per frame
    void Update()
    {
        //explosions grow in size
        if (scalable)
        {
            //makes the gameObject grow in size
            transform.localScale +=  new Vector3 (0.001f , 0.001f, 0.001f);
        }
        //destroys the gameObject when 1 second has passed
        Destroy(gameObject, 1f);
    }


}
