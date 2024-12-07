using UnityEngine;


namespace Urth
{
    public enum STANCE
    {
        ACTIVE,
        COMBAT,
        IDLE,
        STEALTH,
        UNCONSCIOUS,
    }

    public class UrthAnimController : MonoBehaviour
    {
        [Header("References")]
        public CreatureManager creatureManager;
        public UrthCharacterManager characterManager;
        public Rigidbody rigidbodyCharacter;
        [SerializeField] LayerMask groundMask;
        [Space(10)]

        [Header("Animation specifics")]
        public float velocityAnimationMultiplier = 1f;
        public bool lockRotationOnWall = true;
        public float groundCheckerThreshold = 0.4f;
        public float climbThreshold = 0.5f;


        private Animator anim;
        private float originalColliderHeight;


        /**/


        private void Awake()
        {
            anim = this.GetComponent<Animator>();
        }


        private void Start()
        {
            //originalColliderHeight = characterManager.GetOriginalColliderHeight();
        }


        private void Update()
        {
            //anim.SetFloat("velocity", rigidbodyCharacter.velocity.magnitude * velocityAnimationMultiplier);

            //anim.SetBool("isGrounded", CheckAnimationGrounded());

            //anim.SetBool("isJump", characterManager.GetJumping());

            //anim.SetBool("isTouchWall", characterManager.GetTouchingWall());
            //if (lockRotationOnWall) characterManager.SetLockRotation(characterManager.GetTouchingWall());

            //anim.SetBool("isClimb", characterManager.GetTouchingWall() && rigidbodyCharacter.velocity.y > climbThreshold);

            //anim.SetBool("isCrouch", characterManager.GetCrouching());

            //anim.SetLayerWeight(1, characterManager.GetLeftLayerWeight());
            //anim.SetLayerWeight(2, characterManager.GetRightLayerWeight());

            //anim.SetFloat("leftIdleIdx", characterManager.GetLeftGuardIdx());
            //anim.SetFloat("rightId leIdx", characterManager.GetRightGuardIdx());
            
            //anim.SetBool("isPoseLeft", characterManager.GetGuarding());
            //anim.SetFloat("leftPoseIdx", characterManager.GetLeftGuardIdx());
            //anim.SetBool("isPoseRight", characterManager.GetGuarding());
            //anim.SetFloat("rightPoseIdx", characterManager.GetRightGuardIdx());

            //anim.SetFloat("leftActionIdx", characterManager.GetLeftGuardIdx());
            //anim.SetFloat("rightActionIdx", characterManager.GetRightGuardIdx());

            //if (characterManager.GetLeftAction())
            //{
            //    anim.ResetTrigger("trigActionLeft");
            //    anim.SetTrigger("trigActionLeft");
            //}
            //if (characterManager.GetRightAction())
            //{
            //    anim.ResetTrigger("trigActionRight");
            //    anim.SetTrigger("trigActionRight");
            //}
        }


        private bool CheckAnimationGrounded()
        {
            return Physics.CheckSphere(characterManager.transform.position - new Vector3(0, originalColliderHeight / 2f, 0), groundCheckerThreshold, groundMask);
        }

        void OnStrikeAboutToEnd(AnimationEvent animationEvent)
        {//Attack animations will have OnStrikeAboutToEnd events that occur before OnStrikeEnd
            //for 
            Debug.Log("Attack animation end " + animationEvent.stringParameter + " " + animationEvent.intParameter);
            characterManager.EndStrike(animationEvent.intParameter);
        }

        void OnStrikeEnd(AnimationEvent animationEvent)
        {//Attack animations will have OnAttackEnd events that occur when the offsensive portion of the animation ends
            //for 
            Debug.Log("Attack animation end " + animationEvent.stringParameter + " " + animationEvent.intParameter);
            characterManager.EndStrike(animationEvent.intParameter);
        }
        void OnRecoilEnd(AnimationEvent animationEvent)
        {

        }

        public void StartLeftWorkSwing(int swingTypeIdx)
        {
            Debug.Log("StartLeftToolSwing");
            anim.SetBool("leftOverride", true);
            anim.SetInteger("leftIdx", UrthConstants.WORK_IDX);
            anim.SetInteger("leftSubIdx", swingTypeIdx);
        }
        public void StartRightWorkSwing(int swingTypeIdx)
        {
            Debug.Log("StartRightToolSwing");
            anim.SetBool("rightOverride", true);
            anim.SetInteger("rightIdx", UrthConstants.WORK_IDX);
            anim.SetInteger("rightSubIdx", swingTypeIdx);
        }
        void OnWorkSwingApex(AnimationEvent animationEvent)
        {
            if (creatureManager.aimedWorking)
            {
                switch (animationEvent.intParameter)
                {
                    case -1:
                        creatureManager.TryDoAimedWorkLeft();
                        break;
                    case 0:
                        break;
                    case 1:
                        creatureManager.TryDoAimedWorkRight();
                        break;
                }
                Debug.Log("Aimed work tool swing apex, raycast to see if we hit anything.");
            }
        }

        void OnWorkSwingEnd(AnimationEvent animationEvent)
        {
            Debug.Log("End tool swing, check if still working.");
            if (creatureManager.working)
            {
                switch (animationEvent.intParameter)
                {
                    case -1:
                        creatureManager.WorkSwingLeft();
                        break;
                    case 0:
                        break;
                    case 1:
                        creatureManager.WorkSwingRight();
                        break;
                }
            }
            else
            {
                switch (animationEvent.intParameter)
                {
                    case -1:
                        anim.SetBool("leftOverride", false);
                        break;
                    case 0:
                        break;
                    case 1:
                        anim.SetBool("rightOverride", false);
                        break;
                }
            }
        }
        public void SetLeftCombatIdle(int swingTypeIdx)
        {
            Debug.Log("SetLeftCombatIdle");
            anim.SetBool("leftOverride", true);
            anim.SetInteger("leftIdx", UrthConstants.IDLES_IDX);
            anim.SetInteger("leftSubIdx", swingTypeIdx);
        }
        public void SetRightCombatIdle(int swingTypeIdx)
        {
            Debug.Log("SetRightCombatIdle");
            anim.SetBool("rightOverride", true);
            anim.SetInteger("rightIdx", UrthConstants.IDLES_IDX);
            anim.SetInteger("rightSubIdx", swingTypeIdx);
        }
        public void DisableLeft()
        {
            Debug.Log("DisableLeft");
            anim.SetBool("leftOverride", false);
        }
        public void DisableRight()
        {
            Debug.Log("DisableRight");
            anim.SetBool("rightOverride", false);
        }
        public void SetStance(STANCE stance)
        {//#TODO
            Debug.LogWarning("TODO SetStance");
        }
    }
}