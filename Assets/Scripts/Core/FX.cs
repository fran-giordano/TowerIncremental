using UnityEngine;

public static class FX
{
    // Crea un efecto de partículas en una posición; opcionalmente lo tiñe.
    public static void Spawn(GameObject prefab, Vector3 pos, Color? color = null)
    {
        if (!prefab) return;
        var go = Object.Instantiate(prefab, pos, Quaternion.identity);

        if (color.HasValue && go.TryGetComponent(out ParticleSystem ps))
        {
            var main = ps.main;
            main.startColor = color.Value;
        }
    }

    // Reproduce un sonido "sin posición" (al lado de la cámara).
    /*public static void Play(AudioClip clip, float volume = 1f)
    {
        if (!clip || !Camera.main) return;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }*/
}