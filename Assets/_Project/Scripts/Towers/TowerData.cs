using UnityEngine;

namespace TowerIncremental.Towers
{
    public enum TargetMode { Closest, First }
    public enum EffectType { None, Slow, Area}

    [CreateAssetMenu(menuName = "TD/Tower Data")]
    public class TowerData : ScriptableObject
    {
        public string towerName;
        public float range = 6f;
        public float damage = 10f;
        public float fireInterval = 1f;
        public float projectileSpeed = 12f;
        public Projectile projectilePrefab;
        public TargetMode targetMode;

        public int cost = 50;

        [Header ("Efecto")]
        public EffectType effect;
        public float effectValue = 0.5f;    //slow: x vel
        public float effectDuration = 2f;   // slow: seg
        public float areaRadius = 2f;       //Area: radio
        public GameObject hitFX;
        public Color fxColor = Color.yellow;
    }
}
