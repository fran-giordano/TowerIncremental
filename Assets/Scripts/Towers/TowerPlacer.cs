using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] Tower[] towerPrefabs;
    [SerializeField] LayerMask slotMask;

    public Tower[] Towers => towerPrefabs;
    public int Selected { get; private set; } = -1;
    public event Action<int> SelectionChanged;

    Camera cam;
    TowerSlot hovered;

    void Awake() => cam = Camera.main;

    public void Select(int i)
    {
        Selected = i;
        SelectionChanged?.Invoke(Selected);
    }

    void Update()
    {
        for (int i = 0; i < towerPrefabs.Length; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) Select(i);
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) Select(-1);

        bool overUI = EventSystem.current && EventSystem.current.IsPointerOverGameObject();

        TowerSlot slot = null;
        if (Selected >= 0 && !overUI &&
            Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit, 200f, slotMask))
            slot = hit.collider.GetComponentInParent<TowerSlot>();

        if (slot != hovered)
        {
            if (hovered) hovered.SetHighlight(false);
            hovered = slot;
            if (hovered) hovered.SetHighlight(true);
        }

        if (slot && Input.GetMouseButtonDown(0)) TryPlace(slot);
    }

    void TryPlace(TowerSlot slot)
    {
        var prefab = towerPrefabs[Selected];
        if (slot.Occupied || !GameManager.I.TrySpend(prefab.Data.cost)) return;
        slot.TryPlace(prefab);
        slot.SetHighlight(false);
    }
}