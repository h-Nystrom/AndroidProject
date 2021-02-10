using UnityEngine;

public class MoveTowardsPoint : MonoBehaviour{
    [SerializeField] float speed;
    [SerializeField]LayerMask layerMask;
    Camera cam;
    Rigidbody rb;
    RaycastHit hit;

    void Start(){
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }
    void FixedUpdate(){
        
        if (Input.GetMouseButton(0)){
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray.origin, ray.direction * 100, out hit, layerMask);
            Debug.DrawRay(ray.origin, ray.direction * 100,Color.red);
            var newDir = (hit.point - transform.position).normalized;
            rb.AddForce(transform.forward + newDir * speed);
        }
    }
}
