using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public static Cube m_instance { get; private set; }

    public List<string> m_angleList = new List<string>();

    [SerializeField] [Range(0, 360f)] float m_xRotationSpeed;
    [SerializeField] [Range(0, 15f)] public float m_rotationSuccess;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this);
        }
        else
        {
            m_instance = this;
        }
    }

    void Update() => Rotation();

    private void Rotation()
    {
        transform.Rotate(m_xRotationSpeed * Time.deltaTime, 0, 0);
    }
    #region Rotation Success
    public bool RotationSuccess()
    {
        float absEularX = MathF.Abs(GetEularAnglesX());
        bool isSmall = (absEularX % 90) < m_rotationSuccess;

        if (isSmall)
        {
            AngleListUpdate();
            return true;
        }

        return false;
    }

    private void AngleListUpdate()
    {
        if (m_angleList.Count >= 25)
        {
            m_angleList.Clear();
        }

        m_angleList.Add(GetEularAnglesX().ToString("F2"));
    }
    #endregion

    public float GetEularAnglesX() => gameObject.transform.rotation.eulerAngles.x;
}
