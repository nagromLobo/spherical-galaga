using UnityEngine;
using System.Collections;

public abstract class SpawnBehavior : MonoBehaviour
{
    public abstract IEnumerator DoSpawn(EnemySpawner spawner);
}
