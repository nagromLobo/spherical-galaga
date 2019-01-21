using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directions;

public delegate void SpawnEnemy(Vector3 position, RelativeDirection direction);

public class EnemySpawner : Singleton<EnemySpawner> {

    public enum WaveType { Fodder, Broadside, Chaser }

    [System.Serializable]
    public struct SpawnWaveInfo
    {
        public WaveType type;
        public float spawnDelay;
        public int size;
    }

    public List<SpawnWaveInfo> spawnWaveInfos;

    public float spawnAltitude = 12f;

    public float timeToFirstSpawn = 10f;


    public float minSpawnTheta = Mathf.PI / 4f;
    public float maxSpawnTheta = 3f * Mathf.PI / 4f;
    public float spawnThetaDelta = Mathf.PI / 8f;
    public float spawnRhoDelta = Mathf.PI / 8f;

    public float delayBetweenWaves = 12f;

    public float difficultyMultiplier = 0.9f;
    public float timeBetweenHardnessIncrease = 5.0f;
    public float minimumDelayBetweenWaves = 2f;

    private float timeLastHardnessIncrease;

    public GameObject enemyPrefab;
    public GameObject fodderEnemy;
    public GameObject horizonalBroadsideEnemy;
    public GameObject vertialBroadsideEnemy;
    public GameObject chaserEnemy;

    public bool canSpawn = true;

    private List<Vector3> spawnPositions;

    private void CreateSpawnPositions()
    {
        spawnPositions = new List<Vector3>();
        for (var theta = minSpawnTheta; theta <= maxSpawnTheta; theta += spawnThetaDelta)
        {
            for (var rho = 0f; rho <= 2 * Mathf.PI; rho += spawnRhoDelta)
            {
                spawnPositions.Add(new Vector3(theta, rho, spawnAltitude));
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var index = Random.Range(0, spawnPositions.Count);
        return spawnPositions[index];
    }

    private RelativeDirection GetRandomDirection()
    {
        switch(Random.Range(0, 4))
        {
            case 0: return RelativeDirection.LEFT;
            case 1: return RelativeDirection.RIGHT;
            case 2: return RelativeDirection.UP;
            default: return RelativeDirection.DOWN;
        }
    }

    void Start () {
        timeLastHardnessIncrease = Time.time;
        CreateSpawnPositions();
        StartCoroutine(Think());
    }

    private IEnumerator Think()
    {
        yield return new WaitForSeconds(timeToFirstSpawn);
        while(true)
        {
            StartRandomWave();
            var currTime = Time.time;
            var dtHardness = currTime - timeLastHardnessIncrease;
            if(dtHardness > timeBetweenHardnessIncrease) {
                timeLastHardnessIncrease = currTime;
                var newDelayBetweenWaves = difficultyMultiplier * delayBetweenWaves;
                delayBetweenWaves = newDelayBetweenWaves > minimumDelayBetweenWaves ? newDelayBetweenWaves : minimumDelayBetweenWaves;
            }
            yield return new WaitForSeconds(delayBetweenWaves);
        }
    }

    private void StartRandomWave()
    {
        var index = Random.Range(0, spawnWaveInfos.Count);
        var spawnPos = GetRandomSpawnPosition();
        var dir = GetRandomDirection();

        StartCoroutine(SpawnWave(spawnWaveInfos[index], spawnPos, dir));
    }

    public Coroutine StartWave(
        SpawnWaveInfo waveInfo,
        Vector3 location,
        RelativeDirection direction)
    {
        return StartCoroutine(SpawnWave(waveInfo, location, direction));
    }

    private IEnumerator SpawnWave(
        SpawnWaveInfo waveInfo,
        Vector3 location, 
        RelativeDirection direction)
    {
        SpawnEnemy spawnFn = null;
        switch(waveInfo.type) {
            case WaveType.Fodder:
                spawnFn = SpawnFodderEnemy;
                break;

            case WaveType.Broadside:
                spawnFn = SpawnBroadsideEnemy;
                break;

            case WaveType.Chaser:
                spawnFn = SpawnChaserEnemy;
                break;
        }

        for (var i = 0; i < waveInfo.size; i++) {
            if (canSpawn) {
                spawnFn(location, direction);
                yield return new WaitForSeconds(waveInfo.spawnDelay);
            }
        }
    }

    private void SpawnFodderEnemy(Vector3 location, RelativeDirection direction) {

        GameObject newEnemy = InstantiateEnemy(fodderEnemy, location);

        ConfigStateWaveDirectionEnemy(newEnemy, direction);
    }

    private void SpawnBroadsideEnemy(Vector3 location, RelativeDirection direction) {

        GameObject newEnemy = IsVerticalDirection(direction) ?
            InstantiateEnemy(vertialBroadsideEnemy, location) :
            InstantiateEnemy(horizonalBroadsideEnemy, location);

        ConfigStateWaveDirectionEnemy(newEnemy, direction);
    }

    private void SpawnChaserEnemy(Vector3 location, RelativeDirection direction) {
        // ignores direction...
        InstantiateEnemy(chaserEnemy, location);
    }

    private static void ConfigStateWaveDirectionEnemy(GameObject newEnemy, RelativeDirection direction) {
        WaveMovementState[] waveMovements = newEnemy.GetComponents<WaveMovementState>();
        foreach (WaveMovementState waveMovement in waveMovements) {
            waveMovement.waveDirection = direction;
        }
    }

    private bool IsVerticalDirection(RelativeDirection dir)
    {
        return dir == RelativeDirection.DOWN || dir == RelativeDirection.UP;
    }

    private GameObject InstantiateEnemy(GameObject prefab, Vector3 pos)
    {
        return Instantiate(prefab, SpherePhysics.GetWorldPosition(pos), Quaternion.identity);
    }
}
