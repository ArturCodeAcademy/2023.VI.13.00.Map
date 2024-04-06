using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerGravity : MonoBehaviour
{
	public float VerticalVelocity { get; private set; } = 0f;
	public bool IsGrounded { get; private set; } = false;

	public event EventHandler<LandEventArgs> Land;
	public event EventHandler<FallEventArgs> StartedFall;

	[Header("Params")]
	[SerializeField] private float _gravityScale = 1f;
	[SerializeField] private float _minVerticalVelocity = -10f;

	[Header("Components")]
	[SerializeField] private CharacterController _characterController;

	private Vector3 _fallStartPos;

	private void Start()
	{
		_fallStartPos = transform.position;
	}

	private void FixedUpdate()
	{
		CollisionFlags collision = _characterController.Move(Vector3.up * VerticalVelocity * Time.fixedDeltaTime);
		bool isGrounded = collision.HasFlag(CollisionFlags.Below);

		if (!isGrounded && IsGrounded)
		{
			IsGrounded = false;
			_fallStartPos = transform.position;
			StartedFall?.Invoke(this, new(_fallStartPos));
		}
		else if (isGrounded)
		{
			if (!IsGrounded)
				Land?.Invoke(this, new(VerticalVelocity, _fallStartPos, transform.position));
			IsGrounded = true;
			VerticalVelocity = 0f;
		}

		VerticalVelocity += Physics.gravity.y * _gravityScale * Time.fixedDeltaTime;
		VerticalVelocity = Mathf.Max(VerticalVelocity, _minVerticalVelocity);
	}

	public void AddVelocity(float velocity)
	{
		VerticalVelocity += velocity;
	}

	public void SetVelocity(float velocity)
	{
		VerticalVelocity = velocity;
	}

#if UNITY_EDITOR

	[ContextMenu(nameof(TryGetComponents))]
	private void TryGetComponents()
	{
		_characterController ??= GetComponent<CharacterController>();
	}

#endif
}
