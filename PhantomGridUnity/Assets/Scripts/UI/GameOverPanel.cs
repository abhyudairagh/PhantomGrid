using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhantomGrid
{
    public class GameOverPanel : MonoBehaviour, IGameOverPanel
    {
        public event Action RestartClicked;

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
        
        void Start()
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        private void OnRestartClicked()
        {
            RestartClicked?.Invoke();
        }

        public void SetScore(int score, int highScore, TimeSpan currentTime, TimeSpan bestTime)
        {
            currentScoreText.text = score.ToString();
            highScoreText.text = highScore.ToString();
            //currentTimeText.text = currentTime.ToString("hh':'mm':'ss");
            //bestTimeText.text = highScore.ToString("hh':'mm':'ss");
        }

    }

    public interface IGameOverPanel
    {
        event Action RestartClicked;
        void SetScore(int score, int  highScore, TimeSpan currentTime, TimeSpan bestTime);
    }
}
