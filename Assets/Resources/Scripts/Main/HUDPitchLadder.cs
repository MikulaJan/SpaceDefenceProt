using UnityEngine;
using UnityEngine.UI;

public class HUDPitchLadder : MonoBehaviour
{
	public RectTransform ladderPitch;

	public RectTransform ladderRoll;

	public RectTransform ladderUpMarker;

	public RectTransform ladderDownMarker;

	private Text[] ladderNumbers;

	private RectTransform[] ladderNumTransforms;

	private RectTransform[] ladderUpRungTransforms;

	private RectTransform[] ladderDownRungTransforms;

	private Vector3 playerForward = Vector3.zero;

	private Vector3 playerEuler = Vector3.zero; 

	private HUDValues hudValues;

	private Camera cam;

	private float lastFov = 60f;

	private float fov = 60f;

	private int increments;

	private void Awake()
	{
		hudValues = base.gameObject.GetComponentInParent<HUDValues>();
	}

	private void Start()
	{
		cam = hudValues.referenceCamera;
		if (ladderPitch != null)
		{
			fov = ((!hudValues.useOverrideFOV) ? cam.fieldOfView : hudValues.overrideFOV);
			CreatePitchLadder();
			HUDColor componentInParent = GetComponentInParent<HUDColor>();
			componentInParent.ReloadComponents();
			componentInParent.ResetToDefaultColor();
		}
		else
		{
			Debug.LogWarning("HUD Missing reference to pitch ladder pitch.");
		}
	}

	private void FixedUpdate()
	{
		if (!hudValues.useOverrideFOV)
		{
			fov = cam.fieldOfView;
			if (fov != lastFov)
			{
				AdjustLadderSpacing();
			}
		}
		if (ladderRoll != null && ladderPitch != null)
		{
			float z = playerEuler.z;
			ladderRoll.localEulerAngles = new Vector3(0f, 0f, 0f - z);
			Vector3 to = playerForward;
			bool flag = to.y < 0f;
			to.y = 0f;
			float num = Vector3.Angle(playerForward, to);
			if (flag)
			{
				num *= -1f;
			}
			ladderPitch.localPosition = new Vector3(0f, 0f - ScreenPosFromPitchAngle(num), 0f);
			for (int i = 0; i < ladderNumTransforms.Length; i++)
			{
				ladderNumTransforms[i].localEulerAngles = new Vector3(0f, 0f, z);
			}
		}
	}

	public void UpdateFOV(float newFOV)
	{
		fov = newFOV;
	}

	public void SetIncrements(int newInc)
	{
		increments = newInc;
	}

	private void AdjustLadderSpacing()
	{
		if (ladderUpRungTransforms != null && ladderDownRungTransforms != null)
		{
			for (int i = 0; i < ladderUpRungTransforms.Length; i++)
			{
				ladderUpRungTransforms[i].localPosition = new Vector3(0f, ScreenPosFromPitchAngle((i + 1) * increments), 0f);
			}
			for (int j = 0; j < ladderDownRungTransforms.Length; j++)
			{
				ladderDownRungTransforms[j].localPosition = new Vector3(0f, ScreenPosFromPitchAngle((j + 1) * -increments), 0f);
			}
		}
	}

	private void CreatePitchLadder()
	{
		if (ladderUpMarker != null && ladderDownMarker != null)
		{
			ladderUpRungTransforms = new RectTransform[90 / increments - 1];
			ladderDownRungTransforms = new RectTransform[90 / increments - 1];
			for (int i = 1; i < 90 / increments; i++)
			{
				RectTransform rectTransform = Object.Instantiate(ladderUpMarker);
				rectTransform.SetParent(ladderPitch);
				rectTransform.localPosition = new Vector3(0f, ScreenPosFromPitchAngle(i * increments), 0f);
				Text[] componentsInChildren = rectTransform.GetComponentsInChildren<Text>();
				Text[] array = componentsInChildren;
				foreach (Text text in array)
				{
					text.text = (i * increments).ToString("00");
				}
				ladderUpRungTransforms[i - 1] = rectTransform;
				rectTransform = Object.Instantiate(ladderDownMarker);
				rectTransform.SetParent(ladderPitch);
				rectTransform.localPosition = new Vector3(0f, ScreenPosFromPitchAngle(i * -increments), 0f);
				componentsInChildren = rectTransform.GetComponentsInChildren<Text>();
				Text[] array2 = componentsInChildren;
				foreach (Text text2 in array2)
				{
					text2.text = (i * increments).ToString("00");
				}
				ladderDownRungTransforms[i - 1] = rectTransform;
			}
			ladderNumbers = ladderPitch.GetComponentsInChildren<Text>();
			ladderNumTransforms = new RectTransform[ladderNumbers.Length];
			for (int l = 0; l < ladderNumbers.Length; l++)
			{
				ladderNumTransforms[l] = ladderNumbers[l].rectTransform;
			}
		}
		else
		{
			Debug.LogWarning("HUD Missing reference to pitch ladder up or down element.");
		}
	}

	private float ScreenPosFromPitchAngle(float angle)
	{
		float num = Screen.height;
		float num2 = angle + fov / 2f;
		float num3 = num2 * num / fov;
		return num3 - num / 2f;
	}

	public void UpdatePitchLadder(Vector3 forwardVector, Vector3 eulerAngles)
	{
		playerForward = forwardVector;
		playerEuler = eulerAngles;
	}
}
