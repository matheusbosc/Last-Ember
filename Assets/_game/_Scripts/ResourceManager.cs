using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceManager : MonoBehaviour
{
    private int stickAmount, barkAmount;


    public int maxStickAmount, maxBarkAmount;
    public GameObject[] stickPrefabs, barkPrefabs;
    public Transform stickParent, barkParent;
    
    public LayerMask blockingLayers;
    public Collider2D spawnArea, barkSpawnArea;
    public float spawnRadius = 0.5f;

    public bool spawningBark = false;
    
    public static ResourceManager instance;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        bool isPaused = GameManager.instance.isPaused;
        
        if  (isPaused) return;
        
        if (stickAmount <= maxStickAmount)
        {

            int sticksToSpawn = maxStickAmount - stickAmount;

            for (int i = 0; i < sticksToSpawn; i++)
            {
                Spawn();
                stickAmount++;
            }
        }

        if (spawningBark)
        {
            if (barkAmount >= maxBarkAmount) return;
        
            int barkToSpawn = maxBarkAmount - barkAmount;

            for (int i = 0; i < barkToSpawn; i++)
            {
                SpawnBark();
                barkAmount++;
            }
        }
        
    }

    public void RemoveBark()
    {
        barkAmount--;}

    public void RemoveStick()
    {
        stickAmount--;
    }
    
    public void SpawnBark()
    {

        Vector2 pos = GetRandomPointInBounds(barkSpawnArea.bounds);

        
            GameObject barkPrefab = barkPrefabs[Random.Range(0, barkPrefabs.Length)];
                
            Instantiate(barkPrefab, pos, Quaternion.Euler(new Vector3(barkPrefab.transform.eulerAngles.x, 
                barkPrefab.transform.eulerAngles.z, 
                UnityEngine.Random.Range(0, 360))), barkParent);
            return;
        
        
    }
    
    public void Spawn()
    {

            Vector2 pos = GetRandomPointInBounds(spawnArea.bounds);

            // Check clearance
            bool blocked = Physics2D.OverlapCircle(
                pos,
                spawnRadius,
                blockingLayers
            );

            if (!blocked)
            {
                GameObject stickPrefab = stickPrefabs[Random.Range(0, stickPrefabs.Length)];
                
                Instantiate(stickPrefab, pos, Quaternion.Euler(new Vector3(stickPrefab.transform.eulerAngles.x, 
                                                                                stickPrefab.transform.eulerAngles.z, 
                                                                                UnityEngine.Random.Range(0, 360))), stickParent);
                return;
            }
        
    }

    Vector2 GetRandomPointInBounds(Bounds b)
    {
        return new Vector2(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y)
        );
    }
}
