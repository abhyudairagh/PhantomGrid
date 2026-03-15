using System;
using PhantomGrid.ScriptableObjects;
using PhantomGrid.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhantomGrid
{
    public class GameStatusPanel : MonoBehaviour, IGameStatusPanel
    {
        public event Action CloseGameConfirmed
        {
            add => endGameConfirmPanel.CloseGameConfirmed  += value;
            remove => endGameConfirmPanel.CloseGameConfirmed -= value;
        }
        
        [SerializeField]
        private TextMeshProUGUI totalTurnsText;
        [SerializeField]
        private TextMeshProUGUI matchesText;
        [SerializeField]
        private TextMeshProUGUI levelText;
        [SerializeField]
        private TextMeshProUGUI scoreText;
        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private EndGameConfirmPanel  endGameConfirmPanel;
        
        private void Start()
        {
            endGameConfirmPanel.gameObject.SetActive(false);
            closeButton.onClick.AddListener(ShowConfirmation);
        }

        private void ShowConfirmation()
        {
            endGameConfirmPanel.gameObject.SetActive(true);
        }

        public void SetLevel(GameLevel level)
        {
            levelText.text = level.ToString();
        }

        public void SetTotalTurns(int totalTurns)
        {
            totalTurnsText.text = totalTurns.ToString();
        }

        public void SetMatches(int matches)
        {
            matchesText.text = matches.ToString();
        }
        
        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveAllListeners();
        }
    }

    public interface IGameStatusPanel
    {
        void SetLevel(GameLevel level);
        void SetTotalTurns(int totalTurns);
        void SetMatches(int matches);
    }
}
