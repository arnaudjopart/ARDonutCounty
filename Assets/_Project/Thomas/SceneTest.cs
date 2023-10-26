using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneTest : MonoBehaviour
{
    [SerializeField] private ARSession m_arSession;
    [SerializeField] private XROrigin m_xrOrigin;
    [SerializeField] private XRInteractionManager m_xrInteractionManager;
    [SerializeField] private float m_timer;
    private int m_palier;
    private bool m_sceneLoaded = false;

    private void Awake()
    {
        m_arSession.Reset();
    }

    void Start()
    {
        DontDestroyOnLoad(m_arSession);
        DontDestroyOnLoad(m_xrOrigin);
        DontDestroyOnLoad(m_xrInteractionManager);
        m_timer = 0;
        m_palier = 1;
    }

    void Update()
    {
        if (m_sceneLoaded) return;
        m_timer += Time.deltaTime;
        if (m_timer >= m_palier)
        {
            Debug.Log(m_palier.ToString());
            m_palier++;
        }
        if (m_timer >= 5)
        {
            SceneManager.LoadScene(0);
            m_sceneLoaded = true;
        }
    }
}
