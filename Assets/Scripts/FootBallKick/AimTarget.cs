using System;
using UnityEngine;

namespace FootBallKick{
    public class AimTarget : MonoBehaviour
    {
        void OnTriggerEnter(Collider other){
            if (other.gameObject.layer == LayerMask.NameToLayer("Ball")){
                Destroy(gameObject);
            }
        }
    }
}
