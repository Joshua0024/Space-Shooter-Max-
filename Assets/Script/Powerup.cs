using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _powerupSpeed = 3.0f;

    //ID for Powerups
    //0 = Triple shot
    //1 = speed
    //2 = Shields
    [SerializeField] // 0 = Triple Shot 1 - Speed 2 - Shields
    private int powerupID;


    private Powerup _powerup;


    private Player _player;

    [SerializeField]
    private GameObject _thePlayer;

    [SerializeField]
    private AudioClip _powerupSX;


    [SerializeField]
    private float _spaceMagnet = 1.0f;

    [SerializeField]
    private float _magnetRate = 0.5f;

    private float _canMagnet = -1;

   
    
  

   

    void Start()
    {
        _thePlayer = GameObject.FindGameObjectWithTag("Player");

        if (_thePlayer == null)
        {
            Debug.LogError("Player is null");
        }
        
        _player = GameObject.Find("Player").GetComponent<Player>();

        

    }


    // Update is called once per frame
    void Update()
    {
        // move down at a speed of 3 
        // When we leave the screen, destroy this object 
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        _powerupSpeed = Random.Range(2.0f, 5f);

        if (transform.position.y < -3f)
        {

            Destroy(this.gameObject);
        }

        
        if (Input.GetKey(KeyCode.C) && Time.time > _canMagnet)  
        {
            _canMagnet = Time.time + _magnetRate;
            TrackerBeam();
        }
       
    }

    //OnTriggerCollision
    //Only be collectable by the Player (HINT: Use Tags)
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            //communicate with the playerscript
            //handle to the component i want
            //assign the handle to the compnent

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {


                switch (powerupID)
                {
                    case 0:
                        player.tripleshotActive();
                        _player.AddScore(5);
                        break;
                    case 1:
                        player.speedboostActive();
                        _player.AddScore(1);
                        break;
                    case 2:
                        player.shieldsActive();
                        _player.AddScore(3);
                        break;
                    case 3:
                        player.reloadActive();
                        player.AddScore(2);
                        Debug.Log("Ammo Colleted");
                        break;
                    case 4:
                        player.AddingHealth();
                        player.AddScore(4);
                        Debug.Log("Adding Health");
                        break;
                    case 5:
                        player.HeavyFire();
                        player.AddScore(6);
                        Debug.Log("Heavy Fire Actived");
                        break;
                    case 6:
                        player.DangerBall();
                        player.AddScore(-10);
                        Debug.Log("Direct Hit");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;


                }
            }
            AudioSource.PlayClipAtPoint(_powerupSX, transform.position);
            Destroy(this.gameObject);
        }
        else 
        {
            other.gameObject.TryGetComponent<Laser>(out Laser _isEnemyLaser);

            if(_isEnemyLaser != null)
            {
                Destroy(this.gameObject);
            }
            _player.AddScore(-20);

        }

        

    }

    private void TrackerBeam()
    {
        transform.position = Vector3.Lerp(this.transform.position, _thePlayer.transform.position, _spaceMagnet * Time.deltaTime);
    }
    
   
}
