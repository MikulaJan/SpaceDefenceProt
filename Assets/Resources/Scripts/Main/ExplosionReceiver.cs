using UnityEngine;

public class ExplosionReceiver : MonoBehaviour
{
	private Rigidbody rigidbody;

	public void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void OnEnable()
	{
		ExplosionManager.OnExplosion += OnExplosion;
	}

	public void OnDisable()
	{ 
		ExplosionManager.OnExplosion -= OnExplosion;
	}

	private void OnExplosion(Vector3 position, float damage, float radius, float impulse)
	{
		rigidbody.AddExplosionForce(impulse, position, radius);
	}
}
