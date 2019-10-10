using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
   // [System.Serializable]
    public bool movement = true;
    public float rotationSpeed;
    public float dampening;

    // Update is called once per frame
    void Update()
    {
        if (movement)
        {
            transform.Rotate((Vector3.up * rotationSpeed) * (Time.deltaTime * dampening), Space.Self);
        
        }
        
    }
}
