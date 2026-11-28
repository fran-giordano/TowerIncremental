using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace TowerIncremental.Utils
{
    // Recicla instancias de prefabs en vez de crearlas y destruirlas.
    // Uso: PrefabPool.Spawn(prefab, pos, rot) y PrefabPool.Release(go).
    // Release de un objeto que no salió del pool lo destruye, así el código viejo sigue funcionando.
    public static class PrefabPool
    {
        static readonly Dictionary<GameObject, ObjectPool<GameObject>> poolsByPrefab = new Dictionary<GameObject, ObjectPool<GameObject>>();
        static readonly Dictionary<GameObject, ObjectPool<GameObject>> poolsByInstance = new Dictionary<GameObject, ObjectPool<GameObject>>();

        // Los objetos del pool viven en la escena: al descargarla se destruyen y hay que olvidar las referencias.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            Clear();
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        static void OnSceneUnloaded(Scene scene) => Clear();

        public static void Clear()
        {
            poolsByPrefab.Clear();
            poolsByInstance.Clear();
        }

        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            var go = Spawn(prefab.gameObject, position, rotation);
            return go.GetComponent<T>();
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (!poolsByPrefab.TryGetValue(prefab, out var pool))
            {
                pool = CreatePool(prefab);
                poolsByPrefab[prefab] = pool;
            }

            var go = pool.Get();
            // Se posiciona antes de activar, para que OnEnable y la física ya lo vean en su lugar.
            go.transform.SetPositionAndRotation(position, rotation);
            go.SetActive(true);
            return go;
        }

        public static void Release(GameObject go)
        {
            if (!go) return;

            if (!poolsByInstance.TryGetValue(go, out var pool))
            {
                Object.Destroy(go);
                return;
            }

            if (!go.activeSelf) return;   // ya estaba devuelto: evita liberarlo dos veces
            pool.Release(go);
        }

        static ObjectPool<GameObject> CreatePool(GameObject prefab)
        {
            ObjectPool<GameObject> pool = null;
            pool = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    var go = Object.Instantiate(prefab);
                    go.SetActive(false);
                    poolsByInstance[go] = pool;
                    return go;
                },
                actionOnRelease: go => go.SetActive(false),
                actionOnDestroy: go =>
                {
                    poolsByInstance.Remove(go);
                    Object.Destroy(go);
                },
                collectionCheck: false);
            return pool;
        }
    }
}
