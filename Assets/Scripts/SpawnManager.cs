using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyShieldPrefab;

    [SerializeField]
    private GameObject _enemyCircularPrefab;

    [SerializeField]
    private GameObject _enemyBoss;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] powerups;

    [SerializeField]
    private bool _stopSpawning = false;

    [SerializeField]
    private int _powerupCounter = 0;
        
    private int _randomPowerup;
    
    public void StartSpawning()
    {        
       // StartCoroutine(SpawnEnemyRoutine());
       // StartCoroutine(SpawnSecondWaveRoutine());
        StartCoroutine(SpawnThirdFinalWaveRoutine());

        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnPowerupSecondRoutine());
        StartCoroutine(SpawnPowerupThirdRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false){

            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

            int randomSpawn = Random.Range(0, 2);

            if (randomSpawn == 0)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else
            {
                GameObject newEnemy = Instantiate(_enemyShieldPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                newEnemy.transform.GetComponent<Enemy>().ShieldStatus();
            }
                 
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f); 

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f), 7, 0);

            if(_powerupCounter % 4 == 0)
            {
                _randomPowerup = 4;
                _powerupCounter++;
            }
            else if(_powerupCounter % 11 == 0)
            {
                _randomPowerup = 5;
                _powerupCounter++;
            }
            else if(_powerupCounter % 13 == 0)
            {
                _randomPowerup = 6;
                _powerupCounter++;
            }
            else
            {
                _randomPowerup = Random.Range(0, 4);
                _powerupCounter++;
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
        yield return new WaitForSeconds(3.0f);

        OnPlayerDeath();

        for (int j = 0; j < 3; j++)
        {
               Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                               
               GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
               newEnemy.transform.parent = _enemyContainer.transform;
        }

        yield return new WaitForSeconds(15.0f);

        Instantiate(_enemyBoss,new Vector3(0,8,0),Quaternion.identity);
    }

    IEnumerator SpawnPowerupSecondRoutine()
    {
        yield return new WaitForSeconds(20.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                                
            _randomPowerup = Random.Range(0, 5);                     

            Instantiate(powerups[_randomPowerup], posToSpawn, Quaternion.identity);

            float randomX = Random.Range(3, 8);

            yield return new WaitForSeconds(randomX);
        }
    }

    IEnumerator SpawnPowerupThirdRoutine()
    {
        yield return new WaitForSeconds(40.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                       
            _randomPowerup = Random.Range(0, 5);
               
            Instantiate(powerups[_randomPowerup], posToSpawn, Quaternion.identity);

            float randomX = Random.Range(3, 8);

            yield return new WaitForSeconds(randomX);
        }
    }
}
