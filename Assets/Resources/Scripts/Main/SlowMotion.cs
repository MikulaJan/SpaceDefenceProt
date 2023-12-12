using UnityEngine;

public class SlowMotionController : MonoBehaviour
{
    private bool isSlowMotion = false;
    private float originalFixedDeltaTime;

    [SerializeField]
    [Range(0.1f, 0.5f)]
    private float timeslow;

    void Start()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        // Toggle slow motion when 'T' key is pressed
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isSlowMotion = !isSlowMotion;

            if (isSlowMotion)
            {
                // Enable slow motion
                Time.timeScale = timeslow; // Adjust the time scale to your preferred slow-motion effect
                Time.fixedDeltaTime = Time.timeScale * originalFixedDeltaTime;
            }
            else
            {
                // Disable slow motion
                Time.timeScale = 1f;
                Time.fixedDeltaTime = originalFixedDeltaTime;
            }
        }
    }
}
