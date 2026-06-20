using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;       
    public GameObject[] airObstaclePrefabs;   
    public float airSpawnHeight = 1.5f;         
    [Range(0f, 1f)] public float airObstacleChance = 0.3f; 

    public float spawnInterval = 1.5f;
    public float spawnZ = 40f;
    private float timer = 0f;
    private bool gameOver = false;
    public Transform player;

    void Update()
    {
        if (gameOver) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        bool spawnAir = airObstaclePrefabs.Length > 0 && Random.value < airObstacleChance;
        GameObject[] sourceArray = spawnAir ? airObstaclePrefabs : obstaclePrefabs;
        if (sourceArray.Length == 0) return;

        int index = Random.Range(0, sourceArray.Length);
        float spawnY = spawnAir ? airSpawnHeight : 0.5f;

        GameObject obs = Instantiate(sourceArray[index],
            new Vector3(0, spawnY, player.position.z + spawnZ), Quaternion.identity);

        obs.tag = "Obstacle";

        if (obs.GetComponent<MoveLeft>() == null)
            obs.AddComponent<MoveLeft>();

        Collider col = obs.GetComponent<Collider>();
        if (col == null)
        {
            BoxCollider bc = obs.AddComponent<BoxCollider>();
            bc.isTrigger = false;
        }
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }
}