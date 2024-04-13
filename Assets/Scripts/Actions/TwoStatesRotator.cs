using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoStatesRotator : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float _rotationDuration = 1f;
    [SerializeField] private Vector3 _startRotation;
    [SerializeField] private Vector3 _endRotation;
    private float _state = 0f;
    private bool _toEnd = true;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(_startRotation);
    }

	private void Update()
	{
		if (_state >= 1f && _toEnd || _state <= 0f && !_toEnd)
			return;

		if (_toEnd && _state < 1f)
        {
			_state += Time.deltaTime / _rotationDuration;
			_state = Mathf.Min(_state, 1f);
			
		}
		else if (!_toEnd && _state > 0f)
        {
			_state -= Time.deltaTime / _rotationDuration;
			_state = Mathf.Max(_state, 0f);
		}

		transform.localRotation = Quaternion.Euler(Vector3.Lerp(_startRotation, _endRotation, _state));
	}

    public void ChangeDirection()
    {
		_toEnd = !_toEnd;
	}
}
