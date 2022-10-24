using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public WeaponManager weaponManager;

    [Header("Live Movement Data")]
    public float yaw = 0.0f;
    public float pitch = 0.0f;

    public PlayerSettings settings = new PlayerSettings();

    void Update()
    {
        if (!GameManager.Started || GameManager.Paused)
            return;

        UpdatePosition(settings.horizontalSpeed * Input.GetAxis("Mouse X"), settings.verticalSpeed * Input.GetAxis("Mouse Y"));
    }

    public void UpdatePosition(float axisX, float axisY)
    {
        yaw += axisX;
        pitch -= axisY;

        yaw = Mathf.Clamp(yaw, settings.minYaw, settings.maxYaw);
        pitch = Mathf.Clamp(pitch, settings.minPitch, settings.maxPitch);

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}

[System.Serializable]
public class PlayerSettings
{
    [Header("Movement Speed")]
    public float horizontalSpeed = 2.0f;
    public float verticalSpeed = 2.0f;

    [Header("Movement Limits")]
    public float maxPitch = 30f;
    public float minPitch = -15f;

    public float maxYaw = 45f;
    public float minYaw = -45;
}
