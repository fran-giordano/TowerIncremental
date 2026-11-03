using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerData data;
    [SerializeField] Transform muzzle;
    [SerializeField] LayerMask enemyMask;

    readonly Collider[] hits = new Collider[32];
    Transform target;
    float nextFire, nextScan;

    void Update()
    {
        if (Time.time >= nextScan) { 
            nextScan = Time.time + 0.15f;
            target = Acquire();
        }
        if (!target) return;

        var dir = target.position - transform.position;
        dir.y = 0;

        if (Time.time >= nextFire)
        {
            nextFire = Time.time + data.fireInterval;
            Fire();
        }

    }

    Transform Acquire()
        {
            int n = Physics.OverlapSphereNonAlloc(transform.position, data.range, hits, enemyMask);
            Transform best = null; float bestScore = float.MaxValue;
            for (int i = 0; i < n; i++)
            {
                float score = (hits[i].transform.position - transform.position).sqrMagnitude;
                // TargetMode.First: usar progreso en el camino (ver nota)
                if (score < bestScore) { bestScore = score; best = hits[i].transform; }
            }
            return best;
        }
        
    void Fire()
    {
        var p = Instantiate(data.projectilePrefab, muzzle.position, muzzle.rotation);
        p.Init(target, data.damage, data.projectileSpeed);
    }

    void OnDrawGizmosSelected()
    {
        if (!data) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
}
