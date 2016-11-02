using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximim;

        public Count(int min, int max)
        {
            minimum = min;
            maximim = max;
        }

    }


    public int columns = 8;
    public int rows = 8;
    public Count wallcount = new Count(5, 9);
    public Count foodItems = new Count(1, 5);
    public GameObject exit;

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();


    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallcount.minimum, wallcount.maximim);
        LayoutObjectAtRandom(foodTiles, foodItems.minimum, foodItems.maximim);
        int enemyCount = (int) Mathf.Log(level);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (var x = -1; x < columns + 1; x++)
        {
            for (var y = -1; y < rows + 1; y++)
            {
                var toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                var instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    void InitializeList()
    {
        gridPositions.Clear();

        for (var x = 1; x < columns - 1; x++)
        {
            for (var y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    Vector3 RandomPosition()
    {
        var randomIndex = Random.Range(0, gridPositions.Count);
        var randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        var objectCount = Random.Range(min, max - 1);

        for (var i = 0; i < objectCount; i++)
        {
            var randomPositon = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPositon, Quaternion.identity);
        }
    }
}
