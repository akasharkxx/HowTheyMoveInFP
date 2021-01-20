using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraLook : MonoBehaviour
{
    public Camera cameraChildObject;

    public float xSensitivity = 2.0f;
    public float ySensitivity = 2.0f;

    public Vector2 upAndDownRange = new Vector2(90, -50f);

    private void Update()
    {
        float mouseXValue = Input.GetAxis("Mouse X");
        float mouseYValue = Input.GetAxis("Mouse Y");

        float xAngle = mouseXValue * xSensitivity;
        float yAngle = mouseYValue * ySensitivity;

        Vector3 xEulerRotation = transform.rotation.eulerAngles + new Vector3(0f, xAngle, 0f);
        Vector3 yEulerRotation = cameraChildObject.transform.localRotation.eulerAngles + new Vector3(yAngle, 0f, 0f);

        //Debug.Log(yEulerRotation);

        //yEulerRotation.x = Mathf.Clamp(yEulerRotation.x, upAndDownRange.y, upAndDownRange.x);

        this.transform.rotation = Quaternion.Euler(xEulerRotation);
        cameraChildObject.transform.localRotation = Quaternion.Euler(yEulerRotation);
    }
}
