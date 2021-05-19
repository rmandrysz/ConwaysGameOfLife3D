using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int HORIZONTAL_CELL_AMOUNT = 50;
    public static int VERTICAL_CELL_AMOUNT = 50;

    public const int NEIGHBORS_TO_RESURRECT = 3;
    public const int NEIGHBORS_TO_SURVIVE = 2;
    private const int MAX_HEIGHT = 40;
    private int _currentHeight = 0;
    private GameObject[,,] _cellObjects;
    private Cell[,] _cellScripts;

    [Header("Parameters")]
    [Range(0,100)]
    public int spawnChance = 30;

    public float updateDelay = 0.5f;

    [Header("Dependencies")]
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Material _newCellMaterial;
    [SerializeField] private Material _oldCellMaterial;

    private void Awake() 
    {
        _cellObjects = new GameObject[HORIZONTAL_CELL_AMOUNT, VERTICAL_CELL_AMOUNT, MAX_HEIGHT];
        _cellScripts = new Cell[HORIZONTAL_CELL_AMOUNT, VERTICAL_CELL_AMOUNT];
    }

    private void Start() 
    {
        GenerateCells();
        GenerateObjects();
        UpdateCellVisibility(_currentHeight);
        StartCoroutine(UpdateGrid());
    }

    private IEnumerator UpdateGrid() {
        WaitForSeconds delay = new WaitForSeconds(updateDelay);
        while ( true )
        {
            UpdateGridLevel();
            CountCellNeighbors();
            UpdateCells();
            UpdateObjects();

            yield return delay;
        }
    }

    private void UpdateCells() 
    {
        for ( int x = 0; x < HORIZONTAL_CELL_AMOUNT; ++x ) 
        {
            for ( int y = 0; y < VERTICAL_CELL_AMOUNT; ++y )
            {
                bool canResurrect = _cellScripts[x, y].numberOfNeighbors == NEIGHBORS_TO_RESURRECT;
                bool canSurvive = _cellScripts[x, y].numberOfNeighbors == NEIGHBORS_TO_SURVIVE;

                if ( canResurrect )
                {
                    _cellScripts[x, y].setAlive(true);
                }
                else if ( !canSurvive )
                {
                    _cellScripts[x, y].setAlive(false);
                }
            } 
        }
    }

    private void UpdateObjects() 
    {
        bool isMaxHeightReached = _currentHeight >= MAX_HEIGHT;
        int levelToUpdate = _currentHeight % MAX_HEIGHT;

        if (isMaxHeightReached)
        {
            RecycleBottomLevel();
        }

        UpdateCellVisibility(levelToUpdate);


    }

    private void GenerateCells() 
    {
        for ( int x = 0; x < HORIZONTAL_CELL_AMOUNT; ++x ) 
        {
            for ( int y = 0; y < VERTICAL_CELL_AMOUNT; ++y )
            {
                Cell newCell = new Cell();

                bool isAlive = Random.Range(0, 100) <= spawnChance;
                newCell.setAlive(isAlive);

                _cellScripts[x, y] = newCell;
            } 
        }
    }

    private void GenerateObjects()
    {
        Transform parentTransform = this.transform;
        Vector3 position = new Vector3(0, 0, 0);

        for ( int x = 0; x < HORIZONTAL_CELL_AMOUNT; ++x ) 
        {
            for ( int y = 0; y < VERTICAL_CELL_AMOUNT; ++y )
            {
                for ( int z = 0; z < MAX_HEIGHT; ++z )
                {
                    position.Set(x, z, y);
                    Quaternion rotation = Quaternion.identity;

                    _cellObjects[x, y, z] =
                        Instantiate(_cellPrefab, position, rotation, parentTransform) as GameObject;

                    _cellObjects[x, y, z].SetActive(false);
                }
            } 
        }
    }

    private void CountCellNeighbors() 
    {
        for (int x = 0; x < HORIZONTAL_CELL_AMOUNT; ++x)
        {
            for (int y = 0; y < VERTICAL_CELL_AMOUNT; ++y)
            {
                int neighbors = 0;
                neighbors += countNorthNeighbors(x, y);
                neighbors += countSouthNeighbors(x, y);
                neighbors += countWestNeighbors(x, y);
                neighbors += countEastNeighbors(x, y);

                _cellScripts[x, y].numberOfNeighbors = neighbors;
            }
        }
    }

    private int countNorthNeighbors(int x, int y)
    {
        int northNeighbors = 0;

        // ---------------------------------------------------------------------------------------------------------------
        // North
        if (y + 1 < VERTICAL_CELL_AMOUNT)
        {
            if (_cellScripts[x, y + 1].isAlive()) { 
                ++northNeighbors;
            }

            // Northeast 
            if (x + 1 < HORIZONTAL_CELL_AMOUNT)
            {
                if (_cellScripts[x + 1, y + 1].isAlive())
                {
                    ++northNeighbors;
                }
            }
            else
            {
                if (_cellScripts[0, y + 1].isAlive())
                {
                    ++northNeighbors;
                }
            }

            // Northwest
            if (x - 1 >= 0)
            {
                if (_cellScripts[x - 1, y + 1].isAlive())
                {
                    ++northNeighbors;
                }
            } 
            else
            {
                if (_cellScripts[HORIZONTAL_CELL_AMOUNT - 1, y + 1].isAlive())
                {
                    ++northNeighbors;
                }
            }
        }
        else
        {
            if (_cellScripts[x, 0].isAlive())
            {
                ++northNeighbors;
            }

            // Northeast
            if (x + 1 < HORIZONTAL_CELL_AMOUNT)
            {
                if (_cellScripts[x + 1, 0].isAlive())
                {
                    ++northNeighbors;
                }
            }
            else
            {
                if (_cellScripts[0, 0].isAlive())
                {
                    ++northNeighbors;
                }
            }

            // Northwest
            if (x - 1 >= 0)
            {
                if (_cellScripts[x - 1, 0].isAlive())
                {
                    ++northNeighbors;
                }
            }
            else
            {
                if (_cellScripts[HORIZONTAL_CELL_AMOUNT - 1, 0].isAlive())
                {
                    ++northNeighbors;
                }
            }
        }

        return northNeighbors;
    }

    private int countSouthNeighbors(int x, int y) 
    {
        int southNeighbors = 0;

        // ---------------------------------------------------------------------------------------------------------------
        // South
        if (y - 1 >= 0)
        {
            if (_cellScripts[x, y - 1].isAlive())
            {
                ++southNeighbors;
            }

            // Northeast 
            if (x + 1 < HORIZONTAL_CELL_AMOUNT)
            {
                if (_cellScripts[x + 1, y - 1].isAlive())
                {
                    ++southNeighbors;
                }
            }
            else
            {
                if (_cellScripts[0, y - 1].isAlive())
                {
                    ++southNeighbors;
                }
            }

            // Northwest
            if (x - 1 >= 0)
            {
                if (_cellScripts[x - 1, y - 1].isAlive())
                {
                    ++southNeighbors;
                }
            }
            else
            {
                if (_cellScripts[HORIZONTAL_CELL_AMOUNT - 1, y - 1].isAlive())
                {
                    ++southNeighbors;
                }
            }
        }
        else
        {
            if (_cellScripts[x, VERTICAL_CELL_AMOUNT - 1].isAlive())
            {
                ++southNeighbors;
            }
                                
            // Southeast 
            if (x + 1 < HORIZONTAL_CELL_AMOUNT)
            {
                if (_cellScripts[x + 1, VERTICAL_CELL_AMOUNT - 1].isAlive())
                {
                    ++southNeighbors;
                }
            }
            else
            {
                if (_cellScripts[0, VERTICAL_CELL_AMOUNT - 1].isAlive())
                {
                    ++southNeighbors;
                }
            }

            // Southwest
            if (x - 1 >= 0)
            {
                if (_cellScripts[x - 1, VERTICAL_CELL_AMOUNT - 1].isAlive())
                {
                    ++southNeighbors;
                }
            }
            else
            {
                if (_cellScripts[HORIZONTAL_CELL_AMOUNT - 1, VERTICAL_CELL_AMOUNT - 1].isAlive())
                {
                    ++southNeighbors;
                }
            }
        }

        return southNeighbors;
    }

    private int countWestNeighbors(int x, int y) 
    {
        int westNeighbors = 0;

        // ---------------------------------------------------------------------------------------------------------------
        // West
        if (x + 1 < HORIZONTAL_CELL_AMOUNT) 
        {
            if (_cellScripts[x + 1, y].isAlive())
            {
                ++westNeighbors;
            }
        }
        else
        {
            if (_cellScripts[0, y].isAlive())
            {
                ++westNeighbors;
            }

        }

        return westNeighbors;
    }

    private int countEastNeighbors(int x, int y)
    {
        int eastNeighbors = 0;
        // ---------------------------------------------------------------------------------------------------------------
        // East
        if (x - 1 >= 0)
        {
            if (_cellScripts[x - 1, y].isAlive())
            {
                ++eastNeighbors;
            }
        }
        else
        {
            if (_cellScripts[HORIZONTAL_CELL_AMOUNT - 1, y].isAlive())
            {
                ++eastNeighbors;
            }
        }

        return eastNeighbors;
    }

    private void RecycleBottomLevel()
    {
        int levelToTransfer = _currentHeight % MAX_HEIGHT;
        Vector3 topHeight = new Vector3(0.0f, MAX_HEIGHT, 0.0f);

        for ( int x = 0; x < HORIZONTAL_CELL_AMOUNT; ++x ) 
        {
            for ( int y = 0; y < VERTICAL_CELL_AMOUNT; ++y )
            {
                Transform cellTransform = _cellObjects[x, y, levelToTransfer].transform;
                _cellObjects[x, y, levelToTransfer].SetActive(false);
                cellTransform.position += topHeight;
            }
        }
    }

    private void CreateNewLevel()
    {
        Transform parentTransform = this.transform;
        Vector3 position = new Vector3(0, 0, 0);
        Quaternion rotation = Quaternion.identity;

        for ( int x = 0; x < HORIZONTAL_CELL_AMOUNT; ++x ) 
        {
            for ( int y = 0; y < VERTICAL_CELL_AMOUNT; ++y )
            {
                position.Set(x, _currentHeight, y);

                _cellObjects[x, y, _currentHeight] =
                    Instantiate(_cellPrefab, position, rotation, parentTransform) as GameObject;

                _cellObjects[x, y, _currentHeight].SetActive(false);
            } 
        }
    }

    private void UpdateCellVisibility(int levelToUpdate)
    {
        for ( int x = 0; x < HORIZONTAL_CELL_AMOUNT; ++x ) 
        {
            for ( int y = 0; y < VERTICAL_CELL_AMOUNT; ++y )
            {
                bool isAlive = _cellScripts[x, y].isAlive();
                _cellObjects[x, y, levelToUpdate].SetActive(isAlive);
            }
        }
    }

    private void UpdateGridLevel()
    {
        ++_currentHeight;
    }

}
