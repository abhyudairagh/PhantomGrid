using System.Collections.Generic;
using Phantom.Scripts;
using PhantomGrid.Events;
using PhantomGrid.Extension;
using PhantomGrid.Factory;
using UnityEngine;
using Zenject;

namespace PhantomGrid.Managers
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        
        private IUIManager _uiManager;
        private ICardSpriteRepository _cardImageRepository;
        private ICardFactory _cardFactory;
        private IEventBus _eventBus;

        private ICard _reservedToCheckPair;

        private int _totalCards = 0;
        
        [Inject]
        public void Construct(
            IUIManager  uiManager,
            ICardSpriteRepository cardImageRepository,
            ICardFactory cardFactory,
            IEventBus eventBus)
        {
            _uiManager = uiManager;
            _cardImageRepository = cardImageRepository;
            _cardFactory = cardFactory;
            _eventBus  = eventBus;
        }
        
        private void Start()
        {
            _totalCards = _rows  * _columns;
            _uiManager.LoadCardSprites();
            var cardsData = CreateCardsData();
            _uiManager.GenerateCards(_rows, _columns, cardsData);
            
            _eventBus.RegisterEvent<CardFlippedEvent>(OnCardFlipped);
        }

        private void OnCardFlipped(CardFlippedEvent cardFlipEvent)
        {
            ValidateCard(cardFlipEvent.Card);
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
                _reservedToCheckPair.MatchComplete();
                card.MatchComplete();
            }
            else
            {
                _reservedToCheckPair.ResetCard();
                card.ResetCard();
            }

            _reservedToCheckPair = null;
        }

        private IEnumerable<ICard> CreateCardsData()
        {
            var individualCardCount = (_rows * _columns) / 2;
            return _cardFactory.CreatePairs(individualCardCount, _cardImageRepository.TotalCardSpriteCount)
                .Shuffle();
        }

        private void OnDestroy()
        {
            _eventBus.UnregisterEvent<CardFlippedEvent>(OnCardFlipped);
        }
    }

    public interface IGameManager
    {
    }
}