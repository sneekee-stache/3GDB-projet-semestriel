using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{
    [SerializeField] private float mouseSFDP = 50f;
    [SerializeField] private float minC = -70f, maxC = 80f;

    private CharacterController charController;
    public GameObject cameraFDP;
    private float xRotation = 0f;
    private Vector3 playerVelo;

    public Transform ParapluieTransform;

    void Start()
    {
        //cameraFDP = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X") * mouseSFDP * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSFDP * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minC, maxC);

        transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y, 0);
        transform.Rotate(Vector3.up * mouseX);

        transform.DOMove(ParapluieTransform.position, 01f);

        /*if (Input.mouseScrollDelta.y > 0f)
        {
            cameraFDP.transform.position += transform.forward;
        }
        if (Input.mouseScrollDelta.y < 0f)
        {
            cameraFDP.transform.position += -transform.forward;
        }*/
        //cameraFDP.transform.position = Vector3.ClampMagnitude(new Vector3() );

        /*if(Input.mouseScrollDelta.y > 0f && cameraFDP.transform.position.z < -20){
            cameraFDP.transform.position += transform.forward;
        }
        if(Input.mouseScrollDelta.y < 0f && cameraFDP.transform.position.z > -50){
            cameraFDP.transform.position += -transform.forward;
        }*/
        //cameraFDP.transform.position = new Vector3(cameraFDP.transform.position.x, cameraFDP.transform.position.y, cameraFDP.transform.position.z * transform.forward);
    }
}
