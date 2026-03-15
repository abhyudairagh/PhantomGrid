using System;
using PhantomGrid.ScriptableObjects;

namespace Phantom.Scripts
{
    [Serializable]
    public struct GameStatus
    {
        public GameLevel GameLevel;
        public int TotalTurns { get; set; }
        public int Matches { get; set; }
        public int CurrentMatchStreak { get; set; }
        public int CurrentScore { get; set; }   
        public TimeSpan ElapsedTime { get; set; }
    }
}