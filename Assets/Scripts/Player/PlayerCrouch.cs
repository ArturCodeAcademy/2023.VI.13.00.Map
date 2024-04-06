using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterControllerGravity))]
public class PlayerCrouch : MonoBehaviour
{
	public bool IsCrouched { get; private set; }

	[Header("Params")]
	[SerializeField, Min(0.5f)] private float _crouchHeight = 1f;
	[SerializeField, Min(0.5f)] private float _standHeight = 1.8f;
	[SerializeField, Range(0.1f, 10f)] private float _crouchSpeed = 5f;
	[field: SerializeField, Min(0f)] public float CameraOffset { get; private set; } = 0.15f;
	[SerializeField, Range(0, 1f)] private float _headBlockedCheckDistance = 0.1f;

	[Header("Components")]
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private CharacterControllerGravity _gravity;
	[SerializeField] private Transform _viewTransform;

	private float _currentHeight = 0f;
	private LayerMask _playerLayerMask;

	private void Start()
	{
		SetStandParams();
		_playerLayerMask = Player.Instance.PlayerLayerMask;
	}

	private void Update()
	{
		bool isCrouchPressed = Input.GetKey(KeyCode.LeftControl);

		if (isCrouchPressed && _gravity.IsGrounded)
		{
			IsCrouched = true;

			if (_currentHeight == _crouchHeight)
				return;

			_currentHeight -= _crouchSpeed * Time.deltaTime;
			_currentHeight = Mathf.Max(_currentHeight, _crouchHeight);
			CrouchLerp(_currentHeight);
		}
		else if (IsCrouched)
		{
			if (_currentHeight == _standHeight)
			{
				IsCrouched = false;
				return;
			}

			if (!IsHeadBlocked())
			{
				_currentHeight += _crouchSpeed * Time.deltaTime;
				_currentHeight = Mathf.Min(_currentHeight, _standHeight);
				CrouchLerp(_currentHeight);
			}
		}
	}

	private bool IsHeadBlocked()
	{
		Vector3 castCenter = transform.position + Vector3.up * (_currentHeight - _characterController.radius);
		RaycastHit[] hits = Physics.SphereCastAll(castCenter,
													_characterController.radius,
													Vector3.up,
													_headBlockedCheckDistance,
													~_playerLayerMask, // cast all layers except player layer
													QueryTriggerInteraction.Ignore);

		return hits.Length > 0;
	}

	private void SetStandParams()
	{
		IsCrouched = false;
		_currentHeight = _standHeight;
		CrouchLerp(_currentHeight);
	}

	private void CrouchLerp(float height)
	{
		_characterController.height = height;
		_characterController.center = Vector3.up / 2 * height;
		_viewTransform.localPosition = Vector3.up * (height - CameraOffset);
	}

#if UNITY_EDITOR

	[ContextMenu(nameof(TryGetComponents))]
	private void TryGetComponents()
	{
		_characterController ??= GetComponent<CharacterController>();
		_gravity ??= GetComponent<CharacterControllerGravity>();
		_viewTransform ??= GetComponentInChildren<PlayerView>()?.transform;
	}

	private void OnValidate()
	{
		if (_characterController is not null && _viewTransform is not null)
			SetStandParams();

		if (_crouchHeight > _standHeight)
			_crouchHeight = _standHeight;

		if (_crouchHeight < _characterController.radius * 2)
			_crouchHeight = _characterController.radius * 2;

		if (CameraOffset > _crouchHeight)
			CameraOffset = _crouchHeight;
	}

#endif
}