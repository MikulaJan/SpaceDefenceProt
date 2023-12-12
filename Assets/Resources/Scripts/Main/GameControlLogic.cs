using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControlLogic : MonoBehaviour
{
	public Image menuBackdrop;

	public Canvas MainMenuCanvas;

	public MainMenuStates mainMenuStates;

	public Image controllerImage;

	private HUDValues hud;

	private bool atMainMenu = true;

	private bool menuVisible = true;

	public static GameControlLogic reference;

	private GameObject controllerImageGameObject;

	public bool AtMainMenu
	{
		get
		{
			return atMainMenu;
		}
	}

	public bool MenuVisible
	{
		get
		{
			return menuVisible;
		}
	}

	public void Awake()
	{
		reference = this;
		controllerImageGameObject = controllerImage.gameObject;
	}

	public void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelLoad;
	}

	public void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelLoad;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			mainMenuStates.ResetPanels();
			if (!atMainMenu)
			{
				menuVisible = !menuVisible;
				MainMenuCanvas.gameObject.SetActive(menuVisible);
				if (menuBackdrop != null && !atMainMenu)
				{
					menuBackdrop.gameObject.SetActive(menuVisible);
				}
			}
		}
		if (menuVisible)
		{
			controllerImageGameObject.SetActive(!atMainMenu && menuVisible && mainMenuStates.AtReset);
		}
	}

	public void LoadScene(string sceneName)
	{
		atMainMenu = false;
		menuVisible = false;
		SceneManager.LoadScene(sceneName);
		MainMenuCanvas.gameObject.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void OnLevelLoad(Scene scene, LoadSceneMode mode)
	{
		CameraManager.UpdateCameraDepths();
		hud = HUDValues.reference;
	}
}
