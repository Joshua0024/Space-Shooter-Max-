using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour

{
    
    [SerializeField]
    private float _enemySpeed = 1f;

    private float _kamikazeSpeed = 6f;

    private Player _player;

    // handle to animator component 

    private Animator _animate;

    private AudioSource _explosionSX;


    private float _fireRate = 1.0f;
    private float _canFire = -1f;

    private float _enemyFrequency = 1.0f;
    private float _enemyAmplitude = 5.0f;
    private float _enemycycleSpeed = 1.0f;
    private float _enemyAggressive = 3.0f;

    [SerializeField]
    float _rayCastRad = 8.5f;
    [SerializeField]
    float _rayDistance = 8.0f;

    private Vector3 _enemyPos;
    private Vector3 _enemyAxis;

    [SerializeField]
    private GameObject _enemyLaser;

    [SerializeField]
    private GameObject _shieldVisualizer;

    private bool _isenemyshieldActive = false;
    private int _randomshieldedEnemies;
    [SerializeField]
    private SpriteRenderer _shieldRenderer;
    private int _shieldStrengh = 1;

    private bool _isLaserDetected;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _explosionSX = GetComponent<AudioSource>();
        _enemyPos = transform.position;
        _enemyAxis = transform.right;
        _randomshieldedEnemies = Random.Range(0, 7);
        _enemyAggressive = Random.Range(0, 5);


        //null check player
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        //assign the component

        _animate = GetComponent<Animator>();

        if (_animate == null)
        {
            Debug.LogError("The aninator is NULL");
        }

        _randomshieldedEnemies = Random.Range(0, 5);

        if (_randomshieldedEnemies == 3)
        {
            ActiveShield();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4 meters per second

        //if bottom of screen
        //respawn at to with a new random x position

        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1f, 3f);
            _canFire = Time.time + _fireRate;
            GameObject enemyFire = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyFire.GetComponentsInChildren<Laser>();
        }
    }


    void CalculateMovement()
    {

        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        _enemySpeed = Random.Range(3f, 5f);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-15f, 15f);

            transform.position = new Vector3(randomX, 15, 0);
        }

        if (transform.position.x < 1)
        {

            _enemyPos += Vector3.down * Time.deltaTime * _enemycycleSpeed;
            float randomX = Random.Range(-15f, 15f);
            transform.position = _enemyPos + _enemyAxis * Mathf.Sin(Time.time * _enemyFrequency) * _enemyAmplitude;
            _enemycycleSpeed = Random.Range(1f, 5.0f);
        }

        if (_player != null)
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < _enemyAggressive)
            {
                KamikazePlayer();
            }
        }
    }

    public void ActiveShield()
    {
        _isenemyshieldActive = true;
        _shieldVisualizer.SetActive(true);

    }



    private void KamikazePlayer()
    {
        if (transform.position.x < _player.transform.position.x)
        {
            transform.Translate(Vector3.right * _kamikazeSpeed * Time.deltaTime);
        }
        else if (transform.position.x > _player.transform.position.x)
        {
            transform.Translate(Vector3.left * _kamikazeSpeed * Time.deltaTime);
        }
        else if (transform.position.y > _player.transform.position.y)
        {
            transform.Translate(Vector3.down * _kamikazeSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if other is Player 
        //Destroy Us
        //damage the player

        if (other.gameObject.CompareTag("Player"))
        {

            //damage player
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            _animate.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _explosionSX.Play();
            Destroy(this.gameObject, 2.8f);
            
        }

        //if other is laser
        //laser
        //destroy us

        if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            
            if(_isenemyshieldActive)
            {
                _isenemyshieldActive = false;
                _shieldVisualizer.SetActive(false);
                return;
            }

            if (_player != null)
            {
                _player.AddScore(10);

            }

            _animate.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.8f);
        }
    }

   
}