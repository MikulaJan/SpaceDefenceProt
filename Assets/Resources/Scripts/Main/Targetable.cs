using UnityEngine;

public abstract class Targetable : MonoBehaviour, ITargetable
{
	public virtual bool Locked { get; set; }

	public virtual bool PriorityTarget { get; set; }

	public virtual bool Targeted { get; set; }

	public virtual bool Friendly { get; set; }

	public abstract string TypeName { get; set; }

	public abstract string Name { get; set; }

	public abstract Faction Faction { get; }

	public abstract bool Active { get; set; }

	public abstract Vector3 Position { get; }

	public abstract Transform Transform { get; }

	public abstract Vector3 Velocity { get; }

	public HUDTargetBox AssignedHudTargetBox { get; set; }

	public virtual void OnEnable()
	{
		TargetManager.AddTarget(this);
	}

	public virtual void OnDisable()
	{
		TargetManager.RemoveTarget(this);
	}
}
