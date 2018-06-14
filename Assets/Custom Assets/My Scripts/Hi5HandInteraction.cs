using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Basic HandInteraction for grabbing and pinching using Hi5.
/// Assign this script as a component to the root (or any root tranform of the fingers, ex. Robot_LeftHand works) of the Hi5 rig.
/// Make 2 components of this script if you want to track both hands.
/// Reference this component in your code and reference the public variables m_IsGrabbing, m_IsPinching, or centerPointOfPinch in your script.
/// Make sure Left hand is marked m_IsLeftHand as True and Right hand is marked m_IsLeftHand as False.
/// </summary>
public class Hi5HandInteraction : MonoBehaviour
{
    /// <summary>
    /// Variables to be modified in the Inspector
    /// </summary>
    public bool m_IsLeftHand = true;
    public bool m_AutoAssignHandRig = true;
    public bool m_CalculateCenterPointOfPinch = true;

    public int m_GrabPercentage = 50;
    public int m_GrabReleasePercentage = 40;

    public int m_PinchPercentage = 80;
    public int m_PinchReleasePercentage = 70;

    /// <summary>
    /// Optional Variables that may be assigned if m_AutoAssignHandRig is false
    /// </summary>
    public List<Transform> m_ThumbFingerTransforms;
    public List<Transform> m_IndexFingerTransforms;
    public List<Transform> m_MiddleFingerTransforms;
    public List<Transform> m_RingFingerTransforms;
    public List<Transform> m_PinkyFingerTransforms;

    /// <summary>
    /// Variables that do not need to be assigned in the Inspector, only public for display in Inspector
    /// </summary>
    public bool m_IsGrabbing = false;
    public bool m_IsPinching = false;

    public float m_IndexFingerAngle;
    public float m_MiddleFingerAngle;
    public float m_RingFingerAngle;
    public float m_PinkyFingerAngle;

    public float m_IndexPercentage;
    public float m_MiddlePercentage;
    public float m_RingPercentage;
    public float m_PinkyPercentage;

    // Only updated when m_CalculateCenterPointOfPinch is true
    public float pinchDistance;
    public Vector3 centerPointOfPinch; // Reference this variable in another script if you want to know center point of pinch

    /// <summary>
    /// Hardcoded min/max angles of fingers
    /// </summary>
    private float m_IndexAngleMin = 130f;
    private float m_IndexAngleMax = 180f;

    private float m_MiddleAngleMin = 67f;
    private float m_MiddleAngleMax = 180f;

    private float m_RingAngleMin = 50f;
    private float m_RingAngleMax = 180f;

    private float m_PinkyAngleMin = 32f;
    private float m_PinkyAngleMax = 180f;

    private string PrefixName = "Human_";
    private string RighthandInPrefix = "RightHand";
    private string LefthandInPrefix = "LeftHand";
    private string RightInhandInPrefix = "RightHand";
    private string LeftInhandInPrefix = "LeftHand";

    private void Start()
    {
        if (m_AutoAssignHandRig)
        {
            AssignPNJoints();
        }
    }

    /// <summary>
    /// Assign PN Robot joints
    /// </summary>
    private void AssignPNJoints()
    {
        Transform rightHand = transform.Search(PrefixName + RighthandInPrefix);
        Transform leftHand = transform.Search(PrefixName + LefthandInPrefix);

        if (!m_IsLeftHand)
        {
            AssignPNHandJoints(rightHand, PrefixName + RighthandInPrefix, PrefixName + RightInhandInPrefix);
        }
        else
        {
            AssignPNHandJoints(leftHand, PrefixName + LefthandInPrefix, PrefixName + LeftInhandInPrefix);
        }
    }

