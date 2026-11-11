using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] Tower[] towerPrefabs;
    [SerializeField] string[] towerNames;
    [SerializeField] LayerMask slotMask;

    Camera cam;
    int selected = -1;
    TowerSlot hovered;


    void Awake() => cam = Camera.main;

    void Update()
    {
        for (int i = 0; i < towerPrefabs.Length; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) selected = i;
        if (Input.GetKeyDown(KeyCode.Escape)) selected = -1;

        TowerSlot slot = null;
        if (selected >= 0 && Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit, 200f, slotMask))
            slot = hit.collider.GetComponentInParent<TowerSlot>();

        if (slot != hovered)
        {
            if (hovered) hovered.SetHighlight(false);
            hovered = slot;
            if (hovered) hovered.SetHighlight(true);
        }

        if (slot && Input.GetMouseButtonDown(0) && slot.TryPlace(towerPrefabs[selected]))
            slot.SetHighlight(false);
    }

        void OnGUI()   // provisorio
    {
        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            var label = (i == selected ? "> " : "") + (i + 1) + ". " + towerNames[i];
            if (GUI.Button(new Rect(10, 10 + i * 35, 160, 30), label)) selected = i;
        }
    }
}
