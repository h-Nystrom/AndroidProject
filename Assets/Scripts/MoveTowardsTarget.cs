using System;
using UnityEngine;

public class MoveTowardsTarget : MonoBehaviour
{ 
    [SerializeField] Transform target;

    void LateUpdate(){
        transform.position = target.position;
    }
}
