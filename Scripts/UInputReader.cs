using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace Urth
{
    public class UInputReader : MonoBehaviour, IInput
    {
        public ConstructionPlayer constructionPlayer;
        [HideInInspector]
        public Vector2 AxisInput { get; set; }
        [HideInInspector]
        public Vector2 CameraInput { get; set; } = Vector2.zero;
        [HideInInspector]
        public Vector3 LookVector { get; set; } = Vector3.zero;
        [HideInInspector]
        public bool Jump { get; set; }
        [HideInInspector]
        public bool JumpHold { get; set; }
        [HideInInspector]
        public float Zoom { get; set; }
        [HideInInspector]
        public bool Sprint { get; set; }
        [HideInInspector]
        public bool Crouch { get; set; }
        [HideInInspector]
        public POSTURE Posture { get; set; }
        [HideInInspector]
        public GAIT Gait { get; set; }
        [HideInInspector]
        public bool Guard { get; set; }
        [HideInInspector]
        public bool RightStrike { get; set; }
        [HideInInspector]
        public bool RightHold { get; set; }
        [HideInInspector]
        public bool LeftStrike { get; set; }
        [HideInInspector]
        public bool LeftHold { get; set; }
        [Header("Input specs")]
        public UnityEvent changedInputToMouseAndKeyboard;
        public UnityEvent changedInputToGamepad;

        public bool playerInput = false;

        [Header("Enable inputs")]
        public bool enableJump = true;
        public bool enableCrouch = true;
        public bool enableSprint = true;
        public bool enableGuard = true;
        public bool enableRight = true;
        public bool enableLeft = true;

        MalbersAnimations.Utilities.Aim aim;

        //[HideInInspector]
        //public Vector2 AxisInput { get; set; }
        //[HideInInspector]
        //public Vector2 CameraInput { get; set; } = Vector2.zero;
        //[HideInInspector]
        //public bool Jump { get; set; }
        //[HideInInspector]
        //public bool JumpHold { get; set; }
        //[HideInInspector]
        //public float Zoom { get; set; }
        //[HideInInspector]
        //public bool Sprint { get; set; }
        //[HideInInspector]
        //public bool Crouch { get; set; }


        private bool hasJumped = false;
        private bool skippedFrameForJump = false;
        private bool hasLeftStriked = false;
        private bool skippedFrameForLeft = false;
        private bool hasRightStriked = false;
        private bool skippedFrameForRight = false;
        private bool isMouseAndKeyboard = true;
        private bool oldInput = true;

        //DISABLE if using old input system
        private MovementActions movementActions;
        private UrthActions urthActions;



        /**/


        //DISABLE if using old input system
        private void Awake()
        {
            if (playerInput)
            {

                //movementActions.Gameplay.Movement.performed += ctx => OnMove(ctx);

                //movementActions.Gameplay.Jump.performed += ctx => OnJump();
                //movementActions.Gameplay.Jump.canceled += ctx => JumpEnded();

                //movementActions.Gameplay.Camera.performed += ctx => OnCamera(ctx);

                //movementActions.Gameplay.Sprint.performed += ctx => OnSprint(ctx);
                //movementActions.Gameplay.Sprint.canceled += ctx => SprintEnded(ctx);

                //movementActions.Gameplay.Crouch.performed += ctx => OnCrouch(ctx);
                //movementActions.Gameplay.Crouch.canceled += ctx => CrouchEnded(ctx);

                //movementActions.Gameplay.Guard.performed += ctx => OnGuard(ctx);
                //movementActions.Gameplay.Guard.canceled += ctx => GuardEnded(ctx);

                //movementActions.Gameplay.Right.performed += ctx => OnRight(ctx);
                //movementActions.Gameplay.Right.canceled += ctx => RightEnded(ctx);

                //movementActions.Gameplay.Left.performed += ctx => OnLeft(ctx);
                //movementActions.Gameplay.Left.canceled += ctx => LeftEnded(ctx);
                urthActions = new UrthActions();

                urthActions.Gameplay.AccelTime.performed += ctx => OnAccelTime(ctx);
                urthActions.Gameplay.AccelTime.canceled += ctx => AccelTimeEnded(ctx);

                urthActions.Gameplay.DecelTime.performed += ctx => OnDecelTime(ctx);
                urthActions.Gameplay.DecelTime.canceled += ctx => DecelTimeEnded(ctx);

                urthActions.Gameplay.Interact.performed += ctx => OnInteract(ctx);
                urthActions.Gameplay.Interact.canceled += ctx => InteractEnded(ctx);

                urthActions.Gameplay.ScrollWheel.performed += ctx => OnScroll(ctx);

            }
        }

        void Start()
        {
            if (playerInput)
            {
                aim = GameManager.Instance.playerCharacter.GetComponent<MalbersAnimations.Utilities.Aim>();
            }
        }


        //ENABLE if using old input system
        private void Update()
        {
            /*
             
            axisInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;

            if (enableJump)
            {
                if (Input.GetButtonDown("Jump")) OnJump();
                if (Input.GetButtonUp("Jump")) JumpEnded();
            }

            if (enableSprint) sprint = Input.GetButton("Fire3");
            if (enableCrouch) crouch = Input.GetButton("Fire1");

            GetDeviceOld();

            */
        }


        //DISABLE if using old input system
        private void GetDeviceNew(InputAction.CallbackContext ctx)
        {
            oldInput = isMouseAndKeyboard;

            if (ctx.control.device is Keyboard || ctx.control.device is Mouse) isMouseAndKeyboard = true;
            else isMouseAndKeyboard = false;

            if (oldInput != isMouseAndKeyboard && isMouseAndKeyboard) changedInputToMouseAndKeyboard.Invoke();
            else if (oldInput != isMouseAndKeyboard && !isMouseAndKeyboard) changedInputToGamepad.Invoke();
        }


        //ENABLE if using old input system
        private void GetDeviceOld()
        {
            /*

            oldInput = isMouseAndKeyboard;

            if (Input.GetJoystickNames().Length > 0) isMouseAndKeyboard = false;
            else isMouseAndKeyboard = true;

            if (oldInput != isMouseAndKeyboard && isMouseAndKeyboard) changedInputToMouseAndKeyboard.Invoke();
            else if (oldInput != isMouseAndKeyboard && !isMouseAndKeyboard) changedInputToGamepad.Invoke();

            */
        }


        #region Actions

        //DISABLE if using old input system
        public void OnMove(InputAction.CallbackContext ctx)
        {
            AxisInput = ctx.ReadValue<Vector2>();
            GetDeviceNew(ctx);
        }


        public void OnJump()
        {
            Debug.Log("OnJump");
            if (enableJump)
            {
                Debug.Log("enableJump");
                Jump = true;
                JumpHold = true;

                hasJumped = true;
                skippedFrameForJump = false;
            }
        }


        public void JumpEnded()
        {
            Debug.Log("JumpEnded");
            Jump = false;
            JumpHold = false;
        }

        public void OnScroll(InputAction.CallbackContext ctx)
        {
            if(GameUIControl.Instance.mode == UI_MODE.CONSTRUCTION_PLANNING)
            {
                Vector2 wheelInput = ctx.ReadValue<Vector2>();

                if (wheelInput != Vector2.zero)
                {
                    constructionPlayer.AdjustHeight(wheelInput.y/5000f);
                }
            }

        }


        private void FixedUpdate()
        {
            if (hasJumped && skippedFrameForJump)
            {
                Debug.Log("hasJumped");
                Jump = false;
                hasJumped = false;
            }
            if (!skippedFrameForJump && enableJump)
            {
                skippedFrameForJump = true;
                Debug.Log("SkippedFrameForJump");
            }
            if (hasRightStriked && skippedFrameForRight)
            {
                Debug.Log("hasRightStriked");
                RightStrike = false;
                hasRightStriked = false;
            }
            if (!skippedFrameForRight && enableRight)
            {
                skippedFrameForRight = true;
                Debug.Log("skippedFrameForRight");
            }
            if (hasLeftStriked && skippedFrameForLeft)
            {
                Debug.Log("hasLeftStriked");
                LeftStrike = false;
                hasLeftStriked = false;
            }
            if (!skippedFrameForLeft && enableLeft)
            {
                skippedFrameForLeft = true;
                Debug.Log("skippedFrameForLeft");
            }
        }



        //DISABLE if using old input system
        public void OnCamera(InputAction.CallbackContext ctx)
        {
            Vector2 pointerDelta = ctx.ReadValue<Vector2>();
            CameraInput = new Vector2(CameraInput.x + pointerDelta.x, CameraInput.y + pointerDelta.y);
            GetDeviceNew(ctx);
        }

        public void OnDecreaseTimeRate(InputAction.CallbackContext ctx)
        {
            Vector2 pointerDelta = ctx.ReadValue<Vector2>();
            CameraInput = new Vector2(CameraInput.x + pointerDelta.x, CameraInput.y + pointerDelta.y);
            GetDeviceNew(ctx);
        }


        //DISABLE if using old input system
        public void OnSprint(InputAction.CallbackContext ctx)
        {
            if (enableSprint) Sprint = true;
        }


        //DISABLE if using old input system
        public void SprintEnded(InputAction.CallbackContext ctx)
        {
            Sprint = false;
        }


        //DISABLE if using old input system
        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (enableCrouch) Crouch = true;
        }


        //DISABLE if using old input system
        public void CrouchEnded(InputAction.CallbackContext ctx)
        {
            Crouch = false;
        }

        public void OnGuard(InputAction.CallbackContext ctx)
        {
            if (enableGuard) Guard = true;
        }


        public void GuardEnded(InputAction.CallbackContext ctx)
        {
            Guard = false;
        }


        public void OnRight(InputAction.CallbackContext ctx)
        {
            RightHold = true;
        }

        public void RightEnded(InputAction.CallbackContext ctx)
        {
            RightHold = false;
            RightStrike = true;
            hasRightStriked = true;
            skippedFrameForRight = false;
        }
        public void OnLeft(InputAction.CallbackContext ctx)
        {
            LeftHold = true;
        }

        public void LeftEnded(InputAction.CallbackContext ctx)
        {
            LeftHold = false;
            LeftStrike = true;
            hasLeftStriked = true;
            skippedFrameForLeft = false;
        }

        [HideInInspector]
        public bool AccelTime { get; set; }
        [HideInInspector]
        public bool AccelTimeHold { get; set; }
        [HideInInspector]
        public bool DecelTime { get; set; }
        [HideInInspector]
        public bool DecelTimeHold { get; set; }
        //[HideInInspector]
        public bool Interact { get; set; }
        [HideInInspector]
        public bool InteractHold { get; set; }
        public void OnAccelTime(InputAction.CallbackContext ctx)
        {
            if (ctx.action.WasPressedThisFrame())
            {
                AccelTime = true;
            }
        }

        public void AccelTimeEnded(InputAction.CallbackContext ctx)
        {
            AccelTime = false;
            AccelTimeHold = false;
        }

        public void OnDecelTime(InputAction.CallbackContext ctx)
        {
            if (ctx.action.WasPressedThisFrame())
            {
                DecelTime = true;
            }
        }

        public void DecelTimeEnded(InputAction.CallbackContext ctx)
        {
            DecelTime = false;
            DecelTimeHold = false;
        }

        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.action.WasPressedThisFrame())
            {
                Interact = true;
                //RaycastHit hit = aim.DirectionFromCamera(true);
                //Debug.Log(hit.collider.gameObject.tag);
            }
        }

        public void InteractEnded(InputAction.CallbackContext ctx)
        {
            Interact = false;
            InteractHold = false;
        }


        #endregion


        #region Enable / Disable

        //DISABLE if using old input system
        private void OnEnable()
        {
            if (playerInput)
            {
                urthActions.Enable();
            }
        }


        //DISABLE if using old input system
        private void OnDisable()
        {
            if (playerInput)
            {
                urthActions.Disable();
            }
        }

        #endregion
    }
}