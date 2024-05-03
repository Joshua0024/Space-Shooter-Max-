using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShielded : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
        {
            Player Player = other.transform.GetComponent<Player>();

            Player.Damage();
            Destroy(gameObject);
        }

      if (other.CompareTag("Laser"))
        {
            Destroy(gameObject);
        }
    }


}