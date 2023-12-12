using System;
using UnityEngine;

internal class RadarTarget : IComparable<ITargetable>
{
	private ITargetable target;

	private float timeTargeted;

	public ITargetable Target
	{
		get
		{
			return target;
		}
		set
		{
			target = value;
		}
	}

	public bool Locked
	{
		get
		{
			return target.Locked;
		}
		set
		{
			target.Locked = value;
		}
	}

	public bool IsTargeted
	{
		get
		{
			return target.Targeted;
		}
		set
		{
			target.Targeted = value;
		}
	}

	public bool Active
	{
		get
		{
			return target.Active;
		}
	}

	public bool Friendly
	{
		get
		{
			return target.Friendly;
		}
		set
		{
			target.Friendly = value;
		}
	}

	public Faction Faction
	{
		get
		{
			return target.Faction;
		}
	}

	public Vector3 Position
	{
		get
		{
			return target.Position;
		}
	}

	public RadarTarget(ITargetable target)
	{
		this.target = target;
		timeTargeted = 0f;
	}

	public int CompareTo(ITargetable other)
	{
		if (other == target)
		{
			return 0;
		}
		return 1;
	}

	public void SetTargeted(bool targeted)
	{
		target.Targeted = targeted;
		if (targeted)
		{
			timeTargeted = Time.time;
		}
	}

	public float TimeSinceTargeted()
	{
		return Time.time - timeTargeted;
	}
}
