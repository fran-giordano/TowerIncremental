using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    Transform target;
    Vector3 lastPos;
    TowerData data;
    LayerMask enemyMask;

    static readonly Collider[] buf = new Collider[64];

    public void Init(Transform t, TowerData d, LayerMask mask)
    {
        target = t; data = d; enemyMask = mask;
        lastPos = t.position;
    }

    void Update()
    {
       if (target) lastPos = target.position;
        transform.position = Vector3.MoveTowards(transform.position, lastPos, data.projectileSpeed * Time.deltaTime);
        var d = lastPos - transform.position;
        if (d.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(d);
        else Destroy(gameObject);           
    }

    void OnTriggerEnter(Collider other)
    {
        var hp = other.GetComponentInParent<HealthTarget>();
        if (!hp) return;

        if (data.effect == EffectType.Area) ExplodeAt(transform.position);
        else
        {
            hp.TakeDamage(data.damage);
            if (data.effect == EffectType.Slow)
                hp.GetComponentInParent<ISlowable>()?.ApplySlow(data.effectValue, data.effectDuration);
        }
        Destroy(gameObject);
    }

    void ExplodeAt(Vector3 pos)
    {
        int n = Physics.OverlapSphereNonAlloc(pos, data.areaRadius, buf, enemyMask);
        var done = new HashSet<HealthTarget>();
        for (int i = 0; i < n; i++)
        {
            var hp = buf[i].GetComponentInParent<HealthTarget>();
            if (hp && done.Add(hp)) hp.TakeDamage(data.damage);
        }
    }
}