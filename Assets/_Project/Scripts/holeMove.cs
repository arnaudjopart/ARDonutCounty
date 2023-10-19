using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holeMove : MonoBehaviour
{

    public float m_speed = 10;

    private Vector3 m_targetPosition;
    private bool m_isMoving;

    const int LEFT_MOUSE_BUTTON = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_targetPosition = transform.position;
        m_isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
        {
            setTargetPosition();
        }

        if (m_isMoving)
        {
            movingPlayer();
        }
    }
    private void setTargetPosition()
    {
        Plane plane = new Plane(Vector3.up,transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float point = 0f;

        if (plane.Raycast(ray, out point))
        {
            m_targetPosition = ray.GetPoint(point);
            m_isMoving = true;
        }
    }

    void movingPlayer()
    {
        transform.LookAt(m_targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, m_speed * Time.deltaTime);
        if (transform.position == m_targetPosition)
        {
            m_isMoving = false;
        }
    }
}
