using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs; // Array untuk menyimpan prefab musuh
    public int numberOfEnemies = 5; // Jumlah musuh yang akan di-spawn

    public int CurrentEnemiesTotal;

    [Header("Spawn Area")]
    public Vector3 maxSpawnAreaSize; // Ukuran area spawning (lebar, tinggi, kedalaman)

    public Vector3 minSpawnAreaSize;
    [SerializeField] private Transform player; // Referensi ke transform pemain

    private bool OnSpawning;

    void Start()
    {
        SpawnEnemies();
    }

    void Update(){

    }

    void SpawnEnemies()
    {
        
            for (int i = 0; i < numberOfEnemies; i++)
            {
                int RandomNum = Random.Range(0,4);
                Vector3 spawnPosition = new Vector3(0,0,0);

                if(RandomNum == 0){
                    spawnPosition = player.position + new Vector3(
                        Random.Range(-maxSpawnAreaSize.x / 2, -minSpawnAreaSize.x / 2),
                        0, // Pastikan Y tetap sama dengan pemain
                        Random.Range(-maxSpawnAreaSize.z / 2, -minSpawnAreaSize.z / 2)
                    );
                } else if (RandomNum == 1){
                    spawnPosition = player.position + new Vector3(
                        Random.Range(-maxSpawnAreaSize.x / 2, -minSpawnAreaSize.x / 2),
                        0, // Pastikan Y tetap sama dengan pemain
                        Random.Range(minSpawnAreaSize.z / 2, maxSpawnAreaSize.z / 2)
                    );
                } else if (RandomNum == 2){
                    spawnPosition = player.position + new Vector3(
                        Random.Range(minSpawnAreaSize.x / 2, maxSpawnAreaSize.x / 2),
                        0, // Pastikan Y tetap sama dengan pemain
                        Random.Range(-maxSpawnAreaSize.z / 2, -minSpawnAreaSize.z / 2)
                    );
                } else if (RandomNum == 3){
                    spawnPosition = player.position + new Vector3(
                        Random.Range(minSpawnAreaSize.x / 2, maxSpawnAreaSize.x / 2),
                        0, // Pastikan Y tetap sama dengan pemain
                        Random.Range(minSpawnAreaSize.z / 2, maxSpawnAreaSize.z / 2)
                    );
                }


                // Tentukan posisi spawn acak dalam batasan yang ditentukan
            

                // Pilih tipe musuh secara acak dari prefab
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                // Spawn musuh
                if(CurrentEnemiesTotal < numberOfEnemies){
                    GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    enemy.GetComponent<EnemyMovement>().player = player;
                    enemy.GetComponent<EnemyHP>().spawner = this;
                    CurrentEnemiesTotal++;
                } else {
                    OnSpawning = false;
                    break;
                }

                

                
                
                OnSpawning = false;
            
        }
        
    }

    public void OnEnemyDeath(){
        CurrentEnemiesTotal--;
        if(OnSpawning != true){
            OnSpawning = true;
            Invoke("SpawnEnemies",2f);
        }
        
        
    }

    void OnDrawGizmos()
    {
        // Menggambar Gizmos untuk area spawn
        Gizmos.color = Color.red; // Warna Gizmos
        Vector3 center = player.position; // Pusat area spawn di posisi pemain
        Gizmos.DrawWireCube(center, maxSpawnAreaSize); // Menggambar kubus wireframe

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(center, minSpawnAreaSize);
    }
}
