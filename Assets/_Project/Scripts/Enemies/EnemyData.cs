using UnityEngine;

namespace TowerIncremental.Enemies
{
    [CreateAssetMenu(menuName = "TD/Enemy Data")]

    public class EnemyData : ScriptableObject
    {
        public string enemyName;
        public float speed = 3f;
        public float maxHealth = 50f;
        public int damageToBase = 1;
        public int reward = 5; //monedas
        public float scale = 1f;
        public Color color = Color.white;
        public GameObject deathFX;
    }
}
