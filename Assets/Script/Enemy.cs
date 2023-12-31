﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    [SerializeField]
    private float _enemySpeed = 4f;


    private Player _player;

    // handle to animator component 

    private Animator _animate;

    private AudioSource _explosionSX;


    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private float _enemyFrequency = 1.0f;
    private float _enemyAmplitude = 5.0f;
    private float _enemycycleSpeed = 1.0f;

    private Vector3 _enemyPos;
    private Vector3 _enemyAxis;

    [SerializeField]
    private GameObject _enemyLaser;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _explosionSX = GetComponent<AudioSource>();
        _enemyPos = transform.position;
        _enemyAxis = transform.right;
        
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
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyFire = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyFire.GetComponentsInChildren<Laser>();


            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

        }


    }


    void CalculateMovement()
    {

        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        _enemySpeed = Random.Range(1.0f, 10f);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);

            transform.position = new Vector3(randomX, 7, 0);
        }

        if (transform.position.x < 1 )
        {
            float randomX = Random.Range(-8f, 8f);
            _enemyPos += Vector3.down * Time.deltaTime * _enemycycleSpeed;
            transform.position = _enemyPos + _enemyAxis * Mathf.Sin(Time.time * _enemyFrequency) * _enemyAmplitude;
            _enemycycleSpeed = Random.Range(1.0f, 10.0f);
           

        }


    }



    private void OnTriggerEnter2D(Collider2D other)
    {
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
                Destroy(this.gameObject, 2.8f);

            }
        }


    }

}