using System;
using Unity.VisualScripting;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public enum BallState { Attached, Projectile }
    
    [Header("Tracking Settings")]
    private float _ballOffset;
    public GameObject player;
    
    [Header("Physics Settings")]
    private Rigidbody _rb;
    [SerializeField] private float ballSpeed = 20f;
    private float _ballAngle;
    [SerializeField] private float maxAngle;
    
    

    private BallState _currentState;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _ballOffset = 0.7f;
        SetState(BallState.Attached);
    }

    private void FixedUpdate()
    {
        if (_currentState == BallState.Projectile 
            && !Mathf.Approximately(_rb.linearVelocity.sqrMagnitude, ballSpeed*ballSpeed))
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * ballSpeed;
        }
    }
    
    private void LateUpdate()
    {
        if (_currentState == BallState.Attached)
        {
            AttachedTracking();
        }
    }

    private void AttachedTracking()
    {
        Vector3 playerPos = player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y + _ballOffset, playerPos.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Barriers"))
        {
            HandleBarrierBounce(collision);
        }
    }

    private void HandleBarrierBounce(Collision collision)
    {
        GameObject hitObject = collision.gameObject;
        Wall wall = hitObject.GetComponent<Wall>();
        WallOrientation orientation = wall.Orientation;
        
        Vector3 v = _rb.linearVelocity;


        if (orientation == WallOrientation.Top)
        {
            v.y = -v.y;
        }
        else
        {
            v.x = -v.x;
        }
        

        _rb.linearVelocity = v.normalized * ballSpeed;

        // Vector3 v = _rb.linearVelocity;
        // v.y += UnityEngine.Random.Range(-1f, 1f);
        // _rb.linearVelocity = v.normalized * ballSpeed;
    }

    private void SetState(BallState newState)
    {
        switch (newState)
        {
            case BallState.Attached:
                _rb.isKinematic = true;
                break;
            
            case BallState.Projectile:
                _rb.isKinematic = false;
                break;
        }

        _currentState = newState;
    }

    public void Launch()
    {
        SetState(BallState.Projectile);
        float xDir = UnityEngine.Random.Range(-1f, 1f);
        float yDir = UnityEngine.Random.Range(0f, 1f);

        _rb.linearVelocity = new Vector3(xDir, yDir, 0).normalized * ballSpeed;

    }

    public BallState CurState()
    {
        return _currentState;
    }
}
