using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    Transform target;
    Vector3 lastPos;
    float damage, speed;

    public void Init(Transform t, float dmg, float spd)
    {
        target = t; damage = dmg; speed = spd;
        lastPos = t.position;
    }

    void Update()
    {
        if (target) lastPos = target.position;   // si murió, sigue al último punto
        transform.position = Vector3.MoveTowards(transform.position, lastPos, speed * Time.deltaTime);
        if ((lastPos - transform.position).sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(lastPos - transform.position);
        else
            Destroy(gameObject);                 // llegó sin impacto
    }

    void OnTriggerEnter(Collider other)
    {
        var hp = other.GetComponentInParent<HealthTarget>();
        if (!hp) return;
        hp.TakeDamage(damage);
        Destroy(gameObject);
    }
}