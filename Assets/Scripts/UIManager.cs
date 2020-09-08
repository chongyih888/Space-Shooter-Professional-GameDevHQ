using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Image _LivesImg;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _winText;

    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Text _shieldStrengthText;

    [SerializeField]
    private Image[] _thrusterBars;

    // Start is called before the first frame update
    void Start()
    {        
        //assign text component to the handle
        _scoreText.text = "Score: " + 0;
        _shieldStrengthText.text = "Shield: " + 0;

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("The GameMamanger is NULL");
        }

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        //display img sprite
        //give it a new one based on the currentLives index
        _LivesImg.sprite = _liveSprites[currentLives];
        
        if(currentLives < 1)
        {
            GameOverSequence();
        }
    }

    public void WinSequence()
    {
        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        _winText.gameObject.SetActive(true);
        StartCoroutine(WinFlickerRoutine());
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());

    }

    IEnumerator WinFlickerRoutine()
    {
        while (true)
        {
            _winText.text = "YOU WIN";
            yield return new WaitForSeconds(0.5f);
            _winText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo + "/15";
    }

    public void UpdateShieldStrength(int shieldStrength, bool show)
    {
        if (show == true)
        {
            _shieldStrengthText.gameObject.SetActive(true);
            

        }else if(show == false)
        {
            _shieldStrengthText.gameObject.SetActive(false);
        }

        _shieldStrengthText.text = "Shield: " + shieldStrength;
    }   

    public void UpdateThrusterScale(float powerRemaining, bool canBoost)
    {
        if (canBoost)
        {
            _thrusterBars[(int)Mathf.RoundToInt(powerRemaining)].enabled = false;
        }else if(canBoost == false)
        {
            _thrusterBars[(int)Mathf.RoundToInt(powerRemaining)].enabled = true;
        }
        
    }

   
}
