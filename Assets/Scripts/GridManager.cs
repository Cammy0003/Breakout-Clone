using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;

    private int _blockCount;
    private const int NumRows = 2;  // 4
    private const int NumCols = 3;  // 7

    private Vector3[,] _worldGridCache;
    private List<GameObject> _spawnedBlocks = new List<GameObject>();
    
    private void Awake()
    {
        InitializeGridCache();
        _blockCount = NumRows * NumCols;
    }

    private void Start()
    {
        GenerateBlocks();
    }

    private void InitializeGridCache()
    {
        _worldGridCache = new Vector3[NumRows, NumCols];
        float totalWidth = transform.localScale.x;
        float totalHeight = transform.localScale.y;
        float localLeftBound = -totalWidth / 2;
        float localTopBound = totalHeight / 2;
        float cellWidth = totalWidth / NumCols;
        float cellHeight = totalHeight / NumRows;
        
        for (int r = 0; r < NumRows; r++)
        {
            for (int c = 0; c < NumCols; c++)
            {
                float xLocal = localLeftBound + (c + 0.5f) * cellWidth;
                float yLocal = localTopBound - (r + 0.5f) * cellHeight;
                Vector3 localPos = new Vector3(xLocal, yLocal, 0f);
                _worldGridCache[r, c] = transform.position + localPos;
            }
        }
        
    }

    private void GenerateBlocks()
    {
        _spawnedBlocks.Clear();
        foreach (Vector3 position in _worldGridCache)
        {
            GameObject spawnedBlock = Instantiate(blockPrefab, position, Quaternion.identity);
            _spawnedBlocks.Add(spawnedBlock);
            if (spawnedBlock.TryGetComponent<BlockControl>(out var blockControl))
            {
                blockControl.Configure(this);
            }
            
        }
    }
    
    [Header("Waiting Parameters")]
    private readonly WaitForSeconds _delay = new(0.5f);
    private bool _isWaiting;

    private void LateUpdate()
    {
        if (_blockCount <= 0 && !_isWaiting)
        {
            StartCoroutine(WaitResetRoutine());
        }
    }

    private void ResetGrid()
    {
        foreach (GameObject block in _spawnedBlocks)
        {
            block.SetActive(true);
        }

        _blockCount = _spawnedBlocks.Count;
    }

    public int BlockCount { get => _blockCount; set => _blockCount = value; }

    private IEnumerator WaitResetRoutine()
    {
        _isWaiting = true;
        yield return _delay;
        ResetGrid();
        _isWaiting = false;

    }
    
}
