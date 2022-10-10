using System.Collections;
using UnityEngine;
using System.Linq;

public class WaveFunctionCollapse : MonoBehaviour
{
    [SerializeField]
    private Tile[] tiles;

    [SerializeField]
    private Tile[] sideTiles;

    [SerializeField]
    private Tile[] cornerTiles;

    [Space]
    [SerializeField]
    private Material tilesMaterial;

    [Space]
    [SerializeField, Range(0, 37)]
    private int gridSize;

    [SerializeField]
    private bool isColorRandom;

    private const float CELL_SIZE = 2f;

    [Space]
    [SerializeField]
    private float delay;

    private bool isGenerated;

    private bool isProcessingNow;

    private void SetupCamera(int gridSize)
    {
        var camPosition = gridSize / 2 * CELL_SIZE - (gridSize % 2 == 0 ? 1 : 0);

        Camera.main.orthographic = !Settings.isCameraPerspective;

        Camera.main.fieldOfView = 60;

        Camera.main.orthographicSize = gridSize / 2 * CELL_SIZE + CELL_SIZE / 2;

        var camHeight = Mathf.Max(gridSize * 2, 16);

        Camera.main.transform.position = new Vector3(camPosition, camHeight, camPosition);
    }

    private void DestroyGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Generate(int gridSize, float delay)
    {
        if (isProcessingNow) return;

        isProcessingNow = true;

        SetupCamera(gridSize);       

        tilesMaterial.color = Settings.isColorRandom ? Random.ColorHSV() : Color.white;

        this.gridSize = gridSize;
        this.delay = delay;

        if (isGenerated)
            DestroyGrid();

        var gridData = InitializeGridData(gridSize);

        StartCoroutine(GenerateGrid(gridData));

        isGenerated = true;
    }

    private Cell[] InitializeGridData(int gridSize)
    {
        var cellData = new Cell[gridSize * gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                cellData[z * gridSize + x] = new Cell
                {
                    positionInGrid = new Vector2Int(x, z),
                    possibleTiles = tiles.ToList(),
                    sideTiles = sideTiles.ToList(),
                    cornerTiles = cornerTiles.ToList()
                };
            }
        }

        return cellData;
    }

    private IEnumerator GenerateGrid(Cell[] gridData)
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                var currentCell = gridData[z * gridSize + x];

                currentCell.RecalculatePossibleTiles(gridData, gridSize);

                if (!currentCell.Collapse(CELL_SIZE, transform))
                    StopCoroutine(nameof(GenerateGrid));

                yield return new WaitForSeconds(delay);
            }
        }

        isProcessingNow = false;
    }
}