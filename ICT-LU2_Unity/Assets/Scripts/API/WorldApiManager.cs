using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WorldApiManager : MonoBehaviour
{
    [Header("API Configuration")]
    public string baseUrl = "https://avansict2227807.azurewebsites.net";

    private string JwtToken => ApiManager.JwtToken; // or however you store the token

    // -------------
    // DTOs
    // -------------
    [Serializable]
    private class CreateWorldDto
    {
        public string Name;
        public int SizeX;
        public int SizeY;
    }

    [Serializable]
    private class CreateObjectDto
    {
        public string Type;
        public float PositionX;
        public float PositionY;
        public float Rotation;
        public float Scale;
    }

    [Serializable]
    public class WorldData {
        public int id;
        public string name;
        public int sizeX;
        public int sizeY;
        public int ownerId;
    }

    [Serializable]
    public class WorldObjectData {
        public int id;
        public string type;
        public float positionX;
        public float positionY;
        public float rotation;
        public float scale;
    }

    [Serializable]
    public class WorldDataDetailResponse {
        public WorldData worldData;
        public WorldObjectData[] objects;
    }

    // -------------
    // World Endpoints
    // -------------

    public void GetMyWorld()
    {
        StartCoroutine(GetMyWorldsCoroutine());
    }

    private IEnumerator GetMyWorldsCoroutine()
{
    string endpoint = $"{baseUrl}/api/world";
    var request = UnityWebRequest.Get(endpoint);
    request.downloadHandler = new DownloadHandlerBuffer();
    
    if (!string.IsNullOrEmpty(JwtToken))
        request.SetRequestHeader("Authorization", $"Bearer {JwtToken}");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        string raw = request.downloadHandler.text;
        Debug.Log("GetMyWorlds response: " + raw);

        try {
            // JsonHelper to parse the raw array of WorldData
            WorldData[] worlds = JsonHelper.FromJsonArray<WorldData>(raw);
            if (worlds != null && worlds.Length > 0) {
                var worldIds = new List<int>();
                var worldNames = new List<string>();

                foreach (var w in worlds) {
                    worldIds.Add(w.id);
                    worldNames.Add(w.name);
                }
                
                SelectWorldUI selector = FindObjectOfType<SelectWorldUI>();
                if (selector != null) {
                    selector.PopulateWorldsList(worldIds, worldNames);
                }
                else {
                    Debug.LogWarning("No SelectWorldUI found in scene to populate!");
                }
            }
            else {
                Debug.Log("No worlds found in the response array.");
            }
        }
        catch (Exception ex) {
            Debug.LogError($"Failed to parse list of worlds: {ex}");
        }
    }
    else {
        Debug.LogError($"GetMyWorlds error: {request.error} | {request.downloadHandler.text}");
    }
}


    public void GetWorld(int worldId)
    {
        StartCoroutine(GetWorldCoroutine(worldId));
    }

    private IEnumerator GetWorldCoroutine(int worldId)
    {
        string endpoint = $"{baseUrl}/api/world/{worldId}";
        var request = UnityWebRequest.Get(endpoint);
        request.downloadHandler = new DownloadHandlerBuffer();

        if (!string.IsNullOrEmpty(JwtToken))
            request.SetRequestHeader("Authorization", $"Bearer {JwtToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string raw = request.downloadHandler.text;
            Debug.Log("GetWorld response: " + raw);

            try
            {
                var detail = JsonUtility.FromJson<WorldDataDetailResponse>(raw);
                if (detail != null && detail.worldData != null)
                {
                    OnWorldDataReceived(detail);
                }
                else
                {
                    Debug.LogError("Parsed detail is null or missing worldData.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed parse: {ex}");
            }
        }
        else
        {
            Debug.LogError($"GetWorld error: {request.error} | {request.downloadHandler.text}");
        }
    }

    public void CreateWorld(string name, int sizeX, int sizeY)
    {
        StartCoroutine(CreateWorldCoroutine(name, sizeX, sizeY));
    }

    private IEnumerator CreateWorldCoroutine(string name, int sizeX, int sizeY)
    {
        string endpoint = $"{baseUrl}/api/world";
        var dto = new CreateWorldDto { Name = name, SizeX = sizeX, SizeY = sizeY };
        string jsonBody = JsonUtility.ToJson(dto);

        var request = new UnityWebRequest(endpoint, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        if (!string.IsNullOrEmpty(JwtToken))
            request.SetRequestHeader("Authorization", $"Bearer {JwtToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string raw = request.downloadHandler.text;
            Debug.Log("CreateWorld response: " + raw);
            // e.g. returns { "worldId": 123 }
        }
        else
        {
            Debug.LogError($"CreateWorld error: {request.error} | {request.downloadHandler.text}");
        }
    }

    public void DeleteWorld(int worldId)
    {
        StartCoroutine(DeleteWorldCoroutine(worldId));
    }

    private IEnumerator DeleteWorldCoroutine(int worldId)
    {
        string endpoint = $"{baseUrl}/api/world/{worldId}";
        var request = UnityWebRequest.Delete(endpoint);
        request.downloadHandler = new DownloadHandlerBuffer();

        if (!string.IsNullOrEmpty(JwtToken))
            request.SetRequestHeader("Authorization", $"Bearer {JwtToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string raw = request.downloadHandler.text;
            Debug.Log("DeleteWorld response: " + raw);
        }
        else
        {
            Debug.LogError($"DeleteWorld error: {request.error} | {request.downloadHandler.text}");
        }
    }

    // -------------
    // Object Endpoints
    // -------------

    public void AddObjectToWorld(int worldId, string type, float x, float y, float rotation, float scale)
    {
        StartCoroutine(AddObjectToWorldCoroutine(worldId, type, x, y, rotation, scale));
    }

    private IEnumerator AddObjectToWorldCoroutine(int worldId, string type, float x, float y, float rotation, float scale)
    {
        string endpoint = $"{baseUrl}/api/world/{worldId}/objects";
        var dto = new CreateObjectDto {
            Type = type,
            PositionX = x,
            PositionY = y,
            Rotation = rotation,
            Scale = scale
        };
        string jsonBody = JsonUtility.ToJson(dto);

        var request = new UnityWebRequest(endpoint, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        if (!string.IsNullOrEmpty(JwtToken))
            request.SetRequestHeader("Authorization", $"Bearer {JwtToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string raw = request.downloadHandler.text;
            Debug.Log($"AddObjectToWorld response: {raw}");
            // e.g. { "objectId": 123 }
        }
        else
        {
            Debug.LogError($"AddObjectToWorld error: {request.error} | {request.downloadHandler.text}");
        }
    }

    public void GetObjectsInWorld(int worldId)
    {
        StartCoroutine(GetObjectsInWorldCoroutine(worldId));
    }

    private IEnumerator GetObjectsInWorldCoroutine(int worldId)
    {
        string endpoint = $"{baseUrl}/api/world/{worldId}/objects";
        var request = UnityWebRequest.Get(endpoint);
        request.downloadHandler = new DownloadHandlerBuffer();

        if (!string.IsNullOrEmpty(JwtToken))
            request.SetRequestHeader("Authorization", $"Bearer {JwtToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string raw = request.downloadHandler.text;
            Debug.Log($"GetObjectsInWorld response: {raw}");
        }
        else
        {
            Debug.LogError($"GetObjectsInWorld error: {request.error} | {request.downloadHandler.text}");
        }
    }

    public void UpdateObject(int worldId, int objectId, string type, float x, float y, float rotation, float scale)
    {
        StartCoroutine(UpdateObjectCoroutine(worldId, objectId, type, x, y, rotation, scale));
    }

    private IEnumerator UpdateObjectCoroutine(int worldId, int objectId, string type, float x, float y, float rotation, float scale)
    {
        string endpoint = $"{baseUrl}/api/world/{worldId}/objects/{objectId}";
        var dto = new CreateObjectDto {
            Type = type,
            PositionX = x,
            PositionY = y,
            Rotation = rotation,
            Scale = scale
        };
        string jsonBody = JsonUtility.ToJson(dto);

        var request = new UnityWebRequest(endpoint, "PUT");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        if (!string.IsNullOrEmpty(JwtToken))
            request.SetRequestHeader("Authorization", $"Bearer {JwtToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string raw = request.downloadHandler.text;
            Debug.Log($"UpdateObject response: {raw}");
        }
        else
        {
            Debug.LogError($"UpdateObject error: {request.error} | {request.downloadHandler.text}");
        }
    }

    public void DeleteObject(int worldId, int objectId)
    {
        StartCoroutine(DeleteObjectCoroutine(worldId, objectId));
    }

    private IEnumerator DeleteObjectCoroutine(int worldId, int objectId)
    {
        string endpoint = $"{baseUrl}/api/world/{worldId}/objects/{objectId}";
        var request = UnityWebRequest.Delete(endpoint);
        request.downloadHandler = new DownloadHandlerBuffer();

        if (!string.IsNullOrEmpty(JwtToken))
            request.SetRequestHeader("Authorization", $"Bearer {JwtToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string raw = request.downloadHandler.text;
            Debug.Log($"DeleteObject response: {raw}");
        }
        else
        {
            Debug.LogError($"DeleteObject error: {request.error} | {request.downloadHandler.text}");
        }
    }

    // When a world loads successfully, store the data in a static holder and load the "TemplateWorld" scene
    private void OnWorldDataReceived(WorldDataDetailResponse detail) {
        TempWorldDataHolder.WorldData = detail;        
        TempWorldDataHolder.CurrentWorldId = detail.worldData.id;  
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneBuildIndex: 2); 
    }
    
    
}

public static class TempWorldDataHolder {
    public static WorldApiManager.WorldDataDetailResponse WorldData;
    public static int CurrentWorldId; 
}
