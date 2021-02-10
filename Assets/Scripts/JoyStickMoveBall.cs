using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickMoveBall : MonoBehaviour{

    [SerializeField] float speed;
    [SerializeField] FixedJoystick joystick;
    Rigidbody rb;
    
    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var dir = new Vector3(joystick.Horizontal,0,joystick.Vertical);
        if(dir == Vector3.zero)
            return;
        rb.velocity += dir.normalized * speed;
    }
}
