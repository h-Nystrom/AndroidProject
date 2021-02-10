using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchMoveBall : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    Touch touch;
    Vector3 startPosition;
    Rigidbody rb;
    RaycastHit hit;
    [SerializeField]LayerMask layerMask;
    [SerializeField] float speed;
    void Start(){
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }
    void OnMouseDown(){
        rb.drag = 10;
        startPosition = transform.position;
    }
    void OnMouseUp(){
        rb.drag = 0;
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray.origin, ray.direction * 100, out hit, layerMask);
        Debug.DrawRay(ray.origin, ray.direction * 100,Color.red);
        var newDir = hit.point - startPosition;
        rb.AddForce(transform.forward + newDir * speed, ForceMode.Impulse);
    }
}
