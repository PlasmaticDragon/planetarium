using System.Collections;
using UnityEngine;
using System.Runtime.Serialization;

public class PinchDetection : MonoBehaviour
{
    [SerializeField]
    // private float cameraSpeed = 4f;

    private TouchControls controls;
    private Coroutine zoomCouroutine;
    private Transform cameraTransform;

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

    private void Start() {
        controls.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        controls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }

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

            // targetZoom = 0;
            // Zoom out
            if (distance > previousDistance) {
                
            }
            // Zoom in
            else if (distance < previousDistance) {

            }
            //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomLerpSpeed);
            previousDistance = distance;
            yield return null;
        }

    }
}
