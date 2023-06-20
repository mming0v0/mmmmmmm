using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	[SerializeField]
	private GameObject tilePrefab;                             
	[SerializeField]
	private Transform tilesParent;                        
                               
	private Vector2Int puzzleSize = new Vector2Int(4, 4);

    private void Start()
    {
        SpawntIles();
    
    }

    private void SpawntIles()
    {
        for (int y = 0; y < puzzleSize.y; ++y)
        {
            for (int x = 0; x <puzzleSize.x; ++ x)
            {
                Instantiate(tilePrefab, tilesParent);
            }
        }
    }

}

