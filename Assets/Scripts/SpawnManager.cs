using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyCircularPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] powerups;

    [SerializeField]
    private bool _stopSpawning = false;

    [SerializeField]
    private int _rocketCount = 0;

    private int _randomPowerup;
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnSecondWaveRoutine());
        StartCoroutine(SpawnThirdFinalWaveRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false){
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

            GameObject newEnemy = Instantiate(_enemyPrefab,posToSpawn,Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f); 

        while (_stopSpawning == false)
        {

            Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f), 7, 0);

            if (_rocketCount == 11)
            {
                _randomPowerup = 6;
                _rocketCount++;
            }
            else
            {
                _randomPowerup = Random.Range(0, 6);
                _rocketCount++;
            }

            Instantiate(powerups[_randomPowerup], posToSpawn, Quaternion.identity);

            float randomX = Random.Range(3, 8);

            yield return new WaitForSeconds(randomX);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    IEnumerator SpawnSecondWaveRoutine()
    {
        yield return new WaitForSeconds(20.0f);

        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(-11.5f, 2.2f, 0);

            GameObject newEnemy = Instantiate(_enemyCircularPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(10.0f);
        }
    }

    IEnumerator SpawnThirdFinalWaveRoutine()
    {
        yield return new WaitForSeconds(40.0f);

        while(_stopSpawning == false)
        {

        }

    }
}
