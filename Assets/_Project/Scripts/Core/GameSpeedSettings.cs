using UnityEngine;

namespace TowerIncremental.Core
{
    // Velocidades que el jugador puede elegir, en el orden en que cicla el botón.
    [CreateAssetMenu(menuName = "TD/Game Speed Settings")]
    public class GameSpeedSettings : ScriptableObject
    {
        [SerializeField] float[] speeds = { 1f, 1.5f, 2f, 4f };

        public int Count => speeds.Length;

        public float Get(int index) => speeds[index];

        // Índice de la velocidad más cercana a la dada (por si la actual no está en la lista).
        public int IndexOf(float speed)
        {
            int best = 0;
            for (int i = 1; i < speeds.Length; i++)
                if (Mathf.Abs(speeds[i] - speed) < Mathf.Abs(speeds[best] - speed)) best = i;
            return best;
        }
    }
}
