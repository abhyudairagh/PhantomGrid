using PhantomGrid.ScriptableObjects;

namespace PhantomGrid.Events
{
    public class StartGameEvent : EventPayload
    {
        public GameLevel SelectedLevel{ get; }
        
        public StartGameEvent(GameLevel selectedLevel)
        {
            SelectedLevel = selectedLevel;
        }
    }
}