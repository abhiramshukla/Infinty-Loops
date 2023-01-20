using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Transform _cam;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private int _winningScore;
    [SerializeField] private int _currentScore;
    [SerializeField] private float[] _clickedPosition;

    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        GenerateGrid();
        ShuffleGrid();
        _winningScore = CalculateWinScore();
        _currentScore = CalculateStartingScore();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _clickedPosition = DetectCLickedTilePosition();
            if(_clickedPosition != null)
            {
                _currentScore -= CalculateCurrentScore(_clickedPosition);
                _tiles[new Vector2(_clickedPosition[0], _clickedPosition[1])].RotateTile();
                _currentScore += CalculateCurrentScore(_clickedPosition);
            }
        }
    }

    private float[] DetectCLickedTilePosition()
    {
        float[] pos = new float[2];
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if(hit.collider != null)
        {
            pos[0] = hit.collider.transform.position.x;
            pos[1] = hit.collider.transform.position.y;
            return pos;
        }

        return null;
    }

    private void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x,y), Quaternion.identity);
                spawnedTile.name = $"Tile ({x},{y})";

                _tiles[new Vector2(x,y)] = spawnedTile;
            }
        }
        _cam.transform.position = new Vector3((float) _width/2 - 0.5f, (float) _height/2 - 0.5f, -10);
    }

    private int CalculateWinScore()
    {
        int winVal = 0;
        foreach(KeyValuePair<Vector2, Tile> tile in _tiles)
        {
            winVal += tile.Value.NodeValue.CalculateNodeValue();
        }

        return winVal/2;
    }

    private int CalculateStartingScore()
    {
        int currentVal = 0;
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                if(y != _height - 1)
                    if(_tiles[new Vector2(x,y)].NodeValue.Top && _tiles[new Vector2(x,y+1)].NodeValue.Bottom)
                        currentVal++;

                if(x != _width - 1)
                    if(_tiles[new Vector2(x,y)].NodeValue.Right && _tiles[new Vector2(x+1,y)].NodeValue.Left)
                        currentVal++;
            }
        }
        return currentVal;
    }

    private int CalculateCurrentScore(float[] pos)
    {
        int score = 0;
        float x = pos[0], y = pos[1];
        //top
        if(y != _height - 1)
            if(_tiles[new Vector2(x,y)].NodeValue.Top && _tiles[new Vector2(x,y+1)].NodeValue.Bottom)
                score++;
        //right
        if(x != _width - 1)
            if(_tiles[new Vector2(x,y)].NodeValue.Right && _tiles[new Vector2(x+1,y)].NodeValue.Left)
                score++;
        //bottom
        if(y != 0)
            if(_tiles[new Vector2(x,y)].NodeValue.Bottom && _tiles[new Vector2(x,y-1)].NodeValue.Top)
                score++;
        //left
        if(x != 0)
            if(_tiles[new Vector2(x,y)].NodeValue.Left && _tiles[new Vector2(x-1,y)].NodeValue.Right)
                score++;
        
        return score;
    }

    private void ShuffleGrid()
    {
        foreach(KeyValuePair<Vector2, Tile> tile in _tiles)
        {
            int r = Random.Range(0,4);
            tile.Value.RotateTile(r);
        }
    }

}
