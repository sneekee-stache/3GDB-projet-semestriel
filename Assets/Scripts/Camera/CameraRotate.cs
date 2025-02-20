using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraRotate : MonoBehaviour
{
    public TextMeshProUGUI CameraDefinition;
    public GameObject camSigne1;
    public GameObject camSigne2;
    public GameObject camSigne3;
    public int CameraControl = 0;
    // 0 = Camera auto focus
    // 1 = Camera libre
    // 2 = camera fige
    private Animator animator;
    
    [Header("souris")]
    [SerializeField] private float mouseSFDP = 50f;
    [SerializeField] private float minC = -70f, maxC = 80f;

    [Header("Components")]
    public Transform ParapluieTransform;
    public Transform FocusTransform;
    public GameObject cameraFDP;
    public GameObject JumpOrientation;

    private CharacterController charController;
    private float xRotation = 0f;
    private Vector3 playerVelo;

    [Header("Réglages camera")]
    public float CameraRotationAutoHaut = 0.5f;
    public float CameraRotationAutobas = 1f;

    public Transform rotationReference;
    public float speedRotationHorizontale;
    public float TimerAvantRotation;
    private float TimerRotation;
    public float TimerAvantRotationVerticale;
    private float TimerRotationVerticale;
    public float upRotation;
    public float distanceGroundCheck;
    public bool canFocus;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        TimerRotation = TimerAvantRotation;
        //CameraDefinition = GameObject.Find("CamDefinition").GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) SceneManager.LoadScene("Level 2");
        if (Input.GetKeyDown(KeyCode.F6)) SceneManager.LoadScene("Level 0");
        if (Input.GetKeyDown(KeyCode.F7)) SceneManager.LoadScene("Level 1");
        if (Input.GetKeyDown(KeyCode.F8)) SceneManager.LoadScene("test");

        //fait bouger le cam automatiquement en fonction du parapluie
        if (TimerRotationVerticale <= 0f && CameraControl == 0)
        {
            if (JumpOrientation.transform.position.y - gameObject.transform.position.y >= 2 && (transform.rotation.eulerAngles.x <= 280f || transform.rotation.eulerAngles.x >= 330f))
            {
                xRotation -= (JumpOrientation.transform.position.y - gameObject.transform.position.y) * CameraRotationAutoHaut / 10;
            }

            if (JumpOrientation.transform.position.y - gameObject.transform.position.y <= -0.1f && (transform.rotation.eulerAngles.x <= 10f || transform.rotation.eulerAngles.x >= 80f))
            {
                xRotation += (JumpOrientation.transform.position.y - gameObject.transform.position.y) * -CameraRotationAutobas / 10;
            }
            else if (JumpOrientation.transform.position.y - gameObject.transform.position.y <= -1f)
            {
                xRotation += (JumpOrientation.transform.position.y - gameObject.transform.position.y) * -CameraRotationAutobas / 10;
            }
            //Debug.Log(transform.rotation.eulerAngles.x);

            if (JumpOrientation.transform.position.z - gameObject.transform.position.z >= 2)
            {

            }
        }


        /*if (TimerRotationVerticale <= 0f &&CameraControl == 2)
        {
            if (FocusTransform.transform.position.y - gameObject.transform.position.y >= 2 && (transform.rotation.eulerAngles.x <= 280f || transform.rotation.eulerAngles.x >= 330f))
            {
                xRotation -= (FocusTransform.transform.position.y - gameObject.transform.position.y) * CameraRotationAutoHaut / 10;
            }

            if (FocusTransform.transform.position.y - gameObject.transform.position.y <= -0.1f && (transform.rotation.eulerAngles.x <= 10f || transform.rotation.eulerAngles.x >= 80f))
            {
                xRotation += (FocusTransform.transform.position.y - gameObject.transform.position.y) * -CameraRotationAutobas / 10;
            }
            else if (FocusTransform.transform.position.y - gameObject.transform.position.y <= -1f)
            {
                xRotation += (FocusTransform.transform.position.y - gameObject.transform.position.y) * -CameraRotationAutobas / 10;
            }
            //Debug.Log(transform.rotation.eulerAngles.x);
        }*/
        //fait bouger le cam avec input
        float mouseX = Input.GetAxis("Mouse X") * mouseSFDP * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSFDP * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minC, maxC);

        transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y, 0);
        transform.Rotate(Vector3.up * mouseX);
        transform.DOMove(ParapluieTransform.position, 0.5f);

        if (mouseX != 0f || mouseY != 0f)
        {
            TimerRotation = TimerAvantRotation;
            TimerRotationVerticale = TimerAvantRotationVerticale;
            if (CameraControl == 2) TimerRotationVerticale = 0.1f;
        }

        RaycastHit cameraGroundCheck;
        if(Physics.Raycast(cameraFDP.transform.position,-cameraFDP.transform.up,out cameraGroundCheck,distanceGroundCheck)){
            if(cameraGroundCheck.collider.CompareTag("Ground")){
                TimerRotation = TimerAvantRotation;
                TimerRotationVerticale = TimerAvantRotationVerticale;
                if (CameraControl == 2) TimerRotationVerticale = 0.1f;
                xRotation+=5;
            }
        }
        
        TimerRotationVerticale -= Time.deltaTime;
        TimerRotation -= Time.deltaTime;
        if (TimerRotation <= 0f && CameraControl == 0)
        {
            var lookPos = rotationReference.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speedRotationHorizontale);
        }
        if (TimerRotation <= 0f && CameraControl == 2)
        {
            var lookPos = FocusTransform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speedRotationHorizontale);
        }

        if (Input.GetButtonDown("ChangeCamera"))
        {
            CameraControl++;
            CameraDefinition.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            if (CameraControl >= 2 && !canFocus)
            {
                CameraControl = 0;
            }
            else if (CameraControl >= 3 &&canFocus)
            {
                CameraControl = 0;
            }
        }

        if (CameraControl == 0)
        {
            camSigne1.SetActive(true);
            camSigne2.SetActive(false);
            camSigne3.SetActive(false);
            CameraDefinition.text = "Camera fixe";
        }
        if (CameraControl == 1)
        {
            camSigne1.SetActive(false);
            camSigne2.SetActive(true);
            camSigne3.SetActive(false);
            CameraDefinition.text = "Camera libre";
        }
        if (CameraControl == 2)
        {
            camSigne1.SetActive(false);
            camSigne2.SetActive(false);
            camSigne3.SetActive(true);
            CameraDefinition.text = "Camera focus";
        }

        //tentative de look horizontal
        /*Vector3 direction = rotationReference.position - transform.position;
        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speedRotationHorizontale * Time.time);*/
        //transform.LookAt(rotationReference, Vector3.up);
    }
    private void OnDrawGizmos() {
        Gizmos.color=Color.red;
        Gizmos.DrawRay(cameraFDP.transform.position,-cameraFDP.transform.up*distanceGroundCheck);
    }
    public void DisableAnimator()
    {
        animator.enabled = false;
        //cameraZoom.CamReset();
    }
}
