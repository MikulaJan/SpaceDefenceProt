using UnityEngine;

public class ExplosionArgs
{
	private Vector3 position;

	private float damage;

	private float radius;

	private float impulse;

	public Vector3 Position
	{
		get
		{
			return position;
		}
	}

	public float Damage
	{
		get
		{
			return damage;
		}
	}

	public float Radius
	{
		get
		{
			return radius;
		}
	}

	public float Impulse
	{
		get
		{
			return impulse;
		}
	}

	public ExplosionArgs(Vector3 position, float damage, float radius, float impulse)
	{
		this.position = position;
		this.damage = damage;
		this.radius = radius;
		this.impulse = impulse;
	}
}
