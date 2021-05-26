using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raydebug : MonoBehaviour
{
 
    // Update is called once per frame
    void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.left) * 1000;
        Debug.DrawRay(transform.position, fwd, Color.red);
    }
}
