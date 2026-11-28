using System;
using UnityEngine;

namespace TowerIncremental.Core
{
    // Único dueño de Time.timeScale. Combina la velocidad elegida con el congelamiento
    // (pausa o fin de partida), para que despausar vuelva a la velocidad elegida y no a x1.
    // Ningún otro script debe escribir Time.timeScale.
    public static class GameSpeed
    {
        public static float Speed { get; private set; } = 1f;
        public static bool Frozen { get; private set; }

        public static event Action<float> OnSpeedChanged;

        // Time.timeScale sobrevive entre escenas y sin recarga de dominio también los estáticos.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            OnSpeedChanged = null;
            Reset();
        }

        // Partida nueva o vuelta al menú: x1 y sin congelar.
        public static void Reset()
        {
            Frozen = false;
            SetSpeed(1f);
        }

        public static void SetSpeed(float speed)
        {
            Speed = Mathf.Max(0f, speed);
            Apply();
            OnSpeedChanged?.Invoke(Speed);
        }

        public static void SetFrozen(bool frozen)
        {
            Frozen = frozen;
            Apply();
        }

        static void Apply() => Time.timeScale = Frozen ? 0f : Speed;
    }
}
