using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TowerIncremental.Core;
using TowerIncremental.Towers;

namespace TowerIncremental.UI
{
    public class TowerShopUI : MonoBehaviour
    {
        [SerializeField] TowerPlacer placer;
        [SerializeField] Button buttonPrefab;
        [SerializeField] Transform container;
        [SerializeField] Color normalColor = Color.white, selectedColor = Color.green;

        Button[] buttons;

        void Start()
        {
            var towers = placer.Towers;
            buttons = new Button[towers.Length];

            for (int i = 0; i < towers.Length; i++)
            {
                int index = i;                                   // copia local para el lambda
                var d = towers[i].Data;
                var b = Instantiate(buttonPrefab, container);
                b.GetComponentInChildren<TMP_Text>().text = $"{i + 1}. {d.towerName}\n${d.cost}";
                b.onClick.AddListener(() => placer.Select(index));
                buttons[i] = b;
            }

            placer.SelectionChanged += _ => Refresh();
            GameManager.I.CoinsChanged += _ => Refresh();
            Refresh();
        }

        void Refresh()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = GameManager.I.Coins >= placer.Towers[i].Data.cost;
                buttons[i].image.color = i == placer.Selected ? selectedColor : normalColor;
            }
        }
    }
}
