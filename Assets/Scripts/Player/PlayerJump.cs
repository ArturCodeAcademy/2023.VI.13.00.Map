using UnityEngine;

[RequireComponent(typeof(CharacterControllerGravity))]
public class PlayerJump : MonoBehaviour
{
	[Header("Params")]
	[SerializeField, Min(0.5f)] private float _jumpHeight = 1.0f;
	[SerializeField, Min(1)] private int _maxJumps = 3;

	[Header("Components")]
	[SerializeField] private CharacterControllerGravity _gravityController;

	private int _leftJumps;
	private float _jumpVelocity;

	private void Start()
	{
		RefreshLeftJumpsCount();
		_jumpVelocity = Mathf.Sqrt(2 * _jumpHeight * Physics.gravity.magnitude);
	}

	private void OnEnable()
	{
		_gravityController.Land += RefreshLeftJumpsCount;
	}

	private void OnDisable()
	{
		_gravityController.Land -= RefreshLeftJumpsCount;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && _leftJumps --> 0)
		{
			_gravityController.SetVelocity(_jumpVelocity);
		}
	}

	private void RefreshLeftJumpsCount(object sender = default, LandEventArgs e = default)
	{
		_leftJumps = _maxJumps;
	}
}
