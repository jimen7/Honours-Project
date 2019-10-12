using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingCam : MonoBehaviour
{

    public Text planetName;

    Transform currView; //Current transform info of the object the cameara should be facing at 

    public Transform[] cameraPoint;

    float speedFactor = 0.5f;
    float angleFactor = 10f;

    // Start is called before the first frame update
    void Start()
    {
        currView = transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRandomPlanetView()
    {
        Transform temp = null;

    // Check if any planets can be highlighted
        if (cameraPoint.Length>0)
        {
            temp = cameraPoint[Random.Range(0, cameraPoint.Length)];
        }

        //check that the current one has not been used again

        if (currView.gameObject.Equals(temp.gameObject))
        {
            SetRandomPlanetView();
        }
        else
        {
            {
                currView = temp;
                // Planet planet = currView.gameObject.GetComponent<Planet>();

                // foreach (GameObject g in currView.parent)
                // {
                //     if ()
                // }
                planetName.text = currView.parent.GetChild(1).name;
            }
        }
    }



}
