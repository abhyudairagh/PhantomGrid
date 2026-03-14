using System;

namespace Phantom.Scripts
{
    public class Card : ICard
    {
        public event Action MatchCompleted;
        public event Action CardResetted;

        public int Id { get; }
        public int SpriteIndex { get; }

        public Card(int id, int spriteIndex)
        {
          Id = id;
          SpriteIndex = spriteIndex;
        }
    
        public bool Equals(ICard other)
        {
            return  other != null && Id == other.Id;
        }

        public void MatchComplete()
        {
            MatchCompleted?.Invoke();
        }

        public void ResetCard()
        {
            CardResetted?.Invoke();
        }
    }

    public interface ICard : IEquatable<ICard>
    {
        event Action MatchCompleted;
        event Action CardResetted;
        int Id { get; }
        int SpriteIndex { get; }
        void MatchComplete();
        void ResetCard();
    }
}