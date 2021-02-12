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
                
                var test = CalculateLaunchVelocity();
                rb.velocity += test;
                Debug.DrawRay(parent.position, CalculateLaunchVelocity(), Color.yellow, 2f);
                // Debug.DrawRay(lastPosition, Vector3.up * 5, Color.black, 5f);
                // Debug.DrawRay(curvePosition, Vector3.up * 5, Color.black, 5f);
                GetSpin();
                lastPosition = Vector3.zero;
                isDraging = false;
                isKicked = true;
                Invoke(nameof(ReturnBall), 5f);
            }
        }

        void FixedUpdate(){
            if (!isKicked) 
                return;
            rb.AddForce(-parent.right * (curvePosition.x * spinForce), ForceMode.VelocityChange);
        }
        void GetSpin(){
            curvePosition.x -= lastPosition.x;
        }
        Vector3 CalculateLaunchVelocity(){
            const int g = -18;
            var height = 6;
            var displacementY = 3 - parent.position.y;
            // var displacementXZ = new Vector3(lastPosition.x - parent.position.x,0,lastPosition.z - parent.position.z);
            var displacementXZ = curvePosition.normalized * (lastPosition.z + curvePosition.z);
            var velocityY = Vector3.up * Mathf.Sqrt(-2 * g * height);
            var velocityXZ = displacementXZ / (Mathf.Sqrt(-2* height/g) + Mathf.Sqrt(2 * (displacementY - height) / g));
            return velocityXZ + velocityY;
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
            var direction = (parent.forward * screenPositionDifference.y + parent.right * screenPositionDifference.x);
            if (Mathf.Abs(direction.x) > Mathf.Abs(lastPosition.x) && Vector3.Distance(direction, curvePosition) < 2){
                curvePosition = direction;
            }
            lastPosition = direction;
            oldScreenPosition = Input.mousePosition;
        }
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

        // void OnTriggerEnter(Collider other){
        //     if(other.gameObject.layer == LayerMask.NameToLayer("Default"))
        //         maxCurveSpeed = 0;
        // }
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
