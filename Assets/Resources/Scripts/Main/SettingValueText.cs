using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SettingValueText : MonoBehaviour
{
	private Text text;

	public string format;

	public float multiplier = 1f;

	private void Awake()
	{
		text = GetComponent<Text>();
	}

	public void SetTextWithFormat(float value)
	{
		float num = value * multiplier;
		text.text = num.ToString(format);
	}
}
