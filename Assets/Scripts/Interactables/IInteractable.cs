public interface IInteractable
{
    string Name { get; }
    string Description { get; }
    void OnInteract();
}
