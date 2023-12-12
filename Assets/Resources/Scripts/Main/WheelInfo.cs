using System;
using UnityEngine;

[Serializable]
public class WheelInfo
{
	public WheelCollider wheel;

	public string compressLayer;

	public string compressStateName;

	public string extendLayer;

	public string extendStateName;

	public string steeringLayer;

	public string steeringLeftStateName;

	public string steeringRightStateName;

	public bool steering;

	public bool reverseSteer;

	public float steerMaxAngle = 60f;

	public float steerSpeed = 60f;

	public float brakeTorque;
}
