using System;
using UnityEngine;

[Serializable]
public class WaveEntry
{
    public EnemyData type;
    [Min(1)] public int count = 1;
}

[Serializable]
public class Wave
{
    public WaveEntry[] entries;
}

[CreateAssetMenu(menuName = "TD/Difficulty")]
public class DifficultyData : ScriptableObject
{
    // Dificultad elegida en el menú. Static: sobrevive al cambio de escena.
    public static DifficultyData Selected;

    public string displayName = "Fácil";
    [Min(1f)] public float scoreMultiplier = 1f;   // Difícil vale más en el ranking

    [Header("Base e economia")]
    public int baseHealth = 20;
    public int startCoins = 200;

    [Header("Tempo")]
    public float timeLimit = 180f;          // segundos para completar la misión
    public float spawnInterval = 1.5f;      // entre un enemigo y el siguiente
    public float timeBetweenWaves = 8f;     // pausa después de limpiar una oleada

    [Header("Ondas")]
    public Wave[] waves;

    public int TotalEnemies
    {
        get
        {
            int total = 0;
            if (waves == null) return 0;
            foreach (var w in waves)
                if (w.entries != null)
                    foreach (var e in w.entries) total += e.count;
            return total;
        }
    }

    // En el editor, al cambiar un valor: avisa si el tiempo no alcanza ni para que salgan todos.
    void OnValidate()
    {
        if (waves == null) return;
        float spawnTime = TotalEnemies * spawnInterval + Mathf.Max(0, waves.Length - 1) * timeBetweenWaves;
        if (spawnTime >= timeLimit)
            Debug.LogWarning($"{name}: el tiempo límite ({timeLimit}s) no alcanza para que salgan todos los enemigos ({spawnTime}s).", this);
    }
}
