namespace Game
{
    public interface ITimedInteractable
    {
        bool HasInteractionDuration { get; }
        float InteractionDuration { get; }
        void CompleteInteraction(string playerID);
    }

    public interface IInstantInteractable
    {
        void InteractImmediately(string playerID);
    }

}

