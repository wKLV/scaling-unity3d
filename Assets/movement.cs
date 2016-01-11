using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour
{
	public float size = 1f;
	public float speed = 5f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpForce = 2.0f;
	private bool grounded = false;
	
	void FixedUpdate () {
		transform.localScale = new Vector3 (size, size, size);
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.mass = size;
		if (grounded) {
			// Calculate how fast we should be moving
			Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed*size;
			
			// Apply a force that attempts to reach our target velocity
			Vector3 velocity = GetComponent<Rigidbody>().velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
			
			// Jump
			if (canJump && Input.GetButton("Jump")) {
				GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce*size*Mathf.Sqrt(size), 0), ForceMode.Impulse);
                grounded = false;
			}
		}
		
		// We apply gravity manually for more tuning control
		GetComponent<Rigidbody>().AddForce(new Vector3 (0, -gravity * rb.mass, 0));
		

	}

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            size *= 2;
        }
        else if (Input.GetKeyDown("e"))
        {
            size *= 0.5f;
        }
    } 
	
	void OnCollisionStay () {
		grounded = true;    
	}

}

