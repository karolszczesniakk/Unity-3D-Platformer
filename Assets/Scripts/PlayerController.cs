using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;
    public float rotationSpeed = 5;
    public float bounceForce = 8f;

    private Vector3 moveDirection;
    

    public CharacterController charController;

    private Camera theCam;

    public GameObject playerModel;

    public Animator anim;

    public bool isKnocking;
    public float knockbackLength=1f;
    private float knockbackCounter;
    public Vector2 knockbackPower;

    public GameObject enemyHurtbox;

    public GameObject[] playerPieces;

    public bool bStopMove;

    private bool isReallyGrounded;

    //public GameObject player;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        theCam = Camera.main;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!isKnocking && !bStopMove)
        {
            MovementInput();
        }
        if(isKnocking)
        {
            Knock();  
        }

        if(bStopMove)
        {
            StopMove();
        }

        AnimationVariables();

        //enables enemyhitbox when falling down which fixed bug that made enemies getting damaged while jumping up
        EnemyHurtBoxOnOff();

    }

    public void Knockback()
    {
        isKnocking = true;
        knockbackCounter = knockbackLength;
        Debug.Log("Knocked back");
        moveDirection.y = knockbackPower.y;
        charController.Move(moveDirection * Time.deltaTime);
    }

    public void Bounce()
    {
        moveDirection.y = bounceForce;
        charController.Move(moveDirection * Time.deltaTime);
    }

    //enables enemyhitbox when falling down which fixed bug that made enemies getting damaged while jumping up
    public void EnemyHurtBoxOnOff()
    {
        if (moveDirection.y >= -2f)
        {
            enemyHurtbox.SetActive(false);
        }
        if (moveDirection.y < -2f)
        {
            enemyHurtbox.SetActive(true);
        }
    }


    private void StopMove()
    {
        moveDirection = Vector3.zero;
        charController.Move(moveDirection);
    }

    private void Knock()
    {
        knockbackCounter -= Time.deltaTime;

        float yStore = moveDirection.y;
        moveDirection = playerModel.transform.forward * -knockbackPower.x;
        moveDirection.y = yStore;

        if (charController.isGrounded == true)
        {
            moveDirection.y = 0f;
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
        charController.Move(moveDirection * Time.deltaTime);

        if (knockbackCounter <= 0)
        {
            isKnocking = false;
        }
    }


    private void MovementInput()
    {
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveDirection.Normalize();
        moveDirection = moveDirection * moveSpeed;
        moveDirection.y = yStore;
 
        if (charController.isGrounded == true)
        {
            // move direction to -1f because of isGrounded not working properly, if its set to 0 isGrounded is not consistently true when standing still, -0.1f seems to fix it
            moveDirection.y = -1f;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }
        moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

        charController.Move(moveDirection * Time.deltaTime);
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, theCam.transform.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
    }


    private void AnimationVariables()
    {
        anim.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        anim.SetBool("Grounded", charController.isGrounded);
        isReallyGrounded = charController.isGrounded; //debug variable
    }


}
