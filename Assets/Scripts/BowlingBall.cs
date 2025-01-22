using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    public float speed = 10f;
    public float initialShotPower = 600f;
    public float decelerationRate = 0.99f;
    private bool isShot = false;
    private Rigidbody rb;
    public BoxCollider playArea;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Error: Rigidbody is missing!");
            return;
        }
        if(playArea == null)
        {
            Debug.LogError("Error: PlayArea is not assingned!");
            return;
        }
        
    }

    void Update()
    {
        if(!isShot)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 newPosition = transform.position + new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
            Vector3 playAreaMin = playArea.bounds.min;
            Vector3 playAreaMax = playArea.bounds.max;
            newPosition.x = Mathf.Clamp(newPosition.x, playAreaMin.x, playAreaMax.x);
            transform.position = newPosition;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isShot = true;
                rb.isKinematic = false;
                rb.AddForce(Vector3.forward * initialShotPower);
            }

        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x * decelerationRate, rb.velocity.y, rb.velocity.z * decelerationRate);
            if(rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector3.zero;
                isShot = false;
                rb.isKinematic = true;
            }
        }
        
    }
}
