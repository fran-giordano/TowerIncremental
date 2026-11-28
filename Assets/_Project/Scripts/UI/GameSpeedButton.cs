using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TowerIncremental.Core;

namespace TowerIncremental.UI
{
    // Botón del HUD que cicla la velocidad del juego (x1 → x1.5 → x2 → x4 → x1).
    public class GameSpeedButton : MonoBehaviour
    {
        [SerializeField] GameSpeedSettings settings;
        [SerializeField] Button button;
        [SerializeField] TMP_Text label;
        [SerializeField] KeyCode hotkey = KeyCode.F;

        void Awake()
        {
            if (!button) button = GetComponent<Button>();
            if (!label) label = GetComponentInChildren<TMP_Text>();
            button.onClick.AddListener(Cycle);
        }

        void OnEnable()
        {
            GameSpeed.OnSpeedChanged += UpdateLabel;
            UpdateLabel(GameSpeed.Speed);
        }

        void OnDisable() => GameSpeed.OnSpeedChanged -= UpdateLabel;

        void Update()
        {
            bool canChange = CanChange();
            button.interactable = canChange;
            if (canChange && Input.GetKeyDown(hotkey)) Cycle();
        }

        static bool CanChange()
        {
            var gm = GameManager.I;
            return gm && gm.State == GameState.Playing && !gm.Paused;
        }

        void Cycle()
        {
            if (!CanChange() || !settings || settings.Count == 0) return;
            int next = (settings.IndexOf(GameSpeed.Speed) + 1) % settings.Count;
            GameSpeed.SetSpeed(settings.Get(next));
        }

        void UpdateLabel(float speed)
        {
            if (label) label.text = "x" + speed.ToString("0.#", CultureInfo.InvariantCulture);
        }
    }
}
