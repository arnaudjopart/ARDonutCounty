using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NJ
{
    //TODO Audio Please walk to detect something flat (ok good enough place to play), touch the screen where you want to put the hole then the ball + ball touch & hole
    //TODO visual effect ball disappear
    //TODO nb pt
    //TODO each point make your ball far from hole
    //TODO boucing wall
    //TODO hole stay in plane
    public class BouncingBallGame : MonoBehaviour
    {
        public static event System.Action OnBallAppear;

        private const float RAY_CAST_MAX_DISTANCE = 40f;
        private const int BALL_FALL_Y_DESTROY = -4;
        private const float HOLE_SPEED_MOVEMENT = 1f;
        private const float JOYSTICK_MAGNITUDE_DETECT = 0.1f;
        private const float HOLE_BALL_DISTANCE_WALL = 1.1f;

        public GameObject m_holePrefab;
        public GameObject m_ballPrefab;
        public GameObject m_wallPrefab;
        //public GameObject m_joystickPrefab; // hole controller
        public Joystick m_joystickPrefab;
        public float m_ballForce = 6f; // Force pour tirer la bille ForceMode.VelocityChange
        public float m_animationBallInHoleDuration = 0.34f;
        public float m_wallMovementDuration = 2.6f;

        private float minXBound = 0, maxXBound = 1, minYBound = 0, maxYBound = 1;

        private GameObject m_currentHole;
        private GameObject m_currentBall;
        private GameObject m_currentWall;

        private Vector2 positionMouseTouch, joystickInput;
        private Vector3 position, direction, ballPosition, cameraForward;
        private Ray rayMouseTouch;
        private Rigidbody rb;
        private Tween movingWallInfiniteTween;

        public bool isWallMoving { get; set; }

        private void Start()
        {
            m_joystickPrefab.gameObject.SetActive(false);
            //CalculAndSetBounds();
            BallEnter.OnBallEnterHole += BallEnterHole;
            BallEnter.OnBallExitHole += BallExitHole;
            //WallUpDown(false);
        }
        private void WallUpDown(bool _move)
        {
            if (_move)
            {
                Vector3 targetPosition = (m_currentHole.transform.position + m_currentBall.transform.position) / 2.0f;
                targetPosition.y -= m_ballPrefab.gameObject.transform.localScale.y;
                
                //Quaternion cameraRotation = Camera.main.transform.rotation;
                //cameraRotation = Quaternion.Euler(90, 0, 0);
                //float yOffset = m_currentBall.transform.localScale.y * 2f;
                //targetPosition.y += yOffset;
                float distance = Vector3.Distance(m_currentHole.transform.position, targetPosition);
                if (distance >= HOLE_BALL_DISTANCE_WALL)
                {
//Debug.Log("Cam.rotation:" + Camera.main.transform.rotation + " - Quat " + Quaternion.Euler(90, Camera.main.transform.rotation.y, 0));
//Debug.Log("Cam.rotation:" + Camera.main.transform.rotation + " - Quat " + Quaternion.Euler(90, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z));
                    Quaternion rotation = Quaternion.LookRotation(m_currentBall.transform.position - m_currentHole.transform.position);
//Debug.Log("Cam.rotation:" + Camera.main.transform.rotation + " - Quat rotation:" + rotation);
                    m_currentWall = Instantiate(m_wallPrefab, targetPosition, Quaternion.Euler(90, rotation.eulerAngles.y+90, 0));
                    RotateWallAndPosition();
                    /*movingWallInfiniteTween = DOVirtual.Float(targetPosition.y, targetPosition.y + (2 * m_ballPrefab.gameObject.transform.localScale.y), movementDuration, (y) =>
                    {
                        m_currentWall.transform.position = new Vector3(m_currentWall.transform.position.x, y, m_currentWall.transform.position.z);
                    })
                    .OnComplete(() =>
                    {
Debug.Log("finish wall destroy");
                        Destroy(m_currentWall, 0.4f);
                    }).SetLoops(-1, LoopType.Yoyo);*/

                }
                else
                {
Debug.Log("Cylinder is too close to the hole:" + distance + " max:" + HOLE_BALL_DISTANCE_WALL);
                }
            }
        }
        void RotateWallAndPosition()
        {
            float startRotationY = m_currentWall.transform.rotation.eulerAngles.y;
            movingWallInfiniteTween = m_currentWall.transform.DOMoveY(m_currentWall.transform.position.y + (2 * m_ballPrefab.gameObject.transform.localScale.y), m_wallMovementDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .OnKill(() => { movingWallInfiniteTween.Kill(); Destroy(m_currentWall, 0.1f); });

            /*m_currentWall.transform.DORotate(new Vector3(0, startRotationY + 180, 0), 3)
                .SetLoops(-1, LoopType.Yoyo)
                .OnKill(() => Destroy(m_currentWall));*/
        }

        /*void RotateWallAndPosition()
        {
                float startRotationY = m_currentWall.transform.rotation.eulerAngles.y;
                m_currentWall.transform.DOMoveY(m_currentWall.transform.position.y + (2 * m_ballPrefab.gameObject.transform.localScale.y), 2)
                .SetLoops(-1, LoopType.Yoyo);

                    m_currentWall.transform.DORotate(new Vector3(0, 0, startRotationY + 180), 3)
                        .SetLoops(-1, LoopType.Yoyo)
                        .OnKill(() => Destroy(m_currentWall));
        }*/


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
                    //m_currentBall.layer = LayerMask.NameToLayer("HoleContent");
                    rb.isKinematic = true;
                    //Destroy(m_currentBall, 1f);
                    //rb.mass = 1f;
                    if (m_currentBall.transform != null)
                    {
                        m_currentBall.transform.DOScale(Vector3.zero, m_animationBallInHoleDuration).OnComplete(() =>
                        {
                            Destroy(m_currentBall);
                            movingWallInfiniteTween.Kill();
                            Destroy(m_currentWall);
                            //WallUpDown(false);
                        });
                    }
                    else Debug.Log("m_currentBall.transform is null");
                }
                /*
                if (rb != null)
                {
                    rb.isKinematic = false; // Enable the Rigidbody's physics
                    rb.mass = 1f;

                    // You may need to adjust the following values to match your scene and ball size
                    float ballRadius = 0.5f; // m_currentBall.GetComponent<SphereCollider>().radius;

                    // Calculate the target position (center of the hole)
                    Vector3 targetPosition = m_currentHole.transform.position;

                    // Calculate the duration based on the vertical distance to fall into the hole
                    float fallDuration = Mathf.Sqrt(2 * (targetPosition.y - m_currentBall.transform.position.y) / Physics.gravity.y) * 6;
Debug.Log(fallDuration + " - ballRadius:" + ballRadius);
                    // Use DOJump to simulate gravity-based fall into the hole
                    m_currentBall.transform.DOJump(targetPosition, ballRadius, 1, fallDuration);
                    /*.OnComplete(() =>
                    {
                        // Animation is complete; you can destroy the ball or perform other actions
                        Destroy(m_currentBall, 2f);
                    });*/
                    /*Destroy(m_currentBall, 1f);
                }*/
            }
        }
        private void BallExitHole()
        {
            if (m_currentBall != null)
            {
Debug.Log("BallExitHole");
                //m_currentBall.layer = LayerMask.NameToLayer("Default");
                //m_currentBall.GetComponent<Rigidbody>().isKinematic = false;
                //Destroy(m_currentBall);
                //Destroy(m_currentWall);
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
                if (joystickInput.magnitude > JOYSTICK_MAGNITUDE_DETECT)
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
                            OnBallAppear?.Invoke();
                            WallUpDown(true);
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
                            rb.AddForce(direction * m_ballForce, ForceMode.VelocityChange);
                        }
                    }
                }
            }

            //ball fall, retry
            if (m_currentBall != null && m_currentBall.transform.position.y < BALL_FALL_Y_DESTROY)
            {
Debug.Log("ball fall, retry");
                Destroy(m_currentWall);
                //movingWallInfiniteTween.Kill();
                Destroy(m_currentBall);
            }
        }
    }
}