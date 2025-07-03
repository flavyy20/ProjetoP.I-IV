using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveMarko : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    public Animator animator;
    private bool estaCorrendo = false;

    private float timer = 0f; 
    public float minTimeToEnter = 6f; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(-moveX, 0f, -moveZ).normalized;

        estaCorrendo = moveDirection.magnitude >= 0.1f;

        if (animator != null)
        {
            animator.SetBool("Correndo", estaCorrendo);
        }

        timer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            Quaternion correction = Quaternion.Euler(0f, 0f, 0f);
            Quaternion targetRotation = lookRotation * correction;

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinalizarMarko"))
        {
            if (timer >= minTimeToEnter)
            {
                SceneManager.LoadScene("TelaMonitoramento"); 
            }
            else
            {
                Debug.Log("Ainda existe agentes para resgatar!");
            }
        }
    }
}
