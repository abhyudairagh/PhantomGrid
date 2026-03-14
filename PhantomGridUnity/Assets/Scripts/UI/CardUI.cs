using System;
using UnityEngine;
using UnityEngine.UI;

namespace Phantom.Scripts
{
    public class CardUI : MonoBehaviour, ICardUI
    {
        private int FlipTrigger = Animator.StringToHash("Flip");
        
        public event Action<ICard> OnCardOpened;
        public RectTransform rectTransform =>  _cardRect;

        [SerializeField] private GameObject _frontCard;
        [SerializeField] private GameObject _backCard;
        
        [SerializeField]
        private Image _cardImage;
        
        [SerializeField]
        private Button _cardButton;
        
        [SerializeField]
        private RectTransform _cardRect;
        
        [SerializeField]
        private Animator _cardAnimator;
        
        private ICard _card;

        private void Start()
        {
            _cardButton.onClick.AddListener(OnCardClicked);
        }

        public void SetCard(ICard card)
        {
            _card = card;
        }
        
        public void SetPosition(Vector2 position)
        {
            _cardRect.anchoredPosition = position;
        }

        public void SetSize(float width, float height)
        {
            _cardRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            _cardRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        }

        private void OnCardClicked()
        {
            _cardAnimator.SetTrigger(FlipTrigger);
        }
        
        #region AnimationEvents

        public void SwitchCardFace()
        {
            var showFrontCard = _backCard.activeSelf;
            _frontCard.SetActive(showFrontCard);
            _backCard.SetActive(!showFrontCard);
        }
        
        public void CardFlipped()
        {
            if (_frontCard.activeSelf)
            {
                OnCardOpened?.Invoke(_card); 
                _cardButton.interactable = false;
            }
            else
            {
                _cardButton.interactable = true;
            }
        }

        #endregion

        public void ResetCard()
        {
            _cardAnimator.SetTrigger(FlipTrigger);
        }
        
        private void OnDestroy()
        {
            _cardButton.onClick.RemoveAllListeners();
        }
    }

    public interface ICardUI
    {
        event Action<ICard> OnCardOpened;
        RectTransform rectTransform { get; }
        void SetPosition(Vector2  position);
        void SetSize(float width, float height);
        void SetCard(ICard card);
        void ResetCard();
        
    }
}