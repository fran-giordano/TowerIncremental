using System;
using UnityEngine;

[RequireComponent(typeof(HealthTarget))]
public class Enemy : MonoBehaviour, IPathProgress, ISlowable
{
    public static event Action<Enemy> ReachedBase;
    public static event Action<Enemy> Killed;

    public EnemyData Data { get; private set; }
    public float Progress { get; private set; }      // distancia recorrida: mayor = más adelantado

    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    static readonly Color SlowTint = new Color(0.4f, 0.8f, 1f);

    MaterialPropertyBlock block;
    Renderer[] renderers;
    Color showColor;
    float flashUntil;

    HealthTarget hp;
    Transform[] pts;
    int idx;
    float slowFactor = 1f, slowUntil;

    bool reachedBase;

    void Awake()
    {
        hp = GetComponent<HealthTarget>();
        hp.Died += () => 
        {
            FX.Spawn(Data.deathFX, transform.position, Data.color);
            Killed?.Invoke(this);

        };

        hp.Changed += (cur, max) => {
            if (cur < max) flashUntil = Time.time + 0.08f; };

        var rb = GetComponent<Rigidbody>();
        if(!rb) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void Init(EnemyData d, Transform[] path)
    {
        Data = d; pts = path;
        hp.Init(d.maxHealth);
        transform.localScale *= d.scale;
        transform.position = pts[0].position;
        idx = 1;

        block = new MaterialPropertyBlock();
        renderers = GetComponentsInChildren<Renderer>();
        SetColor(d.color);
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
        UpdateColor();

        var target = pts[idx].position;
        var before = transform.position;
        transform.position = Vector3.MoveTowards(before, target, Data.speed * slowFactor * Time.deltaTime);
        Progress += Vector3.Distance(before, transform.position);

        var dir = target - before; dir.y = 0;
        if (dir.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(dir);

        if (transform.position == target && ++idx >= pts.Length)
        {
            ReachBase();
        }
    }

    public void ReachBase()
    {
        if (reachedBase) return;   // que no cuente dos veces
        reachedBase = true;
        ReachedBase?.Invoke(this);
        Destroy(gameObject);
    }

    void UpdateColor()
    {
        Color c = Time.time < flashUntil ? Color.white
                : slowFactor < 1f        ? Color.Lerp(Data.color, SlowTint, 0.6f)
                : Data.color;
        if (c != showColor) SetColor(c);
    }

    void SetColor(Color c)
    {
        showColor = c;
        block.SetColor(BaseColorId, c);
        foreach (var r in renderers) if (r) r.SetPropertyBlock(block);
    }
}