namespace PhantomGrid.Events
{
    public class ResumeGameEvent : EventPayload
    {
        public bool IsResumeSelected { get; }

        public ResumeGameEvent(bool isResumeSelected)
        {
            IsResumeSelected = isResumeSelected;
        }
    }
}