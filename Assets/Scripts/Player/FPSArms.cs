using UnityEngine;

public class FPSArms : MonoBehaviour
{
    public Transform cameraTransform;
    public float swayAmount = 0.02f;
    public float swaySpeed = 3f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Mouse X") * swayAmount;
        float moveY = Input.GetAxis("Mouse Y") * swayAmount;

        Vector3 targetPosition = initialPosition + new Vector3(-moveX, -moveY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * swaySpeed);
    }
}
