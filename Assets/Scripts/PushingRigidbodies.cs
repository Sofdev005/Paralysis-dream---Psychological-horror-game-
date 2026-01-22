using UnityEngine;

public class PushingRigidbodies : MonoBehaviour
{
    // Adjust this value in the Inspector to control how hard the player pushes.
    public float pushForce = 2.0f;

    // This method is called by the CharacterController when it hits something.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 1. Check if the object we hit has a Rigidbody.
        Rigidbody body = hit.collider.attachedRigidbody;

        // If the object has no Rigidbody, or if it's set to Kinematic, do nothing.
        if (body == null || body.isKinematic)
        {
            return;
        }

        // 2. Prevent the player from pushing objects below them (like the ground).
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // 3. Calculate the direction to push the object.
        // We project the player's movement direction onto the flat XZ plane.
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // 4. Apply the force to the Rigidbody.
        // We use ForceMode.Impulse for a sudden, instantaneous push.
        body.AddForce(pushDir * pushForce, ForceMode.Impulse);
    }
}