using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    float zoomScroll;
    [SerializeField, Range(0, 10f)] float mouseSensitivity;
    [SerializeField] public CinemachineFreeLook FrCam;

    private void LateUpdate()
    {
        Zoom();
        AdjustSensitivity();
    }


    public void OnZoom(InputAction.CallbackContext context)
    {
        zoomScroll = context.ReadValue<Vector2>().y;
    }

    private void Zoom()
    {
        if (FrCam == null)
            return;

        if (FrCam.m_Lens.FieldOfView < 1)
            FrCam.m_Lens.FieldOfView = 1;
        else if (FrCam.m_Lens.FieldOfView > 70)
            FrCam.m_Lens.FieldOfView = 70;
        else
            FrCam.m_Lens.FieldOfView -= zoomScroll * Time.deltaTime;
    }

    private void AdjustSensitivity()
    {
        if (FrCam == null)
            return;

        FrCam.m_YAxis.m_MaxSpeed = mouseSensitivity;
        FrCam.m_XAxis.m_MaxSpeed = mouseSensitivity * 100;
    }
}
