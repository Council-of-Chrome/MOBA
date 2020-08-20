using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Capsule;
    void Update()
    {
        transform.position = Capsule.transform.position;
    }
}
