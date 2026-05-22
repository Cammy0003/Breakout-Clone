using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;

    private int _blockCount;
    private const int NumRows = 4;
    private const int NumCols = 7;

    private Vector3[,] _worldGridCache;
    
    private void Awake()
    {
        InitializeGridCache();
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
        foreach (Vector3 position in _worldGridCache)
        {
            Instantiate(blockPrefab, position, Quaternion.identity);
        }
    }

}
