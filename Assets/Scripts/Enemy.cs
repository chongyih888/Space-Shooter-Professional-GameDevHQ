using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    [SerializeField]
    private GameObject _backLaserPrefab;

    private Player _player;

    //handle to animator component
    private Animator _anim;

    private AudioSource _audioSource;

    private float _canOtherFire = -1f;

    private float _fireOtherRate = 3.0f;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    [SerializeField]
    private AudioClip _backLaserAudioclip;

    private Transform _playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource is NULL");
        }

        //assign the component to anim
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The animator is NULL");
        }

        _playerTransform = GameObject.Find("Player").transform;

        if (_playerTransform == null)
        {
            Debug.LogError("The Player Transform is NULL.");
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
            EnemyLaser();

        }

        RaycastHit2D hitPowerupInfo = Physics2D.Raycast(transform.position, Vector2.down);

        if (hitPowerupInfo.collider != null)
        {

            // Debug.Log(hitPowerupInfo.collider.name);

            if (hitPowerupInfo.collider.gameObject.CompareTag("Powerup"))
            {
                if (Time.time > _canOtherFire)
                {
                    _canOtherFire = Time.time + _fireOtherRate;
                    EnemyLaser();
                }
            }
        }

        RaycastHit2D hitPlayerInfo = Physics2D.Raycast(transform.position, Vector2.up);

        if (hitPlayerInfo.collider != null)
        {
            // Debug.Log(hitPlayerInfo.collider.name);

            if (hitPlayerInfo.collider.gameObject.CompareTag("Player"))
            {
                if (Time.time > _canOtherFire)
                {
                    _canOtherFire = Time.time + _fireOtherRate;

                    AudioSource.PlayClipAtPoint(_backLaserAudioclip, transform.position);

                    GameObject enemyBackLaser = Instantiate(_backLaserPrefab, transform.position + new Vector3(0, 1.8f, 0), Quaternion.identity);

                    enemyBackLaser.GetComponent<Laser>().AssignEnemyLaserPlayer();
                }

            }
        }

        Vector2 size = new Vector2(3, 3);

        RaycastHit2D hitPlayerLaserInfo = Physics2D.BoxCast(transform.position, size, 0.0f, Vector2.down);

        if (hitPlayerLaserInfo.collider != null)
        {
            //Debug.Log(hitPlayerLaserInfo.collider.name);

            if (hitPlayerLaserInfo.collider.gameObject.CompareTag("Laser"))
            {
                EnemyMove();
            }
        }


        if (_playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, _playerTransform.position);

            if (distance < 3.0f)
            {
                CalculateMovementToPlayer();
            }
        }

        void CalculateMovement()
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -5f)
            {
                float randomX = Random.Range(-8f, 8f);

                transform.position = new Vector3(randomX, 7, 0);
            }
        }

        void EnemyLaser()
        {
            AudioSource.PlayClipAtPoint(_backLaserAudioclip, transform.position);

            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(-0.1f, -6, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            //trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
        }


        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            //Add 10 to score
            _player.UpdateScore(10);

            //trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
        }

        if (other.tag == "HomingProjectile")
        {
            Destroy(other.gameObject);
            _player.UpdateScore(10);

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
        }
    }

    void CalculateMovementToPlayer()
    {
        if (_playerTransform != null)
        {
            Vector3 direction = _playerTransform.position - transform.position;
            // direction.Normalize();

            // transform.localRotation = Quaternion.LookRotation(direction);

            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }

    void EnemyMove(){

        int randomMove = Random.Range(0, 2);

        if (randomMove == 0)
        {

            transform.Translate(_speed * new Vector3(-5f, 0, 0) * Time.deltaTime);
        }
        else { 
            transform.Translate(_speed * new Vector3(5f, 0, 0) * Time.deltaTime);
        }
        
        
    }
}


