using System;
using UnityEngine;

public static class JsonHelper {
    [Serializable]
    private class Wrapper<T> {
        public T[] Items;
    }

    public static T[] FromJsonArray<T>(string json) {
        
        string newJson = "{\"Items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.Items;
    }

    
    public static string ToJsonArray<T>(T[] array) {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }
}