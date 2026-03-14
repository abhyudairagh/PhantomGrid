using Phantom.Scripts;

namespace PhantomGrid.Events
{
    public class CardFlippedEvent : EventPayload
    {
        public ICard Card { get; }
        
        public CardFlippedEvent(ICard card)
        {
            Card = card;
        }
    }
}