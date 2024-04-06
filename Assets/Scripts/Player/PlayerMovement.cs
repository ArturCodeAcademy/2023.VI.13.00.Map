using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterControllerGravity))]
[RequireComponent(typeof(PlayerCrouch))]
public class PlayerMovement : MonoBehaviour
{
	[field: Header("Params")]
	[SerializeField] private float _walkSpeed = 5f;
	[SerializeField] private float _runSpeed = 10f;
	[SerializeField] private float _crouchSpeed = 2f;
	[SerializeField] private float _nonGroundedAcceleration = 5f;

	[field: Header("Components")]
	[field: SerializeField] private CharacterController _characterController;
	[field: SerializeField] private CharacterControllerGravity _gravity;
	[field: SerializeField] private PlayerCrouch _crouch;

	private Vector3 _lastHorizontalVelocity;

	private void Start()
	{
		_lastHorizontalVelocity = Vector3.zero;
	}

	private void Update()
	{
		Vector3 horizontalVelocity = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		horizontalVelocity = transform.TransformDirection(horizontalVelocity);

		if (_gravity.IsGrounded)
		{
			_lastHorizontalVelocity = horizontalVelocity;

			if (_crouch.IsCrouched)
				_lastHorizontalVelocity *= _crouchSpeed;
			else if (Input.GetKey(KeyCode.LeftShift))
				_lastHorizontalVelocity *= _runSpeed;
			else
				_lastHorizontalVelocity *= _walkSpeed;
		}
		else
		{
			_lastHorizontalVelocity += horizontalVelocity * _nonGroundedAcceleration * Time.deltaTime;
		}

		CollisionFlags collision = _characterController.Move(_lastHorizontalVelocity * Time.deltaTime);

		if (collision.HasFlag(CollisionFlags.Sides))
			_lastHorizontalVelocity = Vector3.zero;
	}

#if UNITY_EDITOR

	[ContextMenu(nameof(TryGetComponents))]
	private void TryGetComponents()
	{
		_characterController ??= GetComponent<CharacterController>();
		_gravity = GetComponent<CharacterControllerGravity>();
		_crouch = GetComponent<PlayerCrouch>();
	}

	private void OnValidate()
	{
		if (_crouchSpeed > _walkSpeed)
			_crouchSpeed = _walkSpeed;

		if (_runSpeed < _walkSpeed)
			_runSpeed = _walkSpeed;
	}

#endif
}
