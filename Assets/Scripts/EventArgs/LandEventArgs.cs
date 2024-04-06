using UnityEngine;

public class LandEventArgs
{
	public float Velocity { get; private set; }
	public Vector3 FallStartPosition { get; private set; }
	public Vector3 LandPosition { get; private set; }
	public float Height => FallStartPosition.y - LandPosition.y;

	public LandEventArgs(float velocity, Vector3 fallStartPosition, Vector3 landPosition)
	{
		Velocity = velocity;
		FallStartPosition = fallStartPosition;
		LandPosition = landPosition;
	}
}
