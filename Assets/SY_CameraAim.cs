using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SY_CameraAim : MonoBehaviour
{
    [SerializeField] private KeyCode switchAim;
    [SerializeField] private Vector3[] positions;
    private int wichPosition;

    /*
    0 = centered
    1 = left side
    2 = right side 
    3 = top side 
     */

    private void Start()
    {
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(switchAim))
        {
            wichPosition++;
            if(wichPosition >= positions.Length)
            {
                wichPosition = 0;
            }

            transform.localPosition = positions[wichPosition];
        }
    }
}
