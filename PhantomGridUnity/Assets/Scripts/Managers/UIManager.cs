using System;
using System.Collections.Generic;
using Phantom.Scripts;
using PhantomGrid.Events;
using PhantomGrid.ScriptableObjects;
using PhantomGrid.UI;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Zenject;

namespace PhantomGrid.Managers
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField]
        private GameObject _gamePanel;
        
        [Header("StartGamePanel")]
        [SerializeField]
        private StartGamePanel _startGamePanel;
        [Header("Game panel")]
        [SerializeField]
        private GameStatusPanel _gameStatusPanel;
        [Header("Gameover Panel")]
        [SerializeField]
        private GameOverPanel _gameOverPanel;
       
        
        [Space]
        [SerializeField]
        private SpriteAtlas _spriteAtlas;
        [SerializeField]
        private CustomGrid _grid;
        
        private ICardSpriteRepository _cardImageRepository;
        private IEventBus _eventBus;
        private IScoreHandler _scoreHandler;
        
        [Inject]
        public void Construct(
            ICardSpriteRepository cardImageRepository,
            IEventBus eventBus,
            IScoreHandler scoreHandler)
        {
            _cardImageRepository = cardImageRepository;
            _eventBus  = eventBus;
            _scoreHandler = scoreHandler;
        }

        private void Start()
        {
            _gameOverPanel.RestartClicked += OnRestartGamePressed;
            _startGamePanel.StartGameClicked  += OnStartGamePressed;
            _startGamePanel.ResumeGame += OnResumeGamePressed;
            _gameStatusPanel.CloseGameConfirmed += CloseGame;
            SetActiveStartGamePanel(true);
        }

        private void CloseGame()
        {
            _eventBus.FireEvent(new CloseGameEvent());
            SetActiveStartGamePanel(true);
        }

        private void OnResumeGamePressed(bool isResumeSelected)
        {
            _eventBus.FireEvent(new ResumeGameEvent(isResumeSelected));
        }

        private void OnRestartGamePressed()
        {
            StartGameWithLevel(_scoreHandler.CurrentGameStatus.GameLevel);
        }

        private void OnStartGamePressed(GameLevel level)
        {
            StartGameWithLevel(level);
        }

        private void StartGameWithLevel(GameLevel level)
        {
            _eventBus.FireEvent(new StartGameEvent(level));
        }

        public void ShowGameScreen(GameLevel level)
        {
            _gameStatusPanel.SetLevel(level);
            SetActiveGameScreen(true);
        }

        private void SetActiveGameScreen(bool enable)
        {
            _gameOverPanel.gameObject.SetActive(false);
            _startGamePanel.gameObject.SetActive(false);
            _gamePanel.SetActive(enable);
        }
        
        private void SetActiveStartGamePanel(bool enable)
        {
            _startGamePanel.gameObject.SetActive(enable);
            _gamePanel.SetActive(false);
        }

        public void LoadCardSprites()
        {
            _cardImageRepository.SetUpRepository(_spriteAtlas);
        }

        public void ShowGameStatusUpdates(GameStatus status)
        {
            _gameStatusPanel.SetMatches(status.Matches);
            _gameStatusPanel.SetTotalTurns(status.TotalTurns);
            _gameStatusPanel.SetScore(status.CurrentScore);
        }

        public void ShowGameOver()
        {
            _gameOverPanel.gameObject.SetActive(true);

            _gameOverPanel.SetScore(_scoreHandler.CurrentGameStatus.CurrentScore, _scoreHandler.GetHighScore(), 
                new TimeSpan(),
                new TimeSpan());
        }

        public void GenerateCards(int rows, int columns, IEnumerable<ICard> cards)
        {
            _grid.Generate(rows, columns, cards);
        }

        private void OnDestroy()
        {
            _startGamePanel.StartGameClicked  -= OnStartGamePressed;
            _gameOverPanel.RestartClicked -= OnRestartGamePressed;
            _startGamePanel.ResumeGame -= OnResumeGamePressed;
            _gameStatusPanel.CloseGameConfirmed -= CloseGame;
        }
    }

    public interface IUIManager
    {
        void ShowGameScreen(GameLevel level);
        void GenerateCards(int rows, int columns, IEnumerable<ICard> cards);
        void LoadCardSprites();
        void ShowGameStatusUpdates(GameStatus status);
        void ShowGameOver();
    }
}