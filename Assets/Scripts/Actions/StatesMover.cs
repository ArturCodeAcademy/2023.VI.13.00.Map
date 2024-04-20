using UnityEngine;
using UnityEngine.Events;

public class StatesMover : MonoBehaviour
{
    [SerializeField] private bool _local = true;
	[SerializeField] private bool _loop = false;
    [SerializeField] private Vector3[] _positions;
	[SerializeField, Min(0.1f)] private float _duration = 1f;
	[SerializeField] private bool _startOnAwake = true;

	[Space(3)]
	public UnityEvent TargetReached;

	private int _currentPositionIndex = 0;
	private float _lerpValue = 0f;
	private Vector3 _startPosition;

	private void Awake()
	{
		TargetReached ??= new UnityEvent();
	}

	private void Start()
	{
		SetCurrentPositionAsStart();
		_lerpValue = _startOnAwake ? 0f : 1f;
	}

	private void Update()
	{
		if (_positions is null or { Length: < 2 })
			return;

		if (_lerpValue >= 1f)
			return;

		_lerpValue += Time.deltaTime / _duration;
		_lerpValue = Mathf.Min(_lerpValue, 1f);

		Vector3 targetPosition = _positions[_currentPositionIndex];
		Vector3 currentPosition = Vector3.Lerp(_startPosition, targetPosition, _lerpValue);

		if (_local && transform.parent is not null)
			transform.localPosition = currentPosition;
		else
			transform.position = currentPosition;

		if (_lerpValue >= 1f)
			TargetReached?.Invoke();
	}

	private void SetCurrentPositionAsStart()
	{
		_startPosition = transform.parent is not null && _local
							? transform.localPosition
							: transform.position;
	}

	public void SetNextPositionAsTarget()
	{
		if (_positions.Length == _currentPositionIndex + 1 && !_loop)
			return;

		SetCurrentPositionAsStart();
		_currentPositionIndex = (_currentPositionIndex + 1) % _positions.Length;
		_lerpValue = 0f;
	}

	public void SetPreviousPositionAsTarget()
	{
		if (_currentPositionIndex == 0 && !_loop)
			return;

		_currentPositionIndex = (_currentPositionIndex - 1 + _positions.Length) % _positions.Length;
		_lerpValue = 0f;
	}

	public void SetPositionAsTarget(int index)
	{
		if (index < 0 || index >= _positions.Length)
			return;

		_currentPositionIndex = index;
		_lerpValue = 0f;
	}

#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
	{
		if (_positions is null or { Length: < 2 })
			return;

		const float SIZE = 0.1f;

		for (int i = 0; i < _positions.Length; i++)
		{
			Vector3 p1 = _positions[i];
			Vector3 p2 = _positions[(i + 1) % _positions.Length];

			if (_local && transform.parent is not null)
			{
				p1 = transform.parent.TransformPoint(p1);
				p2 = transform.parent.TransformPoint(p2);
			}

			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(p1, SIZE);

			if (i == _positions.Length - 1 && !_loop)
				continue;

			Gizmos.color = Color.red;
			Gizmos.DrawLine(p1, p2);
		}
	}

#endif
}
