using System;
using System.Collections.Generic;
using UnityEngine;

namespace FootBallKick{
    public class BallController : MonoBehaviour{
        [SerializeField] float speed;
        [SerializeField] float maxSpeed;
        [SerializeField] float positionsDifferenceDistance;
        [SerializeField] float upForce;
        [SerializeField] float spinForce;
        Camera cam;
        RaycastHit hit;
        Vector3 screenStartPosition;
        Vector3 startPosition;
        Vector3 oldScreenPosition;
        float startTime;
        Rigidbody rb;
        [SerializeField] Vector3 maxCurvePosition;
        
        List<Vector3> positionList = new List<Vector3>();
        void Start(){
            rb = GetComponent<Rigidbody>();
            cam = Camera.main;
            startPosition = transform.position;
        }

        void FixedUpdate(){
            if(rb.velocity.magnitude < 3)
                return;
            rb.AddForce(maxCurvePosition * spinForce, ForceMode.VelocityChange);
        }

        void OnMouseDown(){
            positionList.Clear();
            screenStartPosition = Input.mousePosition;
            oldScreenPosition = screenStartPosition;
            startTime = Time.time;
        }
        void OnMouseDrag(){
            GetCurvePositions();
        }
        void OnMouseUp(){
            if(positionList.Count <= 2)
                return;
            var passedTime = Time.time - startTime;
            var screenPositionDifference = positionList[positionList.Count / 2];
            var direction = screenPositionDifference + (Vector3.up * (positionList[0].z * screenPositionDifference.z * upForce));
            var clampedDirection = Vector3.ClampMagnitude(direction, maxSpeed);
            maxCurvePosition = Vector3.right * positionList[positionList.Count-1].x;
            rb.AddForce(clampedDirection * speed / passedTime);
            positionList.Clear();
        }
        void GetCurvePositions(){
            if (Vector3.Distance(oldScreenPosition, Input.mousePosition) < positionsDifferenceDistance)
                return;
            var screenPositionDifference = (Input.mousePosition - screenStartPosition) / Screen.dpi;
            var direction = Vector3.forward * screenPositionDifference.y + Vector3.right * screenPositionDifference.x;
            // Debug.DrawRay(direction, Vector3.up * 2, Color.black, 5f);
            positionList.Add(direction);
            oldScreenPosition = Input.mousePosition;
        }
        public void ReturnBall(){
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = startPosition;
        }

        void OnTriggerEnter(Collider other){
            if(other.gameObject.layer == LayerMask.NameToLayer("Default"))
                maxCurvePosition = Vector3.zero;
            if (other.gameObject.layer != LayerMask.NameToLayer("Trigger"))
                return;
            Invoke(nameof(ReturnBall), 1f);
        }
    }
}
