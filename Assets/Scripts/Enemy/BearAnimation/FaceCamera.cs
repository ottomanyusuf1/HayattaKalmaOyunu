using System.Data.Common;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform localTrans;

    // Camera facingCamera

    void Start()
    {
        localTrans = GetComponent<Transform>();
    }

    void Update()
    {
        if (Camera.main)
        {
            localTrans.LookAt(2 * localTrans.position - Camera.main.transform.position);
        }
    }
}
