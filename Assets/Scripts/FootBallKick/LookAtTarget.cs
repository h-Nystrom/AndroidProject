using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FootBallKick{
    public class LookAtTarget : MonoBehaviour{
        [SerializeField] Transform target;
        void Start(){
            MessageHandler.instance.SubscribeTo<HitTargetMessage>(PointMessage);
        }

        void OnDestroy(){
            MessageHandler.instance.UnsubscribeFrom<HitTargetMessage>(PointMessage);
        }
        public void UpdatePositionAndRotation(){
            Invoke(nameof(UpdateDelay), 1f);
        }

        void UpdateDelay(){
            transform.position = new Vector3(Random.Range(-30f,31f), 0.5f,Random.Range(-15f,5f));
            transform.LookAt(target);
        }
        void PointMessage(HitTargetMessage hitTargetMessage){
            UpdatePositionAndRotation();
        }
    }
}
