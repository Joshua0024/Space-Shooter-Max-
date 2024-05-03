using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;

        if (collision.tag == "Laser")
        {
            float value;

            if(this.transform.position.x < collision.transform.position.x)
            {
                value = -2f;
                if (Random.Range(0, 2) == 0) this.GetComponentInParent<EnemyDos>().Dodging(value);
            }

            if (this.transform.position.x > collision.transform.position.x)
            {
                value = 2f;
                if (Random.Range(0, 2) == 0) this.GetComponentInParent<EnemyDos>().Dodging(value);
            }
            
        }
    }

   
}


