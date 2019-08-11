using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    public GameManager GM;
    public GameObject GridContainer;
    public GameObject Tile;
    public List<GameObject> tilesGO = new List<GameObject>();
    public List<Tile> tiles = new List<Tile>();
    public List<int> emptyIndexes = new List<int>();

    public float explodeDelay = 0.5f;
    public float tempExplodeDelay = 0.0f;
    public float hotDelay = 1.0f;
    public float hotTime = 0.4f;
    public bool paused = false;

    private void Awake()
    {
        GM = GetComponent<GameManager>();
    }

    public void DrawSquare(int columns)
    {
        AutoGridLayout AGL = GridContainer.GetComponent<AutoGridLayout>();

        //Clear All Children
        foreach (Transform child in GridContainer.transform)
        {
            Destroy(child.gameObject);
        }

        AGL.m_Column = columns;
        
        //Generate Squares
        for(int i = 0; i < columns * columns; i++)
        {
            GameObject temp = Instantiate(Tile, Vector3.zero, Quaternion.identity) as GameObject;
            temp.transform.SetParent(GridContainer.transform, false);
            tiles.Add(temp.GetComponent<Tile>());
            tilesGO.Add(temp);
            emptyIndexes.Add(i);
            tiles[i].ApplyStyle();
        }
    }

    public void GridExplosion()
    {
        //Pick a random number of tiles to keep turn warm
        int rand = Random.Range(1, emptyIndexes.Count - 1);
        //int rand = 15;
        for(int i = 0; i < rand; i++)
        {
            if(emptyIndexes.Count > 1)
            {
                //random number from 0 to lenght of empty index which should be 0
                int rand2 = Random.Range(0, emptyIndexes.Count);
                tiles[emptyIndexes[rand2]].TileState = gridState.Warm;
                tiles[emptyIndexes[rand2]].ApplyStyle();
                emptyIndexes.RemoveAt(rand2);
            }
        }
        //On explode reduce delay by 2%
        explodeDelay *= 0.98f;
    }

    private void Update()
    {
        if (!paused)
        {
            tempExplodeDelay -= Time.deltaTime;
            if (tempExplodeDelay < 0f && emptyIndexes.Count > 1)
            {
                tempExplodeDelay = explodeDelay;
                GridExplosion();
            }

            TickTiles();
        }
    }

    public void TickTiles()
    {
        for(int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].TileState == gridState.Warm || tiles[i].TileState == gridState.Hot)
            {
                tiles[i].TileWarmth += Time.deltaTime;
                if (tiles[i].TileWarmth > hotTime + hotDelay)
                {
                    tiles[i].TileState = gridState.Normal;
                    tiles[i].ApplyStyle();
                    emptyIndexes.Add(i);
                }
                else if(tiles[i].TileWarmth > hotDelay)
                {
                    tiles[i].TileState = gridState.Hot;
                    tiles[i].ApplyStyle();
                    if (GM.PlayerChar != null)
                    {
                        if (tiles[GM.PlayerSCR.convArrToInt(GM.PlayerSCR.location)].TileState == gridState.Hot)
                        {
                            Destroy(GM.PlayerChar);
                            GM.OverLayImg.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style", 0)].dead;
                            GM.gameOver = true;
                        }
                    }
                    }
                else
                {
                    tiles[i].ApplyStyle();
                }
            }
        }
    }

    public void ClearBoard()
    {
        emptyIndexes.Clear();
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].TileState = gridState.Normal;
            tiles[i].ApplyStyle();
            emptyIndexes.Add(i);
        }
    }
}
