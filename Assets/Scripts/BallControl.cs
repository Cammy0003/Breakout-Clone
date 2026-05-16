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

    private BallState _currentState;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _ballOffset = 0.7f;
        SetState(BallState.Attached);
    }

    private void FixedUpdate()
    {
        if (_currentState == BallState.Projectile)
        {
            Launch();
        }
    }

    private void LateUpdate()
    {
        if (_currentState == BallState.Attached)
        {
            Tracking();
        }
    }

    private void Tracking()
    {
        Vector3 playerPos = player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y + _ballOffset, playerPos.z);
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
        _rb.linearVelocity = transform.up * 30f;
    }

    public BallState CurState()
    {
        return _currentState;
    }
}
