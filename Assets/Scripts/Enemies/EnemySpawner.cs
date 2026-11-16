using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy prefab;
    [SerializeField] EnemyData[] types;
    [SerializeField] WaypointPath path;
    [SerializeField] int count = 10;
    [SerializeField] float interval = 1.2f;

    IEnumerator Start()
    {
        yield return null;                           // espera el Awake de WaypointPath
        for (int i = 0; i < count; i++)
        {
            Instantiate(prefab).Init(types[i % types.Length], path.Points);
            yield return new WaitForSeconds(interval);
        }
    }
}