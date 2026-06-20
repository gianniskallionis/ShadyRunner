using UnityEngine;

public class RewardSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public float spawnInterval = 3f;
    public float spawnZ = 15f;
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
            SpawnCoin();
        }
    }
    void SpawnCoin()
    {
        if (coinPrefab == null) return;
        GameObject coin = Instantiate(coinPrefab,
            new Vector3(0, 1, player.position.z + spawnZ), Quaternion.identity);
        coin.tag = "Reward";
        Collider col = coin.GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
        coin.AddComponent<MoveLeft>();
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }
}