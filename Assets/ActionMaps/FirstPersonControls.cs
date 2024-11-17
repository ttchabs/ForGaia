using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstPersonControls : MonoBehaviour, IDamageable
{

    public static FirstPersonControls Instance;
    [Header("---INITIALIZATIONS---")]
    [Space(5)]
    public GameObject playerModel; //The 3D imported model of the player
    public PlayerScriptable playerConfigs;//The data container for the player
    [HideInInspector] public Controls playerInput;
    [HideInInspector] public Animator playerAnimation; //Animations that will be played thrughtout the game

    #region PLAYER MOVEMENT:
    [Header("---MOVEMENT SETTINGS---")]
    [Space(5)]
    //Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
    public AudioSource playerSFX;
    // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component
    #endregion

    #region PLAYER CROUCH:
    [Header("---CROUCH SETTINGS---")] 
    [Space(5)]
    public float crouchHeight = 1f; // Height of the player when crouching
    public float standingHeight = 2f; // Height of te player when standing
    public float crouchSpeed = 1.5f; //Speed at which the player moves when crouching
    private bool _isCrouching = false; //Whethere the player is currently crouching
    #endregion

    #region PLAYEY PICKUP ITEMS:
    [Header("---PICKING UP SETTINGS---")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    [HideInInspector] public GameObject heldObject; // Reference to the currently held object
    public float pickUpRange = 3f; // Range within which objects can be picked up

    #endregion

    #region PLAYER MELEE ATTACKING:
    [Header("---MELEE SETTINGS---")] 
    [Space(5)]
    //public GameObject meleeWeapon;//weapon model in hand 
    public Transform meleeHoldPosition;//location in which the weapon will go to 
    public WeaponScript meleeAttacks;//script attached to the weapon
    [SerializeField] bool _holdingMelee = true;
    public AudioSource swordSwing;
    #endregion

    #region PLAYER SHOOTING:
    [Header("---SHOOTING SETTINGS---")]
    [Space(5)]
    public Transform gunHoldPosition;
    public GunScript gunFire; //The script attached to the gun in hand
    [SerializeField] bool _holdingGun = false;
    #endregion

    #region PLAYER HEALTH:
    [Header("---HEALTH---")]
    [Space(5)]
    public int currentPlayerHP; //current hp of the player
    public event IDamageable.DamageReceivedEvent OnDamageReceived;
    #endregion

    #region UI
    [Header("UI SETTINGS")]
    public TextMeshProUGUI pickUpText;
    //public GameObject healthGrub; //image in the UI
    //public Image healthGrubSprite;
    //public Sprite healReference; // Image in Inspector
    public Image GunSlot;
    public Image SwordSlot;
    public TextMeshProUGUI grubCounttxt;
    public int grubCount = 0;
    #endregion

    #region ANIMATION

    [Header("ANIMATION SETTINGS")] [Space(5)]

    #endregion


    #region INVENTORY
    [Header("Inventory")][Space(5)]

    #endregion

    public string currentScene;

    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject


        if (Instance == null)
        {
            Instance = this;
        }

        else 
        { 
            Destroy(gameObject);        
        }

    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        characterController = GetComponent<CharacterController>();
        playerAnimation = playerModel.GetComponent<Animator>();//Get and store the animator component attached to the GameObject
        SetMaxHP();
    }  

    private void OnEnable()
    {
        // Create a new instance of the input actions
        playerInput = new Controls();
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
        playerInput.Player.Shoot.performed += ctx => UseWeapon(); // Call the Shoot method when shoot input is performed

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed

        //Subscribe to the pick-up-weapon input event
        playerInput.Player.ReloadGun.performed += ctx => ReloadGun(); // Call the ReloadGun method when reload-gun input is performed
    
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); // Call the Crouch method when crouch input is performed
        
        //Subscribe to the melee input event
        playerInput.Player.Melee.performed += ctx => Melee(); // Call the Melee method when melee input is performed

        //Subscribe to the ScrollThroughMelee input event
        playerInput.Player.SwitchWeapons.performed += ctx => SwitchWeapons(); //Call the CallMeleeWeapon method when the CallMeleeWeapon input is performed

        //Subscribe to the USEHEALTHGRUB input event
        playerInput.Player.UseHealthGrub.performed += ctx => UseHealthGrub();

        playerInput.Player.Inventory.performed += ctx => Inventory();

        playerInput.Player.Pause.performed += ctx => PauseGame();
    }

    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();

        PickUpDisplay();
    }

    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
      
        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        float currentSpeed;

        if (moveInput.x ==0 && moveInput.y ==0)
        {
            currentSpeed = 0;
        }
        else if (_isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else if (_holdingMelee == true)
        {
            currentSpeed = moveSpeed * (1 - meleeAttacks.weaponConfigs.WeaponWeight / playerConfigs.MaxWeaponWeight);
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }
        else if (_holdingGun == true)
        {
            currentSpeed = moveSpeed * (1 - gunFire.gunConfigs.GunWeight / playerConfigs.MaxWeaponWeight);
        }
        else
        {
            currentSpeed = moveSpeed;
        }
        
        // Move the character controller based on the movement vector and speed
        characterController.Move(currentSpeed * Time.deltaTime * move);
        //animator.SetFloat("Speed", currentSpeed);
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

    public void UseWeapon()
    {
        if (_holdingGun == true) 
        {
            gunFire.GunTriggerPulled();
        }
        else if (_holdingMelee == true)
        {
            meleeAttacks.SwordSwung(playerAnimation);
        }
    }

    public void Melee() //Right click mouse
    {
        if (meleeAttacks == null)
        {
            _holdingMelee = false;
        }
        if (_holdingMelee == true)
        {
            meleeAttacks.SwordSwung(playerAnimation); //When the player swings the sword, an animation will play and then a coroutine will start
        }
    }

    public void ReloadGun()
    {
/*        // Perform a raycast from the camera's position forward
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
                _holdingMelee = true;          
            }
            else if (hitMeleeWeapon.collider.CompareTag("MeleeWeapon"))
            {
                var itemCollect = hitMeleeWeapon.collider.GetComponent<PickUpFunction>();              
                itemCollect.Pickup();
            }
        }*/
        if(_holdingGun == true)
        {
            StartCoroutine(gunFire.ReloadGun());
        }
    }

    public void SwitchWeapons()
    {
        if (_holdingMelee == true && gunFire != null)
        {
            InventoryManager.Instance.playerHUDManager.UsingGunDisplay();
            meleeHoldPosition.gameObject.SetActive(false);
            gunHoldPosition.gameObject.SetActive(true);
            _holdingMelee = false;
            _holdingGun = true;
        }

        else if (_holdingGun == true && meleeAttacks != null)
        {
            InventoryManager.Instance.playerHUDManager.UsingMeleeDisplay();
            gunHoldPosition.gameObject.SetActive(false);
            meleeHoldPosition.gameObject.SetActive(true);
            _holdingGun = false;
            _holdingMelee = true;
        }
    }

    public void PickUpObject()
    {
        // Check if we are already holding an object
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
            heldObject.GetComponent<Collider>().enabled = true;
            heldObject.transform.parent = null; //player is no longer a parent to the gun(object)
            _holdingGun = false;
        }

        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);


        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("SorterPuzzleStone"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position

                heldObject.transform.SetParent(holdPosition);
                heldObject.transform.SetPositionAndRotation(holdPosition.position, holdPosition.rotation);
            }
