using System;
using UnityEngine;

internal class DoorController : MonoBehaviour
{
    private GameObject m_door;
    private Vector3 m_targetPosition;
    [SerializeField]private GameObject m_doorPrefab;
    private bool m_isMoving;
    [SerializeField] private float m_speed =10f;

    internal void ProcessTouch(Vector3 positionOfHit)
    {
        if (m_door == null)
        {
            m_door = Instantiate(m_doorPrefab, positionOfHit, Quaternion.identity);
            m_targetPosition = positionOfHit;
            return;
        }

        MoveToPosition(positionOfHit);
    }

    private void Update()
    {
        if (m_isMoving)
        {
            var nextPosition = Vector3.Lerp(m_door.transform.position, m_targetPosition, Time.deltaTime * m_speed);
            m_door.transform.position = nextPosition;
            m_isMoving = Vector3.Distance(m_door.transform.position, m_targetPosition) > 0.01f;
        }
    }

    private void MoveToPosition(Vector3 _position)
    {
        m_targetPosition = _position;
        m_isMoving = true;
    }
}