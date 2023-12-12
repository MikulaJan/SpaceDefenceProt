using System;
using UnityEngine;

namespace Arena
{
	public class GameCameraController : MonoBehaviour
	{
		[SerializeField]
		private MouseFlightController mouseFlight;

		public Vector3 chaseCameraPosition = new Vector3(0f, 9f, -30f);

		private Transform photoModeCam;

		private bool loadoutMode;

		private bool paused;

		public MouseFlightController MouseFlightController
		{
			get
			{
				return mouseFlight;
			}
		}

		public event Action OnEnterLoadoutMode;

		public event Action OnLeftLoadoutMode;

		private void Start()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void Update()
		{
			if (!loadoutMode && Input.GetKeyDown(KeyCode.P))
			{
				paused = !paused;
				if (paused)
				{
					EnablePauseMode();
				}
				else
				{
					DisablePauseMode();
				}
			}
		}

		private void EnablePauseMode()
		{
			Time.timeScale = 0f;
			paused = true;
			mouseFlight.EnableInput = false;
			photoModeCam = mouseFlight.RemoveCamera();
			photoModeCam.GetComponent<FreeCamera>().enabled = true;
		}
		private void DisablePauseMode()
        {
            Time.timeScale = 1f;
            paused = false;
            mouseFlight.EnableInput = true;
            mouseFlight.AssignCamera(photoModeCam, chaseCameraPosition);
            photoModeCam.GetComponent<FreeCamera>().enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            photoModeCam = null;
        }
    }
}
