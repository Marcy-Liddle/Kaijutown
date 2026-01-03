using UnityEngine;
using TMPro;

public class objective : MonoBehaviour
{
    public GameObject forceField; //object reference for the forcfield
    public GameObject bunker; //object reference for the bunker


    public GameObject corePrefab; //prefab for power core object
    public GameObject[] cores; //array for power cores 
    private bool[] destroyed; // boolean array for destroyed state

    //total and current number of cores
    public int totalCores = 3;
    public int amountOfCores;

    //source and audioclips for music and sound effects
    private AudioSource source;
    public AudioClip victoryMusic;
    public AudioClip coreDestruction;
    public AudioClip forcefieldDown;

    //UI message
    public GameObject winTextObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        source = GetComponent<AudioSource>();

        cores = new GameObject[totalCores]; 
        destroyed = new bool[totalCores + 2];

        winTextObject.SetActive(false);

        for (int i = 0; i < totalCores; i++)
        {
            //Spawn power cores at random locations in the scene
            var position = new Vector3(Random.Range(-200f, 300f), 0, Random.Range(-800f, 100f));
            
            cores[i] = Instantiate(corePrefab, position, Quaternion.identity) as GameObject ;
            
        }

        amountOfCores = totalCores;
    }

    // Update is called once per frame
    void Update()
    {
        //loop through cores array
        for (int i = 0; i < totalCores; i++)
        {
            //decrease amount of cores when one is destroyed, boolean check to stop the amount from always decreasing
            if (cores[i] == null & destroyed[i] == false)
            {
                //play core destruction sound at random pitch
                source.PlayOneShot(coreDestruction, 1f);
                source.pitch = Random.Range(1.0f, 3.0f);

                amountOfCores = amountOfCores - 1;
                destroyed[i] = true; 
            }

        }

        //If all power cores have been destroyed,
        if (amountOfCores == 0 & destroyed[totalCores] == false)
        {
            //also destroy the forcefield GameObject & play the corresponding sound.
            Destroy(forceField);
            source.PlayOneShot(forcefieldDown, .25f);
            destroyed[totalCores ] = true;
        }

        //If the bunker has been destroyed,
        if (bunker == null & destroyed[totalCores + 1] == false)
        {
            //call the win function.
            win();
            destroyed[totalCores + 1 ] = true;

        }


    }


    public void win()
    {
        Debug.Log("You Win");
        source.clip = victoryMusic;
        source.Play();
        winTextObject.SetActive(true);

    }

}
