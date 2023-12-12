using Arena;
using SpaceDenfece;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public static class CameraManager
{
    private static List<GameCamera> cameras = new List<GameCamera>();

	private static List<ScaledCamera> sceneryCameras = new List<ScaledCamera>();

	private static bool paused;

	[SerializeField]
	public static CameraCategory activeCam = CameraCategory.Chase;

	public static void AddCamera(GameCamera cam)
	{
		cameras.Add(cam);
		UpdateCameraDepths();
	}

	public static void RemoveCamera(GameCamera cam)
	{
		cameras.Remove(cam);
		UpdateCameraDepths();
	}

	public static void UpdateCameraDepths()
	{
		if (sceneryCameras.Count > 0)
		{
			SetAllCamerasToDepthOnly(true);
		}
		else
		{
			SetAllCamerasToDepthOnly(false);
		}
	}

    public static void CycleSelectedCamera()
    {
        if (activeCam == CameraCategory.Cockpit)
        {
            activeCam = CameraCategory.Chase;
            ShowHUD(false);
            ShowMouse(false);
        }
        else if (activeCam == CameraCategory.Chase)
        {
            activeCam = CameraCategory.Cockpit;
            ShowHUD(true);
            ShowMouse(false);
        }
		ActivateSelectedCamera();
    }


    public static void ActivateSelectedCamera()
	{
		foreach (GameCamera camera in cameras)
		{
			if (camera.category == activeCam)
			{
				camera.EnableGameCam(true);
			}
			else
			{
				camera.EnableGameCam(false);
			}
		}
		if (sceneryCameras.Count <= 0)
		{
			return;
		}
		foreach (ScaledCamera sceneryCamera in sceneryCameras)
		{
			sceneryCamera.mainCamera = GetActiveCamera();
		}
	}

	private static void ShowHUD(bool show)
	{
		if (HUDValues.reference != null)
		{
			HUDValues.reference.gameObject.SetActive(show);
		}
	}

	public static GameCamera GetActiveGameCamera()
	{
		GameCamera result = null;
		foreach (GameCamera camera in cameras)
		{
			if (camera.category == activeCam)
			{
				result = camera;
			}
		}
		return result;
	}

	public static Camera GetActiveCamera()
	{
		Camera result = null;
		foreach (GameCamera camera in cameras)
		{
			if (camera.category == activeCam)
			{
				result = camera.Camera;
			}
		}
		return result;
	}

	public static void AddSceneryCamera(ScaledCamera scenery)
	{
		sceneryCameras.Add(scenery);
		SetAllCamerasToDepthOnly(true);
	}

	public static void SetAllCamerasToDepthOnly(bool depth)
	{
		foreach (GameCamera camera in cameras)
		{
			camera.UseDepthOnly(depth);
		}
	}
	public static void ShowMouse(bool value)
	{
		Debug.Log(value);
		if (value)
		{
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
		else
		{
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        
    }

	
    public static void EnablePauseMode(GameObject photoModeCam, ShipAI shipAI)
    {
		ShowHUD(false);
		ShowMouse(false);
        Time.timeScale = 0f;
        paused = true;
        shipAI.usePlayerInput = false;
		photoModeCam.GetComponent<Camera>().enabled = true;
        photoModeCam.GetComponent<FreeCamera>().enabled = true;
		photoModeCam.transform.position = shipAI.transform.position;
		photoModeCam.transform.rotation = shipAI.transform.rotation;
    }
    public static void DisablePauseMode(GameObject photoModeCam, ShipAI shipAI)
    {
        Time.timeScale = 1f;
        paused = false;
        shipAI.usePlayerInput = true;
        photoModeCam.GetComponent<Camera>().enabled = false;
        photoModeCam.GetComponent<FreeCamera>().enabled = false;
    }
}
