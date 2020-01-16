using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultipler = 2;

    [SerializeField]
    private float _thrusterSpeedBoost = 1;
   
    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    private float _canFire = -1f;

    [SerializeField]
    private float _fireRate = 0.5f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _isSpeedBoostActive = false;

    [SerializeField]
    private bool _isShieldsActive = false;

    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _shieldsVisualizer;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    //variable to store the audio clip
    [SerializeField]
    private AudioClip _laserAudioclip;
    
    private AudioSource _audioSource;

    [SerializeField]
    private int _ammoCount = 15;

    private bool _hasAmmo = true;

    [SerializeField]
    private int _shieldStrength;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserAudioclip;
        }
              
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _hasAmmo == true)
        {
            FireLaser();
            AmmoStatus();
            _uiManager.UpdateAmmo(_ammoCount);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _thrusterSpeedBoost = 1.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _thrusterSpeedBoost = 1;
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        
        transform.Translate(direction * _speed * _thrusterSpeedBoost * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.2f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);

        } else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {

            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.55f, 0), Quaternion.identity);
        }

        // play the laser audio clip
        _audioSource.Play();

    }

    public void Damage()
    {
      

        if(_isShieldsActive == true && _shieldStrength > 0)
        {
            ShieldStrength();

            if(_shieldStrength == 0)
            {
                _isShieldsActive = false;
                _shieldsVisualizer.SetActive(false);
            }

            return;

        }
    
        _lives--;

        //if lives is 2
        //enable right engine
        //else if lives is 1
        //enable left engine
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);

        } else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        if (_lives < 0)
        {
            _lives = 0;
        }

        _uiManager.UpdateLives(_lives);


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultipler;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultipler;
    }

    public void ShieldsActive()
    {
        _shieldStrength = 3;
        _uiManager.UpdateShieldStrength(_shieldStrength);
        _isShieldsActive = true;
        _shieldsVisualizer.SetActive(true);
    }

    //method to add 10 to the score!
    //communicate with the ui to update the score
    public void UpdateScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    void AmmoStatus()
    {
        _ammoCount--;

        if(_ammoCount < 1)
        {
            _hasAmmo = false;
        }
    }

    public void ShieldStrength()
    {
        _shieldStrength--;
        _uiManager.UpdateShieldStrength(_shieldStrength);        
    }
   
    public void BulletsActive()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void HealthBoostActive()
    {
        if (_lives < 3)
        {
            if (_lives > 0)
            {
                _lives++;
                _uiManager.UpdateLives(_lives);

                if(_lives > 2)
                {
                    _leftEngine.SetActive(false);

                }

                if (_lives > 1)
                {
                    _rightEngine.SetActive(false);
                }
            }
        }
    }
}



