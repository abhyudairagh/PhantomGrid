using System;
using System.Collections.Generic;
using System.Linq;
using Phantom.Scripts;
using UnityEngine;
using Zenject;

namespace PhantomGrid
{
    public class CustomGrid : MonoBehaviour
    {
        [SerializeField] private float leastBorderPadding = 0;
        
        [SerializeField] private RectTransform _frameRectTransform;
        
        [SerializeField] private CardUI _cardPrefab;

        private IEnumerable<ICard> _cardsData;

        private ICardSpriteRepository _cardSpriteRepository;
        private IInstantiator  _instantiator;

        [Inject]
        public void Construct(ICardSpriteRepository cardSpriteRepository, IInstantiator instantiator)
        {
            _cardSpriteRepository  = cardSpriteRepository;
            _instantiator = instantiator;

        }
        
        [ContextMenu("GenerateCards")]
        public void Generate(int rows, int columns, IEnumerable<ICard> cards)
        {
            if (cards == null || cards.Count() < rows * columns)
            {
                throw new Exception("Data  can not be null or less than " + rows * columns);
            }

            _cardsData = cards;
            foreach (var componentInChild in _frameRectTransform.GetComponentsInChildren<CardUI>())
            {
                DestroyImmediate(componentInChild.gameObject);
            }
            
            var totalWidth = _frameRectTransform.rect.width;
            var totalHeight = _frameRectTransform.rect.height;
            var cardSize = _cardPrefab.rectTransform.rect.size;
            //starting from bottom
            
            var startPosX = totalWidth / rows ;
            var startPosY = totalHeight / columns; 

            var initCardLeftX = startPosX - cardSize.x / 2f;
            var initCardBottomY = startPosY  - cardSize.y / 2f;
            //border overlap size update
            
           var borderOverLapXOffset = initCardLeftX < leastBorderPadding ? leastBorderPadding + initCardLeftX : 0f;
           var borderOverLapYOffset = initCardBottomY < leastBorderPadding ? leastBorderPadding + initCardBottomY : 0f;
           
           cardSize =  new Vector2(cardSize.x - borderOverLapXOffset,
               cardSize.y - borderOverLapYOffset);;
           
           //Set card overlapping size update
           
           var initCardRightX = startPosX +cardSize.x / 2f;
           var initCardTopY = startPosY +cardSize.y / 2f;
           
           var neighbourCardLeftX = (startPosX * 2) - cardSize.x / 2f;
           var neighbourCardBottomY =  (startPosY * 2) - cardSize.y / 2f;
           
           var cardOverLapXOffset = neighbourCardLeftX < initCardRightX + leastBorderPadding ? (initCardRightX - neighbourCardLeftX + leastBorderPadding) : 0f;
           var cardOverLapYOffset = neighbourCardBottomY < initCardTopY + leastBorderPadding ? (initCardTopY -  neighbourCardBottomY + leastBorderPadding) : 0f;
           
           cardSize = new Vector2(cardSize.x - cardOverLapXOffset,
               cardSize.y - cardOverLapYOffset);


           InstantiateAllCards(rows, columns, startPosX, startPosY, cardSize);
        }

        private void InstantiateAllCards(int rows, int columns, float startPosX, float startPosY, Vector2 cardSize)
        {
            var dataIndex = 0;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var cardData = _cardsData.ElementAt(dataIndex);
                    var position = FixOffsetPosition( rows, columns, (i + 1) * startPosX,  (j + 1) * startPosY);
                    var cardUI = _instantiator.InstantiatePrefabForComponent<ICardUI>(_cardPrefab, _frameRectTransform);
                    cardUI.SetPosition(position);
                    cardUI.SetSize(cardSize.x, cardSize.y);
                    cardUI.SetCard(cardData);
                    var sprite = _cardSpriteRepository.GetSpriteFromIndex(cardData.SpriteIndex);
                    cardUI.SetImage(sprite);
                    dataIndex++;
                }
            }
        }

        private Vector2 FixOffsetPosition(int rows, int columns, float xPosition, float yPosition)
        {
            var width = _frameRectTransform.rect.width;
            var height = _frameRectTransform.rect.height;
            return new Vector2(xPosition - ((width/rows)/2f), yPosition - ((height/columns)/2f));
        }
    }
}