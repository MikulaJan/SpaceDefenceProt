using UnityEngine;
using UnityEngine.UI;

public class CycleControlMethodButton : MonoBehaviour
{
	private Text buttonText;

	public Text descriptionText;

	private const string BUTTON_RAW = "CONTROL METHOD: RAW";

	private const string DESCRIPTION_RAW = "Raw:\nStick left/right commands only left roll. Gives maximum control on all axes.";

	private const string BUTTON_HALF = "CONTROL METHOD: HALF";

	private const string DESCRIPTION_HALF = "Half\nStick left/right commands both yaw and roll in equal parts. Traditional space sim control method.";

	private const string BUTTON_BLENDED = "CONTROL METHOD: BLENDED";

	private const string DESCRIPTION_BLENDED = "Blended:\nStick left/right commands only yaw at low deflections. Both roll and yaw at medium deflections, and only roll at full deflection.";

	public void Awake()
	{
		buttonText = GetComponentInChildren<Text>();
		switch (GameSettings.controlMethod)
		{
		case ControlMethod.Blended:
			buttonText.text = "CONTROL METHOD: BLENDED";
			descriptionText.text = "Blended:\nStick left/right commands only yaw at low deflections. Both roll and yaw at medium deflections, and only roll at full deflection.";
			break;
		case ControlMethod.Raw:
			buttonText.text = "CONTROL METHOD: RAW";
			descriptionText.text = "Raw:\nStick left/right commands only left roll. Gives maximum control on all axes.";
			break;
		case ControlMethod.Half:
			buttonText.text = "CONTROL METHOD: HALF";
			descriptionText.text = "Half\nStick left/right commands both yaw and roll in equal parts. Traditional space sim control method.";
			break;
		}
	}

	public void CycleSetting()
	{
		switch (GameSettings.controlMethod)
		{
		case ControlMethod.Blended:
			buttonText.text = "CONTROL METHOD: RAW";
			descriptionText.text = "Raw:\nStick left/right commands only left roll. Gives maximum control on all axes.";
			GameSettings.controlMethod = ControlMethod.Raw;
			break;
		case ControlMethod.Raw:
			buttonText.text = "CONTROL METHOD: HALF";
			descriptionText.text = "Half\nStick left/right commands both yaw and roll in equal parts. Traditional space sim control method.";
			GameSettings.controlMethod = ControlMethod.Half;
			break;
		case ControlMethod.Half:
			buttonText.text = "CONTROL METHOD: BLENDED";
			descriptionText.text = "Blended:\nStick left/right commands only yaw at low deflections. Both roll and yaw at medium deflections, and only roll at full deflection.";
			GameSettings.controlMethod = ControlMethod.Blended;
			break;
		}
	}
}
