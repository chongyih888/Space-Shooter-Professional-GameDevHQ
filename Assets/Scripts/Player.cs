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
    private bool _isHomingProjectileBoostActive = false;

    [SerializeField]
    private bool _isSpeedBoostActive = false;

    [SerializeField]
    private bool _isNegativeSpeedBoostActive = false;

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

    [SerializeField]
    private bool _hasAmmo = true;

    [SerializeField]
    private int _shieldStrength;

    [SerializeField]
    private GameObject _homingProjectilePrefab;

    private CameraShake _camerashake;

    [SerializeField]
    private bool _canLeftShift = true;

    [SerializeField]
    private float _numberOfBars = 3;

    private WaitForSeconds _delayYield;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _camerashake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();

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

        if(_camerashake == null)
        {
            Debug.LogError("The camerashake is NULL");
        }

        _delayYield = new WaitForSeconds(5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire )
        {
            FireLaser();                 
        }
                

        if (Input.GetKey(KeyCode.LeftShift) && (_numberOfBars > 0) && (_canLeftShift == true))
        {                  
            _thrusterSpeedBoost = 1.5f;

            _uiManager.UpdateThrusterScale(_numberOfBars,_canLeftShift);

            _numberOfBars = _numberOfBars - (1 * Time.deltaTime);

            if(_numberOfBars < 1)
            {
                _canLeftShift = false;
                              
            }

        }
        else if (!Input.GetKey(KeyCode.LeftShift) && _numberOfBars < 3 && _canLeftShift == false)
        {
            _uiManager.UpdateThrusterScale(_numberOfBars, _canLeftShift);

            _numberOfBars = _numberOfBars + (1 * Time.deltaTime);

            if (_numberOfBars > 3)
            {
                _canLeftShift = true;
            }
        }
        else
        {
            _thrusterSpeedBoost = 1.0f;
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
            if (_hasAmmo == true && _ammoCount > 0)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                AmmoStatus();
                _audioSource.Play();
            }           
        }
        else if (_isHomingProjectileBoostActive == true)
        {
                Instantiate(_homingProjectilePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            if (_hasAmmo == true && _ammoCount > 0)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.55f, 0), Quaternion.identity);
                AmmoStatus();
                _audioSource.Play();
            }                     
        }
             

    }    

    public void Damage()
    {
        _camerashake.Shake();

        if(_isShieldsActive == true && _shieldStrength > 0)
        {
            ShieldStrength();

            if(_shieldStrength == 0)
            {
                _isShieldsActive = false;
                _shieldsVisualizer.SetActive(false);
                _uiManager.UpdateShieldStrength(_shieldStrength,_isShieldsActive);
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
        yield return _delayYield;
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
        yield return _delayYield;
        _isSpeedBoostActive = false;
        _speed /= _speedMultipler;
    }

    public void NegativeSpeedBoostActive()
    {
        _isNegativeSpeedBoostActive = true;
        _speed /= _speedMultipler;
        StartCoroutine(NegativeSpeedBoostPowerDownRoutine());

    }

    IEnumerator NegativeSpeedBoostPowerDownRoutine()
    {
        yield return _delayYield;
        _isNegativeSpeedBoostActive = false;
        _speed *= _speedMultipler;
    }


    public void ShieldsActive()
    {
        _shieldStrength = 3;        
        _isShieldsActive = true;
        _uiManager.UpdateShieldStrength(_shieldStrength, _isShieldsActive);
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
            _ammoCount = 0;
            _uiManager.UpdateAmmo(_ammoCount);
        }
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void ShieldStrength()
    {
        _shieldStrength--;
        _uiManager.UpdateShieldStrength(_shieldStrength,_isShieldsActive);        
    }
   
    public void BulletsActive()
    {
        _ammoCount = 15;
        _hasAmmo = true;
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

    public void HomingProjectileBoostActive()
    {
        _isHomingProjectileBoostActive = true;
        StartCoroutine(HomingProjectileBoostPowerDownRoutine());
    }

    IEnumerator HomingProjectileBoostPowerDownRoutine()
    {
        yield return _delayYield;
        _isHomingProjectileBoostActive = false;
    }
}



