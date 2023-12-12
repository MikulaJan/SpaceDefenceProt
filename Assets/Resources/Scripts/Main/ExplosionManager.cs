using UnityEngine;

public class ExplosionManager
{
	public delegate void ExplosionEvent(Vector3 position, float damage, float radius, float impulse);

	public static event ExplosionEvent OnExplosion;

	public static void CreateExplosion(Vector3 position, Quaternion rotation, float damage, float radius, float impulse, ParticleSystem explEffect)
	{
		if (explEffect != null)
		{
			Object.Instantiate(explEffect, position, rotation);
		}
		if (ExplosionManager.OnExplosion != null)
		{
			ExplosionManager.OnExplosion(position, damage, radius, impulse);
		}
	}
}
