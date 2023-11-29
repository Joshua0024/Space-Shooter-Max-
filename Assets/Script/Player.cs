using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public or private reference
    // data type (int, float, bool, string)
    // every variable has a name
    //optional value assigned

    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;

    public float horizontalInput;

    public float verticalInput;

    [SerializeField]
    private GameObject _laserPrefab;



    public Vector3 laserOffset = new Vector3(0, 0.8f, 0);

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _rateoffireMultiplier = -200;

    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;


    [SerializeField]
    private GameObject _tripleshotPrefab;

    //variable for isTripleShotActive

    [SerializeField]
    private bool _istripleshotActive = false;
    private bool _isspeedboostActive = false;
    private bool _isshieldActive = false;
    private bool _isreloadActive = false;
    private bool _isheavyfireActive = false;

    //varible reference to the shield visualizer 

    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private GameObject _rightDamage;
    [SerializeField]
    private GameObject _leftDamage;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private float _thrusterSpeed = 4.5f; 

    [SerializeField]
    public GameObject _thruster;

    [SerializeField]
    private SpriteRenderer _shieldSpriteRenderer;

    private int _shieldStrength = 3;

    [SerializeField]
    public int _ammoCount = 15;
    [SerializeField]
    public int _maxAmmo;

    [SerializeField]
    AudioClip _emptyclipSound;



    //varible to store the audio clip 









    // Start is called before the first frame update
    void Start()
    {
        _maxAmmo = _ammoCount;

        //take the current position = new position (0 ,0 ,0)
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _shieldSpriteRenderer = this.transform.Find("Shields").GetComponent<SpriteRenderer>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

    }

    // Update is called once per frame
    void Update()
    {
            CalculatedMovement();

        //if i hit the space key
        //spawn gameObject

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if(_ammoCount == 0)
            {
                AudioSource.PlayClipAtPoint(_emptyclipSound, transform.position);
                return; 
            }
            FireLaser();
        }

        if (_isheavyfireActive && Input.GetKey(KeyCode.Space))
        {
            FireLaser();
        }
       
      




            if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _speed = _speed += _thrusterSpeed;
            _thruster.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            _speed = _speed -= _thrusterSpeed;
            _thruster.SetActive(false); 
        }

    }



    void CalculatedMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        float verticalInput = Input.GetAxis("Vertical");

        // new Vector3(-3.5, 0, 0) *-1 *0 *3.5 * real time 
        //if speedboostactive is false 
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);


        //else speed boost multiplier

        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        //if player position on the y is greater than 0
        //y postion = 0
        //else if position on the y is less than -3.8f
        //y pos = -3.8f

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        //if player on the x > 11
        // x pos = -11
        // else if if player on the x is less than -11
        // x pos = 11

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        AmmoCounter(-1);

        _canFire = Time.time + _fireRate;

        //if space key press, 
        //if tripleshotActive is true
        // fire 3 laser (triple shot prefab)

        //else fire 1 laser            

        //instantiate 3 lasers (triple shot prefab)

        if (_istripleshotActive == true)
        {
            Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

       

        //play the laser audio clip 

    }


    public void Damage()
    {
        if (_isshieldActive == true && _shieldStrength >= 1)
        {
            _shieldStrength--;

            switch(_shieldStrength)
            {
                case 0:
                    _isshieldActive = false;
                    _shield.SetActive(false);
                    break;
                case 1:
                    _shieldSpriteRenderer.color = Color.red;
                    break;
                case 2:
                    _shieldSpriteRenderer.color = Color.magenta;
                    break;

            }

            return; 
        }

        
        _uiManager.UpdateLives(_lives);

        switch (_lives)
        {
            case 2:
                _rightDamage.gameObject.SetActive(true);
                break;
            case 1:
                _leftDamage.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        //if shield is active
        //do nothing....
        //deactivate shields 
        //return; 

        if (_isshieldActive == true)
        {
            _isshieldActive = false;
            _shield.SetActive(false);
            return;
        }

        _lives -= 1;

        //if lives is 2 
        //enable right engine
        //else if lives is 1
        //enable left engine

        if (_lives == 2)
        {
            _rightDamage.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftDamage.SetActive(true);
        }


        _uiManager.UpdateLives(_lives);

        //check if dead
        //destroy us

        if (_lives < 1)
        {

            _spawnManager.OnPlayerDeath();

            Destroy(this.gameObject);
        }

    }

    public void tripleshotActive()
    {
        //tripleshotActive becomes true 
        //start the power down coroutine for triple shot

        _istripleshotActive = true;

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    //IEnumerator TripleShotPowerDownRoutine
    //Wait 5 seconds
    //set the triple shot to false

    IEnumerator TripleShotPowerDownRoutine()
    {

        yield return new WaitForSeconds(5.0f);
        _istripleshotActive = false;

    }

    public void reloadActive()
    {
        _isreloadActive = true;
        _ammoCount = _maxAmmo;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void speedboostActive()
    {
        _isspeedboostActive = true;
        _speed *= _speedMultiplier;

        StartCoroutine(SpeedBoostCooldownRoutine());

    }

    IEnumerator SpeedBoostCooldownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isspeedboostActive = false;
        _speed /= _speedMultiplier;

    }

    public void shieldsActive()
    {
        _isshieldActive = true;
        _shield.SetActive(true);
        _shieldStrength = 3;
        _shieldSpriteRenderer.color = Color.green;


        //enable the visualizer
    }



    //method to add 10 to the score!
    //Communicate with the UI to update the score!
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

    public void AmmoCounter(int bullets)
    {
        _ammoCount += bullets;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void AddingHealth()
    {
        if(_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);

            if (_lives == 2)
            {
                _rightDamage.SetActive(false);
            }
            else if (_lives == 3)
            {
                _leftDamage.SetActive(false);
            }
        }
    }

    public void HeavyFire()
    {
        _isheavyfireActive = true;
        _fireRate *= _rateoffireMultiplier;

        StartCoroutine(FireRatedownRoutine());
    }

    IEnumerator FireRatedownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isheavyfireActive = false;
        _fireRate /= _rateoffireMultiplier;

        _isheavyfireActive = false;
        _ammoCount = _maxAmmo; 

     
    }

    
}