using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour{
    [SerializeField] float zoomSpeed = 0.5f;
    Camera cam;
    void Start(){
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        if (Input.touchCount != 2) return;
        OnZoom();
    }
    void OnZoom(){
        var touchOne = Input.GetTouch(0);
        var touchTwo = Input.GetTouch(1);
        var touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;
        var touchTwoPreviousPosition = touchTwo.position - touchTwo.deltaPosition;
        
        var previousTouchDeltaMagMagnitude = (touchOnePreviousPosition - touchTwoPreviousPosition).magnitude;
        var touchDeltaMagnitude = (touchOne.position - touchTwo.position).magnitude;
        var deltaMagnitudeDifferance = previousTouchDeltaMagMagnitude - touchDeltaMagnitude;
        
        cam.fieldOfView += deltaMagnitudeDifferance * zoomSpeed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 15f, 130f);
    }
    
}
