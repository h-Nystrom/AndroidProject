using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FootBallKick{
    public class FootBallController : MonoBehaviour{
        [SerializeField]LayerMask layerMask;
        [SerializeField] float speed;
        [SerializeField] float speed2;
        [SerializeField] float magnusEffectMultiplier;
        Rigidbody rb;
        Camera cam;
        RaycastHit hit;
        Vector3 airDirection;
        Vector3 forceDirection;
        Vector3 startPosition;
        List<Vector3> swipePositions = new List<Vector3>();
        float startTime;
        void Start(){
            cam = Camera.main;
            rb = GetComponent<Rigidbody>();
            startPosition = transform.position;
        }

        void FixedUpdate(){
            if(rb.velocity.magnitude < 15)
                return;
            rb.AddForce(airDirection * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        void OnMouseDown(){
            StopCoroutine(nameof(AddPositions));
            swipePositions.Clear();
            AddSwipePosition();
            StartCoroutine(nameof(AddPositions));
            forceDirection = MouseWorldPosition();
            startTime = Time.time + speed;
        }
        void OnMouseUp(){
            StopCoroutine(nameof(AddPositions));
            AddSwipePosition();
            var temp = MouseWorldPosition();
            var dir = (temp-transform.position).normalized;
            Debug.DrawRay(transform.position, dir * CalculatePower(),Color.red, 5f);
            //dir.y = 0.1f;
            rb.AddForce(dir * CalculatePower(),ForceMode.Impulse);
            airDirection = (dir-swipePositions[swipePositions.Count / 2]) * magnusEffectMultiplier;
            airDirection.y = 0;
        }

        //If mouse pos > ? add vector pos
        //On mouse drag

        float CalculatePower(){
            var power = Mathf.Max(0,startTime - Time.time);
            return power * speed2;
        }
        public void ReturnBall(){
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = startPosition;
        }
        
        Vector3 MouseWorldPosition(){
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray.origin, ray.direction * 100, out hit, layerMask))
                return Vector3.zero;
            Debug.DrawRay(hit.point, Vector3.up,Color.blue, 5f);
            return hit.point;
        }
        IEnumerator AddPositions(){
            while (swipePositions.Count < 40){
                AddSwipePosition();
                yield return new WaitForSeconds(0.1f);
            }
        }
        void AddSwipePosition(){
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray.origin, ray.direction * 100, out hit, layerMask))
                return;
            Debug.DrawRay(hit.point, Vector3.up,Color.black, 5f);
            swipePositions.Add(hit.point);
        }
    }
}
