using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Referências")]
    public EnemySpawner spawner;

    [Header("Configuração de Waves")]
    [Tooltip("Lista de waves a serem executadas em ordem")]
    public List<Wave> waves = new List<Wave>();

    [Tooltip("Repetir waves após terminar?")]
    public bool loopWaves = true;

    [Header("Trigger de Wave")]
    [Tooltip("Modo de ativação das waves")]
    public WaveTriggerMode triggerMode = WaveTriggerMode.TimeInterval;

    [Tooltip("Tempo entre waves (se modo = TimeInterval)")]
    public float timeBetweenWaves = 60f;

    [Tooltip("Inimigos necessários para próxima wave (se modo = EnemyCount)")]
    public int enemiesPerWave = 50;

    private int currentWaveIndex = 0;
    private float waveTimer = 0f;
    private int enemiesAtWaveStart = 0;
    private bool wavesActive = false;

    void Start()
    {
        if (spawner == null) spawner = GetComponent<EnemySpawner>();

        if (spawner == null)
        {
            Debug.LogError("WaveManager: EnemySpawner não encontrado!");
            enabled = false;
            return;
        }

        if (waves.Count > 0)
        {
            wavesActive = true;
            ApplyWave(0);
        }
    }

    void Update()
    {
        if (!wavesActive || waves.Count == 0) return;

        switch (triggerMode)
        {
            case WaveTriggerMode.TimeInterval:
                UpdateTimeBasedWave();
                break;

            case WaveTriggerMode.EnemyCount:
                UpdateEnemyBasedWave();
                break;

            case WaveTriggerMode.Manual:
                // Waves são ativadas manualmente via código
                break;
        }
    }

    // Atualiza wave baseado em tempo
    void UpdateTimeBasedWave()
    {
        waveTimer += Time.deltaTime;

        if (waveTimer >= timeBetweenWaves)
        {
            NextWave();
            waveTimer = 0f;
        }
    }

    // Atualiza wave baseado em número de inimigos spawnados
    void UpdateEnemyBasedWave()
    {
        int currentEnemies = spawner.GetEnemiesSpawned();
        int enemiesSinceWaveStart = currentEnemies - enemiesAtWaveStart;

        if (enemiesSinceWaveStart >= enemiesPerWave)
        {
            NextWave();
            enemiesAtWaveStart = currentEnemies;
        }
    }

    // Avança para a próxima wave
    public void NextWave()
    {
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Count)
        {
            if (loopWaves)
            {
                currentWaveIndex = 0;
                Debug.Log("WaveManager: Reiniciando waves");
            }
            else
            {
                wavesActive = false;
                Debug.Log("WaveManager: Todas as waves concluídas");
                return;
            }
        }

        ApplyWave(currentWaveIndex);
    }

    // Aplica uma wave específica
    public void ApplyWave(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waves.Count)
        {
            Debug.LogError($"WaveManager: Wave index {waveIndex} inválido!");
            return;
        }

        Wave wave = waves[waveIndex];
        currentWaveIndex = waveIndex;

        Debug.Log($"WAVE {waveIndex + 1}: {wave.waveName}");

        // Aplica as configurações de peso
        foreach (WaveSpawnConfig config in wave.spawnConfigs)
        {
            if (string.IsNullOrEmpty(config.enemyName)) continue;

            // Atualiza peso
            spawner.UpdateSpawnWeight(config.enemyName, config.weight);

            // Habilita/desabilita
            spawner.SetEnemyEnabled(config.enemyName, config.enabled);

            Debug.Log($"  {config.enemyName}: peso={config.weight}, enabled={config.enabled}");
        }
    }

    // Pula para uma wave específica
    public void JumpToWave(int waveIndex)
    {
        ApplyWave(waveIndex);
        waveTimer = 0f;
        enemiesAtWaveStart = spawner.GetEnemiesSpawned();
    }

    // Retorna o índice da wave atual
    public int GetCurrentWaveIndex()
    {
        return currentWaveIndex;
    }

    // Retorna o nome da wave atual
    public string GetCurrentWaveName()
    {
        if (currentWaveIndex < 0 || currentWaveIndex >= waves.Count)
            return "N/A";

        return waves[currentWaveIndex].waveName;
    }

    // Retorna o progresso da wave atual (0-1)
    public float GetWaveProgress()
    {
        switch (triggerMode)
        {
            case WaveTriggerMode.TimeInterval:
                return Mathf.Clamp01(waveTimer / timeBetweenWaves);

            case WaveTriggerMode.EnemyCount:
                int enemiesSinceStart = spawner.GetEnemiesSpawned() - enemiesAtWaveStart;
                return Mathf.Clamp01((float)enemiesSinceStart / enemiesPerWave);

            default:
                return 0f;
        }
    }
}

// Configuração de uma Wave
[System.Serializable]
public class Wave
{
    [Tooltip("Nome descritivo da wave")]
    public string waveName = "Wave 1";

    [Tooltip("Configurações de spawn para esta wave")]
    public List<WaveSpawnConfig> spawnConfigs = new List<WaveSpawnConfig>();
}

/// <summary>
/// Configuração de spawn dentro de uma wave
/// </summary>
[System.Serializable]
public class WaveSpawnConfig
{
    [Tooltip("Nome do inimigo (deve corresponder ao nome do prefab)")]
    public string enemyName;

    [Tooltip("Peso de spawn nesta wave")]
    [Range(0f, 100f)]
    public float weight = 10f;

    [Tooltip("Este inimigo está habilitado nesta wave?")]
    public bool enabled = true;
}

/// <summary>
/// Modo de ativação das waves
/// </summary>
public enum WaveTriggerMode
{
    TimeInterval,  // Waves mudam a cada X segundos
    EnemyCount,    // Waves mudam a cada X inimigos spawnados
    Manual         // Waves mudam manualmente via código
}