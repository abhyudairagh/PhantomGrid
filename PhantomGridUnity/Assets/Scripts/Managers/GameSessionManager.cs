using System.Collections.Generic;
using Phantom.Scripts;
using PhantomGrid.Common;

namespace PhantomGrid.Managers
{
    public class GameSessionManager : IGameSessionManager
    {
        private GameSaveData  _gameSaveData;
        private readonly IPersistantDataModel _persistantDataModel;

        public GameSessionManager(IPersistantDataModel  persistantDataModel)
        {
            _persistantDataModel =  persistantDataModel;
        }

        public bool HasResumption => _persistantDataModel.ContainData<GameSaveData>();

        public void Initialize(int rows, int columns, IEnumerable<Card> cards)
        {
            _gameSaveData = new  GameSaveData(rows, columns, cards);
        }

        public void SaveData(GameStatus gameStatus)
        {
            _gameSaveData.SetGameStatus(gameStatus);
            _persistantDataModel.SaveData(_gameSaveData);
        }

        public GameSaveData LoadData()
        {
            if (HasResumption)
            {
                _gameSaveData = _persistantDataModel.LoadData<GameSaveData>();
            }

            return _gameSaveData;
        }

        public void Reset()
        {
            _persistantDataModel.DeleteData<GameSaveData>();
        }
    }

    public interface IGameSessionManager : IResettable
    {
        bool HasResumption { get; }
        void Initialize(int rows, int columns, IEnumerable<Card> cards);
        void SaveData(GameStatus gameStatus);
        GameSaveData LoadData();

    }
}