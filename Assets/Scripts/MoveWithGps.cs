using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithGps : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody rb;
    Vector3 startDir;

    void Start(){
        rb = GetComponent<Rigidbody>();
        
        Invoke(nameof(StartGps),2f);
        StartCoroutine(UpdateLocation());
        
    }
    IEnumerator UpdateLocation()
    {
        while (true){
            if(!Input.location.isEnabledByUser)
                yield return new WaitForSeconds(1);
            var dir = new Vector3(Input.location.lastData.latitude, 0,Input.location.lastData.longitude);
            print(dir);
            //transform.position = dir;
            yield return new WaitForSeconds(1);
        }
        
    }

    void OnDestroy(){
        Input.compass.enabled = false;
        Input.location.Stop();
        StopCoroutine(UpdateLocation());
    }
    void StartGps(){
        if(Input.location.status == LocationServiceStatus.Running)
            return;
        Input.location.Start();
        print("Started");
        startDir = Input.compass.rawVector;
    }
}
