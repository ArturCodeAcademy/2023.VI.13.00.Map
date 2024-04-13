using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField, Range(1, 5)] private float _interactionDistance = 2f;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;

    [Space(3)]
    [SerializeField] private Camera _camera;

    private IInteractable _currentInteractable;

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
        {
			_currentInteractable?.OnInteract();
		}
	}

	private void FixedUpdate()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        bool isRaycastSuccess = Physics.Raycast(ray,
                                out RaycastHit hit,
                                _interactionDistance,
                                ~Player.Instance.PlayerLayerMask,
                                QueryTriggerInteraction.Ignore);

        if (isRaycastSuccess)
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
				if (_currentInteractable != interactable)
                {
					_currentInteractable = interactable;
					_nameText.text = _currentInteractable.Name;
					_descriptionText.text = _currentInteractable.Description;
				}
			}
			else
			{
				_currentInteractable = null;
				_nameText.text = string.Empty;
				_descriptionText.text = string.Empty;
			}
		}
		else
		{
			_currentInteractable = null;
			_nameText.text = string.Empty;
			_descriptionText.text = string.Empty;
		}
	}
}
