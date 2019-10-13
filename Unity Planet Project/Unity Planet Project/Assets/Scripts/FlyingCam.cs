using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingCam : MonoBehaviour
{

    public Text planetName;

    Transform currView; //Current transform info of the object the cameara should be facing at 

    public Transform[] cameraPoint;

    float speedFactor = 0.2f;   //linear speed of camera
    float angleFactor = 10f;    //Spherical interpolation of the camera compared to obkect it faces 

    // Start is called before the first frame update
    void Start()
    {
        currView = transform;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, currView.position, speedFactor);  //starting point, end point, and how fast it will move in  etween them, Lerp = linear interpolation
        transform.rotation = Quaternion.Slerp(transform.rotation, currView.rotation, angleFactor);

        planetName.rectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, currView.parent.GetChild(0).position);
    }


    public void SetNewPlanet(Transform p)
    {
        currView = p;
        planetName.text = currView.parent.GetChild(0).name; //camera is 2nd child of planet container, so we want the first child which is the planet
    }

    public void SetRandomPlanetView()
    {
        Transform temp = null;

        // Check if any planets can be highlighted
        if (cameraPoint.Length > 0)
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

            currView = temp;
            // Planet planet = currView.gameObject.GetComponent<Planet>();

            // foreach (GameObject g in currView.parent)
            // {
            //     if ()
            // }
            planetName.text = currView.parent.GetChild(0).name; //camera is 2nd child of planet container, so we want the first child which is the planet

        }
    }



}
