using System;
using UnityEngine;
using UnityEngine.UI;

namespace PhantomGrid.UI
{
    public class ResumeGamePanel : MonoBehaviour
    {
        public event Action<bool> ResumeGameResponse;

        [SerializeField]
        private Button _yesButton;
        [SerializeField]
        private Button _noButton;

        private void Start()
        {
            _yesButton.onClick.AddListener(OnYesButtonClicked);
            _noButton.onClick.AddListener(OnNoButtonClicked);
        }

        private void OnNoButtonClicked()
        {
            ResumeGameResponse?.Invoke(false);
        }

        private void OnYesButtonClicked()
        {
            ResumeGameResponse?.Invoke(true);
        }

        private void OnDestroy()
        {
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
        }
    }
}