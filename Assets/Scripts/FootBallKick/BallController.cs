using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FootBallKick{
    public class BallController : MonoBehaviour{
        [SerializeField] float speed;
        [SerializeField] float maxInputSpeed;
        [SerializeField] float positionsDifferenceDistance;
        [SerializeField] float upForce;
        [SerializeField] float spinForce;
        Vector3 screenStartPosition;
        [SerializeField]Transform parent;
        Vector3 startPos;
        Vector3 oldScreenPosition;
        float startTime;
        Rigidbody rb;
        [SerializeField]float maxCurveSpeed;
        [SerializeField] float maxUpForce;
        
        Vector3 oldPosition;
        bool isDraging;
        bool isKicked;
        
        //Raycast:
        [SerializeField] LayerMask layerMask;
        Camera cam;
        RaycastHit hit;

        float curveTime;
        Vector3 lastPosition;
        Vector3 curvePosition;
        
        void Start(){
            rb = GetComponent<Rigidbody>();
            startPos = transform.localPosition;
            cam = Camera.main;
        }

        void Update(){
            if(isKicked)
                return;
            if (Input.GetMouseButtonDown(0) && !isDraging){
                if(!RaycastClickCheck())
                    return;
                OnStartOfKick();
            }
            if (Input.GetMouseButton(0) && isDraging){
                if (Vector3.Distance(oldScreenPosition, Input.mousePosition) < positionsDifferenceDistance)
                    return;
                ScreenToWorldXZPosition();
            }
            if (Input.GetMouseButtonUp(0) && isDraging){
                if(curvePosition == Vector3.zero){
                    isDraging = false;
                    return;
                }
                
                Debug.DrawRay(transform.position + lastPosition * 2, parent.up * 2, Color.black, 2f);
                Debug.DrawRay(transform.position + curvePosition * 2, parent.up * 2, Color.red, 2f);
                GetHeight();
                Debug.DrawRay(parent.position, CalculateLaunchVelocity(lastPosition), Color.yellow, 2f);
                rb.velocity += CalculateLaunchVelocity(lastPosition);
                lastPosition = Vector3.zero;
                curvePosition = Vector3.zero;
                isDraging = false;
                isKicked = true;
                Invoke(nameof(ReturnBall), 5f);
            }
        }
        Vector3 CalculateLaunchVelocity(Vector3 targetPosition){
            const int g = -18;
            const int height = 2;

            var displacementY = targetPosition.y - parent.position.y;
            var displacementXZ = new Vector3(curvePosition.x - parent.position.x,0,targetPosition.z - parent.position.z);
            var velocityY = Vector3.up * Mathf.Sqrt(-2 * g * height);
            var velocityXZ = displacementXZ / (Mathf.Sqrt(-2* height/g) + Mathf.Sqrt(2 * (displacementY - height) / g));
            return velocityXZ + velocityY;
        }
        void GetHeight(){
            //Height of max point
            //Height of lastPosition
            var totalTime = Time.time - startTime;
            // var distance = Vector3.Slerp(parent.position, curvePosition, 1) + lastPosition - curvePosition;
            print("Total time: " + totalTime + " Curve time: " + curveTime);
            print("Start: " + parent.position + " Curve: " + curvePosition + " End: " + lastPosition);
        }
        bool RaycastClickCheck(){
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray.origin, ray.direction * 100f, out hit, layerMask)) 
                return false;
            return hit.collider.gameObject.layer == LayerMask.NameToLayer("Ball");
        }
        void OnStartOfKick(){
            lastPosition = Vector3.zero;
            curvePosition = Vector3.zero;
            screenStartPosition = Input.mousePosition;
            oldScreenPosition = screenStartPosition;
            startTime = Time.time;
            isDraging = true;
        }
        void ScreenToWorldXZPosition(){
            var screenPositionDifference = (Input.mousePosition - screenStartPosition) / Screen.dpi;
            var direction = parent.forward * screenPositionDifference.y + parent.right * screenPositionDifference.x;
            // Debug.DrawRay(transform.position + direction * 2, parent.up * 2, Color.black, 1f);
            if (Mathf.Abs(direction.x) > Mathf.Abs(lastPosition.x) && Vector3.Distance(direction, curvePosition) < 4){
                curvePosition = direction;
                curveTime = Time.time - startTime;
            }
            lastPosition = direction;
            oldScreenPosition = Input.mousePosition;
        }
