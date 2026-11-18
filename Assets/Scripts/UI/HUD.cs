using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text coinsText, baseText;

    void Start()
    {
        var gm = GameManager.I;
        gm.CoinsChanged += SetCoins;
        gm.BaseHealthChanged += SetBase;
        SetCoins(gm.Coins);
        SetBase(gm.BaseHealth);        
    }


    void SetCoins(int c) => coinsText.text = $"Monedas: {c}";
    void SetBase(int h) => baseText.text = $"Castillo: {h}";
}
