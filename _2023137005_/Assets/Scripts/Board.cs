using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	[SerializeField]
	private GameObject tilePrefab;                             
	[SerializeField]
	private Transform tilesParent;   
    
    private	List<Tile>	tileList;
                               
	private Vector2Int puzzleSize = new Vector2Int(4, 4);
    private	float		neighborTileDistance = 102;

    public	Vector3		EmptyTilePosition { set; get; }
    public	int			Playtime { private set; get; } = 0;		
	public	int			MoveCount { private set; get; } = 0;

    private IEnumerator Start()
    {
        tileList = new List<Tile>();

        SpawntIles();

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(tilesParent.GetComponent<RectTransform>());

		yield return new WaitForEndOfFrame();

        tileList.ForEach(x => x.SetCorrectPosition());

        StartCoroutine("OnSuffle");

        StartCoroutine("CalculatePlaytime");
    }

    private void SpawntIles()
    {
        for (int y = 0; y < puzzleSize.y; ++y)
        {
            for (int x = 0; x <puzzleSize.x; ++ x)
            {
                GameObject clone = Instantiate(tilePrefab, tilesParent);
				Tile tile = clone.GetComponent<Tile>();

                tile.Setup(this, puzzleSize.x * puzzleSize.y, y * puzzleSize.x + x + 1);

                tileList.Add(tile);
            }
        }
    }
    private IEnumerator OnSuffle()
	{
		float current	= 0;
		float percent	= 0;
		float time		= 1.5f;

		while ( percent < 1 )
		{
			current += Time.deltaTime;
			percent = current / time;

			int index = Random.Range(0, puzzleSize.x * puzzleSize.y);
            tileList[index].transform.SetAsLastSibling();

            yield return null;
        }
        EmptyTilePosition = tileList[tileList.Count-1].GetComponent<RectTransform>().localPosition;
    }

    public void IsMoveTile(Tile tile)
	{
		if ( Vector3.Distance(EmptyTilePosition, tile.GetComponent<RectTransform>().localPosition) == neighborTileDistance)
		{
			Vector3 goalPosition = EmptyTilePosition;

			EmptyTilePosition = tile.GetComponent<RectTransform>().localPosition;

			tile.OnMoveTo(goalPosition);

            MoveCount ++;
		}
	}

    public void IsGameOver()
	{
		List<Tile> tiles = tileList.FindAll(x => x.IsCorrected == true);

		Debug.Log("Correct Count : "+tiles.Count);
		if ( tiles.Count == puzzleSize.x * puzzleSize.y - 1 )
		{
			Debug.Log("GameClear");
            StopCoroutine("CalculatePlaytime");
            GetComponent<UIController>().OnResultPanel();
        }
    }

    private IEnumerator CalculatePlaytime()
	{
		while ( true )
		{
			Playtime ++;

			yield return new WaitForSeconds(1);
		}
	}
}
