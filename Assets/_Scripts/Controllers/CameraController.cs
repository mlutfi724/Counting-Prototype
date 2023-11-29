using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform boxTransform;
    [SerializeField] private float mouseSpeed = 3;
    [SerializeField] private float orbitDamping = 10;

    //private GameManager gameManager;
    private Vector3 localRot;

    private void Start()
    {
        //gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (GameManager.isGameActive)
        {
            //transform.position = boxTransform.position;

            localRot.x += Input.GetAxis("Mouse X") * mouseSpeed;
            localRot.y -= Input.GetAxis("Mouse Y") * mouseSpeed;

            localRot.y = Mathf.Clamp(localRot.y, -89f, 89f);

            Quaternion quaternionRotation = Quaternion.Euler(localRot.y, localRot.x, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, quaternionRotation, Time.deltaTime * orbitDamping);
        }
    }
}