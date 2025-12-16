using UnityEngine;

public class objective : MonoBehaviour
{
    public GameObject forceField;
    public GameObject core;
    public int cores = 3; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //objective game;
        for (int i = 0; i < cores; i++)
        {
            var position = new Vector3(Random.Range(-90.0f, 90.0f), 0, Random.Range(-50.0f, 50.0f));
            Instantiate(core, position , Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cores == 0)
        {
            Destroy(forceField);

        }

       
    }

    public void win()
    {
        
    }

}
