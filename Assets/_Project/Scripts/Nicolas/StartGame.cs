using UnityEngine;

namespace NJ
{
    //TODO nb pt
    //TODO each point make your ball far from hole
    //TODO boucing wall
    public class BouncingBallGame : MonoBehaviour
    {
        private const float RAY_CAST_MAX_DISTANCE = 40f;
        private const int FALL_Y_DESTROY = -4;

        public GameObject m_holePrefab;
        public GameObject m_ballPrefab;
        //public GameObject m_joystickPrefab; // hole controller
        public Joystick m_joystickPrefab;
        public Vector3 m_holeSpeedMove;
        public float m_ballForce = 5.0f; // Force pour tirer la bille

        public float minXBound, maxXBound, minYBound, maxYBound;

        private GameObject m_currentHole; // Hole actuelle
        private GameObject m_currentBall; // Ball actuelle

        Vector2 positionMouseTouch;
        Vector3 position;
        Ray rayMouseTouch;
        Vector3 ballPosition;
        Vector3 direction;
        Rigidbody rb;
        Vector2 joystickInput;

        private void Start()
        {
            m_joystickPrefab.gameObject.SetActive(false);
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

        private bool IsWithinBounds(Vector3 position)
        {
Debug.Log("IsWithinBounds pos:" + position);
            return position.x >= minXBound && position.x <= maxXBound && position.y >= minYBound && position.y <= maxYBound;
        }
        void Update()
        {
            //move the hole with Joystick
            float minX = -5.0f; // Minimum X coordinate for the screen
            float maxX = 5.0f;  // Maximum X coordinate for the screen
            float minY = -3.0f; // Minimum Y coordinate for the screen
            float maxY = 3.0f;  // Maximum Y coordinate for the screen

            joystickInput = new Vector2(m_joystickPrefab.Horizontal, m_joystickPrefab.Vertical);
            if (joystickInput.magnitude > 0.1f)
            {
                direction = new Vector3(joystickInput.x, 0, joystickInput.y);
                position = m_currentHole.transform.position + direction * Time.deltaTime * 1;

                position.x = Mathf.Clamp(position.x, minX, maxX);
                position.y = Mathf.Clamp(position.y, minY, maxY);

                m_currentHole.transform.position = position;
            }

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
