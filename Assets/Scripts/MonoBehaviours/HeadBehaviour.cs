using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBehaviour : MonoBehaviour
{
    public bool Touched = false;
    private void OnTriggerEnter(Collider other)
    {
        this.Touched = true;
    }
}
