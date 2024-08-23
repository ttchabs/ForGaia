using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControls : MonoBehaviour
{
    #region PLAYER MOVEMENT:
    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
                                   // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component
    #endregion

    #region PLAYER SHOOTING:
    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired
    #endregion

    #region PLAYEY PICKUP ITEMS:
    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    [HideInInspector] public GameObject heldObject; // Reference to the currently held object
    public float pickUpRange = 3f; // Range within which objects can be picked up
    private bool holdingGun = false;
    #endregion

    #region PLAYER CROUCH:
    [Header("CROUCH SETTINGS")] [Space(5)] 
    public float crouchHeight = 1f; // Height of the player when crouching
    public float standingHeight = 2f; // Height of te player when standing
    public float crouchSpeed = 1.5f;//Speed at which the player moves when crouching
    private bool isCrouching = false;//Whethere the player is currently crouching
    #endregion

    #region PLAYER MELEE ATTACKING:
    [Header("MELEE SETTINGS")] [Space(5)]
    //public GameObject meleeWeapon;
    public Transform meleeHoldPosition;
    public PlayerAttacks meleeAttacks;
    public float hitRange = 3f;
    public float swingDelay = 0.5f;
    public int hitDamage = 1;
    private bool _holdingMelee = false;
    private bool _isAttacking = false;
    private bool _cooldownOver = true;
    public LayerMask attackable;
    #endregion

    #region PLAYER HEALTH:
    [Header("Health:")]
    [Space(5)]
    public int currentPlayerHP;

    #endregion 

    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
        meleeAttacks = GetComponent<PlayerAttacks>();
    }

    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        // Subscribe to the look input events
        playerInput.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.Look.canceled += ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed

        // Subscribe to the shoot input event
        playerInput.Player.Shoot.performed += ctx => Shoot(); // Call the Shoot method when shoot input is performed

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed
    
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); // Call the Crouch method when crouch input is performed
        
        //Subscribe to the melee input event
        playerInput.Player.Melee.performed += ctx => Melee(); // Call the Melee method when melee input is performed

        //Subscribe to the pick-up-weapon input event
        playerInput.Player.PickUpMelee.performed += ctx => PickUpMelee(); // Call the PickUpWeapon method when pick-up-melee input is performed

        //Subscribe to the ScrollThroughMelee input event
        playerInput.Player.ScrollThroughMelee.performed += ctx => ScrollThroughMelee(); //Call the ScrollThroughMelee method when the ScrollTroughMelee input is performed
}

    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity(); 
    }

    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);


        float currentSpeed;
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
        // Move the character controller based on the movement vector and speed
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    public void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based on sensitivity
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void Shoot()
    {
        if (holdingGun == true)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Get the Rigidbody component of the projectile and set its velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;

            // Destroy the projectile after 3 seconds
            Destroy(projectile, 3f);
        }
    }

    public void Melee() //Right click mouse
    {
        if (_holdingMelee == true)
        {
            if (_cooldownOver == true && _isAttacking == false)
            {
                Animator anim = meleeAttacks.meleeWeapon.GetComponent<Animator>();
                anim.SetTrigger("Attack");

                _cooldownOver = false;
                _isAttacking = true;

                if (meleeAttacks.meleeWeapon.CompareTag("Sword"))
                {
                    meleeAttacks.SwordAttack();
                    StartCoroutine(Cooldown(1f));
                }

                if (meleeAttacks.meleeWeapon.CompareTag("Knife"))
                {
                    meleeAttacks.KnifeAttack();
                    StartCoroutine(Cooldown(0.2f));
                }

                if (meleeAttacks.meleeWeapon.CompareTag("Longsword"))
                {
                    meleeAttacks.LongswordAttack();
                    StartCoroutine(Cooldown(4));
                }
            }
        }
    }

    public void PickUpMelee()
    {

        if (meleeAttacks.meleeWeapon != null) 
        {
            Animator idleAnimActive = meleeAttacks.meleeWeapon.GetComponent<Animator>();
            idleAnimActive.enabled = false;
            meleeAttacks.meleeWeapon.GetComponent<Rigidbody>().isKinematic = false; 
            meleeAttacks.meleeWeapon.GetComponent<Collider>().isTrigger = false;
            meleeAttacks.meleeWeapon.transform.parent = null;
            _holdingMelee = false;

            //idleAnimActive.SetBool("InHand", false);
        }

        Ray meleeRay = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hitMeleeWeapon;

        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.blue);

        if (Physics.Raycast(meleeRay, out hitMeleeWeapon, pickUpRange))
        {
            if (hitMeleeWeapon.collider.CompareTag("Sword") || hitMeleeWeapon.collider.CompareTag("Knife") || hitMeleeWeapon.collider.CompareTag("Longsword"))
            {

                // Pick up the object
                meleeAttacks.meleeWeapon = hitMeleeWeapon.collider.gameObject;
                meleeAttacks.meleeWeapon.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
                meleeAttacks.meleeWeapon.GetComponent<Collider>().isTrigger = true;

                // Attach the object to the hold position
                meleeAttacks.meleeWeapon.transform.position = meleeHoldPosition.position;
                meleeAttacks.meleeWeapon.transform.rotation = meleeHoldPosition.rotation;
                meleeAttacks.meleeWeapon.transform.parent = meleeHoldPosition;

                Animator idleAnimActive = meleeAttacks.meleeWeapon.GetComponent<Animator>();
                idleAnimActive.enabled = true;

                _holdingMelee = true;
            }
        }
    }

    public void PickUpObject()
    {
        // Check if we are already holding an object
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
            heldObject.transform.parent = null; //player is no longer a parent to the gun(object)
            holdingGun = false;
        }

        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);


        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("PickUp"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
            }
            else if (hit.collider.CompareTag("Gun")) 
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                holdingGun = true;
            }
            else if (hit.collider.CompareTag("SorterPuzzleStone"))
            {
                //Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; //Disable Physics

                //Attach the object to the hold position 
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
            }
        }
    }    
    
    public void ScrollThroughMelee()
    {

    }

    public void ToggleCrouch()
    {
        if (isCrouching)
        {
            characterController.height = standingHeight;
            isCrouching = false;
        }
        else
        {
            characterController.height = crouchHeight;
            isCrouching = true;
        }
    }

    public void KillCharacter()
    {
        Debug.Log("Dead");
    }

    IEnumerator Cooldown(float timer)
    {
        yield return new WaitForSeconds(timer);
        _cooldownOver = true;
        _isAttacking = false;
    }
}
