using UnityEngine;

public class UISettingsHandler : MonoBehaviour
{
	public void SetDeadzone(float deadzone)
	{
		GameSettings.deadzone = deadzone;
	}

	public void SetDeadzoneUsingPercent(float deadzone)
	{
		GameSettings.deadzone = deadzone * 0.01f;
	}

	public void SetControlMethod(ControlMethod controlMethod)
	{
		GameSettings.controlMethod = controlMethod;
	}
}
