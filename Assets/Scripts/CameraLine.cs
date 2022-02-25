using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLine : MonoBehaviour
{
    GameObject target;
    public float smoothing = 5f;

    Vector3 offset;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - target.transform.position;
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = new Vector3(target.transform.position.x, offset.y, target.transform.position.z + offset.z);
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
