using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sam Robichaud 2022
// NSCC-Truro
// Based on tutorial by (Comp - 3 Interactive)  * with modifications *

namespace UnderwaterHorror
{
    public class FirstPersonController_Sam : MonoBehaviour
    {
        public static FirstPersonController_Sam fpsSam;
        private CameraShake cameraShake;
        public bool canMove { get; private set; } = true;
        private bool isRunning => canRun && Input.GetKey(runKey);
        private bool shouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
        private bool shouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;

        #region Settings

        [Header("Functional Settings")]
        [SerializeField] private bool isDashing = false;
        [SerializeField] public bool inWater = false;
        [SerializeField] public bool carryingHeavyObj = false;
        [SerializeField] private bool canRun = true;
        [SerializeField] private bool canJump = true;
        [SerializeField] private bool canCrouch = true;
        [SerializeField] private bool canUseHeadbob = true;
        [SerializeField] private bool canSlideOnSlopes = true;
        [SerializeField] private bool canZoom = true;
        [SerializeField] private bool canInteract = true;
        [SerializeField] private bool useFootsteps = true;

        [Header("Controls")]
        [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
        [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;
        [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;

        [Header("Move Settings")]
        [SerializeField] private float walkSpeed = 4.0f;
        [SerializeField] private float runSpeed = 10.0f;
        [SerializeField] private float crouchSpeed = 2.5f;
        [SerializeField] private float slopeSpeed = 12f;
        [SerializeField] private float suitWalkSpeed = 2.5f;
        [SerializeField] private float suitRunSpeed = 6.5f;
        [SerializeField] private float suitCrouchSpeed = 1.5f;
        [SerializeField] private float suitSlopeSpeed = 8.5f;
        [SerializeField] private float dashSpeed = 15f;
        [SerializeField] private float dashLength = 0.2f;
        private float dashTimer;

        [Header("Look Settings")]
        [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
        [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
        [SerializeField, Range(1, 180)] private float upperLookLimit = 70.0f;
        [SerializeField, Range(-180, 1)] private float lowerLookLimit = -70.0f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 8.0f;
        [SerializeField] private float gravity = 30f;
        [SerializeField] private float waterJumpForce = 5.0f;
        [SerializeField] private float waterGravity = 15.0f;

        [Header("Crouch Settings")]
        [SerializeField] private float crouchHeight = 0.5f;
        [SerializeField] private float standingHeight = 1.8f;
        [SerializeField] private float timeToCrouch = 0.15f;
        [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
        [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
        private bool isCrouching;
        private bool duringCrouchAnimation;

        [Header("Headbob Settings")]
        [SerializeField] private float crouchBobSpeed = 6f;
        [SerializeField] private float walkBobSpeed = 9f;
        [SerializeField] private float runBobSpeed = 12f;
        [SerializeField] private float waterCrouchBobSpeed = 3f;
        [SerializeField] private float waterWalkBobSpeed = 6f;
        [SerializeField] private float waterRunBobSpeed = 9f;

        [SerializeField] private float crouchBobAmount = 0.15f;
        [SerializeField] private float walkBobAmount = 0.3f;
        [SerializeField] private float runBobAmount = 0.45f;
        [SerializeField] private float waterCrouchBobAmount = 0.15f;
        [SerializeField] private float waterWalkBobAmount = 0.3f;
        [SerializeField] private float waterRunBobAmount = 0.45f;

        [SerializeField] private float lowerBobLimit = 0.5f;
        [SerializeField] private float upperBobLimit = 0.9f;
        [SerializeField] private float defaultYPos = 0;
        private float timer;

        [Header("Zoom Settings")]
        [SerializeField] private float timeToZoom = 0.2f;
        [SerializeField] private float zoomFOV = 30f;
        private float defaultFOV;
        private Coroutine zoomRoutine;

        [Header("Footstep Settings")]
        [SerializeField] private float baseStepSpeed = 0.55f;
        [SerializeField] private float crouchStepMultiplier = 1.5f;
        [SerializeField] private float RunStepMultiplier = 0.6f;
        [SerializeField] private float waterStepSpeed = 1.8f;
        [SerializeField] private float waterCrouchStepMultiplier = 2.3f;
        [SerializeField] private float waterRunStepMultiplier = 1.2f;
        [SerializeField] private AudioSource footstepAudioSource = default;
        [SerializeField] private AudioClip[] outsideFootstepClips = default;
        [SerializeField] private AudioClip[] insideFootstepClips = default;
        [SerializeField] private AudioClip[] grassFootstepClips = default;
        private float footstepTimer = 0f;

        private float GetCurrentOffset => (isCrouching && inWater) ? baseStepSpeed * waterCrouchStepMultiplier : (isRunning && inWater && !carryingHeavyObj) ? baseStepSpeed * waterRunStepMultiplier : inWater ? baseStepSpeed * waterStepSpeed : isCrouching ? baseStepSpeed * crouchStepMultiplier : (isRunning && !carryingHeavyObj) ? baseStepSpeed * RunStepMultiplier : baseStepSpeed ;

        // Sliding Settings
        private Vector3 hitPointNormal;
        private bool isSliding
        {
            get
            {
                if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 5.0f))
                {
                    hitPointNormal = slopeHit.normal;

                    //prevents the player from jumping while sliding
                    if (Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit)
                    {
                        canJump = false;
                    }
                    else
                    {
                        canJump = true;
                    }
                    return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
                }
                else { return false; }
            }
        }

        [Header("Interaction Settings")]
        [SerializeField] private Vector3 interactionRayPoint = new Vector3(0.5f, 0.5f, 0);
        [SerializeField] private float interactionDistance = 2.0f;
        [SerializeField] private LayerMask interactionLayer = 7;
        private Interactable currentInteractable;

        #endregion

        [SerializeField]
        private Camera playerCamera;
        private CharacterController characterController;

        private Vector3 moveDirection;
        private Vector2 currentInput;

        private float rotationX = 0;

        [SerializeField] private Vector3 NewGamePos = new Vector3(-0.1f, 0.71f, -20.95f);
        [SerializeField] private Vector3 LastSavePos;

        private void Awake()
        {
            if (fpsSam == null)
            {
                fpsSam = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (fpsSam != null && fpsSam != this)
            {
                Destroy(this.gameObject);
            }

            playerCamera = GetComponentInChildren<Camera>();
            cameraShake = GetComponentInChildren<CameraShake>();
            characterController = GetComponent<CharacterController>();
            defaultYPos = playerCamera.transform.localPosition.y;
            defaultFOV = playerCamera.fieldOfView;        


            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (canMove)
            {
                HandleMovementInput();
                HandleMouseLook(); // look into moving into Lateupdate if motion is jittery

                if (canJump)        { HandleJump();                                         }
                if (canCrouch)      { HandleCrouch();                                       }
                if (canUseHeadbob)  { HandleHeadBob();                                      }
                if (canZoom)        { HandleZoom();                                         }
                if (canInteract)    { HandleInteractionCheck(); HandleInteractionInput();   }
                if (useFootsteps)   { HandleFootsteps();                                    }

                ApplyFinalMovement();
                EnergyDrain();
                PlayerStats.playerStats.RechargeSuit();
            }
        }

        private void LateUpdate()
        {

        }

        public void LockPlayerMovement()
        {
            canMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void UnlockPlayerMovement()
        {
            canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void HandleMovementInput()
        {
            // Read inputs
            currentInput = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxis("Horizontal"));

            // normalizes input when 2 directions are pressed at the same time
            // TODO; find a more elegant solution to normalize, this is a bit of a hack method to normalize it estimates and is not 100% accurate.
            currentInput *= (currentInput.x != 0.0f && currentInput.y != 0.0f) ? 0.7071f : 1.0f;

            // Sets the required speed multiplier
            if (inWater) currentInput *= (isCrouching ? suitCrouchSpeed : (isRunning && PlayerStats.playerStats.suitPower > 0 && !carryingHeavyObj) ? suitRunSpeed : suitWalkSpeed);
            else currentInput *= (isCrouching ? crouchSpeed : (isRunning && !carryingHeavyObj) ? runSpeed : walkSpeed);

            float moveDirectionY = moveDirection.y;
            moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
            moveDirection.y = moveDirectionY;
        }

        private void HandleMouseLook()
        {
            // Rotate camera up/down
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
            rotationX = Mathf.Clamp(rotationX, lowerLookLimit, upperLookLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Rotate player left/right
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);

        }

        private void HandleJump()
        {
            if (shouldJump)
            {
                if (inWater)
                {
                    moveDirection.y = waterJumpForce;
                }
                else
                {
                    moveDirection.y = jumpForce;
                }
            }
        }

        private void HandleCrouch()
        {
            if (shouldCrouch)
            {
                StartCoroutine(CrouchStand());
            }
        }

        private void HandleHeadBob()
        {
            // TODO: find a better headbob system that feels more natural.
        
            if (!characterController.isGrounded) return;

            if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
            {
                //timer += Time.deltaTime;
                //if (inWater)
                //{
                //    Debug.Log("In water");
                //    if (isCrouching)
                //    {
                //        playerCamera.gameObject.transform.localPosition = cameraShake.PerlinHeadBob(timer, 1.0f, waterCrouchBobSpeed, waterCrouchBobAmount, lowerBobLimit, upperBobLimit);
                //        Debug.Log("waterCrouchBob." + " playerCam local pos: " + playerCamera.gameObject.transform.localPosition.y);
                //    }
                //    else if (isRunning)
                //    {
                //        playerCamera.gameObject.transform.localPosition = cameraShake.PerlinHeadBob(timer, 1.0f, waterRunBobSpeed, waterRunBobAmount, lowerBobLimit, upperBobLimit);
                //        Debug.Log("waterRunBob." + " playerCam local pos: " + playerCamera.gameObject.transform.localPosition.y);
                //    }
                //    else
                //    {
                //        playerCamera.gameObject.transform.localPosition = cameraShake.PerlinHeadBob(timer, 1.0f, waterWalkBobSpeed, waterWalkBobAmount, lowerBobLimit, upperBobLimit);
                //        Debug.Log("waterWalkBob." + " playerCam local pos: " + playerCamera.gameObject.transform.localPosition.y);
                //    }
                //}
                //else
                //{
                //    Debug.Log("In Atmosphere");
                //    if (isCrouching)
                //    {
                //        playerCamera.gameObject.transform.localPosition = cameraShake.PerlinHeadBob(timer, 1.0f, crouchBobSpeed, crouchBobAmount, lowerBobLimit, upperBobLimit);
                //        Debug.Log("CrouchBob." + " playerCam local pos: " + playerCamera.gameObject.transform.localPosition.y);
                //    }
                //    else if (isRunning)
                //    {
                //        playerCamera.gameObject.transform.localPosition = cameraShake.PerlinHeadBob(timer, 1.0f, runBobSpeed, runBobAmount, lowerBobLimit, upperBobLimit);
                //        Debug.Log("RunBob." + " playerCam local pos: " + playerCamera.gameObject.transform.localPosition.y);
                //    }
                //    else
                //    {
                //        playerCamera.gameObject.transform.localPosition = cameraShake.PerlinHeadBob(timer, 1.0f, walkBobSpeed, walkBobAmount, lowerBobLimit, upperBobLimit);
                //        Debug.Log("WalkBob." + " playerCam local pos: " + playerCamera.gameObject.transform.localPosition.y);
                //    }
                //}

                if (inWater)
                {
                    //Debug.Log("In Water");
                    //Debug.Log("Timer:" + timer);
                    timer += Time.deltaTime * (isCrouching ? waterCrouchBobSpeed : (isRunning && PlayerStats.playerStats.suitPower > 0 && !carryingHeavyObj) ? waterRunBobSpeed : waterWalkBobSpeed);
                    playerCamera.transform.localPosition = new Vector3(
                        playerCamera.transform.localPosition.x,
                        defaultYPos + (Mathf.Sin(timer) * (isCrouching ? waterCrouchBobAmount : (isRunning && PlayerStats.playerStats.suitPower > 0 && !carryingHeavyObj) ? waterRunBobAmount : waterWalkBobAmount))/2,
                        playerCamera.transform.localPosition.z);
                }
                else
                {
                    //Debug.Log("In Atmosphere");
                    //Debug.Log("Timer:" + timer);
                    timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : (isRunning && !carryingHeavyObj) ? runBobSpeed : walkBobSpeed);
                    playerCamera.transform.localPosition = new Vector3(
                        playerCamera.transform.localPosition.x,
                        defaultYPos + (Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : (isRunning && !carryingHeavyObj) ? runBobAmount : walkBobAmount))/2,
                        playerCamera.transform.localPosition.z);
                }

            }
        }

        private void HandleZoom()
        {
            if (Input.GetKeyDown(zoomKey))
            {
                if (zoomRoutine != null)
                {
                    StopCoroutine(zoomRoutine);
                    zoomRoutine = null;
                }
                zoomRoutine = StartCoroutine(ToggleZoom(true));
            }

            if (Input.GetKeyUp(zoomKey))
            {
                if (zoomRoutine != null)
                {
                    StopCoroutine(zoomRoutine);
                    zoomRoutine = null;
                }
                zoomRoutine = StartCoroutine(ToggleZoom(false));
            }
        }

        private void HandleInteractionCheck()
        {
            if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
            {
                if (hit.collider.gameObject.layer == 7 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID() ) )
                {
                    hit.collider.TryGetComponent(out currentInteractable);

                    if (currentInteractable)
                    {
                        currentInteractable.OnFocus();
                    }
                }
            }
            else if (currentInteractable)
            {
                currentInteractable.OnLoseFocus();
                currentInteractable = null; 
            }
        }

        private void HandleInteractionInput()
        {
            // TODO: Research TAG vs Layermask, at the moment it seems like using a tag as verificatin is going to work better
            // potential option for replacement; https://www.youtube.com/watch?v=5MbR2qJK8Tc


            if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
            {            
                currentInteractable.OnInteract();
            }
        }

        private void HandleFootsteps()
        {
            if (!characterController.isGrounded) return;
            if (currentInput == Vector2.zero) return;

            footstepTimer -= Time.deltaTime;
              

            if (footstepTimer <= 0)
            {
                if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 3))
                {
                    switch (inWater)
                    {
                        case true:
                            if (outsideFootstepClips.Length == 0) return;
                            footstepAudioSource.PlayOneShot(outsideFootstepClips[Random.Range(0, outsideFootstepClips.Length - 1)]);
                            break;

                        case false:
                            if (insideFootstepClips.Length == 0) return;
                            footstepAudioSource.PlayOneShot(insideFootstepClips[Random.Range(0, insideFootstepClips.Length - 1)]);
                            break;
                    }
                }

                footstepTimer = GetCurrentOffset;

            }

        }

        private void ApplyFinalMovement()
        {
            // Apply gravity if the character controller is not grounded
            if (!characterController.isGrounded)
            {
                if (inWater)
                {
                    moveDirection.y -= waterGravity * Time.deltaTime;
                }
                else
                {
                    moveDirection.y -= gravity * Time.deltaTime;
                }
            
            }

            if (characterController.velocity.y < - 1 && characterController.isGrounded)
                moveDirection.y = 0;


            // sliding
            if (canSlideOnSlopes && isSliding)
            {
                if (inWater)
                {
                    moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * suitSlopeSpeed;
                }
                else
                {
                    moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
                }
            }

            // dashing
            UnderwaterDash();
        
        

            // applies movement based on all inputs
            characterController.Move(moveDirection * Time.deltaTime);
        }

        private IEnumerator CrouchStand()
        {
            if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1.0f, ~(LayerMask.GetMask("PostProcessing", "Water"))))
            {
                Debug.Log("CrouchBlocked");
                yield return null;
            }

                duringCrouchAnimation = true;

            float timeElapsed = 0;
            float targetHeight = isCrouching ? standingHeight : crouchHeight;
            float currentHeight = characterController.height;
            Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
            Vector3 currentCenter = characterController.center;

            while (timeElapsed < timeToCrouch)
            {
                characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
                characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        
            characterController.height = targetHeight;
            characterController.center = targetCenter;

            isCrouching = !isCrouching;

            duringCrouchAnimation = false;
        }

        private IEnumerator ToggleZoom(bool isEnter)
        {
            float targetFOV = isEnter ? zoomFOV : defaultFOV;
            float startingFOV = playerCamera.fieldOfView; // capture reference to current FOV
            float timeElapsed = 0;

            while (timeElapsed < timeToZoom)
            {
                playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            playerCamera.fieldOfView = targetFOV;
            zoomRoutine = null;
        }

        private void UnderwaterDash()
        {
            if (PlayerStats.playerStats.suitPower <= 0 || carryingHeavyObj) return;
            if (Input.GetButtonDown("Dash"))
            {
                Debug.Log("Dash Input Pressed");
                isDashing = true;
            }

            if (!inWater) 
            {
                isDashing = false;
                dashTimer = 0;
                return;
            }
            else if (!isDashing)
            {
                dashTimer = 0;
                return;
            }
            else
            {
                //Debug.Log(dashTimer);
                dashTimer += Time.deltaTime;
            }

            if (dashTimer < dashLength && isDashing)
            {
                Debug.Log("Dashing");
                Debug.Log(currentInput.x + " " + currentInput.y);
                moveDirection = ((transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y)) * dashSpeed;
            }
            else
            {
                PlayerStats.playerStats.SuitDashDrain();
                isDashing = false;
                return;
            }
        }

        private void EnergyDrain()
        {
            if (!inWater) return;
            PlayerStats.playerStats.SuitPowerLogic();

            if (!isRunning) return;
            PlayerStats.playerStats.SuitSprintDrain();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Water"))
            {
                this.inWater = true;
                this.gameObject.transform.GetComponentInChildren<FogEffect>().effectActive = true;
                this.gameObject.transform.GetComponentInChildren<UnderWaterEffect>().effectActive = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Water"))
            {
                IndoorTransition();
            }
        }

        public void IndoorTransition()
        {
            this.inWater = false;
            this.gameObject.transform.GetComponentInChildren<FogEffect>().effectActive = false;
            this.gameObject.transform.GetComponentInChildren<UnderWaterEffect>().effectActive = false;
        }

        public void ResetRun()
        {
            isDashing = false;
            inWater = false;
            carryingHeavyObj = false;
            canRun = true;
            canJump = true;
            canCrouch = true;
            canUseHeadbob = true;
            canSlideOnSlopes = true;
            canZoom = true;
            canInteract = true;
            useFootsteps = true;
            canMove = true;
            this.gameObject.GetComponent<CharacterController>().enabled = false;
            Physics.autoSyncTransforms = true;
            this.gameObject.transform.position = NewGamePos;
            Physics.autoSyncTransforms = false;
            this.gameObject.GetComponent<CharacterController>().enabled = true;
        }

        public void DisableCharacterController()
        {
            if (this.gameObject.GetComponent<CharacterController>().enabled == false) return;
            this.gameObject.GetComponent<CharacterController>().enabled = false;
        }

        public void EnableCharacterController()
        {
            if (this.gameObject.GetComponent<CharacterController>().enabled == true) return;
            this.gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }

}
