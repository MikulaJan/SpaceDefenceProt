using UnityEngine;
using UnityEngine.UI;

public class HUDTargetBox : MonoBehaviour
{
	private ITargetable assignedTarget;

	public Text targetType;

	public Text targetName;

	public Text distance;

	public Text priority;

	public Text ally;
	 
	public Image lockDiamond;

	public Image targetBox;

	public Color lockedColor = Color.red;

	public Color friendlyColor = Color.blue;

	private Text[] allText;

	private Image[] allImages;

	private HUDColor targetColor;

	private HUDValues parentHUD;

	private HUDFlightData flightData;

	private void Awake()
	{
		targetColor = GetComponent<HUDColor>();
		allText = GetComponentsInChildren<Text>();
		allImages = GetComponentsInChildren<Image>();
	}

	private void Start()
	{
		ShowWholeTargetBox(false);
	}

	private void Update()
	{
		if (assignedTarget != null)
		{
			flightData = parentHUD.GetFlightData();
			float num = Vector3.Distance(flightData.position, assignedTarget.Position) * 3.28084f;
			distance.text = num.ToString("00000");
			targetType.text = assignedTarget.TypeName;
			targetName.text = assignedTarget.Name;
			if (CalculateDisplayPosition())
			{
				UpdateTargetBoxStates();
				lockDiamond.enabled = assignedTarget.Locked;
				priority.enabled = assignedTarget.PriorityTarget && !assignedTarget.Friendly;
				ally.enabled = assignedTarget.Friendly;
			}
		}
		else
		{
			DestroyTargetBox();
		}
	}

	private bool CalculateDisplayPosition()
	{
		bool result = false;
		Vector3 rhs = assignedTarget.Position - flightData.position;
		float num = Vector3.Dot(flightData.forwardVector, rhs);
		if (num > 0f)
		{
			ShowWholeTargetBox(true);
			Vector3 position = Camera.main.WorldToScreenPoint(assignedTarget.Position);
			position.z = 0f;
			base.transform.position = position;
			result = true;
		}
		else
		{
			ShowWholeTargetBox(false);
		}
		return result;
	}

	private void UpdateTargetBoxStates()
	{
		if (assignedTarget.Friendly)
		{
			ShowTextOnly(true, distance);
			targetColor.SetColor(friendlyColor);
			if (assignedTarget.Targeted)
			{
				int num = (int)(Time.time * 6f) % 2;
				ShowBoxOnly(num == 0);
			}
			else
			{
				ShowBoxOnly(true);
			}
		}
		else if (assignedTarget.Locked)
		{
			ShowBoxOnly(true);
			targetBox.color = lockedColor;
			lockDiamond.color = lockedColor;
		}
		else if (assignedTarget.Targeted)
		{
			int num2 = (int)(Time.time * 6f) % 2;
			ShowBoxOnly(num2 == 0);
			targetColor.ResetToDefaultColor();
		}
		else
		{
			ShowTextOnly(false, targetType);
			targetColor.ResetToDefaultColor();
		}
	}

	private void ShowWholeTargetBox(bool show)
	{
		ShowBoxOnly(show);
		ShowTextOnly(show);
	}

	private void ShowBoxOnly(bool show)
	{
		Image[] array = allImages;
		foreach (Image image in array)
		{
			image.enabled = show;
		}
	}

	private void ShowTextOnly(bool show, Text exception)
	{
		ShowTextOnly(show);
		exception.enabled = !exception;
	}

	private void ShowTextOnly(bool show)
	{
		Text[] array = allText;
		foreach (Text text in array)
		{
			text.enabled = show;
		}
	}

	public void AssignTarget(ITargetable newTarget)
	{
		assignedTarget = newTarget;
	}

	public void AssignParentHUD(HUDValues newHUD)
	{
		parentHUD = newHUD;
	}

	public void DestroyTargetBox()
	{
		Object.Destroy(base.gameObject);
	}
}
