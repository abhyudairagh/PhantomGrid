using System;
using Newtonsoft.Json;
using PhantomGrid.Common;

namespace Phantom.Scripts
{
    [Serializable]
    public class Card : ICard
    {
        public event Action MatchCompleted;
        public event Action CardResetted;

        public int Id { get; }
        public int SpriteIndex { get; }
        
        [JsonProperty]
        public bool IsMatchComplete { get; private set; } 

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
            IsMatchComplete  = true;
            MatchCompleted?.Invoke();
        }

        public void Reset()
        {
            CardResetted?.Invoke();
        }
    }

    public interface ICard : IEquatable<ICard>, IResettable
    {
        event Action MatchCompleted;
        event Action CardResetted;
        int Id { get; }
        int SpriteIndex { get; }
        bool IsMatchComplete { get; }
        void MatchComplete();
    }
}