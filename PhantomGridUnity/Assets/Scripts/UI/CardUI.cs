using System;
using PhantomGrid.Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Phantom.Scripts
{
    public class CardUI : MonoBehaviour, ICardUI
    {
        private int FlipTrigger = Animator.StringToHash("Flip");
        private int HideTrigger = Animator.StringToHash("Hide");

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

        private IEventBus _eventBus;
        
        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus =  eventBus;
        }
        
        private void Start()
        {
            _cardButton.onClick.AddListener(OnCardClicked);
            _frontCard.SetActive(false);
            _backCard.SetActive(true);
        }

        public void SetCard(ICard card)
        {
            _card = card;
            _card.MatchCompleted += OnMatchCompleted;
            _card.CardResetted += ResetCard;
        }

        private void OnMatchCompleted()
        {
            _cardAnimator.SetTrigger(HideTrigger);
            _cardButton.interactable = false;
        }

        public void SetImage(Sprite sprite)
        {
            _cardImage.sprite = sprite;
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

        public void OnSwitchCardFace()
        {
            var showFrontCard = _backCard.activeSelf;
            _frontCard.SetActive(showFrontCard);
            _backCard.SetActive(!showFrontCard);
        }
        
        public void OnCardFlipped()
        {
            if (_frontCard.activeSelf)
            {
                _cardButton.interactable = false;
                _eventBus.FireEvent(new CardFlippedEvent(_card));
            }
            else
            {
                _cardButton.interactable = true;
            }
        }

        public void OnCardHide()
        {
            gameObject.SetActive(false);
        }

        #endregion

        public void ResetCard()
        {
            _cardAnimator.SetTrigger(FlipTrigger);
        }
        
        private void OnDestroy()
        {
            if (_card != null)
            {
                _card.MatchCompleted -= OnMatchCompleted;
                _card.CardResetted -= ResetCard;
            }
            _cardButton.onClick.RemoveAllListeners();
        }
    }

    public interface ICardUI
    {
        RectTransform rectTransform { get; }
        void SetPosition(Vector2  position);
        void SetSize(float width, float height);
        void SetCard(ICard card);
        void SetImage(Sprite sprite);
        void ResetCard();
        
    }
}