using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnInterval = 1.5f;
    public float spawnDistance = 10f; // Distância do player onde o inimigo nasce

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0;
        }
    }

    void SpawnEnemy()
    {
        // Decide aleatoriamente se é esquerda (0) ou direita (1)
        int side = Random.Range(0, 2);

        // Define a direção: -1 é esquerda, 1 é direita
        float direction = (side == 0) ? -1f : 1f;

        // Calcula a posição de spawn baseada na posição ATUAL do player
        Vector3 spawnPos = new Vector3(player.position.x + (direction * spawnDistance), player.position.y, 0);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
