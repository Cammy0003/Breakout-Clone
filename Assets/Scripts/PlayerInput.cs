using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private BallControl attachedBall;
    
    private Rigidbody _rb; 
    private Vector2 _moveInput;
    
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnLaunch(InputAction.CallbackContext ctx)
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
        float moveX = _moveInput.x * moveSpeed * Time.fixedDeltaTime;
        Vector3 newPos = _rb.position + new Vector3(moveX, 0, 0);
        
        _rb.MovePosition(newPos);
    }
    
    
}
