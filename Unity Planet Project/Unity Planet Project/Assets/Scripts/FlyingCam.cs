using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingCam : MonoBehaviour
{


    float ScrollWheelChange;
    Vector3 zoomBy = new Vector3(0f, 0f, 0f);

    public ParticleSystem[] particles;

    bool particlesEnabled = false;

    public Text planetName;

    Transform currView; //Current transform info of the object the cameara should be facing at 

    public Transform[] cameraPoint;

    Transform systemOverviewLocation;

    float speedFactor = 0.03f;   //linear speed of camera
    float angleFactor = 0.03f;    //Spherical interpolation of the camera compared to obkect it faces 

    bool hyperdriveStatus = false;


    // Start is called before the first frame update
    void Start()
    {
        currView = transform;

        foreach (ParticleSystem p in particles)
        {
            p.Stop();
        }

        systemOverviewLocation = GameObject.Find("System Overview").transform;


    }

    // Update is called once per frame
    void Update()
    {
        ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");

        //zoom in and out
        if (ScrollWheelChange != 0)
        {                                            //If the scrollwheel has changed
            float R = ScrollWheelChange * 15;                                   //The radius from current camera
            float PosX = Camera.main.transform.eulerAngles.x + 90;              //Get up and down
            float PosY = -1 * (Camera.main.transform.eulerAngles.y - 90);       //Get left to right
            PosX = PosX / 180 * Mathf.PI;                                       //Convert from degrees to radians
            PosY = PosY / 180 * Mathf.PI;                                       //^
            float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                    //Calculate new coords
            float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                    //^
            float Y = R * Mathf.Cos(PosX);                                      //^
            zoomBy = zoomBy + new Vector3(X, Y, Z);
        }

        transform.position = Vector3.Lerp(transform.position, currView.position + zoomBy, speedFactor); //starting point, end point, and how fast it will move in  etween them, Lerp = linear interpolation
        transform.rotation = Quaternion.Slerp(transform.rotation, currView.rotation, angleFactor);
    }


    public void SetNewPlanet(Transform p)
    {
        zoomBy = new Vector3(0, 0, 0);    //Reset zoom values
        currView = p;

        for (int i = 0; i < currView.parent.childCount; i++)
        {
            if (currView.parent.GetChild(i).GetComponent<Planet>() != null)
            {
                planetName.text = currView.parent.GetChild(i).name;
                break;
            }
        }
    }

    public void SetSysOverview(Transform p)
    {
        zoomBy = new Vector3(0, 0, 0);    //Reset zoom values
        currView = p;
        planetName.text = "";
    }

    public bool GetHyperdriveStatus()
    {
        return hyperdriveStatus;
    }

    public void ChangeHyperdriveStatus()
    {
        hyperdriveStatus = !hyperdriveStatus;
    }

    public void SetRandomPlanetView()
    {
        Transform temp = null;
        zoomBy = new Vector3(0, 0, 0);    //Reset zoom values

        if (cameraPoint.Length > 0) //Set rabdom point near a planet from the predifined points
        {
            temp = cameraPoint[Random.Range(0, cameraPoint.Length)];
        }

        if (currView.gameObject.Equals(temp.gameObject))    //check that the current one has not been used again
        {
            SetRandomPlanetView();
        }
        else
        {
            currView = temp;
            for (int i = 0; i < currView.parent.childCount; i++)
            {
                if (currView.parent.GetChild(i).GetComponent<Planet>() != null)
                {
                    planetName.text = currView.parent.GetChild(i).name;
                    break;
                }
            }
        }
    }

    public void ChangePartcileBolean()
    {
        particlesEnabled = !particlesEnabled;
    }

    public void Hyperdrive()
    {
        if (!particlesEnabled)
        {
            foreach (ParticleSystem p in particles)
            {
                p.Play();
            }
            ChangePartcileBolean();
            StartCoroutine(ChangeBackground(CameraClearFlags.SolidColor));
            ChangeHyperdriveStatus();
        }
        else
        {
            foreach (ParticleSystem p in particles)
            {
                p.Stop();
            }
            currView = systemOverviewLocation;
            ChangePartcileBolean();
            Camera.main.clearFlags = CameraClearFlags.Skybox;
            ChangeHyperdriveStatus();
        }
    }

    IEnumerator ChangeBackground(CameraClearFlags v)
    {
        yield return new WaitForSeconds(1.5f);
        Camera.main.clearFlags = v;
    }

}
