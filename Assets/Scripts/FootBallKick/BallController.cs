using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FootBallKick{
    public class BallController : MonoBehaviour{
        const int minIndex = 3;
        [SerializeField] float positionsDifferenceDistance;
        [SerializeField] float spinForceMultiplier;
        [SerializeField]Transform parent;
        [SerializeField] LayerMask layerMask;
        [SerializeField] float distanceMultiplier;
        [SerializeField] float heightMultiplier;
        Camera cam;
        Rigidbody rb;
        RaycastHit hit;
        Vector3 lastPosition;
        [SerializeField]Vector3 curvePosition;
        Vector3 screenStartPosition;
        Vector3 startPos;
        Vector3 oldScreenPosition;
        float startTime;
        float maxCurveHeight;
        bool isDraging;
        bool isKicked;
        List<Vector3> swipePositions = new List<Vector3>();
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
                print("Click");
            }
            if (Input.GetMouseButton(0) && isDraging){
                if (Vector3.Distance(oldScreenPosition, Input.mousePosition) < positionsDifferenceDistance)
                    return;
                ScreenToWorldPosition();
            }

            if (!Input.GetMouseButtonUp(0) || !isDraging) return;
            OnKick();
        }
        void FixedUpdate(){
            if (!isKicked || rb.velocity.magnitude < 10) 
                return;
            rb.AddForce(-parent.right * (curvePosition.x * spinForceMultiplier));
        }
        void FindMaxCurvePosition(){
            curvePosition = swipePositions[minIndex-1];
            var miss = 0;
            for (var i = minIndex; i < swipePositions.Count; i++){
                if (Mathf.Abs(parent.InverseTransformPoint(swipePositions[i]).x) > Mathf.Abs(parent.InverseTransformPoint(curvePosition).x)){
                    curvePosition = swipePositions[i];
                    miss = 0;
                }
                else if(miss < 2){
                    miss++;
                }
                else{
                    swipePositions.Clear();
                    break;
                }
            }
            swipePositions.Clear();
        }

        IEnumerator AirResistance(){
            while (curvePosition.x != 0){
                curvePosition.x = Mathf.MoveTowards(curvePosition.x, 0, 5 * Time.fixedDeltaTime);
                yield return new WaitForSeconds(0.1f);
            }
        }
        float CalculateCurveHeight(){
            var mouseTravelDistance = (Input.mousePosition.y + heightMultiplier - screenStartPosition.y) / Screen.dpi;
            var totalTime = Time.time + 1f - startTime;
            return Mathf.Clamp(mouseTravelDistance / totalTime,0.5f,12);
        }
        void GetSpin(){
            curvePosition.x = Mathf.Clamp(transform.InverseTransformPoint(curvePosition - lastPosition).x, -15, 15);
            print(curvePosition.x);
        }
        Vector3 CalculateLaunchVelocity(){
            const int g = -18;
            var displacementXZ = curvePosition.normalized * (parent.InverseTransformPoint(curvePosition).x + parent.InverseTransformPoint(lastPosition + curvePosition).z);
            // Debug.DrawRay(displacementXZ, Vector3.up * 2, Color.cyan, 2f);
            var velocityY = Vector3.up * Mathf.Sqrt(-2 * g * maxCurveHeight);
            var velocityXZ = displacementXZ / (Mathf.Sqrt(-2* maxCurveHeight/g) + Mathf.Sqrt(2 * (0 - maxCurveHeight) / g));
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

        void OnKick(){
            if (swipePositions.Count < minIndex){
                swipePositions.Clear();
                isDraging = false;
                return;
            }
            ScreenToWorldPosition();
            FindMaxCurvePosition();
            // Debug.DrawRay(parent.position + curvePosition, Vector3.up * 10, Color.black, 5f);
            // Debug.DrawRay(parent.position + lastPosition, Vector3.up * 10, Color.white, 5f);
            maxCurveHeight = CalculateCurveHeight();
            rb.velocity += CalculateLaunchVelocity();
            GetSpin();
            isDraging = false;
            isKicked = true;
            Invoke(nameof(ReturnBall), 5f);
            StartCoroutine(AirResistance());
        }
        void ScreenToWorldPosition(){
            var screenPositionDifference = (Input.mousePosition - screenStartPosition) / Screen.dpi;
            var direction = parent.forward * (Mathf.Min(screenPositionDifference.y,14) * distanceMultiplier)  + parent.right * (screenPositionDifference.x * distanceMultiplier);
            lastPosition = direction;
            oldScreenPosition = Input.mousePosition;
            swipePositions.Add(direction);
            print("Add " + direction);
            // Debug.DrawRay(parent.position + direction, Vector3.up * 2, Color.red, 5f);
        }
        public void ReturnBall(){
            StopCoroutine(AirResistance());
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
        void OnTriggerEnter(Collider other){
            if(other.gameObject.layer == LayerMask.NameToLayer("Default"))
                curvePosition = Vector3.zero;
        }
        //DeveloperBuildTools:
        public void SetDistanceUpdateValue(float value){
            positionsDifferenceDistance = value;
        }
        public void SetSpinValue(float value){
            positionsDifferenceDistance = value;
        }
        public void SetDistanceM(float value){
            distanceMultiplier = value;
        }

        public void SetHeightMultiplier(float value){
            heightMultiplier = value;
        }
        
    }
}
