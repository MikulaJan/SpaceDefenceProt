using UnityEngine;

public interface ITargetable
{
	string Name { get; set; }

	string TypeName { get; set; }

	Faction Faction { get; }

	Transform Transform { get; }

	Vector3 Velocity { get; }

	Vector3 Position { get; }

	bool Locked { get; set; }

	bool Targeted { get; set; }

	bool PriorityTarget { get; set; }

	bool Active { get; set; }

	bool Friendly { get; set; }

	HUDTargetBox AssignedHudTargetBox { get; set; }
}
