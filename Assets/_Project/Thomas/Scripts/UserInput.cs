using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Thomas
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField] private InputListenerBase m_listener;
        [SerializeField] private Button[] m_button;

        private void Update()
        {
            if (m_listener == null) 
                return;
            if(Input.GetMouseButton(0))
            {
#if UNITY_EDITOR
                m_listener.ProcessTouch(Input.mousePosition);
#else
                m_listener.ProcessTouch(Input.GetTouch(0).position);
#endif
            }

            if (Input.GetMouseButtonDown(0))
            {
#if UNITY_EDITOR
                m_listener.ProcessTouchDown(Input.mousePosition);
#else
                m_listener.ProcessTouchDown(Input.GetTouch(0).position);
#endif
            }
            if (Input.GetMouseButtonUp(0))
            {
#if UNITY_EDITOR
                m_listener.ProcessTouchUp(Input.mousePosition);
#else
                m_listener.ProcessTouchUp(Input.GetTouch(0).position);
#endif
            }
            for (int i = 0; i < m_button.Length; i++)
            {
                if(Input.GetMouseButton(i))
                    m_listener.ProcessButtonDown(m_button[i]);
            }
        }
    }
}