using UnityEngine;

public static class WeaponHelpers
{
	public static Vector3 GetForwardWithDeviationAt100m(Transform relativeTo, float dispersion)
	{
		if (relativeTo != null)
		{
			Vector3 direction = Random.insideUnitSphere * dispersion;
			direction += Vector3.forward * 100f;
			return relativeTo.TransformDirection(direction);
		}
		Debug.LogError("Tried to fire gun with no transform.");
		return Vector3.forward;
	}

	public static Vector3 CalculateLeadForGuns(float muzzleVelocity, Vector3 targetPos, Vector3 targetVel, Vector3 ownPos, Vector3 ownVel)
	{
		if (muzzleVelocity <= 0f)
		{
			muzzleVelocity = 1f;
		}
		Vector3 b = targetPos + targetVel;
		Vector3 a = ownPos + ownVel;
		float num = Vector3.Distance(a, b);
		float num2 = num / muzzleVelocity;
		return (targetVel - ownVel) * num2 + targetPos;
	}
}
