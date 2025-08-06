using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR;

public class OculusTrankingItem : MonoBehaviour
{
    [SerializeField] Transform m_root;
    [SerializeField] Transform m_lhand;
    [SerializeField] Transform m_rhand;    

    public Transform root => m_root;

    private void Start()
    {
        ARVRInputManager.instance.initTrackingItem(this);
        ARVRInputManager.instance.initialize(m_lhand, m_rhand);
    }
}
