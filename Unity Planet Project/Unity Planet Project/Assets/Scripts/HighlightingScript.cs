using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightingScript : MonoBehaviour
{
    void OnMouseEnter()
    {
        print(gameObject.name);
        Debug.Log("Mouse is over GameObject.");
    }
}
