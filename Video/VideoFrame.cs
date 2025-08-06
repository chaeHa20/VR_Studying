using UnityEngine;
using UnityEngine.Video;

public class VideoFrame : MonoBehaviour
{
    [SerializeField] protected VideoPlayer m_videoPlayer;

    protected virtual void Start()
    {
        if (null != m_videoPlayer)
            m_videoPlayer.Stop();
    }

    public void stopVideo()
    {
        if (null == m_videoPlayer)
            return;

        m_videoPlayer.Stop();
    }

    public void playVideo()
    {
        if (null == m_videoPlayer)
            return;

        if (m_videoPlayer.isPlaying)
            m_videoPlayer.Pause();
        else
            m_videoPlayer.Play();
    }

    public virtual void playPrevVideoClip()
    {

    }

    public virtual void playNextVideoClip()
    {

    }
}
