using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhantomGrid
{
    public class GameOverPanel : MonoBehaviour, IGameOverPanel
    {
        public event Action RestartClicked;
        public event Action ExitGameClicked;
        [SerializeField]
        private TextMeshProUGUI currentScoreText;
        [SerializeField]
        private TextMeshProUGUI highScoreText;
        [SerializeField]
        private TextMeshProUGUI currentTimeText;
        [SerializeField]
        private TextMeshProUGUI bestTimeText;
        [SerializeField]
        private Button restartButton;
        [SerializeField]
        private Button exitGameButton;
        
        void Start()
        {
            restartButton.onClick.AddListener(OnRestartClicked);
            exitGameButton.onClick.AddListener(OnExitGame);
        }

        private void OnExitGame()
        {
            ExitGameClicked?.Invoke();
        }

        private void OnRestartClicked()
        {
            RestartClicked?.Invoke();
        }

        public void SetScore(int score, int highScore)
        {
            currentScoreText.text = score.ToString();
            highScoreText.text = highScore.ToString();
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveAllListeners();
            exitGameButton.onClick.RemoveAllListeners();
        }
    }

    public interface IGameOverPanel
    {
        event Action RestartClicked;
        event Action ExitGameClicked;
        void SetScore(int score, int  highScore);
    }
}
