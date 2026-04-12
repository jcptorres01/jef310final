using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public ThirdPersonCam.CameraStyle zoneStyle;

    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonCam cam = other.GetComponentInParent<ThirdPersonCam>();

        if (cam != null)
        {
            cam.SwitchCameraStyle(zoneStyle);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ThirdPersonCam cam = other.GetComponentInParent<ThirdPersonCam>();

        if (cam != null)
        {
            cam.SwitchCameraStyle(ThirdPersonCam.CameraStyle.Basic);
        }
    }
}