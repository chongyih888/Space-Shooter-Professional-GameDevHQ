using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;

    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;

    
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //rotate object on the z axis
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);

    }

    //check for Laser collision(Trigger)
    //instantiate explosion at the position of the asteriod (us)
    //destroy the explosion after 3 seconds
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(GetComponent<Collider2D>());
            _spawnManager.StartSpawning();
            Destroy(this.gameObject,0.5f);
        }
    }
}
