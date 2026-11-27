using UnityEngine;
using TowerIncremental.Enemies;

namespace TowerIncremental.Core
{
    [RequireComponent(typeof(Collider))]

    public class BaseZone : MonoBehaviour
    {
        void Awake() => GetComponent<Collider>().isTrigger = true;

        void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponentInParent<Enemy>();
            if (enemy) enemy.ReachBase();
        } 
    }
}
