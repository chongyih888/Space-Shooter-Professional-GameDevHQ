using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField]
    private int _lives = 5;

    private float _speed = 1.0f;

    [SerializeField]
    private GameObject _bubblePrefab;

    private Player _player;

    private Transform _playerTransform;

    [SerializeField]
    private Transform _targetA;

    [SerializeField]
    private Transform _targetB;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _playerTransform = GameObject.Find("Player").transform;

        if (_playerTransform == null)
        {
            Debug.LogError("The Player Transform is NULL.");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);

        if (_playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, _playerTransform.position);

            if (distance < 5.0f)
            {
                if (Time.time > _canFire)
                {
                    _fireRate = Random.Range(3f, 7f);
                    _canFire = Time.time + _fireRate;

                    Instantiate(_bubblePrefab, transform.position, Quaternion.identity);
                }

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

            _lives--;

            _speed = 0;

            if (_lives < 1)
            {
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject);
                _uiManager.WinSequence();
            }
        }


        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
                         
            _player.UpdateScore(10);
            _lives--;
            
            _speed = 0;                    

            if (_lives < 1)
            {
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject);
                _uiManager.WinSequence();
            }
        }

        if (other.tag == "HomingProjectile")
        {
            Destroy(other.gameObject);

            _player.UpdateScore(10);
            _lives--;
                       
            _speed = 0;

            if (_lives < 1)
            {
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject);
                _uiManager.WinSequence();
            }
        }
    }

}
