using UnityEngine;

public enum TargetMode { Closest, First }

[CreateAssetMenu(menuName = "TD/Tower Data")]
public class TowerData : ScriptableObject
{
    public float range = 6f;
    public float damage = 10f;
    public float fireInterval = 1f;
    public float projectileSpeed = 12f;
    public Projectile projectilePrefab;
    public TargetMode targetMode;
}