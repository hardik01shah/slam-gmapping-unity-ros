using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updown : MonoBehaviour
{
    //public float speed = 10.0f;
    //public float rotationSpeed = 100.0f;
    //public Rigidbody rb;
    public Vector3 translation;
    public Vector3 rotation;
    public static float pass;
    public GameObject go;

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        //translation = Input.GetAxis("Vertical") * speed;
        //rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation = RosSharp.RosBridgeClient.TwistSubscriber.linearVelocity;
        rotation = RosSharp.RosBridgeClient.TwistSubscriber.angularVelocity;

        Vector3 currenteulerAngles = go.transform.rotation.eulerAngles;
        pass = currenteulerAngles.y;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation.z *= Time.deltaTime;
        translation.z*=10;
        rotation.y *= Time.deltaTime;
        rotation.y*=10;
        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation.z);

        // Rotate around our y-axis
        transform.Rotate(0, rotation.y, 0); 
    }
}
