using PhantomGrid.ScriptableObjects;
using UnityEngine;
using Newtonsoft.Json;

namespace PhantomGrid
{
    public class PersistantDataModel : IPersistantDataModel
    {
        private const string HIGH_SCORE_KEY = "HighScore";
        private const string SAVE_DATA_KEY = "SaveData";

        public bool ContainData<T>()
        {
             return  PlayerPrefs.HasKey(GenerateKey(typeof(T).Name, SAVE_DATA_KEY));
        }

        public void SaveHighScore(GameLevel level, int score)
        {
            var key = GenerateKey(level.ToString(), HIGH_SCORE_KEY);
            PlayerPrefs.SetInt(key, score);
        }

        public int LoadHighScore(GameLevel level)
        {
            var key = GenerateKey(level.ToString(), HIGH_SCORE_KEY);

            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            return 0;
        }

        public void SaveData<T>(T data)
        {
            var json = JsonConvert.SerializeObject(data);
            var key  = GenerateKey(typeof(T).Name, SAVE_DATA_KEY);
            PlayerPrefs.SetString(key, json);
        }

        public T LoadData<T>()
        {
            var key  = GenerateKey(typeof(T).Name, SAVE_DATA_KEY);
            if (PlayerPrefs.HasKey(key))
            {
                return JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(key));
            }

            return default;
        }

        public void DeleteData<T>()
        {
            var key  = GenerateKey(typeof(T).Name, SAVE_DATA_KEY);
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }


        private string GenerateKey(params  string[] keys)
        {
            var strKey = "";
            foreach (var key in keys)
            {
                strKey += key;
            }
            return strKey;
        }
        
        
    }

    public interface IPersistantDataModel
    {
        bool ContainData<T>();
        void SaveHighScore(GameLevel level, int score);
        int LoadHighScore(GameLevel level);
        void SaveData<T>(T data);
        T LoadData<T>();
        void DeleteData<T>();
    }
}
