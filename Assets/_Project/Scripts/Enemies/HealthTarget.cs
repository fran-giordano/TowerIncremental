using System;
using UnityEngine;

namespace TowerIncremental.Enemies
{
    public class HealthTarget : MonoBehaviour
    {
        [SerializeField] float maxHealth = 50f;
        [SerializeField] bool destroyOnDeath = true;

        public float Max => maxHealth;
        public float Current { get; private set; }
        public event Action<float, float> Changed;   // (actual, max) → barra de vida
        public event Action Died;

        void Awake() => Current = maxHealth;

        public void Init(float max)
        {
            maxHealth = Current = max;
            Changed?.Invoke(Current, maxHealth);
        }

        public void TakeDamage(float dmg)
        {
            if (Current <= 0f) return;

            Current = Mathf.Max(0f, Current - dmg);
            Changed?.Invoke(Current, maxHealth);

            if (Current > 0f) return;

            Died?.Invoke();

            if (destroyOnDeath) Destroy(gameObject);
        }
    }
}
