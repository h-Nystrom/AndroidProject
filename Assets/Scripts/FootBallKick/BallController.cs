using System;
using UnityEngine;

namespace FootBallKick{
    public class BallController : MonoBehaviour{
        [SerializeField] float speed;
        [SerializeField] float maxSpeed;
        [SerializeField] float positionsDifferenceDistance;
        Camera cam;
        RaycastHit hit;
        Vector3 screenStartPosition;
        Vector3 startPosition;
        Vector3 oldScreenPosition;
        float startTime;
        Rigidbody rb;
        void Start(){
            rb = GetComponent<Rigidbody>();
            cam = Camera.main;
            startPosition = transform.position;
        }
        void OnMouseDown(){
            screenStartPosition = Input.mousePosition;
            oldScreenPosition = oldScreenPosition;
            startTime = Time.time;
        }

        void OnMouseDrag(){
            if (Vector3.Distance(oldScreenPosition, Input.mousePosition) < positionsDifferenceDistance)
                return;
            print("Distance: " + Vector3.Distance(oldScreenPosition, Input.mousePosition));
            oldScreenPosition = Input.mousePosition;
            var passedTime = Time.time - startTime;
            var screenPositionDifference = (Input.mousePosition - screenStartPosition) / Screen.dpi;
            var direction = Vector3.forward * screenPositionDifference.y + Vector3.right * screenPositionDifference.x;
            Debug.DrawRay(direction, Vector3.up, Color.black, 5f);
        }

        void OnMouseUp(){
            var passedTime = Time.time - startTime;
            var screenPositionDifference = (Input.mousePosition - screenStartPosition)/ Screen.dpi;
            var direction = Vector3.forward * screenPositionDifference.y + Vector3.right * screenPositionDifference.x;
            var clampedDirection = Vector3.ClampMagnitude(direction, maxSpeed);
            //rb.AddForce(clampedDirection * speed / passedTime);
        }
        public void ReturnBall(){
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = startPosition;
        }
    }
}
