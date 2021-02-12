using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FollowPath : MonoBehaviour{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float speed = 0.5f;
    Camera cam;
    Queue<Vector3> paths = new Queue<Vector3>();
    Vector3 currentPath;
    RaycastHit hit;
    Rigidbody rb;
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            StartCoroutine(nameof(AddPositions));
        }
        if (!Input.GetMouseButtonUp(0)) return;
        StopCoroutine(nameof(AddPositions));
        AddToQueue();
        UpdateCurrentPath();
    }
    void FixedUpdate(){
        if(currentPath == Vector3.zero)
            return;
        Move();
        if (Vector3.Distance(transform.position, currentPath) < 1.15f){
            UpdateCurrentPath();
        }
    }

    void UpdateCurrentPath(){
        currentPath = paths.Count > 0 ? paths.Dequeue() : Vector3.zero;
    }
    void Move(){
        var dir = (currentPath - transform.position).normalized;
        rb.velocity += dir * speed;
    }
    IEnumerator AddPositions(){
        while (true){
            AddToQueue();
            yield return new WaitForSeconds(0.1f);
        }
    }
    void AddToQueue(){
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        
        if (!Physics.Raycast(ray.origin, ray.direction * 100, out hit, layerMask))
            return;
        Debug.DrawRay(hit.point, Vector3.up * 10,Color.red, 10f);
        paths.Enqueue(hit.point);
    }
}
