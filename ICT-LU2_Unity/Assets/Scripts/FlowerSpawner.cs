using UnityEngine;

public class FlowerSpawner : MonoBehaviour {
    
    [Header("Spawn Settings")]
    public Sprite[] flowerSprites;
    public GameObject flowerPrefab;
    public Vector2 areaSize = new Vector2(20f, 10f);
    public int flowerCount = 20;
    public Transform parentObject;
    
    
    [Header("Seed Settings")]
    public int seed;
    public bool useRandomSeed;

    [ContextMenu("Reseed and Respawn")]
    public void RespawnFlowers() {
        foreach (Transform flower in transform)
            DestroyImmediate(flower.gameObject);
        
        int appliedSeed = useRandomSeed ? Random.Range(0, int.MaxValue) : seed;
        Random.InitState(appliedSeed);
        Debug.Log("Seed: " + appliedSeed);

        for (int i = 0; i < flowerCount; i++) {
            Vector2 spawnPos = new Vector2(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
            );
            
            GameObject flower = Instantiate(flowerPrefab, spawnPos, Quaternion.identity, parentObject ? parentObject : transform);
            SpriteRenderer spriteRenderer = flower.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = flowerSprites[Random.Range(0, flowerSprites.Length)];
        }
    }
    
    void Start() {
        RespawnFlowers();
    }
}
