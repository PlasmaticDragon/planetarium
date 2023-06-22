using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

public class VRCamera : MonoBehaviour
{ // flag to keep track whether we are dragging or not
    public  bool isDragging = false;

    // starting point of a camera movement
    float startMouseX;
    float startMouseY;

    // Camera component
    public Camera cam;

    // Zoom components
    private float targetZoom;
    private float zoomFactor = 10f;
    [SerializeField] private float zoomLerpSpeed = 10;

    // Phone Zoom
    [SerializeField]
    private float cameraSpeed = 0.01f;

    private TouchControls controls;
    private Coroutine zoomCouroutine;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        // Get our camera component
        cam = this.gameObject.GetComponent<Camera>();

        // Set target zoom
        cam.fieldOfView = 60f;
        targetZoom = cam.fieldOfView;
        
        controls.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        controls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }

    private void Awake() {
        controls = new TouchControls();
        cameraTransform = Camera.main.transform;
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
/**
    private void Start() {
        controls.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        controls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }
    **/

    private void ZoomStart() {
        zoomCouroutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd() {
        StopCoroutine(zoomCouroutine);
    }

    IEnumerator ZoomDetection() {
        float previousDistance = 0f, distance = 0f;
        while(true) {
            distance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(),
                controls.Touch.SecondaryFingerPosition.ReadValue<Vector2>());

            // Zoom out
            if (distance > previousDistance) {
                targetZoom -= cameraSpeed;
            }
            // Zoom in
            else if (distance < previousDistance) {
                targetZoom += cameraSpeed;
            }
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomLerpSpeed);
            previousDistance = distance;
            yield return null;
        }

    }

    // End Of Phone Zoom

	// Update is called once per frame
	void Update () {

        // Scroll
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 10f, 120f);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomLerpSpeed);
		    
        // if we press the left button and we haven't started dragging
        if(Input.GetMouseButtonDown(0) && !isDragging )
        {                
            // set the flag to true
            isDragging = true;

            // save the mouse starting position
            startMouseX = Input.mousePosition.x;
            startMouseY = Input.mousePosition.y;
        }
        // if we are not pressing the left btn, and we were dragging
        else if(Input.GetMouseButtonUp(0) && isDragging)
        {                
            // set the flag to false
            isDragging = false;
        }
    }

    void LateUpdate()
    {
        // Check if we are dragging
         if(isDragging)
        {
            //Calculate current mouse position
            float endMouseX = Input.mousePosition.x;
            float endMouseY = Input.mousePosition.y;

            //Difference (in screen coordinates)
            float diffX = endMouseX - startMouseX;
            float diffY = endMouseY - startMouseY;

            //New center of the screen
            float newCenterX = Screen.width / 2 + diffX;
            float newCenterY = Screen.height / 2 + diffY;

            //Get the world coordinate , this is where we want to look at
            Vector3 LookHerePoint = cam.ScreenToWorldPoint(new Vector3(newCenterX, newCenterY, cam.nearClipPlane));

            //Make our camera look at the "LookHerePoint"
            transform.LookAt(LookHerePoint);

            //starting position for the next call
            startMouseX = endMouseX;
            startMouseY = endMouseY;
        }
    }
}