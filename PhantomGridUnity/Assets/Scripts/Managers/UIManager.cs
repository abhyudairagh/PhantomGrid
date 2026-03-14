using System.Collections.Generic;
using Phantom.Scripts;
using PhantomGrid.Events;
using UnityEngine;
using UnityEngine.U2D;
using Zenject;

namespace PhantomGrid.Managers
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField]
        private SpriteAtlas _spriteAtlas;
        [SerializeField]
        private CustomGrid _grid;
        
        private ICardSpriteRepository _cardImageRepository;
       
        
        [Inject]
        public void Construct(ICardSpriteRepository cardImageRepository)
        {
            _cardImageRepository = cardImageRepository;
        }

        public void LoadCardSprites()
        {
            _cardImageRepository.SetUpRepository(_spriteAtlas);
        }

        public void GenerateCards(int rows, int columns, IEnumerable<ICard> cards)
        {
            _grid.Generate(rows, columns, cards);
        }
    }

    public interface IUIManager
    {
        void GenerateCards(int rows, int columns, IEnumerable<ICard> cards);
        void LoadCardSprites();
    }
}