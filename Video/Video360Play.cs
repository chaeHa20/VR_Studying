using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video360Play : VideoFrame
{
    [SerializeField] List<VideoClip> m_clips = new List<VideoClip>();

    private int m_curClipIndex = -1;

    protected override void Start()
    {
        m_curClipIndex = 0;
        m_videoPlayer.clip = m_clips[m_curClipIndex];

        base.Start();
    }

    public override void playPrevVideoClip()
    {
        if (null == m_videoPlayer)
            return;
        if (0 == m_curClipIndex)
            return;

        m_curClipIndex--;

        m_videoPlayer.Stop();
        m_videoPlayer.clip = m_clips[m_curClipIndex];
        m_videoPlayer.Play();
    }

    public override void playNextVideoClip()
    {
        if (null == m_videoPlayer)
            return;
        if (m_clips.Count - 1 == m_curClipIndex)
            return;

        m_curClipIndex++;
        

        m_videoPlayer.Stop();
        m_videoPlayer.clip = m_clips[m_curClipIndex];
        m_videoPlayer.Play();
    }
}
