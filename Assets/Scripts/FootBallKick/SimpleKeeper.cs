using UnityEngine;

namespace FootBallKick{
    public class SimpleKeeper : MonoBehaviour{
        [SerializeField] float travelDistance;
        [SerializeField] float speed;
        Rigidbody rb;
        Vector3 positionTarget;
        Vector3 movementDir;
        void Start(){
            rb = GetComponent<Rigidbody>();
            positionTarget = transform.position + (transform.right * travelDistance);
            movementDir = transform.right;
        } 
        void FixedUpdate(){
            rb.MovePosition(transform.position + movementDir * (speed * Time.fixedDeltaTime));
            if (Vector3.Distance(transform.position, positionTarget) > 0.5f)
                return;
            positionTarget.x = -positionTarget.x;
            movementDir = -movementDir;
        }
    }
}
