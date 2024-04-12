using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SY_PlayerMovement : MonoBehaviour
{
    [Header(">> References")]
    [SerializeField] private Rigidbody rb;

    [Header(">> Inputs")]
    [SerializeField] private KeyCode forward = KeyCode.W;
    [SerializeField] private KeyCode backward = KeyCode.S;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode right = KeyCode.D;

    [SerializeField] private KeyCode jump = KeyCode.Space;


    [Header(">> Horizontal Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelerationMultiplier, decelerationMultiplier;
    [SerializeField] private AnimationCurve acceleration, deceleration;
    [SerializeField] private float LaterlaSpeedDebuff;
    [SerializeField] private float softSpeedCap;

    [Header(">> Vertical Movement")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool activateGravity = true;
    [SerializeField] private float Gravity;
    [SerializeField] private float TerminalVelocity;
    [SerializeField] private float jumpForce, jumpAmmount, JumpLeft, LeepStrenght;
    [SerializeField] private float groundDistance;
    [SerializeField] private bool cumulativeJumps,leep;


    private bool moving;
    private float VectorX;
    [SerializeField] private float VectorY;
    private float VectorZ;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HorizontalMouvement();
        VerticalMovement();

        // apply movement 
        rb.velocity = (transform.TransformDirection(new Vector3(VectorZ, VectorY, VectorX)));
    }

    private void HorizontalMouvement()
    {
        //check for input
        if (!moving)
        {
            if (Input.GetKey(forward) || Input.GetKey(backward) || Input.GetKey(left) || Input.GetKey(right))
            {
                moving = true;
            }
            else
            {
                moving = false;
            }
        }

        //get Acceleration Rate
        float accelX = acceleration.Evaluate((float)System.Math.Round(Mathf.Abs(VectorX) / maxSpeed, 2)) * accelerationMultiplier;
        float accelY = acceleration.Evaluate((float)System.Math.Round(Mathf.Abs(VectorZ) / maxSpeed, 2)) * accelerationMultiplier * LaterlaSpeedDebuff;

        //get deceleration rate
        float deccelX = deceleration.Evaluate((float)System.Math.Round(Mathf.Abs(VectorX) / maxSpeed, 2)) * decelerationMultiplier;
        float deccelY = deceleration.Evaluate((float)System.Math.Round(Mathf.Abs(VectorZ) / maxSpeed, 2)) * decelerationMultiplier * LaterlaSpeedDebuff;


        // movement on global X axis 
        if (Input.GetKey(forward) && VectorX < maxSpeed) { VectorX += accelX; } // move forward
        else if (Input.GetKey(backward) && VectorX > -maxSpeed) { VectorX -= accelX; } // move backward
        else if (!Input.GetKey(forward) && !Input.GetKey(backward))
        { VectorX = Mathf.Lerp(0, VectorX, 1 - deccelX); } // if idle input on x axis , slow down on this axis


        // movement on global Z axis 
        if (Input.GetKey(right) && VectorZ < maxSpeed) { VectorZ += accelY; } // move to the right
        else if (Input.GetKey(left) && VectorZ > -maxSpeed) { VectorZ -= accelY; } // move to the left
        else if (!Input.GetKey(right) && !Input.GetKey(left))
        { VectorZ = Mathf.Lerp(0, VectorZ, 1 - deccelY); } // if idle input on z axis , slow down on this axis

        // friction 
        //decelerate when max speed is reach 
        if (VectorX > maxSpeed || VectorX < -maxSpeed) { VectorX -= Mathf.Lerp(0, VectorX, softSpeedCap / 10); }
        if (VectorZ > maxSpeed || VectorZ < -maxSpeed) { VectorZ -= Mathf.Lerp(0, VectorZ, softSpeedCap / 10); }

        //Freez Movement On low Speed
        if (!moving)
        {
            if (VectorX > 0.001f && VectorX < -0.001f) { VectorX = 0; }
        }
    }
    private void VerticalMovement()
    {
        Ray ray = new Ray(transform.localPosition, Vector3.down);
        RaycastHit hit;

        VectorY -= Gravity * Time.deltaTime;
        if (Input.GetKeyDown(jump) && JumpLeft > 0)
        {
            if (!cumulativeJumps)
            {
                VectorY = 0;
            }

            VectorY = jumpForce;
            JumpLeft--;
        }
        // if grounded
        if (Physics.Raycast(ray, out hit, groundDistance, ~playerLayer) && VectorY < 1)
        {
            VectorY = -0.1f;
            JumpLeft = jumpAmmount;
        }

    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.localPosition, Vector3.down);
        //Debug.DrawLine(ray.origin, ray.origin + ray.direction * groundDistance, Color.red, 0.1f);
    }
}

