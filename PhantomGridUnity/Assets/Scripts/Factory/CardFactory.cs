using System;
using System.Collections.Generic;
using Phantom.Scripts;
using Zenject;
using Random = UnityEngine.Random;

namespace PhantomGrid.Factory
{
    public class CardFactory : ICardFactory
    {
        private readonly IInstantiator _instantiator;

        public CardFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        public IEnumerable<ICard> CreatePairs(int count, int spritesCount)
        {
            var cards = new List<ICard>();
            var spritesIndexes = new List<int>();

            for (var i = 0; i < spritesCount; i++)
            {
                spritesIndexes.Add(i);
            }
                
            for(var i = 0; i < count; i++)
            {
                var randomIndex = Random.Range(0, spritesIndexes.Count);
                var spriteIndex = spritesIndexes[randomIndex];
                var id = i + 1;
               var original =  CreateIndividualCard(id, spriteIndex);
               var pair = CreateIndividualCard(id, spriteIndex);
               cards.Add(original);
               cards.Add(pair);
               spritesIndexes.RemoveAt(randomIndex);
            }
            return cards;
        }

        private ICard CreateIndividualCard(int id, int spriteIndex)
        {
            var card = _instantiator.Instantiate<Card>(new List<object> { id, spriteIndex});
            return card;
        }
    }

    public interface ICardFactory
    {
        IEnumerable<ICard> CreatePairs(int count, int spritesCount);
    }
}