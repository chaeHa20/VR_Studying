using System;
using System.Collections;
using UnityEngine;


public class ARVRCoroutineManager : MonoSingleton<ARVRCoroutineManager>
{
    private float m_duration = 1.0f;
    private float m_frequency = 1.0f;
    private float m_amplitude = 1.0f;
    private eController m_hand;

    private Coroutine m_coroutine = null;

    public void playVibration(eController hand)
    {
        playVibration(0.06f, 1.0f, 1.0f, hand);
    }

    public void waitDestroyed(float waitTime, Action callback)
    {
        StartCoroutine(coDestroyed(waitTime, callback));
    }

    private void playVibration(float duration, float frequency, float amplitudem, eController hand)
    {
        m_duration = duration;
        m_frequency = frequency;
        m_amplitude = amplitudem;
        m_hand = hand;

#if Oculus
        stop();
        m_coroutine = StartCoroutine(coVibrationCoroutine());
#endif
    }

    private void stop()
    {
        if (null != m_coroutine)
        {
            StopCoroutine(m_coroutine);
            m_coroutine = null;
        }
    }

    IEnumerator coVibrationCoroutine()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < m_duration)
        {
            elapsedTime += Time.deltaTime;
            OVRInput.SetControllerVibration(m_frequency, m_amplitude, (OVRInput.Controller)m_hand);

            yield return null;
        }

        OVRInput.SetControllerVibration(0, 0, (OVRInput.Controller)m_hand);
    }

    IEnumerator coDestroyed(float destroyedTime, Action callback)
    {
        yield return new WaitForSeconds(destroyedTime);

        callback?.Invoke();
    }
}
