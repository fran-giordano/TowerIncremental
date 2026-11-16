using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerData data;
    [SerializeField] Transform muzzle;
    [SerializeField] Transform head;         // parte que rota 
    [SerializeField] LayerMask enemyMask;
    [SerializeField] float turnSpeed = 720f;

    readonly Collider[] hits = new Collider[32];
    Transform target;
    float nextFire, nextScan;

    public TowerData Data => data;

    void Update()
    {
        if (Time.time >= nextScan) { nextScan = Time.time + 0.15f; target = Acquire(); }
        if (!target) return;

        var dir = target.position - transform.position; dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
        {
            var rotTarget = head ? head : transform;
            rotTarget.rotation = Quaternion.RotateTowards(rotTarget.rotation,
                Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
        }

        if (Time.time >= nextFire) { nextFire = Time.time + data.fireInterval; Fire(); }
    }

    Transform Acquire()
    {
        int n = Physics.OverlapSphereNonAlloc(transform.position, data.range, hits, enemyMask);
        Transform best = null; float bestScore = float.MaxValue;
        for (int i = 0; i < n; i++)
        {
            float score = (hits[i].transform.position - transform.position).sqrMagnitude;
            if (data.targetMode == TargetMode.First)
            {
                var pp = hits[i].GetComponentInParent<IPathProgress>();
                if (pp != null) score = -pp.Progress;      // menor score = mejor
            }
            if (score < bestScore) { bestScore = score; best = hits[i].transform; }
        }
        return best;
    }

    void Fire()
    {
        var p = Instantiate(data.projectilePrefab, muzzle.position, muzzle.rotation);
        p.Init(target, data, enemyMask);
    }

    void OnDrawGizmosSelected()
    {
        if (!data) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
}