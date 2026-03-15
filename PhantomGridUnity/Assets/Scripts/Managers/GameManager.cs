using System;
using System.Collections.Generic;
using System.Linq;
using Phantom.Scripts;
using PhantomGrid.Events;
using PhantomGrid.Extension;
using PhantomGrid.Factory;
using PhantomGrid.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace PhantomGrid.Managers
{
    public class GameManager : MonoBehaviour
    {
        private IUIManager _uiManager;
        private ICardSpriteRepository _cardImageRepository;
        private ICardFactory _cardFactory;
        private IEventBus _eventBus;

        private ICard _reservedToCheckPair;

        private int _totalCards = 0;
        
        private IGameSettings _gameSettings;
        private IScoreHandler _scoreHandler;
        private IGameSessionManager _gameSessionManager;
        private ISoundManager _soundManager;

        [Inject]
        public void Construct(
            IUIManager  uiManager,
            ICardSpriteRepository cardImageRepository,
            ICardFactory cardFactory,
            IEventBus eventBus,
            IGameSettings gameSettings,
            IScoreHandler scoreHandler,
            IGameSessionManager gameSessionManager,
            ISoundManager soundManager)
        {
            _uiManager = uiManager;
            _cardImageRepository = cardImageRepository;
            _cardFactory = cardFactory;
            _eventBus  = eventBus;
            _gameSettings = gameSettings;
            _scoreHandler = scoreHandler;
            _gameSessionManager = gameSessionManager;
            _soundManager = soundManager;
        }
        
        private void Start()
        {
            _eventBus.RegisterEvent<CardFlippedEvent>(OnCardFlipped);
            _eventBus.RegisterEvent<StartGameEvent>(OnStartGame);
            _eventBus.RegisterEvent<ResumeGameEvent>(OnResumeGame);
            _eventBus.RegisterEvent<CloseGameEvent>(OnCloseGame);
        }

        private void OnCloseGame(CloseGameEvent closeGame)
        {
            _gameSessionManager.Reset();
            _scoreHandler.Reset();
        }

        private void OnResumeGame(ResumeGameEvent payload)
        {
            if (payload.IsResumeSelected)
            {
                SetUpResumeGame();
            }
            else
            {
                _gameSessionManager.Reset();
            }
        }

        private void SetUpResumeGame()
        {
            var gameSaveData = _gameSessionManager.LoadData();
            _uiManager.LoadCardSprites();
            SetUpGame(gameSaveData.Rows, gameSaveData.Columns, gameSaveData.GameStatus.GameLevel, gameSaveData.Cards);
            _scoreHandler.LoadSavedGameStatus(gameSaveData.GameStatus);
            _uiManager.ShowGameStatusUpdates(_scoreHandler.CurrentGameStatus);
            _uiManager.ShowGameScreen(gameSaveData.GameStatus.GameLevel);
        }

        private void OnStartGame(StartGameEvent payload)
        {
             var selectedGameLevel =  payload.SelectedLevel;
             var gameLevel = _gameSettings.GetLevel(selectedGameLevel);
             
             var rows = (int)gameLevel.gridSize.x;
             var columns = (int)gameLevel.gridSize.y;
            
             
             _uiManager.LoadCardSprites();
            var cardsData = CreateCardsData(rows, columns);
            
            SetUpGame(rows, columns, selectedGameLevel, cardsData);
            _gameSessionManager.Initialize(rows, columns, cardsData.Cast<Card>());
            _uiManager.ShowGameScreen(selectedGameLevel);
        }

        private void SetUpGame(int rows, int columns, GameLevel  gameLevel, IEnumerable<ICard> cardsData)
        {
            ResetScores();
            _totalCards = cardsData.Where(card => !card.IsMatchComplete).Count();
            _uiManager.GenerateCards(rows, columns, cardsData);
            _scoreHandler.SetGameLevel(gameLevel);
        }

        private void ResetScores()
        {
            _scoreHandler.Reset();
            _uiManager.ShowGameStatusUpdates(_scoreHandler.CurrentGameStatus);
        }

        private void OnCardFlipped(CardFlippedEvent cardFlipEvent)
        {
            _soundManager.PlaySound(SoundType.CardFlip);
            
            ValidateCard(cardFlipEvent.Card);
            _uiManager.ShowGameStatusUpdates(_scoreHandler.CurrentGameStatus);

            if (_totalCards <= 0)
            {
                SetGameOver();
            }
        }

        private void SetGameOver()
        {
            _scoreHandler.SaveHighScore();
            _uiManager.ShowGameOver();
            _gameSessionManager.Reset();
            _soundManager.PlaySound(SoundType.GameOver);
        }

        private void ValidateCard(ICard card)
        {
            if (_reservedToCheckPair == null)
            {
                _reservedToCheckPair = card;
                return;
            }

            if (_reservedToCheckPair.Equals(card))
            {
                HandleMatchFound(card);
            }
            else
            {
                HandleMatchFindFailed(card);
            }
            _gameSessionManager.SaveData(_scoreHandler.CurrentGameStatus);
            _reservedToCheckPair = null;
        }

        private void HandleMatchFindFailed(ICard card)
        {
            _reservedToCheckPair.Reset();
            card.Reset();
            _scoreHandler.MatchFailed();
            _soundManager.PlaySound(SoundType.MatchFail);
        }

        private void HandleMatchFound(ICard card)
        {
            _reservedToCheckPair.MatchComplete();
            card.MatchComplete();
            _scoreHandler.MatchFound();
            _totalCards -= 2;
            
            _soundManager.PlaySound(SoundType.MatchSuccess);
        }

        private IEnumerable<ICard> CreateCardsData(int rows, int columns)
        {
            var individualCardCount = (rows * columns) / 2;
            return _cardFactory.CreatePairs(individualCardCount, _cardImageRepository.TotalCardSpriteCount)
                .Shuffle();
        }

        private void OnDestroy()
        {
            _eventBus.UnregisterEvent<CardFlippedEvent>(OnCardFlipped);
            _eventBus.UnregisterEvent<StartGameEvent>(OnStartGame);
            _eventBus.UnregisterEvent<ResumeGameEvent>(OnResumeGame);
            _eventBus.UnregisterEvent<CloseGameEvent>(OnCloseGame);
        }
    }

}