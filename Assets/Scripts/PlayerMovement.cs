using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    public bool lockPlayerControls;
    public bool inWater;

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float sprintSpeed;

    [SerializeField]
    private float groundCheckDepth;

    public enum MovementTypes 
    { 
        walking,
        sprinting,
        suitWalking,
        suitSprinting
    }

    public MovementTypes movementType;

    [SerializeField]
    public float[] MovementSpeeds = new float[]
    {
        5.0f, 7.0f, 3.8f, 4.5f
    };


    // Start is called before the first frame update
    void Start()
    {
        inWater = false;
        lockPlayerControls = false;
        characterController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        //Jump();
    }

    void MovePlayer()
    {
        if (lockPlayerControls == true) return;
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Debug.Log("Movement magnitude: " + direction.magnitude + (" Character Velocity: " + characterController.velocity.magnitude));
        
        if (Input.GetButton("Sprint") && Input.GetAxisRaw("Vertical") == 1 && inWater)
        {
            movementType = MovementTypes.suitSprinting;
        }
        else if (inWater)
        {
            movementType = MovementTypes.suitWalking;
        }
        else if (Input.GetButton("Sprint") && Input.GetAxisRaw("Vertical") == 1)
        {
            movementType = MovementTypes.sprinting;
        }
        else
        {
            movementType = MovementTypes.walking;
        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1 && Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1)
        {
            Debug.Log("Normalizing movement. Character Velocity: " + characterController.velocity.magnitude);
            direction.Normalize();
        }

        characterController.Move(direction * Time.deltaTime * MovementSpeeds[(int)movementType]);
    }

    void Jump()
    {
        if (lockPlayerControls == true) return;
        if (Grounded() == false) return;

    }

    bool Grounded()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hit))
        {
            Debug.Log("Hit");
            return true;
        }

        return false;
    }
}
