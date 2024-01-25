using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

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

    private float _spawnTimer = 7f;

    private float _istimerActive = 0f;

    private float _waveTime = 0;

    private float _waveTimer = 60f;

    [SerializeField]
    private int _waveNumber = 1;

    [SerializeField]
    private TextMeshProUGUI _wavecountText;







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

        _waveTime = Time.time + _waveTimer;

        while (_stopSpawning == false && Time.time <= _waveTime)
        {
            spawnPos.x = Random.Range(-8f, 8f);
            spawnPos.y = 5f;

            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5.0f);

        }
        _waveNumber++;

        Debug.Log("End of Wave");
        StartCoroutine(WaveBreak());
    }

    IEnumerator WaveBreak()
    {
        if (_waveNumber != 4)
        {
            Debug.Log("Waiting");
            ActivateWaveText();
            yield return new WaitForSeconds(5f);
            _wavecountText.gameObject.SetActive(false);
            Debug.Log("Spawn New Wave");
            StartCoroutine(SpawnRoutine());


        }
        else
        {

        }
    }

    IEnumerator SpawnPowerupRoutine()

    {
        //every 3-7 seconds,spawen in a powerup

        Vector3 spawnPos = Vector3.zero;

        while (_stopSpawning == false)
        {

            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 6);
            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));

        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    private void ActivateWaveText()
    {
        _wavecountText.text = "Wave:" + _waveNumber.ToString();
        _wavecountText.gameObject.SetActive(true);
    }
}
