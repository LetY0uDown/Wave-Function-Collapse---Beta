using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell
{
    public bool isCollapsed;

    public Tile topLeftCorner;
    public Tile topRightCorner;
    public Tile bottomLeftCorner;
    public Tile bottomRightCorner;

    public List<Tile> sideTiles;

    public List<Tile> cornerTiles;

    public Tile tile;

    public List<Tile> possibleTiles;

    public Vector2Int positionInGrid;

    public void RecalculatePossibleTiles(Cell[] gridData, int gridSize)
    {
        if (GetIndex(gridSize) == 0)
        {
            possibleTiles = new List<Tile> { cornerTiles[6] };
            return;
        }

        var (top, left) = GetNeighborsPositions(gridSize);

        if (top > -1)
        {
            var topNeighbor = gridData[top];
            var option = new string(topNeighbor.tile.options[2].Reverse().ToArray());

            var tilesForSearch = possibleTiles;

            if (IsSideTile(gridSize))
                tilesForSearch = sideTiles;

            if (CheckForCorner(gridSize))
                tilesForSearch = cornerTiles;            

            possibleTiles = tilesForSearch.Where(t => t.options[0] == option).ToList();
        }

        if (left > -1)
        {
            var leftNeighbor = gridData[left];
            var option = new string(leftNeighbor.tile.options[1].Reverse().ToArray());

            var tilesForSearch = possibleTiles;

            if (IsSideTile(gridSize))
                tilesForSearch = top > -1 ? possibleTiles : sideTiles;

            if (CheckForCorner(gridSize))
                tilesForSearch = cornerTiles;

            possibleTiles = tilesForSearch.Where(t => t.options[3] == option).ToList();
        }
    }

    private bool CheckForCorner(int gridSize)
    {
        if (positionInGrid.x + positionInGrid.y == 0)
            return true;

        if (GetIndex(gridSize) == gridSize * gridSize - 1)
            return true;

        if (GetIndex(gridSize) == gridSize - 1)
            return true;

        if (GetIndex(gridSize) == gridSize * gridSize - gridSize)
            return true;

         return false;
    }

    private bool IsSideTile(int gridSize)
    {
        return positionInGrid.x == gridSize - 1 || positionInGrid.y == gridSize - 1 || positionInGrid.y == 0 || positionInGrid.x == 0;
    }

    public bool Collapse(float cellSize, Transform parent)
    {
        var randomIndex = Random.Range(0, possibleTiles.Count);
        var newTile = possibleTiles[randomIndex];

        if (newTile == null) return false;

        tile = Object.Instantiate(newTile, parent);

        tile.transform.position = new Vector3(positionInGrid.y * cellSize, 0, positionInGrid.x * cellSize);

        isCollapsed = true;

        return true;
    }

    private (int top, int left) GetNeighborsPositions(int gridSize)
    {
        return (
            positionInGrid.y > 0 ? GetIndex(gridSize) - gridSize : -1,         // TOP
            positionInGrid.x > 0 ? GetIndex(gridSize) - 1 : -1                 // LEFT
        );
    }

    private int GetIndex(int gridSize) => positionInGrid.y * gridSize + positionInGrid.x;
}