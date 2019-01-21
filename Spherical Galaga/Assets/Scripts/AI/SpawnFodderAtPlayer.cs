using UnityEngine;
using System.Collections;
using Directions;

public class SpawnFodderAtPlayer : MonoBehaviour
{
    public float delayBeforeFirstSpawn = 5f;
    public float minDelayBetweenWaves = 10f;
    public float maxDelayBetweenWaves = 20f;

    public float delayBetweenSubwaves = 2.5f;
    public int minSubwaves = 3;
    public int maxSubwaves = 3;

    public float minRangeToPole = Mathf.PI / 5f;

    public EnemySpawner.SpawnWaveInfo waveInfo;


    void Start()
    {
        StartCoroutine(Think());
    }

    public IEnumerator Think()
    {
        var spawner = GetComponent<EnemySpawner>();

        yield return new WaitForSeconds(delayBeforeFirstSpawn);
        while (true)
        {
            yield return DoSubwave(spawner);

            var wait = Random.Range(minDelayBetweenWaves, maxDelayBetweenWaves);
            yield return new WaitForSeconds(wait);
        }
    }

    public IEnumerator DoSubwave(EnemySpawner spawner)
    {
        var dir = Random.Range(0, 2) > 0 ? RelativeDirection.LEFT : RelativeDirection.RIGHT;
        var waves = Random.Range(minSubwaves, maxSubwaves + 1);

        for(var i = 0; i < waves; i++)
        {
            dir = SpawnAtPlayer(spawner, dir);
            yield return new WaitForSeconds(delayBetweenSubwaves);
        }
    }

    private RelativeDirection SpawnAtPlayer(EnemySpawner spawner, RelativeDirection dir)
    {
        var player = GameManager.instance.Player;

        if (player == null) return ReverseDirection(dir);

        var playerPhysics = player.GetComponent<SpherePhysics>();
        var theta = playerPhysics.normalizedTheta;
        var rho = playerPhysics.normalizedRho;

        if (IsHorizontal(dir) && playerPhysics.IsNearPole(minRangeToPole))
        {
            dir = Random.Range(0, 2) > 0 ? RelativeDirection.DOWN : RelativeDirection.UP;
        }

        switch(dir)
        {
            case RelativeDirection.LEFT:
            case RelativeDirection.RIGHT:
                rho += Mathf.PI / 4f * (dir == RelativeDirection.LEFT ? 1 : -1);
                break;
            case RelativeDirection.UP:
            case RelativeDirection.DOWN:
                theta += Mathf.PI / 4f * (dir == RelativeDirection.DOWN ? 1 : -1);
                break;
        }

        var spawnLoc = new Vector3(theta, rho, spawner.spawnAltitude);

        spawner.StartWave(waveInfo, spawnLoc, dir);

        return ReverseDirection(dir);
    }

    private RelativeDirection ReverseDirection(RelativeDirection direction)
    {
        switch(direction)
        {
            case RelativeDirection.DOWN: return RelativeDirection.UP;
            case RelativeDirection.UP: return RelativeDirection.DOWN;   
            case RelativeDirection.LEFT: return RelativeDirection.RIGHT;
            case RelativeDirection.RIGHT:
            default: 
                return RelativeDirection.LEFT;
        }
    }

    private bool IsHorizontal(RelativeDirection direction)
    {
        return direction == RelativeDirection.LEFT || direction == RelativeDirection.RIGHT;
    }

    public void OnDrawGizmosSelected()
    {
        float minTheta = minRangeToPole;
        float maxTheta = Mathf.PI - minRangeToPole;

        Gizmos.color = Color.white;
        SphereGizmos.DrawMeridian(minTheta);
        SphereGizmos.DrawMeridian(maxTheta);
    }
}