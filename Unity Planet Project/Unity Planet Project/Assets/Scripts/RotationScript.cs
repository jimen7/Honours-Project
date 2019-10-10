using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public float rotationSpeed;
    public float dampening;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate((Vector3.up * rotationSpeed) * (Time.deltaTime * dampening), Space.Self);
        
    }
}
