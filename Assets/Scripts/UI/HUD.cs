using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text coinsText, baseText;
    [SerializeField] TMP_Text timerText, waveText;   // opcionales
    [SerializeField] WaveSpawner spawner;            // si queda vacío, se busca en la escena

    void Awake()     { if (!spawner) spawner = FindFirstObjectByType<WaveSpawner>(); }
    void OnEnable()  { if (spawner) spawner.WaveStarted += OnWaveStarted; }
    void OnDisable() { if (spawner) spawner.WaveStarted -= OnWaveStarted; }

    void Start()
    {
        var gm = GameManager.I;
        gm.CoinsChanged += SetCoins;
        gm.BaseHealthChanged += SetBase;
        SetCoins(gm.Coins);
        SetBase(gm.BaseHealth);
        if (waveText) waveText.text = "";
    }

    void OnDestroy()
    {
        var gm = GameManager.I;
        if (!gm) return;
        gm.CoinsChanged -= SetCoins;
        gm.BaseHealthChanged -= SetBase;
    }

    // El timer cambia en cada frame: se lee en Update. La oleada cambia pocas veces: evento.
    void Update()
    {
        if (!timerText || !GameManager.I) return;
        int t = Mathf.CeilToInt(GameManager.I.TimeLeft);
        timerText.text = $"{t / 60}:{t % 60:00}";
        timerText.color = t <= 10 ? Color.red : Color.white;
    }

    void SetCoins(int c) => coinsText.text = $"Moedas: {c}";
    void SetBase(int h)  => baseText.text = $"Castelo: {h}";
    void OnWaveStarted(int current, int total) { if (waveText) waveText.text = $"Onda {current}/{total}"; }
}
