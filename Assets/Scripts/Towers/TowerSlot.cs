using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    [SerializeField] Transform anchor;
    [SerializeField] Renderer rend;
    [SerializeField] Color freeColor = Color.green, busycolor = Color.red;

    public bool Occupied {get; private set; }
    public Vector3 AnchorPosition => anchor ? anchor.position : transform.position;
    Color baseColor;

    void Awake()
    {
        if (!rend) rend = GetComponentInChildren<Renderer>();
        baseColor = rend.material.color;
    }

    public bool TryPlace(Tower prefab)
    {
        if (Occupied) return false;
        var pos = anchor ? anchor.position : transform.position;
        Instantiate(prefab, pos, Quaternion.identity);
        Occupied = true;
        return true;
    }

    public void SetHighlight(bool on) =>
        rend.material.color = on ? (Occupied ? busycolor : freeColor) : baseColor;
}
