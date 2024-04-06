using UnityEngine;

public class FallEventArgs
{
	public Vector3 FallStartPosition { get; private set; }

	public FallEventArgs(Vector3 fallStartPosition)
	{
		FallStartPosition = fallStartPosition;
	}
}
