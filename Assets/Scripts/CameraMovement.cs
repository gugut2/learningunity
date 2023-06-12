using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    private bool isPanning = false;
    [SerializeField] private float mouseSpeed = 3f;
    [SerializeField] private float orbitDamping = 10f;
    private Vector3 localRot;
    private Quaternion quaternion = Quaternion.Euler(50f, 0f, 0f);

    // Update is called once per frame
    void Update()
    {   
        ApplyPan();
    }

    public void ApplyPan()
    {
        if(isPanning == true)
        {
            localRot.x += Input.GetAxis("Mouse X") * mouseSpeed;
            quaternion = Quaternion.Euler(50f, localRot.x, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, Time.deltaTime * orbitDamping);
        }
    }

    public void CameraPan(InputAction.CallbackContext context)
    {
        isPanning = true;

        if(context.canceled)
        {
            isPanning = false;
        }
    }
}
