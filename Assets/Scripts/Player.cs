using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float jumpPower = 1.0f;

    private float inputMoveAxis;
    private float lastInputMoveAxis;
    private bool isGround;

    private Rigidbody rb;
    private InputAction moveAction;
    private InputAction jumpAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PlayerInput playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Start()
    {
        
    }

    void Update()
    {
        inputMoveAxis = moveAction.ReadValue<Vector2>().x;
    }

    void FixedUpdate()
    {
        //左右移動
        var velocity = rb.linearVelocity;
        if (isGround)
        {
            velocity.x = inputMoveAxis * speed;
            rb.linearVelocity = velocity;
            lastInputMoveAxis = inputMoveAxis;
        }
        else
        {
            velocity.x = lastInputMoveAxis * speed;
            rb.linearVelocity = velocity;
        }

        //接地判定
        var origin = transform.position + new Vector3(0, 0.01f, 0);
        RaycastHit hit;
        int layerMask = 1 << 6;
        isGround = Physics.Raycast(origin, Vector3.down, out hit, 0.02f, layerMask);

        //ジャンプ
        if (jumpAction.WasPressedThisFrame() && isGround)
        {
            rb.AddForce(0, jumpPower, 0, ForceMode.Impulse);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal")) GameClear();
    }

    void GameClear()
    {
        Debug.Log("GameClear");
    }
}
