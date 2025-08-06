using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : Scene
{
    [SerializeField] CameraRotator m_camRotator;
    [SerializeField] VoxelGenerator m_voxelGenerator;

    protected override void update(float dt)
    {
        if (null != m_camRotator)
        m_camRotator.updateCameraRotate(dt);
        if (null != m_voxelGenerator)
        {
            m_voxelGenerator.updateGenerator();
        }
    }
}
