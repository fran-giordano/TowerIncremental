using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] Tower[] towerPrefabs;
    [SerializeField] LayerMask slotMask;

    [SerializeField] GameObject placeFx;

    [SerializeField] LineRenderer rangeRing;
    [SerializeField] Color okColor = new Color(0.3f, 1f, 0.4f);
    [SerializeField] Color noColor = new Color(1f, 0.3f, 0.3f);

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
<<<<<<< Updated upstream
        if (GameManager.I.Paused || GameManager.I.State != GameState.Playing) return;
=======
        
        if (GameManager.I.Paused || GameManager.I.State != GameState.Playing)
        {
            rangeRing.enabled = false;
            return;
        } 
>>>>>>> Stashed changes
        
        for (int i = 0; i < towerPrefabs.Length; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) Select(i);
        if (Input.GetMouseButtonDown(1)) Select(-1);

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

        UpdateRangePreview(slot);
        if (slot && Input.GetMouseButtonDown(0)) TryPlace(slot);
    }

    void TryPlace(TowerSlot slot)
    {
        var prefab = towerPrefabs[Selected];
        if (slot.Occupied || !GameManager.I.TrySpend(prefab.Data.cost)) return;
        slot.TryPlace(prefab);
        FX.Spawn(placeFx, slot.AnchorPosition);
        slot.SetHighlight(false);
    }

    void UpdateRangePreview(TowerSlot slot)
    {
        if (!slot) {rangeRing.enabled = false; return;}

        var data = towerPrefabs[Selected].Data;
        bool ok = !slot.Occupied && GameManager.I.Coins >= data.cost;

        const int segments = 64;
        Vector3 center = slot.AnchorPosition;
        rangeRing.positionCount = segments;

        for (int i= 0 ; i < segments; i++)
        {
            float a = i* Mathf.PI * 2f / segments;
            rangeRing.SetPosition(i, center + new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * data.range);
        }

        rangeRing.startColor = rangeRing.endColor = ok ? okColor : noColor;
        rangeRing.enabled = true;
    }
}