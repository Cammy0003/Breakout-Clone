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
    
    [Header("External Parameters")]
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
        else if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerBounce(collision);
        }
    }

    private void HandleBarrierBounce(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        Vector3 incomingVel = -collision.relativeVelocity;
        Vector3 reflectedVel = Vector3.Reflect(incomingVel, normal);

        float minTan = Mathf.Tan(15f * Mathf.Deg2Rad);

        if (Math.Abs(reflectedVel.y) < Mathf.Abs(reflectedVel.x) * minTan)
        {
            float signY = reflectedVel.y >= 0 ? 1f : -1f;
            if (Mathf.Approximately(reflectedVel.y, 0f))
            {
                signY = UnityEngine.Random.value > 0.5f ? 1f : -1f;
            }

            reflectedVel.y = Mathf.Abs(reflectedVel.x) * minTan * signY;
        }

        _rb.linearVelocity = reflectedVel.normalized * ballSpeed;
    }

    private void HandlePlayerBounce(Collision collision)
    {
        float paddleCenter = collision.transform.position.x;
        float paddleWidth = collision.collider.bounds.size.x;
        float paddleImpactPoint = collision.contacts[0].point.x;
        float paddleRelative = (paddleImpactPoint - paddleCenter) / (paddleWidth / 2);
        paddleRelative = Mathf.Clamp(paddleRelative, -1f, 1f);
        
        float maxTheta = 70f * Mathf.Deg2Rad;
        float theta = paddleRelative * maxTheta;
        Vector3 reflectedVel = new Vector3(Mathf.Sin(theta), Mathf.Cos(theta), 0f);
        
        _rb.linearVelocity = reflectedVel.normalized * ballSpeed;
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
