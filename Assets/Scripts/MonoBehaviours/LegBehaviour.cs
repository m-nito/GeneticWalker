using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegBehaviour : MonoBehaviour
{
    public void Forward(float speed)
    {
        this.gameObject.transform.Rotate(Vector3.left, speed);
    }
}
