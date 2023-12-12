using UnityEngine;

public class Warhead : MonoBehaviour
{
	[Tooltip("Damage of anything inside the explosion radius.")]
	public float damage = 100f;

	[Tooltip("Explosion radius in meters.")]
	public float radius = 10f;

	[Tooltip("Impulse force of the explosion.")]
	public float impulse;

	[Tooltip("Prefab for the effect that gets played on the explosion.")]
	public ParticleSystem explosionFXPrefab;

	private bool armed;

	private const float IMPULSE_MULTIPLIER = 1000f;

	public void Arm(bool arm) 
	{
		armed = arm;
	}

	public void Explode()
	{
		Explode(base.transform.position);
	}

	public void Explode(Vector3 explodeAt)
	{
		if (armed)
		{
			if ((bool)explosionFXPrefab)
			{
				Object.Instantiate(explosionFXPrefab, explodeAt, base.transform.rotation);
			}
			ExplosionManager.CreateExplosion(explodeAt, base.transform.rotation, damage, radius, impulse * 1000f, explosionFXPrefab);
		}
	}
}
