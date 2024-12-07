using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Urth;

namespace Urth
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class UrthCharacterManager : MonoBehaviour
    {
        [Header("Movement specifics")]
        [Tooltip("Layers where the player can stand on")]
        [SerializeField] LayerMask groundMask;
        [Tooltip("Base player speed")]
        public float movementSpeed = 14f;
        [Range(0f, 1f)]
        [Tooltip("Minimum input value to trigger movement")]
        public float crouchSpeedMultiplier = 0.248f;
        [Range(0.01f, 0.99f)]
        [Tooltip("Minimum input value to trigger movement")]
        public float movementThreshold = 0.01f;
        [Space(10)]

        [Tooltip("Speed up multiplier")]
        public float dampSpeedUp = 0.2f;
        [Tooltip("Speed down multiplier")]
        public float dampSpeedDown = 0.1f;


        [Header("Jump and gravity specifics")]
        [Tooltip("Jump velocity")]
        public float jumpVelocity = 20f;
        [Tooltip("Multiplier applied to gravity when the player is falling")]
        public float fallMultiplier = 1.7f;
        [Tooltip("Multiplier applied to gravity when the player is holding jump")]
        public float holdJumpMultiplier = 5f;
        [Range(0f, 1f)]
        [Tooltip("Player friction against floor")]
        public float frictionAgainstFloor = 0.3f;
        [Range(0.01f, 0.99f)]
        [Tooltip("Player friction against wall")]
        public float frictionAgainstWall = 0.839f;
        [Space(10)]

        [Tooltip("Player can long jump")]
        public bool canLongJump = true;


        [Header("Slope and step specifics")]
        [Tooltip("Distance from the player feet used to check if the player is touching the ground")]
        public float groundCheckerThreshold = 0.1f;
        [Tooltip("Distance from the player feet used to check if the player is touching a slope")]
        public float slopeCheckerThreshold = 0.51f;
        [Tooltip("Distance from the player center used to check if the player is touching a step")]
        public float stepCheckerThreshold = 0.6f;
        [Space(10)]

        [Range(1f, 89f)]
        [Tooltip("Max climbable slope angle")]
        public float maxClimbableSlopeAngle = 53.6f;
        [Tooltip("Max climbable step height")]
        public float maxStepHeight = 0.74f;
        [Space(10)]

        [Tooltip("Speed multiplier based on slope angle")]
        public AnimationCurve speedMultiplierOnAngle = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [Range(0.01f, 1f)]
        [Tooltip("Multipler factor on climbable slope")]
        public float canSlideMultiplierCurve = 0.061f;
        [Range(0.01f, 1f)]
        [Tooltip("Multipler factor on non climbable slope")]
        public float cantSlideMultiplierCurve = 0.039f;
        [Range(0.01f, 1f)]
        [Tooltip("Multipler factor on step")]
        public float climbingStairsMultiplierCurve = 0.637f;
        [Space(10)]

        [Tooltip("Multipler factor for gravity")]
        public float gravityMultiplier = 6f;
        [Tooltip("Multipler factor for gravity used on change of normal")]
        public float gravityMultiplyerOnSlideChange = 3f;
        [Tooltip("Multipler factor for gravity used on non climbable slope")]
        public float gravityMultiplierIfUnclimbableSlope = 30f;
        [Space(10)]

        public bool lockOnSlope = false;


        [Header("Wall slide specifics")]
        [Tooltip("Distance from the player head used to check if the player is touching a wall")]
        public float wallCheckerThreshold = 0.8f;
        [Tooltip("Wall checker distance from the player center")]
        public float hightWallCheckerChecker = 0.5f;
        [Space(10)]

        [Tooltip("Multiplier used when the player is jumping from a wall")]
        public float jumpFromWallMultiplier = 30f;
        [Tooltip("Factor used to determine the height of the jump")]
        public float multiplierVerticalLeap = 1f;


        [Header("Sprint and crouch specifics")]
        [Tooltip("Sprint speed")]
        public float sprintSpeed = 20f;
        [Tooltip("Multipler applied to the collider when player is crouching")]
        public float crouchHeightMultiplier = 0.5f;
        [Tooltip("FP camera head height")]
        public Vector3 POV_normalHeadHeight = new Vector3(0f, 0.5f, -0.1f);
        [Tooltip("FP camera head height when crouching")]
        public Vector3 POV_crouchHeadHeight = new Vector3(0f, -0.1f, -0.1f);


        [Header("References")]
        [Tooltip("Character camera")]
        public GameObject characterCamera;
        [Tooltip("Character model")]
        public GameObject characterModel;
        [Tooltip("Character rotation speed when the forward direction is changed")]
        public float characterModelRotationSmooth = 0.1f;
        [Space(10)]

        [Tooltip("Default character mesh")]
        public GameObject meshCharacter;
        [Tooltip("Crouch character mesh")]
        public GameObject meshCharacterCrouch;
        [Tooltip("Head reference")]
        public Transform headPoint;
        [Space(10)]

        [Tooltip("Input reference")]
        public InputReaderAbstract input;
        [Tooltip("Creature manager")]
        public CreatureManager creatureManager;

        [Space(10)]

        public bool debug = true;


        [Header("Events")]
        [SerializeField] UnityEvent OnJump;
        [Space(15)]

        public float minimumVerticalSpeedToLandEvent;
        [SerializeField] UnityEvent OnLand;
        [Space(15)]

        public float minimumHorizontalSpeedToFastEvent;
        [SerializeField] UnityEvent OnFast;
        [Space(15)]

        [SerializeField] UnityEvent OnWallSlide;
        [Space(15)]

        [SerializeField] UnityEvent OnSprint;
        [Space(15)]

        [SerializeField] UnityEvent OnCrouch;
        [Space(15)]



        private Vector3 forward;
        private Vector3 globalForward;
        private Vector3 reactionForward;
        private Vector3 down;
        private Vector3 globalDown;
        private Vector3 reactionGlobalDown;

        private float currentSurfaceAngle;
        private bool currentLockOnSlope;

        private Vector3 wallNormal;
        private Vector3 groundNormal;
        private Vector3 prevGroundNormal;
        private bool prevGrounded;

        private float coyoteJumpMultiplier = 1f;

        private bool isGrounded = false;
        private bool isTouchingSlope = false;
        private bool isTouchingStep = false;
        private bool isTouchingWall = false;
        private bool isJumping = false;
        private bool isCrouch = false;

        public float leftLayerWeight = 0f;
        public float rightLayerWeight = 0f;
        private int leftGuardIdx = 0;
        private int rightGuardIdx = 0;
        private bool isGuard = false;
        private bool isStrikingRight = false;
        private bool isStrikingLeft = false;
        private bool isHoldingRight = false;
        private bool isHoldingLeft = false;
        private int rightStrikeIdx = 0;
        private int leftStrikeIdx = 0;

        private Vector2 axisInput;
        private Vector3 lookInput;
        private bool jump;
        private bool jumpHold;
        private bool sprint;
        private bool crouch;
        private bool guard;
        private bool rightHold;
        private bool rightStrikeInput;
        private bool rightStrikeActive;
        private bool rightStrikeRecoil;
        private bool leftHold;
        private bool leftStrikeInput;
        private bool leftStrikeActive;
        private bool leftStrikeRecoil;

        private bool centerHold;
        private bool centerStrikeInput;
        private bool centerStrikeActive;
        private bool centerStrikeRecoil;

        [HideInInspector]
        public float targetAngle;
        new private Rigidbody rigidbody;
        new private CapsuleCollider collider;
        private float originalColliderHeight;

        private Vector3 currVelocity = Vector3.zero;
        private float turnSmoothVelocity;
        private bool lockRotation = false;


        /**/


        private void Awake()
        {
            rigidbody = this.GetComponent<Rigidbody>();
            collider = this.GetComponent<CapsuleCollider>();
            originalColliderHeight = collider.height;

            SetFriction(frictionAgainstFloor, true);
            currentLockOnSlope = lockOnSlope;

            //urth changes
            //creatureManager = this.GetComponent<CreatureManager>();
        }


        private void Update()
        {
            //input
            axisInput = input.AxisInput;
            lookInput = input.LookVector;
            jump = input.Jump;
            jumpHold = input.JumpHold;
            sprint = input.Sprint;
            crouch = input.Crouch;
            guard = input.Guard;

            rightStrikeInput = input.RightStrike;
            leftStrikeInput = input.LeftStrike;
            rightHold = input.RightHold;
            leftHold = input.LeftHold;

        }


        private void FixedUpdate()
        {
            //local vectors
            CheckGrounded();
            CheckStep();
            CheckWall();
            CheckSlopeAndDirections();

            //movement
            MoveCrouch();
            MoveWalk();
            MoveRotation();
            MoveJump();
            //MoveArms();



            //gravity
            ApplyGravity();

            //events
            UpdateEvents();
        }


        #region Checks

        private void CheckGrounded()
        {
            prevGrounded = isGrounded;
            isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), groundCheckerThreshold, groundMask);
        }


        private void CheckStep()
        {
            bool tmpStep = false;
            Vector3 bottomStepPos = transform.position - new Vector3(0f, originalColliderHeight / 2f, 0f) + new Vector3(0f, 0.05f, 0f);

            RaycastHit stepLowerHit;
            if (Physics.Raycast(bottomStepPos, globalForward, out stepLowerHit, stepCheckerThreshold, groundMask))
            {
                RaycastHit stepUpperHit;
                if (RoundValue(stepLowerHit.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), globalForward, out stepUpperHit, stepCheckerThreshold + 0.05f, groundMask))
                {
                    //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                    tmpStep = true;
                }
            }

            RaycastHit stepLowerHit45;
            if (Physics.Raycast(bottomStepPos, Quaternion.AngleAxis(45, transform.up) * globalForward, out stepLowerHit45, stepCheckerThreshold, groundMask))
            {
                RaycastHit stepUpperHit45;
                if (RoundValue(stepLowerHit45.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), Quaternion.AngleAxis(45, Vector3.up) * globalForward, out stepUpperHit45, stepCheckerThreshold + 0.05f, groundMask))
                {
                    //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                    tmpStep = true;
                }
            }

            RaycastHit stepLowerHitMinus45;
            if (Physics.Raycast(bottomStepPos, Quaternion.AngleAxis(-45, transform.up) * globalForward, out stepLowerHitMinus45, stepCheckerThreshold, groundMask))
            {
                RaycastHit stepUpperHitMinus45;
                if (RoundValue(stepLowerHitMinus45.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), Quaternion.AngleAxis(-45, Vector3.up) * globalForward, out stepUpperHitMinus45, stepCheckerThreshold + 0.05f, groundMask))
                {
                    //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                    tmpStep = true;
                }
            }

            isTouchingStep = tmpStep;
        }


        private void CheckWall()
        {
            bool tmpWall = false;
            Vector3 tmpWallNormal = Vector3.zero;
            Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + hightWallCheckerChecker, transform.position.z);

            RaycastHit wallHit;
            if (Physics.Raycast(topWallPos, globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(45, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(90, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(135, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(180, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(225, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(270, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(315, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }

            isTouchingWall = tmpWall;
            wallNormal = tmpWallNormal;
        }


        private void CheckSlopeAndDirections()
        {
            prevGroundNormal = groundNormal;

            RaycastHit slopeHit;
            if (Physics.SphereCast(transform.position, slopeCheckerThreshold, Vector3.down, out slopeHit, originalColliderHeight / 2f + 0.5f, groundMask))
            {
                groundNormal = slopeHit.normal;

                if (slopeHit.normal.y == 1)
                {

                    forward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    globalForward = forward;
                    reactionForward = forward;

                    SetFriction(frictionAgainstFloor, true);
                    currentLockOnSlope = lockOnSlope;

                    currentSurfaceAngle = 0f;
                    isTouchingSlope = false;

                }
                else
                {
                    //set forward
                    Vector3 tmpGlobalForward = transform.forward.normalized;
                    Vector3 tmpForward = new Vector3(tmpGlobalForward.x, Vector3.ProjectOnPlane(transform.forward.normalized, slopeHit.normal).normalized.y, tmpGlobalForward.z);
                    Vector3 tmpReactionForward = new Vector3(tmpForward.x, tmpGlobalForward.y - tmpForward.y, tmpForward.z);

                    if (currentSurfaceAngle <= maxClimbableSlopeAngle && !isTouchingStep)
                    {
                        //set forward
                        forward = tmpForward;// * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);
                        globalForward = tmpGlobalForward;// * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);
                        reactionForward = tmpReactionForward;// * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);
                        if (tmpForward.y > 0)
                        {
                            forward *= speedMultiplierOnAngle.Evaluate(Mathf.Sin(tmpForward.y * Mathf.PI / 2));
                            globalForward *= speedMultiplierOnAngle.Evaluate(Mathf.Sin(tmpForward.y * Mathf.PI / 2));
                            reactionForward *= speedMultiplierOnAngle.Evaluate(Mathf.Sin(tmpForward.y * Mathf.PI / 2));
                        }

                        SetFriction(frictionAgainstFloor, true);
                        currentLockOnSlope = lockOnSlope;
                    }
                    else if (isTouchingStep)
                    {
                        //set forward
                        forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);
                        globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);
                        reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);

                        SetFriction(frictionAgainstFloor, true);
                        currentLockOnSlope = true;
                    }
                    else
                    {
                        //set forward
                        forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);
                        globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);
                        reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);

                        SetFriction(0f, true);
                        currentLockOnSlope = lockOnSlope;
                    }

                    currentSurfaceAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                    isTouchingSlope = true;
                }

                //set down
                down = Vector3.Project(Vector3.down, slopeHit.normal);
                globalDown = Vector3.down.normalized;
                reactionGlobalDown = Vector3.up.normalized;
            }
            else
            {
                groundNormal = Vector3.zero;

                forward = Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;
                globalForward = forward;
                reactionForward = forward;

                //set down
                down = Vector3.down.normalized;
                globalDown = Vector3.down.normalized;
                reactionGlobalDown = Vector3.up.normalized;

                SetFriction(frictionAgainstFloor, true);
                currentLockOnSlope = lockOnSlope;
            }
        }

        #endregion


        #region Move

        private void MoveCrouch()
        {
            if (crouch && isGrounded)
            {
                isCrouch = true;
                if (meshCharacterCrouch != null && meshCharacter != null) meshCharacter.SetActive(false);
                if (meshCharacterCrouch != null) meshCharacterCrouch.SetActive(true);

                float newHeight = originalColliderHeight * crouchHeightMultiplier;
                collider.height = newHeight;
                collider.center = new Vector3(0f, -newHeight * crouchHeightMultiplier, 0f);

                headPoint.position = new Vector3(transform.position.x + POV_crouchHeadHeight.x, transform.position.y + POV_crouchHeadHeight.y, transform.position.z + POV_crouchHeadHeight.z);
            }
            else
            {
                isCrouch = false;
                if (meshCharacterCrouch != null && meshCharacter != null) meshCharacter.SetActive(true);
                if (meshCharacterCrouch != null) meshCharacterCrouch.SetActive(false);

                collider.height = originalColliderHeight;
                collider.center = Vector3.zero;

                headPoint.position = new Vector3(transform.position.x + POV_normalHeadHeight.x, transform.position.y + POV_normalHeadHeight.y, transform.position.z + POV_normalHeadHeight.z);
            }
        }


        private void MoveWalk()
        {
            float crouchMultiplier = 1f;
            if (isCrouch) crouchMultiplier = crouchSpeedMultiplier;

            if (axisInput.magnitude > movementThreshold)
            {
                targetAngle = Mathf.Atan2(axisInput.x, axisInput.y) * Mathf.Rad2Deg + (characterCamera != null ? characterCamera.transform.eulerAngles.y : 0);

                if (!sprint) rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, forward * movementSpeed * crouchMultiplier, ref currVelocity, dampSpeedUp);
                else rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, forward * sprintSpeed * crouchMultiplier, ref currVelocity, dampSpeedUp);
            }
            else rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, Vector3.zero * crouchMultiplier, ref currVelocity, dampSpeedDown);
        }


        private void MoveRotation()
        {
            float angle = Mathf.SmoothDampAngle(characterModel.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, characterModelRotationSmooth);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            if (!lockRotation) characterModel.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            else
            {
                var lookPos = -wallNormal;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                characterModel.transform.rotation = rotation;
            }
        }


        private void MoveJump()
        {
            //jumped
            if (jump && isGrounded && ((isTouchingSlope && currentSurfaceAngle <= maxClimbableSlopeAngle) || !isTouchingSlope) && !isTouchingWall)
            {
                rigidbody.velocity += Vector3.up * jumpVelocity;
                isJumping = true;
            }
            //jumped from wall
            else if (jump && !isGrounded && isTouchingWall)
            {
                rigidbody.velocity += wallNormal * jumpFromWallMultiplier + (Vector3.up * jumpFromWallMultiplier) * multiplierVerticalLeap;
                isJumping = true;

                targetAngle = Mathf.Atan2(wallNormal.x, wallNormal.z) * Mathf.Rad2Deg;

                forward = wallNormal;
                globalForward = forward;
                reactionForward = forward;
            }

            //is falling
            if (rigidbody.velocity.y < 0 && !isGrounded) coyoteJumpMultiplier = fallMultiplier;
            else if (rigidbody.velocity.y > 0.1f && (currentSurfaceAngle <= maxClimbableSlopeAngle || isTouchingStep))
            {
                //is short jumping
                if (!jumpHold || !canLongJump) coyoteJumpMultiplier = 1f;
                //is long jumping
                else coyoteJumpMultiplier = 1f / holdJumpMultiplier;
            }
            else
            {
                isJumping = false;
                coyoteJumpMultiplier = 1f;
            }
        }

        private int GetGuardIdx(GUARD guard)
        {
            switch (guard)
            {
                case GUARD.HAND:
                    return 0;
                case GUARD.SHIELD:
                    return 1;
                case GUARD.ONE_HAND:
                    return 2;
                case GUARD.TWO_HAND:
                    return 3;
            }
            return 0;
        }

        private int GetStrikeIdx(ATTACK_FORM attackForm)
        {
            switch (attackForm)
            {
                case ATTACK_FORM.JAB:
                    return 0;
                case ATTACK_FORM.PUNCH:
                    return 1;
                case ATTACK_FORM.HAYMAKER:
                    return 2;
                case ATTACK_FORM.THRUST:
                    return 3;
                case ATTACK_FORM.LUNGE_THRUST:
                    return 4;
                case ATTACK_FORM.DRAW_CUT:
                    return 5;
                case ATTACK_FORM.PUSH_CUT:
                    return 6;
                case ATTACK_FORM.SWING:
                    return 7;
                case ATTACK_FORM.BACKSWING:
                    return 8;
                case ATTACK_FORM.OVERHEAD:
                    return 9;
                case ATTACK_FORM.UNDERHAND:
                    return 10;
            }
            return 0;
        }

        private void MoveArms()
        {
            leftLayerWeight = (guard || leftHold || leftStrikeActive || leftStrikeRecoil) ? 1f : 0f;
            rightLayerWeight = (guard || rightHold || rightStrikeActive || rightStrikeRecoil) ? 1f : 0f;
            isGuard = guard;
            leftGuardIdx = guard ? GetGuardIdx(creatureManager.offense.leftWeap.guard) : 0;
            rightGuardIdx = guard ? GetGuardIdx(creatureManager.offense.rightWeap.guard) : 0;
            leftStrikeIdx = creatureManager.offense.leftWeap ? GetStrikeIdx(creatureManager.offense.leftWeap.primaryForm) : 0;
            rightStrikeIdx = creatureManager.offense.rightWeap ? GetStrikeIdx(creatureManager.offense.rightWeap.primaryForm) : 0;
            isHoldingLeft = leftHold;
            isHoldingRight = rightHold;
            isStrikingRight = rightStrikeInput;
            isStrikingLeft = leftStrikeInput;
            if (leftStrikeInput && !leftStrikeActive)
            {
                Debug.Log("starting left strike");
                creatureManager.offense.StartStrike(SYMMETRY.LEFT);
                leftStrikeActive = true;
            }
            if (rightStrikeInput && !rightStrikeActive)
            {
                Debug.Log("starting right strike");
                creatureManager.offense.StartStrike(SYMMETRY.RIGHT);
                rightStrikeActive = true;
            }

        }

        #endregion


        #region Gravity

        private void ApplyGravity()
        {
            Vector3 gravity = Vector3.zero;

            if (currentLockOnSlope || isTouchingStep) gravity = down * gravityMultiplier * -Physics.gravity.y * coyoteJumpMultiplier;
            else gravity = globalDown * gravityMultiplier * -Physics.gravity.y * coyoteJumpMultiplier;

            //avoid little jump
            if (groundNormal.y != 1 && groundNormal.y != 0 && isTouchingSlope && prevGroundNormal != groundNormal)
            {
                //Debug.Log("Added correction jump on slope");
                gravity *= gravityMultiplyerOnSlideChange;
            }

            //slide if angle too big
            if (groundNormal.y != 1 && groundNormal.y != 0 && (currentSurfaceAngle > maxClimbableSlopeAngle && !isTouchingStep))
            {
                //Debug.Log("Slope angle too high, character is sliding");
                if (currentSurfaceAngle > 0f && currentSurfaceAngle <= 30f) gravity = globalDown * gravityMultiplierIfUnclimbableSlope * -Physics.gravity.y;
                else if (currentSurfaceAngle > 30f && currentSurfaceAngle <= 89f) gravity = globalDown * gravityMultiplierIfUnclimbableSlope / 2f * -Physics.gravity.y;
            }

            //friction when touching wall
            if (isTouchingWall && rigidbody.velocity.y < 0) gravity *= frictionAgainstWall;

            rigidbody.AddForce(gravity);
        }

        #endregion


        #region Events

        private void UpdateEvents()
        {
            if ((jump && isGrounded && ((isTouchingSlope && currentSurfaceAngle <= maxClimbableSlopeAngle) || !isTouchingSlope)) || (jump && !isGrounded && isTouchingWall)) OnJump.Invoke();
            if (isGrounded && !prevGrounded && rigidbody.velocity.y > -minimumVerticalSpeedToLandEvent) OnLand.Invoke();
            if (Mathf.Abs(rigidbody.velocity.x) + Mathf.Abs(rigidbody.velocity.z) > minimumHorizontalSpeedToFastEvent) OnFast.Invoke();
            if (isTouchingWall && rigidbody.velocity.y < 0) OnWallSlide.Invoke();
            if (sprint) OnSprint.Invoke();
            if (isCrouch) OnCrouch.Invoke();
        }



        #endregion


        #region Friction and Round

        private void SetFriction(float _frictionWall, bool _isMinimum)
        {
            collider.material.dynamicFriction = 0.6f * _frictionWall;
            collider.material.staticFriction = 0.6f * _frictionWall;

            if (_isMinimum) collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
            else collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
        }


        private float RoundValue(float _value)
        {
            float unit = (float)Mathf.Round(_value);

            if (_value - unit < 0.000001f && _value - unit > -0.000001f) return unit;
            else return _value;
        }

        #endregion


        #region GettersSetters

        public bool GetGrounded() { return isGrounded; }
        public bool GetTouchingSlope() { return isTouchingSlope; }
        public bool GetTouchingStep() { return isTouchingStep; }
        public bool GetTouchingWall() { return isTouchingWall; }
        public bool GetJumping() { return isJumping; }
        public bool GetCrouching() { return isCrouch; }
        public float GetLeftLayerWeight() { return leftLayerWeight; }
        public float GetRightLayerWeight() { return rightLayerWeight; }

        public int GetLeftGuardIdx() { return leftGuardIdx; }
        public int GetRightGuardIdx() { return rightGuardIdx; }
        public bool GetGuarding() { return isGuard; }
        public bool GetLeftHold() { return isHoldingLeft; }
        public bool GetRightHold() { return isHoldingRight; }

        public bool GetLeftAction() { return isStrikingLeft; }
        public void SetLeftStrike(bool val) { isStrikingLeft = val; }
        public bool GetRightAction() { return isStrikingRight; }
        public void SetRightStrike(bool val) { isStrikingRight = val; }
        public int GetLeftStrikeIdx() { return leftStrikeIdx; }
        public int GetRightStrikeIdx() { return rightStrikeIdx; }
        public float GetOriginalColliderHeight() { return originalColliderHeight; }

        public void SetLockRotation(bool _lock) { lockRotation = _lock; }

        #endregion

        #region Combat

        public Dictionary<BodyPartId, bool> strikeBools;
        public void StartStrike(int sym)
        {//only works for left/right/center strikes for now TODO
            CreatureOffenseController creatureOffense = (CreatureOffenseController)creatureManager.offense;
            switch (sym)
            {
                case -1:
                    creatureOffense.StartStrike(SYMMETRY.LEFT);
                    break;
                case 0:
                    creatureOffense.StartStrike(SYMMETRY.CENTER);
                    break;
                case 1:
                    creatureOffense.StartStrike(SYMMETRY.RIGHT);
                    break;
            }
        }
        public void AboutToEndStrike(int sym)
        {//only works for left/right/center strikes for now TODO
            //The range of AttackPoints is reduced after this event, to minimize overshoot on last frame of motion.
            CreatureOffenseController creatureOffense = (CreatureOffenseController)creatureManager.offense;
            switch (sym)
            {
                case -1:
                    //leftStrikeActive = false;
                    creatureOffense.PrepEndStrike(SYMMETRY.LEFT);
                    break;
                case 0:
                    //centerStrikeActive = false;
                    creatureOffense.PrepEndStrike(SYMMETRY.CENTER);
                    break;
                case 1:
                    //rightStrikeActive = false;
                    creatureOffense.PrepEndStrike(SYMMETRY.RIGHT);
                    break;
            }
        }

        public void EndStrike(int sym)
        {//only works for left/right/center strikes for now TODO
            CreatureOffenseController creatureOffense = (CreatureOffenseController)creatureManager.offense;
            switch (sym)
            {
                case -1:
                    leftStrikeActive = false;
                    leftStrikeRecoil = true;
                    creatureOffense.EndStrike(SYMMETRY.LEFT);
                    break;
                case 0:
                    centerStrikeActive = false;
                    centerStrikeRecoil = true;
                    creatureOffense.EndStrike(SYMMETRY.CENTER);
                    break;
                case 1:
                    rightStrikeActive = false;
                    rightStrikeRecoil = true;
                    creatureOffense.EndStrike(SYMMETRY.RIGHT);
                    break;
            }
        }
        public void EndRecoil(int sym)
        {//only works for left/right/center strikes for now TODO
            CreatureOffenseController creatureOffense = creatureManager.offense;
            switch (sym)
            {
                case -1:
                    leftStrikeRecoil = false;
                    creatureOffense.EndStrike(SYMMETRY.LEFT);
                    break;
                case 0:
                    centerStrikeRecoil = false;
                    creatureOffense.EndStrike(SYMMETRY.CENTER);
                    break;
                case 1:
                    rightStrikeRecoil = false;
                    creatureOffense.EndStrike(SYMMETRY.RIGHT);
                    break;
            }
        }
        public void EndBodyPartStrike(string limb, int sym)
        {//only works for left/right/center strikes for now TODO
            BODY_PART strikingPart = (BODY_PART)System.Enum.Parse(typeof(BODY_PART), limb);
            SYMMETRY symmetry = SYMMETRY.CENTER;
            switch (sym)
            {
                case -1:
                    symmetry = (SYMMETRY.LEFT);
                    break;
                case 0:
                    symmetry = (SYMMETRY.CENTER);
                    break;
                case 1:
                    symmetry = (SYMMETRY.RIGHT);
                    break;
            }
            BodyPartId bpid = new BodyPartId(strikingPart, symmetry);
            strikeBools[bpid] = false;
            creatureManager.offense.EndLimbStrike(bpid);
        }

        #endregion

        #region Gizmos

        private void OnDrawGizmos()
        {
            if (debug)
            {
                rigidbody = this.GetComponent<Rigidbody>();
                collider = this.GetComponent<CapsuleCollider>();

                Vector3 bottomStepPos = transform.position - new Vector3(0f, originalColliderHeight / 2f, 0f) + new Vector3(0f, 0.05f, 0f);
                Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + hightWallCheckerChecker, transform.position.z);

                //ground and slope
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), groundCheckerThreshold);

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), slopeCheckerThreshold);

                //direction
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + forward * 2f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, transform.position + globalForward * 2);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, transform.position + reactionForward * 2f);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + down * 2f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, transform.position + globalDown * 2f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, transform.position + reactionGlobalDown * 2f);

                //step check
                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos, bottomStepPos + globalForward * stepCheckerThreshold);

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + new Vector3(0f, maxStepHeight, 0f) + globalForward * (stepCheckerThreshold + 0.05f));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos, bottomStepPos + Quaternion.AngleAxis(45, transform.up) * (globalForward * stepCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + Quaternion.AngleAxis(45, Vector3.up) * (globalForward * stepCheckerThreshold) + new Vector3(0f, maxStepHeight, 0f));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos, bottomStepPos + Quaternion.AngleAxis(-45, transform.up) * (globalForward * stepCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + Quaternion.AngleAxis(-45, Vector3.up) * (globalForward * stepCheckerThreshold) + new Vector3(0f, maxStepHeight, 0f));

                //wall check
                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + globalForward * wallCheckerThreshold);

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(45, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(90, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(135, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(180, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(225, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(270, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(315, transform.up) * (globalForward * wallCheckerThreshold));
            }
        }

        #endregion
    }
}