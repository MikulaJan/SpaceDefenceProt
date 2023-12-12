using UnityEngine;

public class MainMenuStates : MonoBehaviour
{
	public RectTransform scenarioPanel;

	public RectTransform settingPanel;

	public RectTransform quitConfirmPanel;

	public bool AtReset { get; set; }

	public void Start()
	{
		ResetPanels();
	}

	public void ResetPanels()
	{
		scenarioPanel.gameObject.SetActive(false);
		settingPanel.gameObject.SetActive(false);
		quitConfirmPanel.gameObject.SetActive(false);
		AtReset = true;
	}

	public void ScenarioClicked()
	{
		scenarioPanel.gameObject.SetActive(true);
		settingPanel.gameObject.SetActive(false);
		quitConfirmPanel.gameObject.SetActive(false);
		AtReset = false;
	}

	public void SettingsClicked()
	{
		scenarioPanel.gameObject.SetActive(false);
		settingPanel.gameObject.SetActive(true);
		quitConfirmPanel.gameObject.SetActive(false);
		AtReset = false;
	}

	public void QuitClicked()
	{
		scenarioPanel.gameObject.SetActive(false);
		settingPanel.gameObject.SetActive(false);
		quitConfirmPanel.gameObject.SetActive(true);
		AtReset = false;
	}
}
