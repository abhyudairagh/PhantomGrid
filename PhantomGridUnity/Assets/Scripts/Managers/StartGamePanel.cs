using System;
using PhantomGrid.ScriptableObjects;
using PhantomGrid.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PhantomGrid.Managers
{
    public class StartGamePanel : MonoBehaviour
    {
        public event Action<GameLevel> StartGameClicked;
        
        public event Action<bool> ResumeGame
        {
            add => _resumeGamePanel.ResumeGameResponse += value;
            remove => _resumeGamePanel.ResumeGameResponse -= value;
        }
        
        
        [SerializeField]
        private ToggleGroup _levelToggleGroup;
        [SerializeField]
        private Button _startButton;
        
        [SerializeField] private ResumeGamePanel _resumeGamePanel;
        private IGameSessionManager _gameSessionManager;
        
        [Inject]
        public void Construct(IGameSessionManager gameSessionManager)
        {
            _gameSessionManager = gameSessionManager;
        }
        
        private void Start()
        {
            _startButton.onClick.AddListener(OnStartGamePressed);
            _resumeGamePanel.ResumeGameResponse += OnGameResumeResponse;
            CheckResumption();
        }

        private void CheckResumption()
        {
            _resumeGamePanel.gameObject.SetActive(false);
            if (_gameSessionManager.HasResumption)
            {
                _resumeGamePanel.gameObject.SetActive(true);
            }
        }

        private void OnGameResumeResponse(bool canResume)
        {
            _resumeGamePanel.gameObject.SetActive(false);
        }

        private void OnStartGamePressed()
        {
            var levelIndex = _levelToggleGroup.GetFirstActiveToggle().transform.GetSiblingIndex();
            StartGameClicked?.Invoke((GameLevel)levelIndex);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveAllListeners();
            _resumeGamePanel.ResumeGameResponse -= OnGameResumeResponse;

        }
    }
}