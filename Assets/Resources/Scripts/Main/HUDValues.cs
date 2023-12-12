using UnityEngine;
using UnityEngine.UI;

public class HUDValues : MonoBehaviour
{
	[Header("HUD Options")]
	[Tooltip("Camera for which all the HUD elements will be relative from.\n\nWill default to the main camera if not defined.")]
	public Camera referenceCamera;

	[Tooltip("Set this to override the reference camera's field of view. This is used for the spacing on the pitch ladder.\n\nThis should only be used when the HUD and player camera are separate.\n\nWhen false, the HUD will use the camera's field of view.")]
	public bool useOverrideFOV;

	[Tooltip("FOV to use instead of the camera's.\n\nRequires \"Use Override\" FOV to be true.")]
	public float overrideFOV = 60f;

	[Tooltip("Spacing of degree increments on the pitch ladder. (Every X degrees has a ladder increment.)")]
	public int ladderIncrements = 10;

	[Header("Display Text")]
	public Text speedText;

	public Text altitudeText; 

	public Text timerText;

	public Text scoreText;

	public Text headingText;

	public Text alphaText;

	public Text selWeaponText;

	public Text brakeText;

	[Header("HUD Elements")]
	public RectTransform headingTape;

	public HUDPitchLadder pitchLadder;

	public Image throttleBar;

	public HUDFlightPathMarker flightPathMarker;

	public RectTransform WarningBox;

	public RectTransform DisconnectBox;

	public RectTransform GearDownBox;

	private HUDFlightData flightData;

	public static HUDValues reference;

	private void Awake()
	{
		if (referenceCamera == null)
		{
			Debug.LogWarning("HUD not assigned player camera. Defaulting to main camera.");
			referenceCamera = Camera.main;
		}
		if (reference == null)
		{
			reference = this;
		}
		pitchLadder.SetIncrements(ladderIncrements);
	}

	private void Start()
	{
		flightPathMarker.SetCamera(referenceCamera);
		ShowWarningBox(false);
	}

	private void Update()
	{
		if (timerText != null)
		{
			timerText.text = Time.time.ToString("000000.0");
		}
		else
		{
			Debug.LogWarning("HUD timer not assigned.");
		}
	}

	public void GearDownPct(float pct)
	{
		if (GearDownBox != null)
		{
			GearDownBox.gameObject.SetActive(pct > 0f);
		}
		else
		{
			Debug.LogWarning("Gear down box not assigned.");
		}
	}

	public void ShowWarningBox(bool show)
	{
		if (WarningBox != null)
		{
			WarningBox.gameObject.SetActive(show);
		}
		else
		{
			Debug.LogWarning("HUD warning box not assigned.");
		}
	}

	public void ShowDisconnect(bool show)
	{
		if (DisconnectBox != null)
		{
			DisconnectBox.gameObject.SetActive(show);
		}
		else
		{
			Debug.LogWarning("FCS Disconnect box not assigned.");
		}
	}

	public void UpdateFlightParams(float throttle, float speed, float altitude, float alpha, Vector3 forwardVector, Vector3 eulerAngles, Vector3 position, Vector3 velocity)
	{
		if (throttleBar != null)
		{
			throttleBar.rectTransform.localScale = new Vector3(1f, throttle, 1f);
		}
		else
		{
			Debug.LogWarning("HUD throttle bar not assigned.");
		}
		if (speedText != null)
		{
			speedText.text = speed.ToString("0000");
		}
		else
		{
			Debug.LogWarning("HUD speed text not assigned.");
		}
		if (altitudeText != null)
		{
			altitudeText.text = altitude.ToString("00000");
		}
		else
		{
			Debug.LogWarning("HUD altitude text not assigned.");
		}
		if (alphaText != null)
		{
			alphaText.text = alpha.ToString("0.0");
		}
		else
		{
			Debug.LogWarning("HUD alpha text not assigned.");
		}
		if (headingText != null)
		{
			headingText.text = eulerAngles.y.ToString("000");
		}
		else
		{
			Debug.LogWarning("HUD heading text not assigned.");
		}
		if (headingTape != null)
		{
			SlideHeadingTape(eulerAngles.y);
		}
		else
		{
			Debug.LogWarning("HUD heading tape not assigned.");
		}
		if (pitchLadder != null)
		{
			pitchLadder.UpdatePitchLadder(forwardVector, eulerAngles);
		}
		else
		{
			Debug.LogWarning("HUD pitch ladder not assigned.");
		}
		if (flightPathMarker != null)
		{
			flightPathMarker.UpdateFPM(position, velocity);
		}
		else
		{
			Debug.LogWarning("HUD flight path marker not assigned.");
		}
		flightData.throttle = throttle;
		flightData.speed = speed;
		flightData.altitude = altitude;
		flightData.forwardVector = forwardVector;
		flightData.eulerAngles = eulerAngles;
		flightData.position = position;
		flightData.velocity = velocity;
	}

	public void UpdateSelectedWeapon(string name, int ammo)
	{
		if (selWeaponText != null)
		{
			selWeaponText.text = ammo + " " + name;
		}
		else
		{
			Debug.LogWarning("Selected weapon text not assigned.");
		}
	}

	public HUDFlightData GetFlightData()
	{
		return flightData;
	}

	private void SlideHeadingTape(float heading)
	{
		if (heading > 180f)
		{
			heading -= 360f;
		}
		headingTape.localPosition = new Vector3((0f - heading) * 1.33f, 0f);
	}

	public void UpdateScore(int score)
	{
		scoreText.text = score.ToString("000000");
	}

	public void UpdateBrakes(float brakePct)
	{
		if (brakeText != null)
		{
			brakeText.enabled = brakePct > 0.1f;
		}
		else
		{
			Debug.LogWarning("Brake text not assigned.");
		}
	}
}
