using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhantomGrid.ScriptableObjects
{
    public enum GameLevel
    {
        VeryEasy = 0,
        Easy = 1,
        Normal = 2,
        Hard = 3,
        Expert = 4
    }
    
    [Serializable]
    public struct LevelMap
    {
        public GameLevel level;
        public Vector2 gridSize;
    }
    
    [Serializable]
    public class GameSettings : IGameSettings
    {
        [SerializeField]
        private List<LevelMap> _levelMaps;

        public LevelMap GetLevel(GameLevel levelMap)
        {
            return _levelMaps.Find(x => x.level == levelMap);
        }
    }

    public interface IGameSettings
    {
        LevelMap GetLevel(GameLevel levelMap);
    }
}