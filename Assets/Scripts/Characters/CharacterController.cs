using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //PlayerControls controls;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float groundRaycastAvstånd = 0.1f;
    public float pickupRaycastDistance = 2f;

    private float throwForce = 4f;

    private bool springer = false;
    private bool hoppar = false;
    private bool ärPåMark = false;
    private bool isCarrying = false;
    private GameObject carriedObject;
    //private SwordSwing swordSwing;
    private Animator animator;
    public ParticleSystem dust;
    public GameObject rightArm;

    private int defaultLayer;

    [SerializeField]
    private Transform cameraTransform;
    
    private Rigidbody rb;
    private Transform playerCameraTransform;
    private void Awake()
    {


    }
    private void OnEnable()
    {
        //controls.GamePlay.Enable();
    }
    private void OnDisable()
    {
        //controls.GamePlay.Disable();
    }
    void Sprint()
    {
        springer = true;
    }
    void StopSprint()
    {
        springer = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //swordSwing = GetComponentInChildren<SwordSwing>();
        animator = GetComponent<Animator>();
        playerCameraTransform = Camera.main.transform;
        rightArm = GameObject.Find("HoldPosition");
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(playerCameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = (moveHorizontal * playerCameraTransform.right + moveVertical * cameraForward).normalized;

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

            if (springer)
            {
                movement *= sprintSpeed;
            }
            else
            {
                movement *= walkSpeed;
            }

            animator.SetBool("Running", true); // Enable the "Run" animation state
        }
        else
        {
            movement = Vector3.zero;
            animator.SetBool("Running", false); // Disable the "Run" animation state
        }

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        movement = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movement;
        movement.Normalize();

        /* if (Input.GetMouseButtonDown(0)) //Innan nya inputSystemet
         {
             AttackSword();

         }


         if (Input.GetKeyDown(KeyCode.LeftShift))
         {
             springer = true;
         }
         if (Input.GetKeyUp(KeyCode.LeftShift))
         {
             springer = false;
         }
        */
        /*
        if (Input.GetButtonDown("Jump"))
        {
            if (!isCarrying)
            {
                PickUpObject();
              
            }
            else
            {
                DropObject();
              
            }
        }
        */
        if (isCarrying)
        {
            CarryObject();
          
        }
        if (movement.magnitude > 0.1f)
        {
            CreateDust();
        }
    }

    void AttackSword()
    {
        if (isCarrying)
        {
            return;
        }

    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundRaycastAvstånd))
        {
            ärPåMark = true;
            //Debug.Log("Jag står på marken");
        }
        else
        {
            ärPåMark = false;
            
        }
    }

    void PickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRaycastDistance))
        {
            if (hit.collider.CompareTag("PickUp"))
            {
                carriedObject = hit.collider.gameObject;
                

                defaultLayer = carriedObject.layer;

                
                int carriedObjectLayer = LayerMask.NameToLayer("CarriedObject");
                carriedObject.layer = carriedObjectLayer;

               
                int playerLayer = LayerMask.NameToLayer("Player");
                Physics.IgnoreLayerCollision(playerLayer, carriedObjectLayer, true);

                carriedObject.transform.position = rightArm.transform.position;
                carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                carriedObject.transform.SetParent(transform);
             // carriedObject.transform.localPosition = new Vector3(0f, 0.0f, 0.0f);
                isCarrying = true;
                animator.SetBool("Grabbing", true);
            }
        }
    }

    void CarryObject()
    {
        if (carriedObject != null)
        {
            Vector3 desiredPosition = transform.position + transform.forward * 0.5f + Vector3.up * 0.5f;
            carriedObject.transform.position = desiredPosition;
            carriedObject.GetComponent<Rigidbody>().velocity = rb.velocity;
        }
        

    }

    public void DropObject()
    {
        if (carriedObject !=null)
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int carriedObjectLayer = LayerMask.NameToLayer("CarriedObject");
            Physics.IgnoreLayerCollision(playerLayer, carriedObjectLayer, false);

            carriedObject.transform.SetParent(null);
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject.layer = defaultLayer;
            carriedObject.layer = defaultLayer;

            Rigidbody carriedObjectRb = carriedObject.GetComponent<Rigidbody>();

            Vector3 throwDirection = transform.forward + Vector3.up;

            carriedObjectRb.velocity = throwDirection * throwForce;
            carriedObjectRb.angularVelocity = new Vector3(0f, 2f, 0f);
            carriedObject = null;
            isCarrying = false;
        }
        
        animator.SetBool("Grabbing", false);
    }

    void CreateDust()
    {
        dust.Play();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRaycastAvstånd);

        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * pickupRaycastDistance);
    }
}
