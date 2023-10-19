using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NJ
{
    //TODO visual effect ball disappear
    //TODO nb pt
    //TODO each point make your ball far from hole
    //TODO boucing wall
    //TODO hole stay in plane
    public class BouncingBallGame : MonoBehaviour
    {
        private const float RAY_CAST_MAX_DISTANCE = 40f;
        private const int FALL_Y_DESTROY = -4;
        private const float HOLE_SPEED_MOVEMENT = 1f;

        public GameObject m_holePrefab;
        public GameObject m_ballPrefab;
        //public GameObject m_joystickPrefab; // hole controller
        public Joystick m_joystickPrefab;
        public float m_ballForce = 5.0f; // Force pour tirer la bille
        public float AnimationBallInHoleDuration = 0.5f;

        private float minXBound = 0, maxXBound = 1, minYBound = 0, maxYBound = 1;

        private GameObject m_currentHole; // Hole actuelle
        private GameObject m_currentBall; // Ball actuelle

        private Vector2 positionMouseTouch, joystickInput;
        private Vector3 position, direction, ballPosition, cameraForward;
        private Ray rayMouseTouch;
        private Rigidbody rb;

        private void Start()
        {
            m_joystickPrefab.gameObject.SetActive(false);
            //CalculAndSetBounds();
            BallEnter.OnBallEnterHole += BallEnterHole;
            BallEnter.OnBallExitHole += BallExitHole;
        }

        /*private void CalculAndSetBounds()
        {
                float objectHalfWidth = yourObjectTransform.localScale.x / 2;
                float objectHalfHeight = yourObjectTransform.localScale.y / 2;

                Vector3 minScreenPoint = Camera.main.WorldToViewportPoint(new Vector3(-objectHalfWidth, -objectHalfHeight, 0));
                Vector3 maxScreenPoint = Camera.main.WorldToViewportPoint(new Vector3(objectHalfWidth, objectHalfHeight, 0));

                minXBound = minScreenPoint.x;
                maxXBound = maxScreenPoint.x;
                minYBound = minScreenPoint.y;
                maxYBound = maxScreenPoint.y;
        }*/

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
                    //rb.isKinematic = true; rb.mass = 10f; //moche
                    //m_currentBall.layer = LayerMask.NameToLayer("HoleContent");
                    m_currentBall.transform.DOScale(Vector3.zero, AnimationBallInHoleDuration).OnComplete(() => { Destroy(m_currentBall, 1f); });

                    /*Vector3 targetPosition = new Vector3(0f, 2f, 0f);
                    m_currentBall.transform.DOScale(Vector3.zero, 1.2f).OnComplete(() =>
                    {
                        m_currentBall.transform.position = targetPosition;
                    });*/
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

        private bool IsWithinBounds(Vector3 position)
        {
            Debug.Log("IsWithinBounds pos:" + position);
            return position.x >= minXBound && position.x <= maxXBound && position.y >= minYBound && position.y <= maxYBound;
        }
        private bool IsUIElementClicked() {
            return EventSystem.current.IsPointerOverGameObject();
        }
    void Update()
        {
            //move the hole with Joystick
            if (m_joystickPrefab.isActiveAndEnabled) {
                joystickInput = new Vector2(m_joystickPrefab.Horizontal, m_joystickPrefab.Vertical);
                if (joystickInput.magnitude > 0.05f)
                {
                    direction = new Vector3(joystickInput.x, 0, joystickInput.y);
                    cameraForward = Camera.main.transform.forward;
                    cameraForward.y = 0;
                    direction = Quaternion.LookRotation(cameraForward) * direction;
                    direction = direction.normalized;
                    position = m_currentHole.transform.position + direction * Time.deltaTime * HOLE_SPEED_MOVEMENT;
                    //if (IsWithinBounds(position))
                    {
                        m_currentHole.transform.position = position;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0) && !IsUIElementClicked())
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
//Debug.Log("Hole - Vector2.up" + Vector2.up);
                        //if (hit.transform.CompareTag("Ground"))
                        {
                            m_currentHole = Instantiate(m_holePrefab, hit.point, Quaternion.identity);
                            m_joystickPrefab.gameObject.SetActive(true);
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
Debug.Log("Ball layer:" + m_currentBall.layer);
                    }
                }
                else if (m_currentHole != null && m_currentBall != null && positionMouseTouch != null)
                {
                    if (Physics.Raycast(rayMouseTouch, out RaycastHit hit, RAY_CAST_MAX_DISTANCE) && hit.collider.CompareTag("Player"))
                    {
Debug.Log("Clic on current ball");
                        position = hit.point;
                        ballPosition = m_currentBall.transform.position;
                        direction = (ballPosition - position).normalized;

                        rb = m_currentBall.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.AddForce(direction * m_ballForce, ForceMode.Impulse);
                        }
                    }
                }
            }

            //ball fall, retry
            if (m_currentBall != null && m_currentBall.transform.position.y < FALL_Y_DESTROY)
                Destroy(m_currentBall);
        }
    }
}
