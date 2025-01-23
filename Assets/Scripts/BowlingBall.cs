using UnityEngine;

public class BowlingBallController : MonoBehaviour
{
    public float speed = 10f;
    public float initialShotPower = 3600f;
    public float decelerationRate = 0.99f; 
    private bool isShot = false;
    private Rigidbody rb;
    public Camera followCamera; 
    public Camera mainCamera; 
    public BoxCollider playArea; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (followCamera == null || mainCamera == null)
        {
            Debug.LogError("Error,Fixed the camera");
            return;
        }

        followCamera.enabled = true;
        mainCamera.enabled = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (isShot)
        {
            Rigidbody otherRb = collision.collider.attachedRigidbody;
            if (otherRb != null)
            {
                Vector3 impactForce = rb.velocity * rb.mass;
                otherRb.AddForce(impactForce, ForceMode.Impulse);
            }

            followCamera.enabled = false;
            mainCamera.enabled = true;
        }
    }

    void Update()
    {
        if (!isShot)
        {
            float horizontalInput = Input.GetAxis("Horizontal") ;
            Vector3 newPosition = transform.position + new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);

            Vector3 playAreaMin = playArea.bounds.min;
            Vector3 playAreaMax = playArea.bounds.max;
            newPosition.x = Mathf.Clamp(newPosition.x, playAreaMin.x, playAreaMax.x);
            transform.position = newPosition;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isShot = true;
                rb.isKinematic = false;
                rb.AddForce(Vector3.forward * initialShotPower);
            }
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * decelerationRate);
            if(rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector3.zero;
                isShot = false;
                rb.isKinematic = true;
            }
        }
    }
}
