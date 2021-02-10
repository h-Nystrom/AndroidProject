using UnityEngine;

public class TiltController : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate(){
        var dir = new Vector3(Input.acceleration.x, 0, Input.acceleration.y);
        if(dir == Vector3.zero)
            return;
        rb.velocity += dir * speed;
    }
}


