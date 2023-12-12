using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDColor : MonoBehaviour
{
	public Color hudColor = Color.green;

	private MaskableGraphic[] allGraphics;

	public List<MaskableGraphic> excludeList;

	private Color startingColor = Color.green;

	private void Start()
	{
		startingColor = hudColor;
		ReloadComponents();
		ResetToDefaultColor();
	}

	public void SetColor(Color color, List<MaskableGraphic> customExclusion)
	{
		MaskableGraphic[] array = allGraphics;
		foreach (MaskableGraphic maskableGraphic in array)
		{
			if (!customExclusion.Contains(maskableGraphic))
			{
				maskableGraphic.color = color;
			}
		} 
	}

	public void SetColor(Color color)
	{
		SetColor(color, excludeList);
	}

	public void ReloadComponents()
	{
		allGraphics = GetComponentsInChildren<MaskableGraphic>();
	}

	public void ResetToDefaultColor()
	{
		SetColor(startingColor);
	}
}
