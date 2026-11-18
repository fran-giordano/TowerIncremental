using System;
using UnityEngine;

[RequireComponent(typeof(HealthTarget))]
public class Enemy : MonoBehaviour, IPathProgress, ISlowable
{
    public static event Action<Enemy> ReachedBase;
    public static event Action<Enemy> Killed;

    public EnemyData Data { get; private set; }
    public float Progress { get; private set; }      // distancia recorrida: mayor = más adelantado

    HealthTarget hp;
    Transform[] pts;
    int idx;
    float slowFactor = 1f, slowUntil;

    void Awake()
    {
        hp = GetComponent<HealthTarget>();
        hp.Died += () => Killed?.Invoke(this);
    }

    public void Init(EnemyData d, Transform[] path)
    {
        Data = d; pts = path;
        hp.Init(d.maxHealth);
        transform.localScale *= d.scale;
        transform.position = pts[0].position;
        idx = 1;

        var block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", d.color);       // URP/Lit
        foreach (var r in GetComponentsInChildren<Renderer>()) r.SetPropertyBlock(block);
    }

    public void ApplySlow(float factor, float duration)
    {
        slowFactor = Mathf.Min(slowFactor, factor);  // se aplica el slow más fuerte
        slowUntil = Mathf.Max(slowUntil, Time.time + duration);
    }

    void Update()
    {
        if (pts == null) return;
        if (Time.time > slowUntil) slowFactor = 1f;

        var target = pts[idx].position;
        var before = transform.position;
        transform.position = Vector3.MoveTowards(before, target, Data.speed * slowFactor * Time.deltaTime);
        Progress += Vector3.Distance(before, transform.position);

        var dir = target - before; dir.y = 0;
        if (dir.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(dir);

        if (transform.position == target && ++idx >= pts.Length)
        {
            ReachedBase?.Invoke(this);
            Destroy(gameObject);
        }
    }
}