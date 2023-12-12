using UnityEngine;

public class Target : Targetable
{
	public bool active = true;

	[Space]
	public string typeName;

	public string targetName;
	 
	public Faction faction = Faction.White;

	[Space]
	public bool priorityTarget;

	public bool friendly;

	public override Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	public override Vector3 Velocity
	{
		get
		{
			return Vector3.zero;
		}
	}

	public override Vector3 Position
	{
		get
		{
			return base.transform.position;
		}
	}

	public override string Name
	{
		get
		{
			return targetName;
		}
		set
		{
			targetName = value;
		}
	}

	public override string TypeName
	{
		get
		{
			return typeName;
		}
		set
		{
			typeName = value;
		}
	}

	public override Faction Faction
	{
		get
		{
			return faction;
		}
	}

	public override bool PriorityTarget
	{
		get
		{
			return priorityTarget;
		}
		set
		{
			priorityTarget = value;
		}
	}

	public override bool Active
	{
		get
		{
			return active;
		}
		set
		{
			active = value;
		}
	}

	public override bool Friendly
	{
		get
		{
			return friendly;
		}
		set
		{
			friendly = value;
		}
	}
}
