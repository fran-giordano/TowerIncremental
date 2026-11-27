using UnityEngine;

namespace TowerIncremental.Enemies
{
    public class WaypointPath : MonoBehaviour
    {
        public Transform[] Points { get; private set; }

        void Awake()
        {
            Points = new Transform[transform.childCount];
            for (int i = 0; i < Points.Length; i++) Points[i] = transform.GetChild(i);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            for (int i = 1; i < transform.childCount; i++)
                Gizmos.DrawLine(transform.GetChild(i - 1).position, transform.GetChild(i).position);
        }
    }
}
