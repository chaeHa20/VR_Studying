using UnityEngine;

public class DrawCrossHairManager : MonoSingleton<DrawCrossHairManager>
{
    private const float m_scaleWeight = 0.02f;
    private const float m_oculusScaleWeight = 0.005f;

    private Ray m_ray;
    private Plane m_plane = new Plane(Vector3.up, 0);
    private Vector3 m_originScale = Vector3.one;

    public Ray ray => m_ray;

    public void drawCrosshir(Transform crosshair, bool isHand, eController hand)
    {
        var scaleWeight = 1.0f;
#if Oculus
        scaleWeight = m_oculusScaleWeight;
#else
        scaleWeight = m_scaleWeight;
#endif

        if (isHand)
            m_ray = getIsHandRay(hand);
        else
            m_ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if(m_plane.Raycast(m_ray, out float distance))
        {
            crosshair.position = m_ray.GetPoint(distance);
            crosshair.forward = -Camera.main.transform.forward;

            crosshair.localScale = m_originScale * scaleWeight * Mathf.Max(1.0f, distance);
        }
        else
        {
            crosshair.position = m_ray.origin + m_ray.direction * 100.0f;
            crosshair.forward = -Camera.main.transform.forward;

            var outDistance = (crosshair.position - m_ray.origin).magnitude;
            crosshair.localScale = m_originScale * scaleWeight * Mathf.Max(1.0f, outDistance);
        }
    }
    private Ray getIsHandRay(eController hand)
    {
#if Oculus
            return new Ray(ARVRInputManager.instance.getPosition(hand), ARVRInputManager.instance.getDirection(hand));
#else
            return Camera.main.ScreenPointToRay(Input.mousePosition);
#endif
    }
}
