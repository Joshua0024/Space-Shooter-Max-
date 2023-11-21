using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    [SerializeField]
    public GameObject _thrusters;

    private float _speed = 5.5f; 

    // Update is called once per frame
    void Update()
    {
        RocketThrusters();
    }

        void RocketThrusters()
        {
            if (Input.GetKey(KeyCode.UpArrow))
                {
                    if(_thrusters != null)
                    {
                       _speed = 8f;
                        
                       _thrusters.SetActive(true);
                    }
                   else

                    {

                     _speed = 5.5f;

                     _thrusters.SetActive(false); 

                    }

                }
        }
        
}
