using System;

public interface IInteractable
{
    public event Action<InteractionType> OnInteracted;

}

public enum InteractionType
{
    Interaction,
    OnTriggerEnter,
}