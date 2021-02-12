using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FootBallKick{
    public class TargetSpawner : MonoBehaviour{
        [SerializeField] AimTarget targetPrefab;

        void Start(){
            MessageHandler.instance.SubscribeTo<HitTargetMessage>(SpawnNewTarget);
            ActuallySpawnTarget();
        }
        void OnDestroy(){
            MessageHandler.instance.UnsubscribeFrom<HitTargetMessage>(SpawnNewTarget);
        }

        void ActuallySpawnTarget(){
            var position = new Vector3(Random.Range(-5f, 6f),Random.Range(1f,6f),-0.2f);
            var instance = Instantiate(targetPrefab, transform);
            instance.transform.localPosition = position;
        }
        void SpawnNewTarget(HitTargetMessage hitTargetMessage){
            Invoke(nameof(ActuallySpawnTarget), 1f);
        }
    }
}
