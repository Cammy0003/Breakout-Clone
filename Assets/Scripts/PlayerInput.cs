using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private BallControl attachedBall;
    
    private Rigidbody _rb;
    private Collider _col;
    private Vector2 _moveInput;

    [Header("Wall Collision Parameters")] 
    [SerializeField] private GameObject barrier; // RightBarrier
    private float _minPaddleBound;
    private float _maxPaddleBound;
    
    
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // _col = GetComponent<Collider>();
    }

    private void Start()
    {
        float wallXPos = barrier.transform.position.x;
        float halfPaddleXLength = transform.localScale.x / 2;
        _minPaddleBound = -wallXPos + halfPaddleXLength;
        _maxPaddleBound = wallXPos - halfPaddleXLength;
    }


    private void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnLaunch(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && attachedBall != null 
                                    && attachedBall.CurState() == BallControl.BallState.Attached)
        {
            attachedBall.Launch();
            attachedBall = null;
        }
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float moveX = _moveInput.x * moveSpeed * Time.deltaTime;
        float targetX = _rb.position.x + moveX;
        
        targetX = Mathf.Clamp(targetX, _minPaddleBound, _maxPaddleBound);

        Vector3 newPos = new Vector3(targetX, _rb.position.y, _rb.position.z);
        _rb.MovePosition(newPos);
    }
}
