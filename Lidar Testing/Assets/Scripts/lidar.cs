using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lidar : MonoBehaviour
{
    // Update is called once per frame
	public float targetDistance;
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit)){
        	targetDistance = hit.distance;
           	print("Found an object - distance: " + hit.distance);
        }
    }
}
