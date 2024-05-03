
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
   
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    public float _rotateSpeed = 200f;

    private Transform _target;

    private Rigidbody2D rb;

    private float _liveTime = 5f; 


    // Start is called before the first frame update
    void Start()
    {
        Enemy _enemy = transform.GetComponent<Enemy>();

        rb = GetComponent<Rigidbody2D>();

        _target = GameObject.FindGameObjectWithTag("Enemy").transform;

        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        _liveTime -= Time.deltaTime;

        if(transform.position.y > 8 || _liveTime <= 0f)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        if (_target == null)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        else
        {
            Vector2 direction = ((Vector2)_target.position - rb.position);

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * _rotateSpeed;

            rb.velocity = transform.up * _speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit" + other.transform.name);

        if (!other.CompareTag("enemy")) return;



        
            Debug.Log("homing hit enemy");
            Destroy(other.gameObject);
        
    }

}
