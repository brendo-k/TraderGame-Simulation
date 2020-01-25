using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheifSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject theif;
    void Start()
    {
        float randX = Random.Range(-2.5f,3.5f);
        while(randX > -0.5f && randX < 1.5){
            randX = Random.Range(-2.5f,3.5f);
        }
        float randY = Random.Range(-3f, 3f);
        while(randY > -1f && randY < 1){
            randY = Random.Range(-3f, 3f);
        }
        GameObject newTheif = Instantiate(theif, new Vector3(randX, 0f, randY), Quaternion.identity);
        newTheif.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