/*            else if (hit.collider.CompareTag("Gun")) 
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
                gunFire = heldObject.GetComponent<GunScript>();
                heldObject.GetComponent<Collider>().enabled = false;

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
                holdingGun = true;
            }
*/
            ////////

            if (hit.collider.CompareTag("MeleeWeapon"))
            {
                var meleePickUp = hit.collider.GetComponent<PickUpFunction>();
                var meleeData = hit.collider.GetComponent<WeaponScript>();               
                meleePickUp.MeleePickUp(meleeData.weaponConfigs);               
            }

            else if (hit.collider.CompareTag("Gun")) 
            {
                var gunPickUp = hit.collider.GetComponent<PickUpFunction>();
                var gunData = hit.collider.GetComponent<GunScript>();
                gunPickUp.GunPickUp(gunData.gunConfigs);
                
            } 
            else if (hit.collider.CompareTag("PickUp"))
            {
                var pickUp = hit.collider.GetComponent<PickUpFunction>();
                pickUp.Pickup();
            }
        }
    }

    public void PickUpDisplay() //Displays the name of the item you are picking up
    {
        Ray displayRay = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit displayHit;
        if (Physics.Raycast(displayRay, out displayHit, pickUpRange))
        {
            if(displayHit.collider.TryGetComponent(out PickUpFunction name))
            {
                pickUpText.text = "(E)";
            }
            else
            {
                pickUpText.text = null;
            }
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
        SetCurrentHP();
        OnDamageReceived?.Invoke();
        playerConfigs.PlayPlayerHitSFX(playerSFX);
        if (currentPlayerHP <= 0)
            StartCoroutine(playerConfigs.PlayerDeath(currentScene, playerSFX));
    }

    public void SetCurrentHP()
    {
        InventoryManager.Instance.playerHUDManager.SetCurrentHPValue(currentPlayerHP);
    }
    public void SetMaxHP()
    {
        currentPlayerHP = playerConfigs.MaxPlayerHP;
        InventoryManager.Instance.playerHUDManager.SetMaxSliderHPValue(currentPlayerHP);
    }

    public void UseHealthGrub()
    {
        if (grubCount > 0)
        {
            DamageReceived(-10);
            grubCount--;
            grubCounttxt.text = grubCount.ToString();
            if (currentPlayerHP >= playerConfigs.MaxPlayerHP)
                currentPlayerHP = playerConfigs.MaxPlayerHP;
        }
        else {
            return;
        }

    }

    public void MeleeInitialise(GameObject melee)
    {
        meleeAttacks = melee.GetComponent<WeaponScript>();
        melee.GetComponent<Collider>().enabled = false;
        melee.GetComponent<Rigidbody>().isKinematic = true;
        melee.transform.SetPositionAndRotation(meleeHoldPosition.position, meleeHoldPosition.rotation);
    }

    public void GunInitialise(GameObject gun)
    {
        gunFire = gun.GetComponent<GunScript>();
        gun.GetComponent <Collider>().enabled = false;
        gun.GetComponent<Rigidbody>().isKinematic = true;
        gun.transform.SetPositionAndRotation(gunHoldPosition.position, gunHoldPosition.rotation);
    }

    public void RemoveMelee()
    {
        Destroy(meleeAttacks.gameObject);
        meleeAttacks = null;   
    }

    public void RemoveGun()
    {
        Destroy(gunFire.gameObject);
        gunFire = null;
    }



    public void PauseGame()
    {
        PauseManager.instance.PauseGame();
    }

    public void Inventory()
    {
        InventoryManager.Instance.OpenInventoryPanel();
    }


}
