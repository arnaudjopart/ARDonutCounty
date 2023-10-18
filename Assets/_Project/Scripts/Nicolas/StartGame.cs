using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

namespace NJ
{
    //TODO nb pt
    //TODO each point make your ball far from hole
    //TODO boucing wall
    public class BouncingBallGame : MonoBehaviour
    {
        private const float RAY_CAST_MAX_DISTANCE = 40f;
        private const int FALL_Y_DESTROY = -4;

        public GameObject m_holePrefab; // Prefab du trou
        public GameObject m_ballPrefab; // Prefab de la bille
        public float m_ballForce = 5.0f; // Force pour tirer la bille

        private GameObject m_currentHole; // Hole actuelle
        private GameObject m_currentBall; // Ball actuelle

        Vector2 positionMouseTouch;
        Vector3 mousePosition;
        Ray rayMouseTouch;
        Vector3 ballPosition;
        Vector3 direction;
        Rigidbody rb;

        private void Start()
        {
            BallEnter.OnBallEnterHole += BallEnterHole;
            BallEnter.OnBallExitHole += BallExitHole;
        }
        private void OnDestroy()
        {
            BallEnter.OnBallEnterHole -= BallEnterHole;
            BallEnter.OnBallExitHole -= BallExitHole;
        }

        private void BallEnterHole()
        {
            if (m_currentBall != null)
            {
                /*m_currentBall.layer = LayerMask.NameToLayer("HoleContent");
                m_currentBall.GetComponent<Rigidbody>().isKinematic = true;
                //Rigidbody rb = m_currentBall.GetComponent<Rigidbody>();
                //rb.velocity = new Vector3(0.3f, 0.3f, 0.3f);
                //rb.velocity = Vector3.zero;
                //rb.isKinematic = true;
                m_currentBall.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => { m_currentBall = null;});*/
                rb = m_currentBall.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    m_currentBall.layer = LayerMask.NameToLayer("HoleContent");

                    /*Vector3 targetPosition = new Vector3(0f, 2f, 0f);
                    m_currentBall.transform.DOScale(Vector3.zero, 1.2f).OnComplete(() =>
                    {
                        m_currentBall.transform.position = targetPosition;
                    });*/
                    Destroy(m_currentBall);
                }
            }
        }
        private void BallExitHole()
        {
            if (m_currentBall != null)
            {
                //m_currentBall.layer = LayerMask.NameToLayer("Default");
                //m_currentBall.GetComponent<Rigidbody>().isKinematic = false;
                Destroy(m_currentBall);
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
#if UNITY_EDITOR
                positionMouseTouch = Input.mousePosition;
#else
                positionMouseTouch = Input.GetTouch(0).position;
#endif
                rayMouseTouch = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (m_currentHole == null && positionMouseTouch != null)
                {
                    // Premier clic : Créer un trou sur un plan horizontal
                    if (Physics.Raycast(rayMouseTouch, out RaycastHit hit, RAY_CAST_MAX_DISTANCE))
                    {
Debug.Log("Hole");
                        //if (hit.transform.CompareTag("Ground"))
                        {
                            m_currentHole = Instantiate(m_holePrefab, hit.point, Quaternion.identity);
                        }
                    }
                }
                else if (m_currentBall == null && positionMouseTouch != null)
                {
                    // Deuxième clic : Créer une bille sur le sol
                    if (Physics.Raycast(rayMouseTouch, out RaycastHit hit, RAY_CAST_MAX_DISTANCE))
                    {
                        //if (hit.transform.CompareTag("Ground"))
                        {
                            m_currentBall = Instantiate(m_ballPrefab, hit.point, Quaternion.identity);
                        }
Debug.Log("Ball layer:" + m_currentBall.layer.ToString());
                    }
                }
                else if (m_currentHole != null && m_currentBall != null && positionMouseTouch != null)
                {
                    if (Physics.Raycast(rayMouseTouch, out RaycastHit hit, RAY_CAST_MAX_DISTANCE) && hit.collider.CompareTag("Player"))
                    {
Debug.Log("Clic on current ball");
                        mousePosition = hit.point;
                        ballPosition = m_currentBall.transform.position;
                        direction = (ballPosition - mousePosition).normalized;

                        rb = m_currentBall.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.AddForce(direction * m_ballForce, ForceMode.Impulse);
                        }
                    }
                }
            }
            if (m_currentBall != null && m_currentBall.transform.position.y < FALL_Y_DESTROY) //ball fall, retry
                Destroy(m_currentBall);
        }
    }
}
