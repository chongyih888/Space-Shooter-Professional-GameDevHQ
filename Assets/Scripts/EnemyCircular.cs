using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircular : MonoBehaviour
{
    [SerializeField]
    private float _speed = -50.0f;

    [SerializeField]
    private float _verticalSpeed = 2;

    [SerializeField]
    private float _verticalAmplitude = 0.125f;

    private Vector3 _sineVer;

    private float _time;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    [SerializeField]
    private GameObject _bombPrefab;

    [SerializeField]
    private AudioClip _enemyCircularAudioclip;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _enemyCircularAudioclip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_bombPrefab, transform.position, Quaternion.identity);
            _audioSource.Play();
        }
    }

    void CalculateMovement()
    {
        _time += Time.deltaTime;
        _sineVer.y = Mathf.Sin(_time * _verticalSpeed) * _verticalAmplitude;
        transform.position = new Vector3(transform.position.x + _speed * Time.deltaTime, transform.position.y + _sineVer.y,transform.position.z);
    }
}
