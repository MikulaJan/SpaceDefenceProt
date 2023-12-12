using UnityEngine;

public class TurretRotations
{
	private Transform transform;

	private Transform turretBase;

	private Transform turretBarrels;

	private float turnRate = 30f;

	private float maxLeftTraverse = 60f;

	private float maxRightTraverse = 60f;

	private float maxElevation = 60f;

	private float maxDepression = 5f;

	private bool hasLimitedTraverse;

	public TurretRotations(Transform transform, Transform turretBase, Transform turretBarrels, float turnRate, float maxLeftTraverse, float maxRightTraverse, bool limitedTraverse, float maxElevation, float maxDepression)
	{
		this.transform = transform;
		this.turretBase = turretBase;
		this.turretBarrels = turretBarrels;
		this.turnRate = turnRate;
		this.maxLeftTraverse = maxLeftTraverse;
		this.maxRightTraverse = maxRightTraverse;
		this.maxElevation = maxElevation;
		this.maxDepression = maxDepression;
		hasLimitedTraverse = limitedTraverse;
	}

	public void RotateturretBaseToTarget(Vector3 aimPos)
	{
		Vector3 forward = transform.InverseTransformPoint(aimPos);
		forward.y = 0f;
		Quaternion to = Quaternion.LookRotation(forward);
		Quaternion localRotation = Quaternion.RotateTowards(turretBase.localRotation, to, turnRate * Time.deltaTime);
		turretBase.localRotation = localRotation;
		if (hasLimitedTraverse)
		{
			Vector3 localEulerAngles = turretBase.localEulerAngles;
			localEulerAngles.y = ConvertEuler.Clamp(localEulerAngles.y, maxLeftTraverse, maxRightTraverse);
			turretBase.localEulerAngles = localEulerAngles;
		}
	}

	public void RotateturretBarrelsToTarget(Vector3 aimPos)
	{
		Vector3 forward = turretBase.InverseTransformPoint(aimPos);
		forward.x = 0f;
		Quaternion to = Quaternion.LookRotation(forward);
		Quaternion localRotation = Quaternion.RotateTowards(turretBarrels.localRotation, to, 2f * turnRate * Time.deltaTime);
		turretBarrels.localRotation = localRotation;
		Vector3 eulerAngles = turretBarrels.localRotation.eulerAngles;
		eulerAngles.x = ConvertEuler.Clamp(eulerAngles.x, maxElevation, maxDepression);
		eulerAngles.y = 0f;
		eulerAngles.z = 0f;
		turretBarrels.localEulerAngles = eulerAngles;
	}
}
