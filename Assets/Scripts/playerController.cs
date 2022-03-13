using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class playerController : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public bool isRun = false, isStompAttack = false, isLeftAttack = false, isRightAttack = false;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;
    private Animator animator;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
      
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
            

        }
        run();
        stompAttack();
        leftArm();
        rightArm();
        shield();
        meditate();
    }

    public void run()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isRun = true;
            activateLayer("Base Layer");
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }
    public void stompAttack() { 
        if (Input.GetKey(KeyCode.C))
        {
            activateLayer("attack");
            animator.SetBool("isStompAttack", true);
        }
        else
        {
            animator.SetBool("isStompAttack", false);
        }
    }
    public void leftArm()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            activateLayer("attack");
            animator.SetBool("isLeftArmAttack", true);
        }
        else
        {
            animator.SetBool("isLeftArmAttack", false);
        }
    }
    public void rightArm()
    {
        if(Input.GetKey(KeyCode.X))
        {
            activateLayer("attack");
            animator.SetBool("isRightArmAttack", true);
        }
        else
        {
            animator.SetBool("isRightArmAttack", false);
        }
    }
    public void shield()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            activateLayer("shield");
            animator.SetBool("isShield", true);
        }
        else
        {
            animator.SetBool("isShield", false);
        }
    }
    public void meditate()
    {
        if (Input.GetKey(KeyCode.E))
        {
            activateLayer("meditate");
            animator.SetBool("isMeditate", true);
        }
        else
        {
           // animator.SetBool("isMeditate", false);
        }
    }
    public void activateLayer(string layername)
        {
            for (int i = 0; i < animator.layerCount; i++)
            {
                animator.SetLayerWeight(i, 0);
            }
            animator.SetLayerWeight(animator.GetLayerIndex(layername), 1);
        }
    }