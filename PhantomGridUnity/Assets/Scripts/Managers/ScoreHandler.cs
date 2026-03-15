using Phantom.Scripts;
using PhantomGrid.Common;
using PhantomGrid.ScriptableObjects;

namespace PhantomGrid.Managers
{
    public class ScoreHandler : IScoreHandler
    {
        public GameStatus CurrentGameStatus =>  _currentGameStatus;
        
        private GameStatus _currentGameStatus;
        private readonly IPersistantDataModel _persistantDataModel;

        public ScoreHandler(IPersistantDataModel persistantDataModel)
        {
            _persistantDataModel =  persistantDataModel;
        }

        public void SetGameLevel(GameLevel gameLevel)
        {
            _currentGameStatus.GameLevel = gameLevel;
        }
        
        public void MatchFound()
        {
            AddTurn();
            AddMatched();
            CalculateScore();
        }

        public void MatchFailed()
        {
            AddTurn();
            ResetStreak();
        }

        private void AddMatched()
        {
            _currentGameStatus.Matches++;
            AddStreak();
        }

        private void AddTurn()
        {
            _currentGameStatus.TotalTurns++;
        }

        private void AddStreak()
        {
            _currentGameStatus.CurrentMatchStreak++;
        }

        private void ResetStreak()
        {
            _currentGameStatus.CurrentMatchStreak = 1;
        }

        private void CalculateScore()
        {
            _currentGameStatus.CurrentScore += _currentGameStatus.CurrentMatchStreak;
        }

        public void SaveHighScore()
        {
            var highScore = GetHighScore();
            if (CurrentGameStatus.CurrentScore > highScore)
            {
                _persistantDataModel.SaveHighScore(_currentGameStatus.GameLevel, CurrentGameStatus.CurrentScore);
            }
        }

        public int GetHighScore()
        {
            return _persistantDataModel.LoadHighScore(_currentGameStatus.GameLevel);
        }

        public void LoadSavedGameStatus(GameStatus gameStatus)
        {
            _currentGameStatus = gameStatus;
        }

        public void Reset()
        {
            _currentGameStatus =  new GameStatus();
        }
    }

    public interface IScoreHandler : IResettable
    {
        GameStatus CurrentGameStatus { get; }
        void SetGameLevel(GameLevel gameLevel);
        void MatchFound();
        void MatchFailed();
        void SaveHighScore();
        int GetHighScore();
        void LoadSavedGameStatus(GameStatus saveData);
    }
}