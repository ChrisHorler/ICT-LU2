using System;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject buildingPrefab;
    public GameObject backgroundWorld;

    [Serializable]
    public class TypeSprite
    {
        public string typeName;
        public Sprite sprite;
    }
    public TypeSprite[] knownSprites;

    void Start()
    {
        if (TempWorldDataHolder.WorldData == null) {
            Debug.LogWarning("No World Data to Load");
        }
        
        var detail = TempWorldDataHolder.WorldData;
        Debug.Log($"Loading world ID = {detail.worldData.id}, Name = {detail.worldData.name}");
        
        SetupBackground(detail.worldData.sizeX, detail.worldData.sizeY);

        foreach (var objData in detail.objects)
        {
            SpawnObject(objData);
        }
    }

    private void SetupBackground(int sizeX, int sizeY)
    {
        if (backgroundWorld != null) {
            backgroundWorld.transform.localScale = new Vector3(sizeX, sizeY, 1);
        }
    }

    private void SpawnObject(WorldApiManager.WorldObjectData objData)
    {
        Sprite chosenSprite = null;
        foreach (var ts in knownSprites) {
            if (ts.typeName.ToLower() == objData.type.ToLower()) {
                chosenSprite = ts.sprite;
                break;
            }
        }

        if (chosenSprite == null)
        {
            Debug.LogWarning($"Unkown object type: {objData.type}");
            return;
        }
        
        Vector3 pos = new Vector3(objData.positionX, objData.positionY, 0);
        Quaternion rot = Quaternion.Euler(0, 0, objData.rotation);
        
        GameObject spawned = Instantiate(buildingPrefab, pos, rot);
        spawned.transform.localScale = new Vector3(objData.scale, objData.scale,0);
        
        SpriteRenderer sr = spawned.GetComponent<SpriteRenderer>();
        if (sr != null) {
            sr.sprite = chosenSprite;
        }
        
        Debug.Log($"Spawned {objData.type} at ({pos.x}, {pos.y}), scale = {objData.scale}, rotation = {objData.rotation}");
    }
}
