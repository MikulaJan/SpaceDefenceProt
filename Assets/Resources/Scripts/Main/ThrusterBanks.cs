using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class ThrusterBanks : MonoBehaviour
{
	[Tooltip("Thruster power required before the effect can activate.")]
	public float threshold = 0.05f;

	public List<Thruster> translateUp;

	public List<Thruster> translateDown;

	public List<Thruster> translateLeft;

	public List<Thruster> translateRight;

	public List<Thruster> translateForward;

	public List<Thruster> translateReverse;

	public List<Thruster> pitchUp;

	public List<Thruster> pitchDown;

	public List<Thruster> yawLeft;

	public List<Thruster> yawRight; 

	public List<Thruster> rollLeft;

	public List<Thruster> rollRight;

	private Vector3 translation;

	private Vector3 rotation;

	private void Update()
	{
		if (translation.x > threshold)
		{
			foreach (Thruster item in translateRight)
			{
				item.SetPower1(Mathf.Abs(translation.x));
			}
			foreach (Thruster item2 in translateLeft)
			{
				item2.SetPower1(0f);
			}
		}
		else if (translation.x < 0f - threshold)
		{
			foreach (Thruster item3 in translateRight)
			{
				item3.SetPower1(0f);
			}
			foreach (Thruster item4 in translateLeft)
			{
				item4.SetPower1(Mathf.Abs(translation.x));
			}
		}
		else
		{
			foreach (Thruster item5 in translateRight)
			{
				item5.SetPower1(0f);
			}
			foreach (Thruster item6 in translateLeft)
			{
				item6.SetPower1(0f);
			}
		}
		if (translation.y > threshold)
		{
			foreach (Thruster item7 in translateUp)
			{
				item7.SetPower1(Mathf.Abs(translation.y));
			}
			foreach (Thruster item8 in translateDown)
			{
				item8.SetPower1(0f);
			}
		}
		else if (translation.y < 0f - threshold)
		{
			foreach (Thruster item9 in translateUp)
			{
				item9.SetPower1(0f);
			}
			foreach (Thruster item10 in translateDown)
			{
				item10.SetPower1(Mathf.Abs(translation.y));
			}
		}
		else
		{
			foreach (Thruster item11 in translateUp)
			{
				item11.SetPower1(0f);
			}
			foreach (Thruster item12 in translateDown)
			{
				item12.SetPower1(0f);
			}
		}
		if (translation.z > threshold)
		{
			foreach (Thruster item13 in translateForward)
			{
				item13.SetPower1(Mathf.Abs(translation.z));
			}
			foreach (Thruster item14 in translateReverse)
			{
				item14.SetPower1(0f);
			}
		}
		else if (translation.z < 0f - threshold)
		{
			foreach (Thruster item15 in translateForward)
			{
				item15.SetPower1(0f);
			}
			foreach (Thruster item16 in translateReverse)
			{
				item16.SetPower1(Mathf.Abs(translation.z));
			}
		}
		else
		{
			foreach (Thruster item17 in translateForward)
			{
				item17.SetPower1(0f);
			}
			foreach (Thruster item18 in translateReverse)
			{
				item18.SetPower1(0f);
			}
		}
		if (rotation.x > threshold)
		{
			foreach (Thruster item19 in pitchUp)
			{
				item19.SetPower2(0f);
			}
			foreach (Thruster item20 in pitchDown)
			{
				item20.SetPower2(Mathf.Abs(rotation.x));
			}
		}
		else if (rotation.x < 0f - threshold)
		{
			foreach (Thruster item21 in pitchUp)
			{
				item21.SetPower2(Mathf.Abs(rotation.x));
			}
			foreach (Thruster item22 in pitchDown)
			{
				item22.SetPower2(0f);
			}
		}
		else
		{
			foreach (Thruster item23 in pitchUp)
			{
				item23.SetPower2(0f);
			}
			foreach (Thruster item24 in pitchDown)
			{
				item24.SetPower2(0f);
			}
		}
		if (rotation.y > threshold)
		{
			foreach (Thruster item25 in yawLeft)
			{
				item25.SetPower2(0f);
			}
			foreach (Thruster item26 in yawRight)
			{
				item26.SetPower2(Mathf.Abs(rotation.y));
			}
		}
		else if (rotation.y < 0f - threshold)
		{
			foreach (Thruster item27 in yawLeft)
			{
				item27.SetPower2(Mathf.Abs(rotation.y));
			}
			foreach (Thruster item28 in yawRight)
			{
				item28.SetPower2(0f);
			}
		}
		else
		{
			foreach (Thruster item29 in yawLeft)
			{
				item29.SetPower2(0f);
			}
			foreach (Thruster item30 in yawRight)
			{
				item30.SetPower2(0f);
			}
		}
		if (rotation.z > threshold)
		{
			foreach (Thruster item31 in rollRight)
			{
				item31.SetPower2(Mathf.Abs(0f));
			}
			{
				foreach (Thruster item32 in rollLeft)
				{
					item32.SetPower2(Mathf.Abs(rotation.z));
				}
				return;
			}
		}
		if (rotation.z < 0f - threshold)
		{
			foreach (Thruster item33 in rollRight)
			{
				item33.SetPower2(Mathf.Abs(rotation.z));
			}
			{
				foreach (Thruster item34 in rollLeft)
				{
					item34.SetPower2(0f);
				}
				return;
			}
		}
		foreach (Thruster item35 in rollLeft)
		{
			item35.SetPower2(0f);
		}
		foreach (Thruster item36 in rollRight)
		{
			item36.SetPower2(0f);
		}
	}

	public void SetTranslationRotation(Vector3 i_Translation, Vector3 i_Rotation)
	{
		translation = i_Translation;
		rotation = MathUtil.ClampVec3(i_Rotation, -1f, 1f);
	}

	public void DetachAndStopAllParticleEffects()
	{
		DetachThrustersInBank(translateUp);
		DetachThrustersInBank(translateDown);
		DetachThrustersInBank(translateLeft);
		DetachThrustersInBank(translateRight);
		DetachThrustersInBank(translateForward);
		DetachThrustersInBank(translateReverse);
		DetachThrustersInBank(pitchUp);
		DetachThrustersInBank(pitchDown);
		DetachThrustersInBank(yawLeft);
		DetachThrustersInBank(yawRight);
		DetachThrustersInBank(rollLeft);
		DetachThrustersInBank(rollRight);
	}

	private void DetachThrustersInBank(List<Thruster> bank)
	{
		foreach (Thruster item in bank)
		{
			item.DetachAndStopThrusterVFX();
		}
	}

	[ContextMenu("Clear all thruster banks.")]
	[ExecuteInEditMode]
	public void ClearAllBanks()
	{
		translateUp.Clear();
		translateDown.Clear();
		translateLeft.Clear();
		translateRight.Clear();
		translateForward.Clear();
		translateReverse.Clear();
		pitchUp.Clear();
		pitchDown.Clear();
		yawLeft.Clear();
		yawRight.Clear();
		rollLeft.Clear();
		rollRight.Clear();
	}

	[ContextMenu("Auto assign thruster banks based on thruster definitions.")]
	[ExecuteInEditMode]
	public void AutoAssignThrusterBanks()
	{
		ClearAllBanks();
		Thruster[] componentsInChildren = GetComponentsInChildren<Thruster>();
		Thruster[] array = componentsInChildren;
		foreach (Thruster thruster in array)
		{
			switch (thruster.translateDir)
			{
			case ThrusterTranslationDirection.Up:
				translateUp.Add(thruster);
				break;
			case ThrusterTranslationDirection.Down:
				translateDown.Add(thruster);
				break;
			case ThrusterTranslationDirection.Left:
				translateLeft.Add(thruster);
				break;
			case ThrusterTranslationDirection.Right:
				translateRight.Add(thruster);
				break;
			case ThrusterTranslationDirection.Forward:
				translateForward.Add(thruster);
				break;
			case ThrusterTranslationDirection.Reverse:
				translateReverse.Add(thruster);
				break;
			}
			switch (thruster.rotationDir)
			{
			case ThrusterRotationalDirection.PitchUp:
				pitchUp.Add(thruster);
				break;
			case ThrusterRotationalDirection.PitchDown:
				pitchDown.Add(thruster);
				break;
			case ThrusterRotationalDirection.YawLeft:
				yawLeft.Add(thruster);
				break;
			case ThrusterRotationalDirection.YawRight:
				yawRight.Add(thruster);
				break;
			case ThrusterRotationalDirection.RollLeft:
				rollLeft.Add(thruster);
				break;
			case ThrusterRotationalDirection.RollRight:
				rollRight.Add(thruster);
				break;
			}
		}
	}
}
