using System;
using System.Collections.Generic;
using UnityEngine;

public enum eController
{
#if Oculus
    LTouch = OVRInput.Controller.LTouch,
    RTouch = OVRInput.Controller.RTouch,
#else
    LTouch = 0,
    RTouch = 1,
#endif
}

public enum eButtonTarget
{
    Fire1,
    Fire2,
    Fire3,
    Jump,
}

public enum eControllerButton
{
#if Oculus
    One = OVRInput.Button.One,
    Two = OVRInput.Button.Two,
    Thumbstick = OVRInput.Button.PrimaryThumbstick,
    IndexTrigger = OVRInput.Button.PrimaryIndexTrigger,
    HandTrigger = OVRInput.Button.PrimaryHandTrigger,
#else
    One = eButtonTarget.Fire1,
    Two = eButtonTarget.Jump,
    Thumbstick = eButtonTarget.Fire1,
    IndexTrigger = eButtonTarget.Fire3,
    HandTrigger = eButtonTarget.Fire2,
#endif
}


[Serializable]
public class ARVRHands
{
    public Transform hand;
}

public class ARVRInputManager : NonMonoSingleton<ARVRInputManager>
{
    private Dictionary<eController, ARVRHands> m_hands = new Dictionary<eController, ARVRHands>();
    private const float m_positionZValue = 0.7f;
#if Oculus
    private OculusTrankingItem m_trackingitem;
#endif

    public virtual void initialize(Transform leftHand, Transform rightHand)
    {
        var lHand = new ARVRHands();
        var rHand = new ARVRHands();

        lHand.hand = leftHand;
        rHand.hand = rightHand;

        setHand(eController.LTouch, lHand);
        setHand(eController.RTouch, rHand);
    }

    public void initTrackingItem(OculusTrankingItem item)
    {
#if Oculus
        m_trackingitem = item;
#endif
    }

    public void initHand(eController key, Transform hand)
    {
        if (!m_hands.ContainsKey(key))
            m_hands.Add(key, new ARVRHands());

        m_hands[key].hand = hand;

        updateDirection(key);
        updatePosition(key);
    }

    private void setHand(eController key, ARVRHands hand)
    {
        if (m_hands.ContainsKey(key))
            m_hands[key] = hand;
        else
            m_hands.Add(key, hand);

        updateDirection(key);
        updatePosition(key);
    }

    private void updateDirection(eController key)
    {
        if (!m_hands.ContainsKey(key))
            return;

        var dir = Vector3.zero;
        var pos = Vector3.zero;
#if Oculus
        var touch = (OVRInput.Controller)key;
        dir = OVRInput.GetLocalControllerRotation(touch) * Vector3.forward;
        dir = m_trackingitem.root.TransformDirection(dir);
#else
        dir = m_hands[key].hand.position - Camera.main.transform.position;
#endif
        m_hands[key].hand.forward = dir;
    }

    private void updatePosition(eController key)
    {
        if (!m_hands.ContainsKey(key))
            return;

        var pos = Vector3.zero;
#if Oculus
        var touch = (OVRInput.Controller)key;
        pos = OVRInput.GetLocalControllerPosition(touch);
#else

        pos = Input.mousePosition;
        pos.z = m_positionZValue;

        pos = Camera.main.ScreenToViewportPoint(pos);

#endif
        m_hands[key].hand.position = pos;
    }

    public void updateCenter()
    {
#if Oculus
        OVRManager.display.RecenterPose();
#endif
    }

    public void updateCenter(Transform target, Vector3 direction)
    {

        target.forward = target.rotation * direction;
    }


    public Vector3 getDirection(eController key)
    {
        updateDirection(key);

        if (m_hands.TryGetValue(key, out ARVRHands hand))
            return hand.hand.forward;
        else
            return Vector3.zero;
    }

    public Vector3 getPosition(eController key)
    {
        updatePosition(key);

        if (m_hands.TryGetValue(key, out ARVRHands hand))
            return hand.hand.position;
        else
            return Vector3.zero;
    }

    public virtual bool getController(eControllerButton virtualMask, eController key)
    {
#if Oculus
        return OVRInput.Get((OVRInput.Button)virtualMask, (OVRInput.Controller)key);
#else
        return Input.GetButton(((eButtonTarget)virtualMask).ToString());
#endif
    }

    public virtual bool getControllerDown(eControllerButton virtualMask, eController key)
    {
#if Oculus
        return OVRInput.GetDown((OVRInput.Button)virtualMask, (OVRInput.Controller)key);
#else
        return Input.GetButtonDown(((eButtonTarget)virtualMask).ToString());
#endif

    }

    public virtual bool getControllerUp(eControllerButton virtualMask, eController key)
    {
#if Oculus
        return OVRInput.GetUp((OVRInput.Button)virtualMask, (OVRInput.Controller)key);
#else
        return Input.GetButtonUp(((eButtonTarget)virtualMask).ToString());
#endif
    }

    public virtual float getControllerAxis(string axis, eController key)
    {
#if Oculus
        if ("Horizontal" == axis || "Mouse X" == axis)
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)key).x;
        else
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, (OVRInput.Controller)key).y;
#else
        return Input.GetAxis(axis);
#endif
    }

    public virtual void playVibration(eController controller)
    {
        return;
    }
}
