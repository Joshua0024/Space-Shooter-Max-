using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

        private IEnumerator coroutine;

        [SerializeField]
        private GameObject _enemyPrefab;
        
        [SerializeField]
        private GameObject _enemyContainer;

        private bool _stopSpawning = false;

        [SerializeField]
        private GameObject[] powerups;

    // Start is called before the first frame update
  

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    //spawn game objects every 5 seconds 
    //Create a coroutine of type IEnumerator -- Yield Events 
    //while loop

    IEnumerator SpawnRoutine()
    {
        //while loop (infinite loop)
            //Instantiate enemy prefab 
            //Yield wait for 5 seconds 

      Vector3 spawnPos = Vector3.zero;  

      yield return new WaitForSeconds(3.0f);

      while(_stopSpawning == false)  
      {
            spawnPos.x = Random.Range(-8f, 8f);
            spawnPos.y = 7f;
      
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            
            newEnemy.transform.parent = _enemyContainer.transform;
            
            yield return new WaitForSeconds(5.0f);

      }
        


    }

    IEnumerator SpawnPowerupRoutine()

    {
        //every 3-7 seconds,spawen in a powerup

        Vector3 spawnPos = Vector3.zero;

        while(_stopSpawning == false)
        {

            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int  randomPowerUp = Random.Range(0, 3);
            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
   
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
