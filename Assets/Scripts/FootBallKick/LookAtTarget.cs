using UnityEngine;

namespace FootBallKick{
    public class LookAtTarget : MonoBehaviour{
        [SerializeField] Transform target;
        Transform camTransform;
        void Start(){
            camTransform = Camera.main.transform;
            UpdateRotation();
        }
        public void UpdateRotation(){
            camTransform.LookAt(target);
        }
    }
}
