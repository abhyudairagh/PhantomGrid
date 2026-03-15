using System;
using UnityEngine;
using UnityEngine.UI;

namespace PhantomGrid.UI
{
    public class EndGameConfirmPanel : MonoBehaviour
    {
        public event Action CloseGameConfirmed;
        
        [SerializeField]
        private Button _yesButton;
        [SerializeField]
        private Button _noButton;

        private void Start()
        {
            _noButton.onClick.AddListener(()=>
            {
                gameObject.SetActive(false);
            });
            _yesButton.onClick.AddListener(()=>
            {
                gameObject.SetActive(false);
                CloseGameConfirmed?.Invoke();
            });
        }

        private void OnDestroy()
        {
            _noButton.onClick.RemoveAllListeners();
            _yesButton.onClick.RemoveAllListeners();
        }
    }
}