    private void AssignPNHandJoints(Transform handBase, string handPrefix, string handInPrefix)
    {
        m_ThumbFingerTransforms.Add(handBase.Search(handPrefix + "Thumb1"));
        m_ThumbFingerTransforms.Add(m_ThumbFingerTransforms[0].Find(handPrefix + "Thumb2"));
        m_ThumbFingerTransforms.Add(m_ThumbFingerTransforms[1].Find(handPrefix + "Thumb3"));
        m_ThumbFingerTransforms.Add(m_ThumbFingerTransforms[2].Find(handPrefix + "Thumb4"));
        
        m_IndexFingerTransforms.Add(handBase.Search(handInPrefix + "Index1"));
        m_IndexFingerTransforms.Add(m_IndexFingerTransforms[0].Find(handInPrefix + "Index2"));
        m_IndexFingerTransforms.Add(m_IndexFingerTransforms[1].Find(handInPrefix + "Index3"));
        m_IndexFingerTransforms.Add(m_IndexFingerTransforms[2].Find(handInPrefix + "Index4"));

        m_MiddleFingerTransforms.Add(handBase.Search(handInPrefix + "Middle1"));
        m_MiddleFingerTransforms.Add(m_MiddleFingerTransforms[0].Find(handInPrefix + "Middle2"));
        m_MiddleFingerTransforms.Add(m_MiddleFingerTransforms[1].Find(handInPrefix + "Middle3"));
        m_MiddleFingerTransforms.Add(m_MiddleFingerTransforms[2].Find(handInPrefix + "Middle4"));

        m_RingFingerTransforms.Add(handBase.Search(handInPrefix + "Ring1"));
        m_RingFingerTransforms.Add(m_RingFingerTransforms[0].Find(handInPrefix + "Ring2"));
        m_RingFingerTransforms.Add(m_RingFingerTransforms[1].Find(handInPrefix + "Ring3"));
        m_RingFingerTransforms.Add(m_RingFingerTransforms[2].Find(handInPrefix + "Ring4"));

        m_PinkyFingerTransforms.Add(handBase.Search(handInPrefix + "Pinky1"));
        m_PinkyFingerTransforms.Add(m_PinkyFingerTransforms[0].Find(handInPrefix + "Pinky2"));
        m_PinkyFingerTransforms.Add(m_PinkyFingerTransforms[1].Find(handInPrefix + "Pinky3"));
        m_PinkyFingerTransforms.Add(m_PinkyFingerTransforms[2].Find(handInPrefix + "Pinky4"));
    }

    void Update()
    {
        m_IndexFingerAngle = GetAngle(m_IndexFingerTransforms);
        m_MiddleFingerAngle = GetAngle(m_MiddleFingerTransforms);
        m_RingFingerAngle = GetAngle(m_RingFingerTransforms);
        m_PinkyFingerAngle = GetAngle(m_PinkyFingerTransforms);

        m_IndexPercentage = NormalizedAngle(m_IndexAngleMin, m_IndexAngleMax, m_IndexFingerAngle);
        m_MiddlePercentage = NormalizedAngle(m_MiddleAngleMin, m_MiddleAngleMax, m_MiddleFingerAngle);
        m_RingPercentage = NormalizedAngle(m_RingAngleMin, m_RingAngleMax, m_RingFingerAngle);
        m_PinkyPercentage = NormalizedAngle(m_PinkyAngleMin, m_PinkyAngleMax, m_PinkyFingerAngle);

        m_IsGrabbing = IsGrabbing();
        m_IsPinching = IsPinching();

        if (m_CalculateCenterPointOfPinch)
        {
            pinchDistance = Vector3.Distance(m_IndexFingerTransforms[3].position, m_ThumbFingerTransforms[3].position);
            centerPointOfPinch = CalculateCenterPointOfPinch();
        }
    }

    float GetAngle(List<Transform> fingerTransforms)
    {
        Vector3 left = fingerTransforms[0].position;
        Vector3 middle = fingerTransforms[1].position;
        Vector3 right = fingerTransforms[3].position; // finger tip
        Vector3 vLeft = left - middle;
        Vector3 vRight = right - middle;

        return Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(vLeft, vRight) / (vLeft.magnitude * vRight.magnitude));
    }

    float NormalizedAngle(float angleMin, float angleMax, float angle)
    {
        float normalizedAngle, percantageValue;

        normalizedAngle = Mathf.InverseLerp(angleMax, angleMin, angle);
        percantageValue = (int)(normalizedAngle * 100);
        return percantageValue;
    }

    public bool IsGrabbing()
    {
        // If not grabbing and all fingers > grabPercentage
        if (!m_IsGrabbing &&
            m_MiddlePercentage > m_GrabPercentage &&
            m_RingPercentage > m_GrabPercentage &&
            m_PinkyPercentage > m_GrabPercentage)
        {
            return true;
        }
        // If grabbing and all fingers > releasePercentage
        else if (m_IsGrabbing &&
            (m_MiddlePercentage > m_GrabReleasePercentage ||
            m_RingPercentage > m_GrabReleasePercentage ||
            m_PinkyPercentage > m_GrabReleasePercentage))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsPinching()
    {
        if (!m_IsPinching &&
            m_IndexPercentage > m_PinchPercentage)
        {
            return true;
        }
        else if (m_IsPinching &&
            m_IndexPercentage > m_PinchReleasePercentage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector3 CalculateCenterPointOfPinch()
    {
        Vector3 centerPointOfPinch = new Vector3();
        Vector3 vectorToThumb = m_ThumbFingerTransforms[3].position - m_IndexFingerTransforms[3].position;

        vectorToThumb.Normalize();

        centerPointOfPinch = m_IndexFingerTransforms[3].position + (vectorToThumb * (pinchDistance * 0.5f));

        return centerPointOfPinch;
    }

}

public static class Extensions
{
    public static Transform Search(this Transform target, string name)
    {
        if (target.name == name) return target;

        for (int i = 0; i < target.childCount; ++i)
        {
            var result = Search(target.GetChild(i), name);

            if (result != null) return result;
        }

        return null;
    }
}