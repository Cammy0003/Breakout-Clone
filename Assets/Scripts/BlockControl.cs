using System;
using Unity.VisualScripting;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    private GridManager _gridManager;
    private GameObject _block;

    private void Awake()
    {
        _block = gameObject;
    }

    public void Configure(GridManager manager)
    {
        _gridManager = manager;
    }
    

    private void OnCollisionExit(Collision _)
    {
        _gridManager.BlockCount -= 1;
        Debug.Log(_gridManager.BlockCount);
        _block.SetActive(false);
    }
    
    
}
