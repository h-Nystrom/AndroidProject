using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPhoneDirection : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody rb;
    Vector3 startDir;

    void Start(){
        rb = GetComponent<Rigidbody>();
        
        Invoke(nameof(StartGps),2f);
    }
    void FixedUpdate()
    {
        var dir = Quaternion.Euler(0, Input.compass.magneticHeading, 0) * startDir;
        rb.velocity += dir.normalized * speed;
    }

    void OnDisable(){
        Input.compass.enabled = false;
        Input.location.Stop();
    }
    void StartGps(){
        if(Input.location.status == LocationServiceStatus.Running)
            return;
        Input.location.Start();
        Input.compass.enabled = true;
        print("Started");
        startDir = Input.compass.rawVector;
    }
}
