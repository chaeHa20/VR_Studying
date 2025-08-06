using System.Collections.Generic;
using UnityEngine;

public class VideoScene : Scene
{
    [SerializeField] CameraRotator m_camRotator;
    [SerializeField] GazePointerCtrl m_gazePointerCtrl;
    [SerializeField] List<VideoFrame> m_vframes = new List<VideoFrame>();

    protected override void update(float dt)
    {
        if (null != m_camRotator)
            m_camRotator.updateCameraRotate(dt);
        if (null != m_gazePointerCtrl)
            m_gazePointerCtrl.updateGazePointer(dt);

        updateVideo();
    }

    private void updateVideo()
    {
        if (Input.GetKeyDown(KeyCode.S))
            foreach (var vframe in m_vframes)
                vframe.stopVideo();

        if (Input.GetKeyDown(KeyCode.Space))
            foreach (var vframe in m_vframes)
                vframe.playVideo();

        if (Input.GetKeyDown(KeyCode.LeftBracket))
            foreach (var vframe in m_vframes)
                vframe.playPrevVideoClip();

        if (Input.GetKeyDown(KeyCode.RightBracket))
            foreach (var vframe in m_vframes)
                vframe.playNextVideoClip();
    }
}
