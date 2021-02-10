using UnityEngine;

namespace FootBallKick{
    public class TargetSpawner : MonoBehaviour{
        [SerializeField] AimTarget targetPrefab;

        void Awake(){
            
        }
        [ContextMenu(nameof(SpawnNewTarget))]
        public void SpawnNewTarget(){
            print("Test");
            var position = new Vector3(Random.Range(-5f, 6f),Random.Range(1f,6f),0);
            var instance = Instantiate(targetPrefab, transform);
            instance.transform.localPosition = position;
        }
    }
}
