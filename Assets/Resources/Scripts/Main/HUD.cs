using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	public Camera referenceCam;

	public Text speedText;

	public Image fpm;

	public static HUD reference;

	private float speed;

	private Vector3 position;

	private Vector3 velocity;

	private void Start()
	{
		if (reference == null)
		{
			reference = this;
		}
	}

	private void Update()
	{
		speedText.text = speed.ToString("000");
	}

	private void FixedUpdate()
	{
		PositionFPM();
	}

	private void PositionFPM()
	{
		if ((bool)referenceCam)
		{
			Vector3 vector = position + velocity * 100f;
			vector.z = Mathf.Clamp(vector.z, 0f, 100f);
			fpm.transform.position = referenceCam.WorldToScreenPoint(vector);
		}
	}

	public void SetStats(float spd, Vector3 pos, Vector3 vel)
	{
		speed = spd;
		position = pos;
		velocity = vel;
	}
}
