using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

	[field: SerializeField] public LayerMask PlayerLayerMask { get; private set; }

	private void Awake()
	{
		Instance = this;
	}
}
