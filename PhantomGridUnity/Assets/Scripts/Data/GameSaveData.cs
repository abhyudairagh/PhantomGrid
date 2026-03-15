using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Phantom.Scripts
{
    [Serializable]
    public class GameSaveData
    {
        [JsonProperty]
        public GameStatus GameStatus {get; private set;}
        [JsonProperty]
        public int Rows {get ; private set;}
        [JsonProperty]
        public int Columns {get ; private set;}

        public IEnumerable<Card> Cards => _cards;

        [JsonProperty] private IEnumerable<Card> _cards;
        
        public GameSaveData(int rows, int columns, IEnumerable<Card> cards)
        {
            Rows = rows;
            Columns = columns;
            _cards = cards;
        }

        public void SetGameStatus(GameStatus gameStatus)
        {
            GameStatus = gameStatus;
        }
    }
}