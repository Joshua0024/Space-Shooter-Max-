using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDos : MonoBehaviour
{


    [SerializeField]
    private float _enemySpeed = 1f;
   

    private Player _player;

    // handle to animator component 

    private Animator _animate;

    private AudioSource _explosionSX;


    private float _fireRate = 1.0f;
    private float _canFire = -1f;

   
    private float _enemycycleSpeed = 1.0f;


    [SerializeField]
    float _rayCastRad = 8.5f;
    [SerializeField]
    float _rayDistance = 8.0f;


    private Vector3 _enemyPos;
    

    [SerializeField]
    private GameObject _enemyLaser;

    [SerializeField]
    private GameObject _shieldVisualizer;

    private bool _isenemyshieldActive = false;
    private int _randomshieldedEnemies;

 

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _explosionSX = GetComponent<AudioSource>();
        _enemyPos = transform.position;
        _randomshieldedEnemies = Random.Range(0, 7);
        


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


            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
            BackFlash();
        }

        transform.Translate(Vector3.down * 3 * Time.deltaTime);
        if (transform.position.y < -7)
        {
            transform.position = new Vector3(Random.Range(-9, 10), 8, 0);
        }


    }


    void CalculateMovement()
    {

        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        _enemySpeed = Random.Range(0.5f, 1f);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-15f, 15f);

            transform.position = new Vector3(randomX, 15, 0);
        }

        if (transform.position.x < 1)
        {

            _enemyPos += Vector3.down * Time.deltaTime * _enemycycleSpeed;
            float randomX = Random.Range(-15f, 15f);
            _enemycycleSpeed = Random.Range(1f, 3.0f);


        }

        


    }



    private void BackFlash()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _rayCastRad, Vector2.up, _rayDistance, LayerMask.GetMask("Player"));

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player") && Time.time > _canFire)
            {
                Debug.DrawRay(transform.position, hit.point, Color.white);
                Debug.Log("Player Detected");
                FireFlashbagBackward();
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.position + transform.right, Color.red);
            Debug.Log("Player Not tDetected");
        }
    }

    private void FireFlashbagBackward()
    {
        _fireRate = Random.Range(7f, 7f);
        _canFire = Time.time + _fireRate;
        GameObject enemyFire = Instantiate(_enemyLaser, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 108.0f));
        Laser[] lasers = enemyFire.GetComponentsInChildren<Laser>();


        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    private void ActiveShield()
    {
        _isenemyshieldActive = true;
        _shieldVisualizer.SetActive(true);

    }

    public void Dodging(float value)
    {
        transform.position = new Vector2(transform.position.x + value, transform.position.y);
        Debug.Log("Dodging");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (_isenemyshieldActive == true)
        {
            _isenemyshieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        // if other is Player 
        //Destroy Us
        //damage the player

        if (other.tag == "Player")
        {

            //damage player
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }



            _animate.SetTrigger("OnEnemyDeath");

            Destroy(this.gameObject, 2.8f);

            _explosionSX.Play();
        }


        //if other is laser
        //laser
        //destroy us

        if (other.tag == "Laser")
        {
            Laser laser = other.GetComponent<Laser>();
            if (laser != null && laser._isEnemyLaser != true)
            {

                Destroy(other.gameObject);
                // Add 10 to Score
                if (_player != null)
                {
                    _player.AddScore(10);
                }

                _animate.SetTrigger("OnEnemyDeath");


                _explosionSX.Play();

                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.0f);

            }
        }


    }

}