using Unity.Multiplayer.PlayMode;
using UnityEngine;

public class PowerUpSpawnPoint : MonoBehaviour
{
    public GameObject[] powerUps;
    public float spawnInterval = 40f;
    private float timeSinceLastSpawn;
    private GameObject currentPowerUp;
   
    private void Start()
    {
        SpawnPowerUp();
        timeSinceLastSpawn = spawnInterval;
        
    }
    private void Update()
    {
        if (currentPowerUp == null)
        {
            if (timeSinceLastSpawn < spawnInterval) 
            {
                timeSinceLastSpawn += Time.deltaTime; 
            }
            else
            {
                SpawnPowerUp(); 
                timeSinceLastSpawn = 0;     
            }  
        }            
    }

    private void SpawnPowerUp()
    {
        if (powerUps.Length == 0)
        {
            return;
        }
        int index = Random.Range(0, powerUps.Length);
        currentPowerUp = Instantiate(powerUps[index], transform.position, Quaternion.identity);
        currentPowerUp.transform.SetParent(this.transform);
        
        
        
    }

}
