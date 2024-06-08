using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Queue<GameObject> activeTiles = new Queue<GameObject>();

    public Transform playerTransform;
    public GameObject[] tilePrefabs;
    public GameObject[] roadsideTilePrefabs;

    public float zSpawn = 0;
    public float tileLength = 30;

    public int numberOfTiles;
    public int numberOfRoadsideTiles;
    int roadsideTilesCreated = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == 0)
            {
                SpawnTile(0);
                SpawnRoadsideTiles(1, 1);
            }
            else
            {
                SpawnTile(Random.Range(0, tilePrefabs.Length));
                SpawnRoadsideTiles(Random.Range(0, roadsideTilePrefabs.Length),
                    Random.Range(0, roadsideTilePrefabs.Length));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.z - 45 > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            SpawnRoadsideTiles(Random.Range(0, roadsideTilePrefabs.Length),
                Random.Range(0, roadsideTilePrefabs.Length));
            DeleteTiles();
        }
    }

    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Enqueue(go);
        zSpawn += tileLength;
    }

    public void SpawnRoadsideTiles(int leftTileIndex, int rightTileIndex)
    {
        GameObject left = Instantiate(roadsideTilePrefabs[leftTileIndex], new Vector3(0.35F + (tileLength / 8.12f), 0, tileLength * roadsideTilesCreated - 10), transform.rotation);
        GameObject right = Instantiate(roadsideTilePrefabs[rightTileIndex], new Vector3(-(0.35F + (tileLength / 1.535f)), 0, tileLength * roadsideTilesCreated - 10), transform.rotation);

        activeTiles.Enqueue(left);
        activeTiles.Enqueue(right);

        roadsideTilesCreated++;
    }

    private void DeleteTiles()
    {
        // Удаляем один основной и два боковых тайла
        for (int i = 0; i < 3; i++)
        {
            if (activeTiles.Count > 0)
            {
                GameObject tileToDelete = activeTiles.Dequeue();
                Destroy(tileToDelete);
            }
        }
    }
}
