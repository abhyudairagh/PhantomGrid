using UnityEngine;
using UnityEngine.U2D;

namespace PhantomGrid
{
    public class CardSpriteRepository : ICardSpriteRepository
    {
        private SpriteAtlas _spriteAtlas;
        private Sprite[] _cardSprites;
        
        public int TotalCardSpriteCount => _cardSprites.Length;
        
        public void SetUpRepository(SpriteAtlas spriteAtlas)
        {
            _spriteAtlas = spriteAtlas;
            _cardSprites =  new Sprite[_spriteAtlas.spriteCount];
            spriteAtlas.GetSprites(_cardSprites);
        }

        public Sprite GetSpriteFromIndex(int index)
        {
            return _cardSprites[index];
        }
    }

    public interface ICardSpriteRepository
    {
        int TotalCardSpriteCount { get; }
        
        void SetUpRepository(SpriteAtlas spriteAtlas);

        Sprite GetSpriteFromIndex(int index);
    }
}