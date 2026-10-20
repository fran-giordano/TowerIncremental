using UnityEngine;

public class DamageZoneTester : MonoBehaviour
{
    public float damageRadius = 5f;
    public float damageAmount = 20f;
    public LayerMask targetLayer; // Optional: Optimize by picking your target layer

    void Update()
    {
        // Press Spacebar to trigger the radius damage explosion
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerRadiusDamage();
        }
    }

void TriggerRadiusDamage()
{
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius, targetLayer);

    foreach (Collider hit in hitColliders)
    {
        // Bypasses assembly reference restrictions by sending a message over to the object
        hit.gameObject.SendMessage("TakeDamage", damageAmount, SendMessageOptions.DontRequireReceiver);
        Debug.Log($"Sent damage command to {hit.name}");
    }
}

    // Visualizes the damage radius in the Scene View
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
