using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject staticBall;


    private GameObject _currBall;
    private bool _isLaunched = false;
    private const float BallOffset = 0.7f;
    private float _lowerBound = -10f;

    private void Update()
    {
        if (!_isLaunched) return;

        float ballYPos = _currBall.transform.position.y;
        if (ballYPos < _lowerBound)
        {
            DestroyBall();
        }

    }

    private void LateUpdate()
    {
        if (_isLaunched) return;
        AttachedTracking();
    }
    
    private void AttachedTracking()
    {
        Vector3 playerPos = player.transform.position;
        staticBall.transform.position = new Vector3(playerPos.x, playerPos.y + BallOffset, playerPos.z);
    }
    
    public void LaunchBall()
    {
        if (_isLaunched) return;
        
        _isLaunched = true;
        staticBall.SetActive(false);
        _currBall = Instantiate(ballPrefab, staticBall.transform.position, Quaternion.identity);
    }

    private void DestroyBall()
    {
        _isLaunched = false;
        staticBall.SetActive(true);
        Destroy(_currBall);
    }
    
    
}