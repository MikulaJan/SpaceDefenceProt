﻿using UnityEngine;
using UnityEngine.UI;

namespace SpaceDenfece.UI
{
    /// <summary>
    /// Updates the position of this GameObject to reflect the position of the mouse
    /// when the player ship is using mouse input. Otherwise, it just hides it.
    /// </summary>
    public class MouseCrosshairUI : MonoBehaviour
    {
        private Image crosshair;

        private void Awake()
        {
            crosshair = GetComponent<Image>();
        }

        private void Update()
        {
            if (crosshair != null)
            {
                crosshair.transform.position = Input.mousePosition;
            }
        }
    }
}