// void FixedUpdate(){
        //     if(!isKicked)
        //         return;
        //     rb.velocity += oldPosition;
        //     if (Vector3.Distance(transform.position, oldPosition) < 1.15f){
        //         print("Yes");
        //         oldPosition = positionQueue.Count > 0 ? positionQueue.Dequeue() : Vector3.zero;
        //         var dir = (oldPosition - transform.position).normalized;
        //     }
        //
        // }
        // void FixedUpdate(){
        //     if(rb.velocity.magnitude < 1)
        //          return;
        //     rb.AddForce(parent.right * (maxCurveSpeed * rb.velocity.magnitude * 0.5f), ForceMode.Force);
        //  }
        // void OnMouseDown(){
        //     if(Vector3.Distance(parent.position, transform.position) > 0.1f)
        //         return;
        //     positionList.Clear();
        //     screenStartPosition = Input.mousePosition;
        //     oldScreenPosition = screenStartPosition;
        //     startTime = Time.time;
        //     isDraging = true;
        // }
        // void OnMouseUp(){
        //     isDraging = false;
        //     if (positionList.Count <= 2) return;
        //     var passedTime = Time.time - startTime;
        //     var screenPositionDifference = positionList[positionList.Count / 2];
        //     //maxCurveSpeed = positionList[positionList.Count-1].x - screenPositionDifference.x;
        //     //maxUpForce = Vector3.Distance(positionList[0], positionList[positionList.Count - 1]) * upForce;
        //     var direction = screenPositionDifference;// + (Vector3.up * maxUpForce);
        //     var clampedDirection = Vector3.ClampMagnitude(direction, maxInputSpeed);
        //     Debug.DrawRay(parent.position, clampedDirection * speed / passedTime, Color.red, 5f);
        //     //rb.AddForce(clampedDirection * speed / passedTime);
        //     
        //     positionList.Clear();
        //     // Invoke(nameof(ReturnBall), 4f);
        // }
        // void GetCurvePositions(){
        //     if (Vector3.Distance(oldScreenPosition, Input.mousePosition) < positionsDifferenceDistance)
        //         return;
        //     var screenPositionDifference = (Input.mousePosition - screenStartPosition) / Screen.dpi;
        //     var direction = parent.forward * screenPositionDifference.y + parent.right * screenPositionDifference.x;
        //     Debug.DrawRay(transform.position + direction * 2, parent.up * 2, Color.black, 5f);
        //     positionList.Add(direction);
        //     oldScreenPosition = Input.mousePosition;
        // }
        public void ReturnBall(){
            gameObject.SetActive(false);
            Invoke(nameof(ReturnBallDelay),1f);
        }

        void ReturnBallDelay(){
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.rotation = parent.rotation;
            transform.localPosition = startPos;
            gameObject.SetActive(true);
            isKicked = false;
        }
        // void OnCollisionStay(Collision other){
        //     if (other.gameObject.layer == LayerMask.NameToLayer("Ground")){
        //         if(maxCurveSpeed == 0)
        //             return;
        //         maxCurveSpeed = Mathf.MoveTowards(maxCurveSpeed, 0, 5 * Time.fixedDeltaTime);
        //     }
        //     
        // }

        void OnTriggerEnter(Collider other){
            if(other.gameObject.layer == LayerMask.NameToLayer("Default"))
                maxCurveSpeed = 0;
        }
        //DeveloperBuildTools:
        public void SetSpeed(float value){
            speed = value;
        }

        public void SetInputSpeed(float value){
            maxInputSpeed = value;
        }

        public void SetSpinMultiplier(float value){
            spinForce = value;
        }

        public void SetUpForce(float value){
            upForce = value;
        }

        public void SetUpdateRate(float value){
            positionsDifferenceDistance = value;
        }
        
    }
}
