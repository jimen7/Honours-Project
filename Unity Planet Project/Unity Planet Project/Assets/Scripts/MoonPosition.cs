using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonPosition : MonoBehaviour
{

    float portion = 3.67f/2.31f;    //Taken from ann instance where the planet is as far as we want it from the moon

    System.Random rand = new System.Random();

   public void MoveMoon()
    {


        if (rand.Next(0, 2) != 0)
        {
            
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z *  portion);
        }
        else
        {
            gameObject.SetActive(true);
        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
