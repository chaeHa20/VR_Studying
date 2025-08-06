using Oculus.Interaction.Samples;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VoxelGenerator : MonoBehaviour
{
    [SerializeField] VoxelObject m_voxelPrefabs;
    [SerializeField] GameObject m_parent;
    [SerializeField] GameObject m_poolParent;
    [SerializeField] Transform m_crosshair;
    [SerializeField] int m_voxelPoolSize = 20;

    [SerializeField] List<VoxelObject> m_voxelPool = new List<VoxelObject>();

    private void Start()
    {
        for (int index = 0; index < m_voxelPoolSize; index++)
        {
            VoxelObject voxel = Instantiate<VoxelObject>(m_voxelPrefabs);
            voxel.transform.SetParent(m_poolParent.transform);
            voxel.gameObject.name = "voxel_" + (m_voxelPool.Count + 1).ToString();
            voxel.gameObject.SetActive(false);

            m_voxelPool.Add(voxel);
        }
    }

    private void drawCrosshair(eController key)
    {
        DrawCrossHairManager.instance.drawCrosshir(m_crosshair, true, key);
    }

    public void updateGenerator()
    {
#if Oculus
        var isRight = ARVRInputManager.instance.getController(eControllerButton.One, eController.RTouch);
        var isLeft = ARVRInputManager.instance.getController(eControllerButton.One, eController.LTouch);
        if (isRight || isLeft)
        {
            var key = isRight ? eController.RTouch : eController.LTouch;
            drawCrosshair(key);
#else
        if (Input.GetMouseButtonUp(0))
        {
#endif

#if Oculus
            Ray ray = new Ray(ARVRInputManager.instance.getPosition(key), ARVRInputManager.instance.getDirection(key));
#else
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (0 < m_voxelPool.Count)
                {
                    VoxelObject voxel = m_voxelPool[0];
                    voxel.transform.SetParent(m_parent.transform);
                    voxel.transform.localScale = Vector3.one;
                    voxel.transform.position = hitInfo.point;
                    voxel.gameObject.SetActive(true);
                    voxel.initObject(this);

                    m_voxelPool.RemoveAt(0);
                }
            }
        }
    }

    public void pushObject(VoxelObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(m_poolParent.transform);

        if (m_voxelPool.Count < m_voxelPoolSize)
            m_voxelPool.Add(obj);
    }
}
