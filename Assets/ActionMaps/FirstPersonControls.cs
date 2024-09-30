using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class FirstPersonControls : MonoBehaviour, IDamageable
{
    [Header("INITIALIZATIONS")]
    [Space(5)]
    public GameObject playerModel;//The 3D imported model of the player
    public PlayerScriptable playerConfigs;//The data container for the player 

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
    public GunScript gunFire; //The script attached to the gun in hand
    public bool _readyToShoot = true;
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
    private bool _isCrouching = false;//Whethere the player is currently crouching
    #endregion

    #region PLAYER MELEE ATTACKING:
    [Header("MELEE SETTINGS")] [Space(5)]
    public GameObject meleeWeapon;//weapon model in hand 
    public WeaponScript meleeAttacks;//script attached to the weapon
    public Transform meleeHoldPosition;//location in which the weapon will go to 
    private Animator weaponAnimation;//animation attached to the weapon will be played
    public bool holdingMelee = false;
    public bool _cooldownOver = true;
    #endregion

    #region PLAYER HEALTH:
    [Header("HEALTH:")]
    [Space(5)]
    public int currentPlayerHP;//current hp of the player

    public event IDamageable.DamageReceivedEvent OnDamageReceived;
    #endregion

    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
        weaponAnimation = playerModel.GetComponent<Animator>();//Get and store the animator component attached to the GameObject
        //weaponAnimation.runtimeAnimatorController = meleeAttacks.weaponConfigs.uniqueAnimation;
        currentPlayerHP = playerConfigs.MaxPlayerHP;
        //OnDamageReceived += KnockedBack;
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
        playerInput.Player.ScrollThroughMelee.performed += ctx => CallMeleeWeapon(); //Call the CallMeleeWeapon method when the CallMeleeWeapon input is performed
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
        if (_isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else if (meleeAttacks != null)
        {
            currentSpeed = moveSpeed * (1 - meleeAttacks.weaponConfigs.WeaponWeight / playerConfigs.MaxWeaponWeight);
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }
        else
        {
            currentSpeed = moveSpeed;
        }
        // Move the character controller based on the movement vector and speed
        characterController.Move(move * currentSpeed * Time.deltaTime);
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
        if (holdingGun == true && _readyToShoot == true ) 
        {
            gunFire.GunTriggerPulled();
        }
    }

    public void Melee() //Right click mouse
    {
        if (holdingMelee == true && meleeAttacks.cooldown == false)
        {
            meleeAttacks.cooldown = true;//When the player clicks right-click, cooldown starts
            weaponAnimation.runtimeAnimatorController = meleeAttacks.weaponConfigs.uniqueAnimation;   
            //checks what is currently in the player's melee hand and activates an animation based on the weapon type
            switch (meleeAttacks.weaponConfigs.meleeType)
            {
                case MeleeWeaponType.Barefist:
                    Debug.Log("clawed");
                    break;
                case MeleeWeaponType.Light:
                    weaponAnimation.SetTrigger("LightWeaponAttack");
                    //weaponAnimation.Play("LightWeaponAttack", 0, 0);
                    break;
                case MeleeWeaponType.Medium:
                    weaponAnimation.SetTrigger("MediumWeaponAttack");
                    //weaponAnimation.Play("MediumWeaponAttack", 0, 0);
                    break;

                case MeleeWeaponType.Heavy:
                    weaponAnimation.SetTrigger("HeavyWeaponAttack");
                    //weaponAnimation.Play("HeavyWeaponAttack", 0, 0);
                    break;
                default:
                    break;
            }
            //StartCoroutine(Attacks(meleeAttacks.weaponConfigs.SwingCooldown));
            StartCoroutine(meleeAttacks.CooldownCounter());
        }
    }

    public void PickUpMelee()
    {
        //Check if we are already holding a weapon
        if (meleeWeapon != null) 
        {
            meleeWeapon.GetComponent<Rigidbody>().isKinematic = false; //Enable Physics
            meleeWeapon.GetComponent<Collider>().enabled = true; //Reactivate the collider so it can land on the ground 
            meleeWeapon.transform.parent = null;
            meleeAttacks = null;
            //weaponAnimation.runtimeAnimatorController = null;
            holdingMelee = false;
        }

        // Perform a raycast from the camera's position forward
        Ray meleeRay = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hitMeleeWeapon;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.blue);

        if (Physics.Raycast(meleeRay, out hitMeleeWeapon, pickUpRange))
        {
            // Check if the hit object has the tag "MeleeWeapon"
            if (hitMeleeWeapon.collider.CompareTag("MeleeWeapon"))
            {
                // Pick up the object
                meleeWeapon = hitMeleeWeapon.collider.gameObject;
                meleeWeapon.GetComponent<Rigidbody>().isKinematic = true; //Disable physics
                meleeWeapon.GetComponent<Collider>().enabled = false; //Switch off the collider to make sure objects don't hit it constantly
                meleeAttacks = meleeWeapon.GetComponent<WeaponScript>(); //Initialise the weaponScript component of the weapon that is held
               
                // Attach the melee weapon to the hold position
                meleeWeapon.transform.position = meleeHoldPosition.position;
                meleeWeapon.transform.rotation = meleeHoldPosition.rotation;
                meleeWeapon.transform.parent = meleeHoldPosition;
                holdingMelee = true;          
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
            if (hit.collider.CompareTag("PickUp") || hit.collider.CompareTag("SorterPuzzleStone"))
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
                gunFire = heldObject.GetComponent<GunScript>();

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
                holdingGun = true;
            }
        }
    } 
    
    public void CallMeleeWeapon() //Places the melee weapon away or calls it to hand
    {

    }

    public void ToggleCrouch()
    {
        //CHecks if the crouch button was pressed
        if (_isCrouching)
        {
            characterController.height = standingHeight;
            _isCrouching = false;
        }
        else
        {
            characterController.height = crouchHeight;
            _isCrouching = true;
        }
    }

    //Cooldown clock for the melee attacks
    public IEnumerator Attacks(float cd)
    {
        if (!_cooldownOver)
        {
            yield return new WaitForSeconds(cd);
            _cooldownOver = true;
            yield break;
        }

    }

    //Knockback taken when a enemy hits the player
    public IEnumerator KnockedBack(Vector3 direction)
    {
        Vector3 knockback = new Vector3(0, Mathf.Sqrt(2f * -gravity * jumpHeight) * 0.15f, direction.z);
        velocity = knockback;
        
        yield return new WaitForSeconds(0.5f);
        velocity = Vector3.zero;
    }

    //The function that is called whenever the player is hit by an enemy
    public void DamageReceived(int damageAmount)
    {
        currentPlayerHP -= damageAmount;
        OnDamageReceived?.Invoke(damageAmount);

        if (currentPlayerHP <= 0)
            playerConfigs.PlayerDeath();
    }

}
