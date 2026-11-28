using System.Collections.Generic;
using UnityEngine;
using TowerIncremental.Core;
using TowerIncremental.Enemies;
using TowerIncremental.Utils;

namespace TowerIncremental.Towers
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        Transform target;
        Vector3 lastPos;
        TowerData data;
        LayerMask enemyMask;
        [SerializeField] AudioSource bulletShootSfx;             // se vazio, usa o AudioSource do objeto

        const float MaxLifeTime = 4f;
        Vector3 origin;
        float life;
        bool consumed;

        static readonly Collider[] buf = new Collider[64];

        public void Init(Transform t, TowerData d, LayerMask mask)
        {
            target = t; data = d; enemyMask = mask;
            lastPos = t.position;
            origin = transform.position;
            life = 0f;          // estado de un uso anterior (pooling)
            consumed = false;
        }

        void Update()
        {
            life += Time.deltaTime;
            if (bulletShootSfx) bulletShootSfx.Play();
            float maxDist = data.range * 3f;
            if (life > MaxLifeTime || (transform.position - origin).sqrMagnitude > maxDist * maxDist) 
            {
                PrefabPool.Release(gameObject);
                return;
            }

            // Un enemigo devuelto al pool queda inactivo (antes se destruía): se suelta el objetivo
            if (target && !target.gameObject.activeInHierarchy) target = null;
            if (target) lastPos = target.position;
            transform.position = Vector3.MoveTowards(transform.position, lastPos, data.projectileSpeed * Time.deltaTime);
            var d = lastPos - transform.position;
            if (d.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(d);
            else {
                if (data.effect == EffectType.Area) ExplodeAt(transform.position);
                PrefabPool.Release(gameObject);
            }        
        }

        void OnTriggerEnter(Collider other)
        {
            if (consumed) return;

            var hp = other.GetComponentInParent<HealthTarget>();
            if (!hp) return;

            consumed = true;

            if (data.effect == EffectType.Area) ExplodeAt(transform.position);
            else
            {
                FX.Spawn(data.hitFX, transform.position, data.fxColor);
                hp.TakeDamage(data.damage);
                if (data.effect == EffectType.Slow)
                    hp.GetComponentInParent<ISlowable>()?.ApplySlow(data.effectValue, data.effectDuration);
            }
            PrefabPool.Release(gameObject);
        }

        void ExplodeAt(Vector3 pos)
        {
            FX.Spawn(data.hitFX, pos, data.fxColor);
            int n = Physics.OverlapSphereNonAlloc(pos, data.areaRadius, buf, enemyMask);
            var done = new HashSet<HealthTarget>();
            for (int i = 0; i < n; i++)
            {
                var hp = buf[i].GetComponentInParent<HealthTarget>();
                if (hp && done.Add(hp)) hp.TakeDamage(data.damage);
            }
        }
    }
}
