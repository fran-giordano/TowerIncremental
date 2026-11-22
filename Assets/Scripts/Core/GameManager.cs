using System;
using UnityEngine;

public enum GameState { Playing, Won, Lost }

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [SerializeField] DifficultyData defaultDifficulty;   // se usa si no se pasó por el menú
    [SerializeField] WaveSpawner spawner;                // si queda vacío, se busca en la escena
    [SerializeField] AudioSource baseHitSfx;             // si queda vacío, usa el AudioSource del objeto

    public DifficultyData Difficulty { get; private set; }
    public GameState State { get; private set; }
    public float TimeLeft { get; private set; }
    public int Coins { get; private set; }
    public int BaseHealth { get; private set; }
    public int Kills { get; private set; }

    public event Action<int> CoinsChanged;
    public event Action<int> BaseHealthChanged;
    public event Action BaseDestroyed;
    public event Action<GameState> GameEnded;            // único aviso de fin: panel, ranking, etc.

    void Awake()
    {
        if (I && I != this) { Destroy(gameObject); return; }
        I = this;

        // ? : y no ??: los objetos de Unity redefinen la comparación con null.
        Difficulty = DifficultyData.Selected ? DifficultyData.Selected : defaultDifficulty;
        if (!Difficulty)
        {
            Debug.LogError("GameManager: falta asignar Default Difficulty en el Inspector.", this);
            enabled = false;
            return;
        }

        if (!spawner) spawner = FindFirstObjectByType<WaveSpawner>();
        if (!baseHitSfx) baseHitSfx = GetComponent<AudioSource>();

        Coins = Difficulty.startCoins;
        BaseHealth = Difficulty.baseHealth;
        TimeLeft = Difficulty.timeLimit;
        State = GameState.Playing;
        Time.timeScale = 1f;                             // es global y sobrevive entre escenas
    }

    void OnEnable()
    {
        Enemy.Killed += OnKilled;
        Enemy.ReachedBase += OnReachedBase;
        if (spawner) spawner.AllWavesCleared += OnAllWavesCleared;
    }

    void OnDisable()
    {
        Enemy.Killed -= OnKilled;
        Enemy.ReachedBase -= OnReachedBase;
        if (spawner) spawner.AllWavesCleared -= OnAllWavesCleared;
    }

    void OnDestroy() { if (I == this) I = null; }

    void Update()
    {
        if (State != GameState.Playing) return;

        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            End(GameState.Lost);
        }
    }

    void OnKilled(Enemy e)
    {
        if (State != GameState.Playing) return;

        Kills++;
        AddCoins(e.Data.reward);
    }

    void OnReachedBase(Enemy e)
    {
        if (State != GameState.Playing) return;
        if (baseHitSfx) baseHitSfx.Play();

        BaseHealth = Mathf.Max(0, BaseHealth - e.Data.damageToBase);
        BaseHealthChanged?.Invoke(BaseHealth);

        if (BaseHealth == 0)
        {
            BaseDestroyed?.Invoke();
            End(GameState.Lost);
        }
    }

    void OnAllWavesCleared()
    {
        if (State == GameState.Playing) End(GameState.Won);
    }

    void End(GameState result)
    {
        State = result;
        Time.timeScale = 0f;                             // congela enemigos, torres y proyectiles
        
        int score = Kills * 10;
        if (result == GameState.Won)
            {
                score += Mathf.CeilToInt(TimeLeft) * 10 + BaseHealth * 50;
            }

        ScoreBoard.Add(score, Difficulty.displayName);        
        GameEnded?.Invoke(result);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        CoinsChanged?.Invoke(Coins);
    }

    public bool TrySpend(int amount)
    {
        if (State != GameState.Playing || Coins < amount) return false;
        Coins -= amount;
        CoinsChanged?.Invoke(Coins);
        return true;
    }
}
