using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [SerializeField] int startCoins = 150;
    [SerializeField] int startBaseHealth = 20;

    public int Coins { get; private set; }
    public int BaseHealth { get; private set; }

    public event Action<int> CoinsChanged;
    public event Action<int> BaseHealthChanged;
    public event Action BaseDestroyed;

    void Awake()
    {
        if (I && I != this) { Destroy(gameObject); return; }
        I = this;
        Coins = startCoins;
        BaseHealth = startBaseHealth;
    }

    void OnEnable()  { Enemy.Killed += OnKilled;  Enemy.ReachedBase += OnReachedBase; }
    void OnDisable() { Enemy.Killed -= OnKilled;  Enemy.ReachedBase -= OnReachedBase; }

    void OnKilled(Enemy e) => AddCoins(e.Data.reward);

    void OnReachedBase(Enemy e)
    {
        if (BaseHealth <= 0) return;
        BaseHealth = Mathf.Max(0, BaseHealth - e.Data.damageToBase);
        BaseHealthChanged?.Invoke(BaseHealth);
        if (BaseHealth == 0) BaseDestroyed?.Invoke();
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        CoinsChanged?.Invoke(Coins);
    }

    public bool TrySpend(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        CoinsChanged?.Invoke(Coins);
        return true;
    }
}