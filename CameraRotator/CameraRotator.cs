using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    private Vector3 m_angle = Vector3.zero;
    private float m_sensitivity = 200.0f;

    private void Start()
    {
        m_angle = Camera.main.transform.eulerAngles;
        m_angle.x *= -1;
    }

    public void updateCameraRotate(float dt)
    {
        float posX = Input.GetAxis("Mouse Y");
        float posY = Input.GetAxis("Mouse X");

        var oldX = m_angle.x;
        var oldY = m_angle.y;

        m_angle.x += posX * m_sensitivity * dt;
        m_angle.y += posY * m_sensitivity * dt;

        m_angle.x = Mathf.Clamp(m_angle.x, -90, 90);

        if (oldX != m_angle.x || oldY != m_angle.y)
            updateCameraAngle();
    }

    private void updateCameraAngle()
    {
        transform.eulerAngles = new Vector3(m_angle.x, m_angle.y, transform.eulerAngles.z);
    }
}
