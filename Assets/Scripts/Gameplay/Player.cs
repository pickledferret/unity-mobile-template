using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] float m_moveSpeed = 5f;

    private Vector3 m_moveDirection;
    private bool m_inputEnabled = true;

    void Update()
    {
        if (m_inputEnabled)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                m_moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
                transform.rotation = Quaternion.LookRotation(m_moveDirection);
            }
            else
            {
                m_moveDirection = Vector3.zero;
            }
        }
    }

    void FixedUpdate()
    {
        m_rigidbody.linearVelocity = m_inputEnabled ? (m_moveDirection * m_moveSpeed) : Vector3.zero;
    }

    public void DisableMovement()
    {
        m_inputEnabled = false;
    }
}
