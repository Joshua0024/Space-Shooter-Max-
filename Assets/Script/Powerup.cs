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
    private AudioClip _powerupSX;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }


    // Update is called once per frame
    void Update()
    {
        // move down at a speed of 3 
        // When we leave the screen, destroy this object 
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);

        if (transform.position.y < -3f)
        {

            Destroy(this.gameObject);
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
                    default:
                        Debug.Log("Default Value");
                        break;


                }
            }
            AudioSource.PlayClipAtPoint(_powerupSX, transform.position);
            Destroy(this.gameObject);
        }
    }
}
