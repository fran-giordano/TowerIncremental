using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerIncremental.Core;

namespace TowerIncremental.Enemies
{
    // Reemplaza a EnemySpawner (mismo .meta: la escena conserva la referencia, el prefab y el path).
    // Las oleadas, intervalos y pausas salen de la dificultad del GameManager.
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] Enemy prefab;
        [SerializeField] WaypointPath path;

        public event Action<int, int> WaveStarted;   // (oleada actual empezando en 1, total)
        public event Action AllWavesCleared;

        public int CurrentWave { get; private set; }
        public int TotalWaves => waves != null ? waves.Length : 0;

        public bool CanCallNextWave { get; private set; }
        public void CallNextWave()
        {
            if (CanCallNextWave) skipRequested = true;
        }

        readonly HashSet<Enemy> alive = new HashSet<Enemy>();
        Wave[] waves;
        float spawnInterval, timeBetweenWaves;

        [SerializeField] int earlyCallBonus = 10; // Si adelantas tenes recompensa
        bool skipRequested;

        void OnEnable()  { Enemy.Killed += OnEnemyGone; Enemy.ReachedBase += OnEnemyGone; }
        void OnDisable() { Enemy.Killed -= OnEnemyGone; Enemy.ReachedBase -= OnEnemyGone; }

        IEnumerator Start()
        {
            yield return null;    // todos los Awake/Start ya corrieron (HUD suscrito)

            var diff = GameManager.I.Difficulty;
            waves = diff.waves;
            spawnInterval = diff.spawnInterval;
            timeBetweenWaves = diff.timeBetweenWaves;

            for (int w = 0; w < waves.Length; w++)
            {
                CurrentWave = w + 1;
                WaveStarted?.Invoke(CurrentWave, waves.Length);

                foreach (var entry in waves[w].entries)
                    for (int i = 0; i < entry.count; i++)
                    {
                        Spawn(entry.type);
                        yield return new WaitForSeconds(spawnInterval);
                    }

                // Última oleada: hay que limpiarla para ganar, no se puede adelantar
                if (w == waves.Length - 1)
                {
                    yield return new WaitUntil(WaveCleared);
                    break;
                }

                // Ya salieron todos: desde acá se puede adelantar
                skipRequested = false;
                CanCallNextWave = true;

                yield return new WaitUntil(() => WaveCleared() || skipRequested);

                // Pausa entre oleadas, cortable con skip
                float t = 0f;
                while (t < timeBetweenWaves && !skipRequested)
                {
                    t += Time.deltaTime;
                    yield return null;
                }

                CanCallNextWave = false;
                if (skipRequested) GameManager.I.AddCoins(earlyCallBonus);
            }

            AllWavesCleared?.Invoke();
        }

        void Spawn(EnemyData type)
        {
            var enemy = Instantiate(prefab);
            enemy.Init(type, path.Points);
            alive.Add(enemy);
        }

        void OnEnemyGone(Enemy e) => alive.Remove(e);    // HashSet: si ya no estaba, no pasa nada

        bool WaveCleared()
        {
            alive.RemoveWhere(e => e == null);            // por si alguno se destruyó por otra vía
            return alive.Count == 0;
        }
    }
}
