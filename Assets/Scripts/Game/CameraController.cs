using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private GameObject pickerObject;
    private Vector3 cameraOffset;

    private void Start()
    {
        pickerObject = PickerController.Instance.gameObject;
        cameraOffset = transform.position - pickerObject.transform.position;
    }

    private void LateUpdate()
    {
        if (pickerObject != null)
        {
            transform.position = pickerObject.transform.position + cameraOffset;
        }
    }
}
