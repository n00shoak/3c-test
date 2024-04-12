using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SY_PlayerRotation : MonoBehaviour
{
    // Start is called before the first frame update
    [Header(">> PlayerRotation")]
    public float rotationSpeedGlobal;
    public float rotationSmoothnessGlobal;
    public float maxAngleUp;
    public float maxAngleDown;

    [SerializeField]
    private Transform cameraPos;
    private float targetRotationY;
    private float targetRotationX;
    public bool isMouseUnlocked = true;
    private float oldTargetRotation;
    private float coordRotation;

    [Header(">> Advanced Rotation Parameter")]
    public float rotationSpeedY;
    public float rotationSpeedX;
    public float rotationSmoothnessY;
    public float rotationSmoothnessX;

    private void Start()
    {
        lockCurs();

        // set data if needed
        if (rotationSpeedY == 0) { rotationSpeedY = rotationSpeedGlobal; }
        if (rotationSpeedX == 0) { rotationSpeedX = rotationSpeedGlobal; }
        if (rotationSmoothnessY == 0) { rotationSmoothnessY = rotationSmoothnessGlobal; }
        if (rotationSmoothnessX == 0) { rotationSmoothnessX = rotationSmoothnessGlobal; }
    }

    public void unlockCurs()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void lockCurs()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseUnlocked)
        {
            // === apply roatation to player ===
            // get input on X axis
            float mouseX = Input.GetAxis("Mouse X");
            // get targeted rotation
            targetRotationY += mouseX * rotationSpeedX * Time.deltaTime;

            // smooth rotation
            float smoothedRotationY = Mathf.LerpAngle(transform.eulerAngles.y, targetRotationY, rotationSmoothnessX * Time.deltaTime);

            // Apply rotation to player
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, smoothedRotationY, transform.eulerAngles.z);


            // === apply roatation to camera ===
            // get input on Y axis
            float mouseY = Input.GetAxis("Mouse Y");

            // get targeted rotation
            targetRotationX -= mouseY * rotationSpeedY * Time.deltaTime;
            targetRotationX = Mathf.Clamp(targetRotationX, -maxAngleDown, maxAngleUp); // limit vertical Rotation

            // smooth rotation
            float smoothedRotationX = Mathf.LerpAngle(cameraPos.eulerAngles.x, targetRotationX, rotationSmoothnessY * Time.deltaTime);

            // Apply rotation to cam
            cameraPos.eulerAngles = new Vector3(smoothedRotationX, cameraPos.eulerAngles.y);
        }
    }
}
