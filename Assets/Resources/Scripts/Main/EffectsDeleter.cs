using UnityEngine;

public class EffectsDeleter : MonoBehaviour
{
	private ParticleSystem pSystem;

	[Tooltip("When true, will destroy the entire gameobject this is attached to.\n\nWhen false, destroys only the first particle system it sees.")]
	public bool destroyGameObject = true;

	private bool hasStarted;

	public void Awake()
	{
		pSystem = GetComponentInChildren<ParticleSystem>();
	}

	public void Start()
	{
		if (pSystem != null && pSystem.isPlaying)
		{
			hasStarted = true;
		}
	}

	public void Update()
	{
		if (pSystem != null && hasStarted && !pSystem.IsAlive(true))
		{
			Object.Destroy(base.gameObject);
		}
	}
}
 