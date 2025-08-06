using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eFrameHit
{
    Prev,
    Cur,
    Next,
}


public class GazePointerCtrl : MonoBehaviour
{
    [SerializeField] Transform m_canvas;
    [SerializeField] Image m_gazeImage;
    [SerializeField] float m_uiScaleValue = 1.0f;
    [SerializeField] float m_gazeChargeTime = 3.0f;

    private Vector3 m_defaultScale;
    private bool m_isHitObject = false;
    private float m_lastGazeTime = 0.0f;
    private Dictionary<eFrameHit, GameObject> m_hitObjects = new Dictionary<eFrameHit, GameObject>();

    private void Start()
    {
        m_defaultScale = m_canvas.localScale;
        m_lastGazeTime = 0.0f;
    }

    public void updateGazePointer(float dt)
    {
        var baseTransform = Camera.main.transform;
        Vector3 dir = baseTransform.TransformPoint(Vector3.forward);

        Ray ray = new Ray(baseTransform.position, dir);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            m_canvas.localScale = m_defaultScale * m_uiScaleValue * hitInfo.distance;
            m_canvas.position = baseTransform.forward * hitInfo.distance;
            if (hitInfo.transform.tag == "GazeObj")
                m_isHitObject = true;

            if (m_hitObjects.ContainsKey(eFrameHit.Cur))
                addHitObject(eFrameHit.Prev, m_hitObjects[eFrameHit.Cur]);
            else
                addHitObject(eFrameHit.Prev, hitInfo.transform.gameObject);

            addHitObject(eFrameHit.Cur, hitInfo.transform.gameObject);
        }
        else
        {
            m_canvas.localScale = m_defaultScale * m_uiScaleValue;
            m_canvas.position = baseTransform.position + dir;
        }

        m_canvas.forward = baseTransform.forward * -1.0f;

        updateGazeTime(dt);
    }

    private void updateGazeTime(float dt)
    {
        if (m_isHitObject)
        {
            if (m_hitObjects[eFrameHit.Cur] == m_hitObjects[eFrameHit.Prev])
                m_lastGazeTime += dt;
            else
                addHitObject(eFrameHit.Prev, m_hitObjects[eFrameHit.Cur]);
        }
        else
        {
            m_lastGazeTime = 0.0f;
            m_hitObjects.Remove(eFrameHit.Prev);
        }

        updateGazeRatio();
    }

    private void updateGazeRatio()
    {
        m_lastGazeTime = Mathf.Clamp(m_lastGazeTime, 0.0f, m_gazeChargeTime);
        m_gazeImage.fillAmount = m_lastGazeTime/m_gazeChargeTime;

        m_isHitObject = false;
        m_hitObjects.Remove(eFrameHit.Cur);
    }

    private void addHitObject(eFrameHit key, GameObject obj)
    {
        if (!m_hitObjects.ContainsKey(key))
            m_hitObjects.Add(key, obj);
        else
            m_hitObjects[key] = obj;
    }
}
