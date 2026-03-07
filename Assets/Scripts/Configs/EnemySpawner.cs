using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /**
     Desenvolver fórmula de somatório de acréscimo de dificuldade;
        Teremos 4 tipos de inimigos.
        Variáveis para o somatório:
        ValorInimigo (VI): o valor que o inimigo tem no mapa;
        ValorMapa (VM): o valor que o mapa tem para spawn de inimigos (começa em 5);
        Tempo (T): tempo contado em segundos;
        Colddown de spawner: tempo até próximo inimigo ser spawnado;
        Fórmula: E = (
         */
    [Header("Configurações do Player")]
    public Transform player;

    [Header("Configurações de Spawn")]
    public float spawnDistance = 10f;
    [SerializeField] private float baseSpawnInterval = 2.0f;
    [SerializeField] private float minSpawnInterval = 0.5f;

    [Header("Lista de Inimigos")]
    [Tooltip("Configure os tipos de inimigos e suas chances de spawn")]
    [SerializeField] public List<SpawnConfig> spawnConfigs = new List<SpawnConfig>();

    [Header("Progressão de Dificuldade")]
    [SerializeField] private bool enableDifficultyScaling = true;
    [SerializeField] private float difficultyIncreaseRate = 0.05f; // 5% a cada intervalo
    [SerializeField] private float difficultyIncreaseInterval = 30f; // A cada 30 segundos

    private float spawnTimer;
    private float difficultyTimer;
    private float currentSpawnInterval;
    private float totalWeight;
    private int enemiesSpawned = 0;

    void Start()
    {
        currentSpawnInterval = baseSpawnInterval;
        CalculateTotalWeight();
        ValidateSpawnConfigs();
    }

    void Update()
    {
        // Não spawna se o jogo está pausado ou acabou
        if (GameManager.instance != null && (GameManager.instance.isPaused || GameManager.instance.isGameOver)) return;

        // Timer de spawn
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0;
        }

        // Progressão de dificuldade
        if (enableDifficultyScaling)
        {
            difficultyTimer += Time.deltaTime;
            if (difficultyTimer >= difficultyIncreaseInterval)
            {
                IncreaseDifficulty();
                difficultyTimer = 0;
            }
        }
    }
    // Spawna um inimigo aleatório baseado nos pesos configurados
    void SpawnEnemy()
    {
        if (spawnConfigs == null || spawnConfigs.Count == 0 || player == null)
        {
            Debug.LogWarning("EnemySpawner: Configurações inválidas!");
            return;
        }

        // Seleciona um inimigo usando randomização por peso
        GameObject selectedEnemy = SelectRandomEnemy();

        if (selectedEnemy == null)
        {
            Debug.LogWarning("EnemySpawner: Nenhum inimigo foi selecionado!");
            return;
        }

        // Decide aleatoriamente o lado (esquerda ou direita)
        int side = Random.Range(0, 2);
        float direction = (side == 0) ? -1f : 1f;

        // Calcula a posição de spawn
        Vector3 spawnPos = new Vector3(
            player.position.x + (direction * spawnDistance),
            player.position.y,
            0
        );

        // Instancia o inimigo
        GameObject spawnedEnemy = Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
        enemiesSpawned++;

        Debug.Log($"Inimigo spawnado: {selectedEnemy.name} (Total: {enemiesSpawned})");
    }

    // Seleciona um inimigo aleatório baseado nos pesos (weighted random) obs: ainda tá em testes
    GameObject SelectRandomEnemy()
    {
        // Gera um número aleatório entre 0 e o peso total
        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        // Percorre as configurações e seleciona baseado no peso
        foreach (SpawnConfig config in spawnConfigs)
        {
            if (!config.enabled) continue;

            cumulativeWeight += config.spawnWeight;

            if (randomValue <= cumulativeWeight) return config.enemyPrefab;
        }

        // Fallback: retorna o primeiro inimigo habilitado
        foreach (SpawnConfig config in spawnConfigs)
            if (config.enabled && config.enemyPrefab != null) return config.enemyPrefab;

        return null;
    }

    // Calcula o peso total de todos os inimigos habilitados
    void CalculateTotalWeight()
    {
        totalWeight = 0f;

        foreach (SpawnConfig config in spawnConfigs)
            if (config.enabled && config.enemyPrefab != null) totalWeight += config.spawnWeight;
        
        Debug.Log($"EnemySpawner: Peso total calculado = {totalWeight}");
    }

    // Valida as configurações de spawn
    void ValidateSpawnConfigs()
    {
        if (spawnConfigs.Count == 0)
        {
            Debug.LogError("EnemySpawner: Nenhuma configuração de spawn definida!");
            return;
        }

        int validConfigs = 0;
        foreach (SpawnConfig config in spawnConfigs) if (config.enabled && config.enemyPrefab != null) validConfigs++;
        
        if (validConfigs == 0) Debug.LogError("EnemySpawner: Nenhuma config válida encontrada!");
        else Debug.Log($"EnemySpawner: {validConfigs} tipos de inimigos configurados");
    }

    // Aumenta a dificuldade reduzindo o intervalo de spawn
    void IncreaseDifficulty()
    {
        float oldInterval = currentSpawnInterval;
        currentSpawnInterval = Mathf.Max(
            minSpawnInterval,
            currentSpawnInterval * (1f - difficultyIncreaseRate)
        );

        if (currentSpawnInterval != oldInterval) Debug.Log($"Dificuldade aumentada!");
    }

    // Reseta o spawner
    public void ResetSpawner()
    {
        spawnTimer = 0;
        difficultyTimer = 0;
        currentSpawnInterval = baseSpawnInterval;
        enemiesSpawned = 0;
        Debug.Log("EnemySpawner resetado");
    }

    // Habilita ou desabilita um tipo de inimigo
    public void SetEnemyEnabled(string enemyName, bool enabled)
    {
        for (int i = 0; i < spawnConfigs.Count; i++)
        {
            if (spawnConfigs[i].enemyPrefab != null && spawnConfigs[i].enemyPrefab.name == enemyName)
            {
                SpawnConfig config = spawnConfigs[i];
                config.enabled = enabled;
                spawnConfigs[i] = config;
                CalculateTotalWeight();
                Debug.Log($"{enemyName} {(enabled ? "habilitado" : "desabilitado")}");
                return;
            }
        }
    }

    public int GetEnemiesSpawned() { return enemiesSpawned; }

    public void UpdateSpawnWeight(string enemyName, float newWeight)
    {
        for (int i = 0; i < spawnConfigs.Count; i++)
        {
            if (spawnConfigs[i].enemyPrefab != null &&
                spawnConfigs[i].enemyPrefab.name == enemyName)
            {
                SpawnConfig config = spawnConfigs[i];
                config.spawnWeight = newWeight;
                spawnConfigs[i] = config;
                CalculateTotalWeight();
                Debug.Log($"Peso de {enemyName} atualizado para {newWeight}");
                return;
            }
        }
    }

}
[System.Serializable]
public class SpawnConfig
{
    [Tooltip("Prefab do inimigo a ser spawnado")]
    public GameObject enemyPrefab;

    [Tooltip("Peso de spawn (maior = mais frequente)")]
    [Range(0f, 100f)]
    public float spawnWeight = 10f;

    [Tooltip("Habilitar este inimigo no spawn?")]
    public bool enabled = true;

    [Tooltip("Nome descritivo (opcional)")]
    public string description;
}