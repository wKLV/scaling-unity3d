using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
[Serializable]
public class MouseLook: MonoBehaviour
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;


    private Quaternion m_CharacterTargetRot;


    public void Start()
    {
        m_CharacterTargetRot = transform.rotation;
    }


    public void Update()
    {
        float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
        float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

		//Vector3 lookTo = transform.localPosition + new Vector3 (-xRot, yRot, 0f);
		//m_CharacterTargetRot = Quaternion.Euler(-xRot, yRot, 0f);
		m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
		m_CharacterTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);
	//	m_CharacterTargetRot.Set (m_CharacterTargetRot.x, m_CharacterTargetRot.y, 0, m_CharacterTargetRot.w);
		m_CharacterTargetRot = Quaternion.Euler(m_CharacterTargetRot.eulerAngles.x, m_CharacterTargetRot.eulerAngles.y, 0);

        if(clampVerticalRotation)
            m_CharacterTargetRot = ClampRotationAroundXAxis (m_CharacterTargetRot);

        if(smooth)
        {
			transform.rotation = Quaternion.Slerp (m_CharacterTargetRot, transform.rotation,
                smoothTime * Time.deltaTime);
        }
        else
        {
            transform.rotation = m_CharacterTargetRot;
        }
    }


    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

        angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}
