using UnityEngine;
using UnityEngine.Events;

public class DefaultInteractable : MonoBehaviour, IInteractable
{
	public UnityEvent Interact;

	[field: SerializeField] public string Name { get; private set; }

	[field: SerializeField] public string Description { get; private set; }

	private void Awake()
	{
		Interact ??= new UnityEvent();
	}

	public void OnInteract()
	{
		Interact?.Invoke();
	}
}
