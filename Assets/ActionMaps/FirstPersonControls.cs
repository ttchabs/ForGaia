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
    public float moveAmount;
    public float moveAMountY;
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

    private GameObject _lastWeapon;
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
    public Transform meleeHoldPosition; //location in which the weapon will go to 
    public WeaponScript meleeAttacks; //script attached to the weapon
    [SerializeField] bool _holdingMelee = true;
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
    public int grubCount = 0;
    #endregion

    #region ANIMATION
    [Header("ANIMATION SETTINGS")] [Space(5)]

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
        currentScene = SceneManager.GetActiveScene().name;
        characterController = GetComponent<CharacterController>();
        playerAnimation = playerModel.GetComponent<Animator>();//Get and store the animator component attached to the GameObject
    }

    void Start()
    {
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
        playerInput.Player.Melee.performed += ctx => StoreLastWeapon(); // Call the Melee method when melee input is performed

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
        //Move();
        //MoveInputs();
        Move();
        LookAround();
        ApplyGravity();
    }

    public void FixedUpdate()
    {
        PickUpDisplay();        
    }

    //---- MOVEMENT BASED CODE---//

    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        //moveAmount = Mathf.Clamp01(Mathf.Abs(move.x) + Mathf.Abs(move.y));
        float currentSpeed;

        if (moveInput.x == 0 && moveInput.y == 0)
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
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // Move the character controller based on the movement vector and speed
        characterController.Move(currentSpeed * Time.deltaTime * move);
        MoveMentAnimations(moveInput.x, moveInput.y);
    }

    public void MoveMentAnimations(float inputX, float inputY)
    {
        //playerAnimation.SetFloat("Horizontal", inputX);
        //playerAnimation.SetFloat("Vertical", inputY);

        float snappedInputX;
        float snappedInputY;

        if(inputX > 0 && inputX <= 0.5f)
        {
            snappedInputX = 0.5f;
        }
        else if(inputX > 0.5f &&  inputX <= 1)
        {
            snappedInputX = 1;
        }
        else if(inputX < 0 && inputX >= -0.5f)
        {
            snappedInputX = -0.5f;
        }
        else if (inputX < -0.5f && inputX >= -1)
        {
            snappedInputX = -1;
        }
        else
        {
            snappedInputX = 0;
        }


        if (inputY > 0 && inputY <= 0.5f)
        {
            snappedInputY = 0.5f;
        }
        else if (inputY > 0.5f && inputY <= 1)
        {
            snappedInputY = 1;
        }
        else if (inputY < 0 && inputY >= -0.5f)
        {
            snappedInputY = -0.5f;
        }
        else if (inputY < -0.5f && inputY >= -1)
        {
            snappedInputY = -1;
        }
        else
        {
            snappedInputY = 0;
        }

        playerAnimation.SetFloat("Horizontal", snappedInputX);
        playerAnimation.SetFloat("Vertical", snappedInputY);
    }

    public void MoveInputs()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        moveAmount = Mathf.Clamp01(Mathf.Abs(move.x) + Mathf.Abs(move.y));
        float currentSpeed;

        if (moveAmount == 0)
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
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // Move the character controller based on the movement vector and speed
        characterController.Move(currentSpeed * Time.deltaTime * move);
        MoveMentAnimations(0, moveAmount);
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
            StoreLastWeapon();
        }
        else
        {
            characterController.height = crouchHeight;
            _isCrouching = true;

            gunHoldPosition.gameObject.SetActive(false);
            meleeHoldPosition.gameObject.SetActive(false);
            _holdingGun = false;
            _holdingMelee = false;
        }
    }

    //----ATTACK RELATED CODE----//
    public void UseWeapon()
    {
        if (_holdingGun == true && _isCrouching == false) 
        {
            gunFire.GunTriggerPulled();
        }
        else if (_holdingMelee == true && _isCrouching == false)
        {
            meleeAttacks.SwordSwung(playerAnimation);
        }
    }

    public void StoreLastWeapon()
    {

        if (_lastWeapon == gunHoldPosition.gameObject)
        {
            _holdingGun = true;
            gunHoldPosition.gameObject.SetActive(true);
        }
        else
        {
            _holdingMelee = true;
            meleeHoldPosition.gameObject.SetActive(true);
        }
    }

    public void ReloadGun()
    {
        if(_holdingGun == true)
        {
            StartCoroutine(gunFire.ReloadGun());
        }
    }

    public void SwitchWeapons()
    {
        if (_holdingMelee == true && gunFire != null && _isCrouching == false)
        {
            InventoryManager.Instance.playerHUDManager.UsingGunDisplay();
            _lastWeapon = gunHoldPosition.gameObject;
            meleeHoldPosition.gameObject.SetActive(false);
            gunHoldPosition.gameObject.SetActive(true);
            _holdingMelee = false;
            _holdingGun = true;
        }

        else if (_holdingGun == true && meleeAttacks != null && _isCrouching == false)
        {
            InventoryManager.Instance.playerHUDManager.UsingMeleeDisplay();
            _lastWeapon = meleeHoldPosition.gameObject ;
            gunHoldPosition.gameObject.SetActive(false);
            meleeHoldPosition.gameObject.SetActive(true);
            _holdingGun = false;
            _holdingMelee = true;
        }
    }

    //----ITEM BASED CODING----//
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

            if(hit.collider.TryGetComponent(out PickUpFunction canPickUp))
            {
                canPickUp.Pickup();
            }

/*            if (hit.collider.CompareTag("MeleeWeapon"))
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
            }*/
        }
    }

    public void PickUpDisplay() //Displays the name of the item you are picking up
    {
        Ray displayRay = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit displayHit;
        if (Physics.Raycast(displayRay, out displayHit, pickUpRange))
        {
           InventoryManager.Instance.playerHUDManager.Interactable(displayHit);
        }
    }

    //----HEALTH AND DAMAGE BASED CODE----//

    public IEnumerator KnockedBack(Vector3 direction) //Knockback taken when a enemy hits the player
    {
        Vector3 knockback = new Vector3(0, Mathf.Sqrt(2f * -gravity * jumpHeight) * 0.15f, direction.z);
        velocity = knockback;
        
        yield return new WaitForSeconds(0.5f);
        velocity = Vector3.zero;
    }

    public void DamageReceived(int damageAmount) //The function that is called whenever the player is hit by an enemy
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
            if (currentPlayerHP >= playerConfigs.MaxPlayerHP)
                currentPlayerHP = playerConfigs.MaxPlayerHP;
        }
        else {
            return;
        }

    }

    //----INVENTORY BASED CODE----//

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
        gun.GetComponent<Collider>().enabled = false;
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
