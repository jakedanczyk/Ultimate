using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
//using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public abstract class BodyManager : MonoBehaviour
{

    //public ThirdPersonCharacter thirdPersonCharacter;
    //public List<Item_Garment> outfit;
    //public Inventory inventory; // all carried items
    //public LootInventory lootInventory;
    //public Transform aimPoint, arrowReleasePoint;

    //public Animator anim;
    //public Item_Weapon primaryWeapon;
    //public CreatureStats stats;
    //public AudioManager audioManager;
    //public AudioSource audioSource;
    //public List<Collision> primaryAttackCollisionList = new List<Collision>();
    //public List<Collision> offHandAttackCollisionList = new List<Collision>();
    //public AudioClip deathGasp;

    ////status bools
    //public bool isAttackingPrimary, isAttackingSecondary, isOnAlert, isGuardRaised, sneaking, isConscious;
    //public bool alive = true;
    //public bool butchered = false;
    //int mainWeaponAnimationLayer = 5;
    //public IEnumerator onCompleteMainWeaponAttackAnimation;
    //public GameObject ragdollPrefab, bodyModel;

    ////Attack pseudo-code
    ////calc damage, speed from:(weapon, attributes, status, skill)
    ////Damage:
    ////Speed:
    ////run animation, use rays and colliders to build collisionList
    ////if collision
    ////      object type... Terrain, Construction, Item, SmallPlant, Tree, BodyPart ?
    ////Terrain... Hard, Compact, Loose

    //public void Awake()
    //{
    //    onCompleteMainWeaponAttackAnimation = OnCompleteMainWeaponAttackAnimation();
    //}

    //public void AttackResolution(BodyPartColliderScript bp, CreatureStats targetStats, RPGSkill attackSkill, int[] d, int[] o, bool isStealthAttack = false, bool isFromBehind = false)
    //{
    //    float dodgeChance = (Mathf.Pow(targetStats.GetStat(RPGStatType.Dodge).StatValue, 1.1f) * targetStats.GetStat(RPGStatType.Agility).StatValue) / (1 + 9 * Mathf.Pow(bp.parentBody.encumbrance, 2));
    //    float deflectChance = Mathf.Pow(targetStats.GetStat(RPGStatType.Deflect).StatValue, 1.1f) * (targetStats.GetStat(RPGStatType.Agility).StatValue + .2f * targetStats.GetStat(RPGStatType.Strength).StatValue) * ((d[1] + 2 * d[2]) * targetStats.GetStat(RPGStatType.Armor).StatValue / (1 + o[1] + 2 * o[2])); //deflecting an attack is very dependent on  hardness and rigidity of armor relative to weapon
    //    float absorbChance = Mathf.Pow(targetStats.GetStat(RPGStatType.Absorb).StatValue, 1.1f) * (.2f * targetStats.GetStat(RPGStatType.Agility).StatValue + targetStats.GetStat(RPGStatType.Strength).StatValue) * (d[0] / (1 + o[0])) * ((d[1] + d[2]) / (1 + o[1] + o[2])) * targetStats.GetStat(RPGStatType.Armor).StatValue + (d[1] + d[2]) / (1 + o[1] + o[2]) * Mathf.Log(targetStats.GetStat(RPGStatType.Weight).StatValue); //to absorb an attack armor must both resist sharp penetration and pad impact of blow
    //    float hitChance = 10f * Mathf.Pow(attackSkill.StatValue, 1.1f) * (stats.GetStat(RPGStatType.Agility).StatValue + .2f * stats.GetStat(RPGStatType.Strength).StatValue);
    //    float criticalChance = 0.01f * attackSkill.StatValue * attackSkill.StatValue * (stats.GetStat(RPGStatType.Agility).StatValue + .2f * stats.GetStat(RPGStatType.Strength).StatValue);

    //    if (!bp.parentBody.isConscious)
    //    { //unable to dodge if unconcious, no skill or agility addition to deflect and absorb, 90% reduction in strength contribution, 8x increase in critical
    //        dodgeChance = 0;
    //        deflectChance = .02f * targetStats.GetStat(RPGStatType.Strength).StatValue * ((d[1] + 2 * d[2]) * targetStats.GetStat(RPGStatType.Armor).StatValue / (1 + o[1] + 2 * o[2]));
    //        absorbChance = .1f * targetStats.GetStat(RPGStatType.Strength).StatValue * (d[0] / o[0]) * ((d[1] + d[2]) / (1 + o[1] + o[2])) * targetStats.GetStat(RPGStatType.Armor).StatValue + (d[1] + d[2]) / (1 + o[1] + o[2]) * Mathf.Log(targetStats.GetStat(RPGStatType.Weight).StatValue);
    //        criticalChance *= 8;
    //    }
    //    else if (!bp.parentBody.isOnAlert)
    //    { // 99% reduction in defense odds if not aware of danger, four times critical
    //        dodgeChance *= .01f;
    //        deflectChance *= .01f;
    //        absorbChance *= .01f;
    //        criticalChance *= 4;
    //    }
    //    else if (isStealthAttack)
    //    { // 90% reduction in defense odds if on alert but don't know attackers location, double critical
    //        dodgeChance *= .1f;
    //        deflectChance *= .1f;
    //        absorbChance *= .1f;
    //        criticalChance *= 2;
    //    }
    //    else if (isFromBehind)
    //    { // 75% reduction in defense odds if attacking form behind, 50% increase critical
    //        dodgeChance *= .25f;
    //        deflectChance *= .25f;
    //        absorbChance *= .25f;
    //        criticalChance *= 1.5f;
    //    }

    //    float sum = dodgeChance + deflectChance + absorbChance + hitChance + criticalChance;
    //    //dodgeChance = dodgeChance / sum;
    //    //deflectChance = deflectChance / sum;
    //    //absorbChance = absorbChance / sum;
    //    //hitChance = hitChance / sum;
    //    //criticalChance = criticalChance / sum;
    //    float roll = UnityEngine.Random.Range(0, sum);
    //    print(roll + " / " + sum);
    //    print(dodgeChance + " , " + deflectChance + " , " + absorbChance + " , " + hitChance);

    //    if (roll < dodgeChance) { bp.parentBody.Dodge(); print("dodge"); return; }
    //    else if (roll < (dodgeChance + deflectChance)) { bp.parentBody.Deflect(); print("deflect"); return; }
    //    else if (roll < (dodgeChance + deflectChance + absorbChance)) { bp.parentBody.Absorb(); print("absorb"); return; }
    //    else if (roll < (dodgeChance + deflectChance + absorbChance + hitChance))
    //    {
    //        audioSource.PlayOneShot(audioManager.hitBody);
    //        //bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).StatCurrentValue -= ((o[0] / d[0]) + ( o[1] / d[1]) + (o[2] / d[2]));
    //        bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue -= (int)(bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).damageModifer * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2])));
    //        attackSkill.GainXP(10);
    //        return;
    //    }
    //    else
    //    {
    //        print(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue);
    //        bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue -= 4 * (int)(bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).damageModifer * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2])));
    //    }
    //    print(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue);
    //    primaryAttackCollisionList.Clear();
    //    //defense: defense skills, armor, encumbrance, stamina, energy, agility, strength, toughness
    //}
    //public void Start()
    //{
    //    InvokeRepeating("Encumbrance", 2f, 2f);
    //    audioManager = AudioManager.audioManagerGameObject.GetComponent<AudioManager>();
    //    var health = stats.GetStat<RPGVital>(RPGStatType.Health);
    //    health.OnCurrentValueChange += OnStatValueChange;
    //}

    //public IEnumerator OnCompleteMainWeaponAttackAnimation()
    //{
    //    while (anim.GetCurrentAnimatorStateInfo(mainWeaponAnimationLayer).loop)
    //        yield return null;
    //    while (!anim.GetCurrentAnimatorStateInfo(mainWeaponAnimationLayer).loop)
    //        yield return null;
    //    isAttackingPrimary = false;
    //    primaryWeapon.weaponCollList[0].isActive = false;
    //    yield return null;
    //}

    //void OnStatValueChange(object sender, EventArgs args)
    //{
    //    print("stat change");
    //    RPGVital vital = (RPGVital)sender;
    //    if (vital != null)
    //    {
    //        print(string.Format("Vital {0}'s OnStatValueChange event was triggered", vital.StatName));
    //        print(vital.StatCurrentValue);
    //    }
    //    if (vital.StatCurrentValue <= 0)
    //    {
    //        alive = false;
    //        anim.SetBool("isAlive", false);
    //    }
    //}

    //public void PickupItem(Item anItem)
    //{
    //    inventory.AddItem(anItem);
    //    //anItem.loose = false;
    //    //        anItem.transform.SetParent(transform.parent);
    //    //anItem.gameObject.SetActive(false);
    //    //Destroy(hit.collider.gameObject);
    //}

    //public void DropItem(Item anItem)
    //{
    //    if (anItem == null)
    //        return;
    //    inventory.selectedItem = null;
    //    inventory.RemoveItem(anItem);
    //    anItem.loose = true;
    //    anItem.gameObject.SetActive(true);
    //    anItem.transform.position = this.transform.position;
    //    anItem.transform.parent = null;
    //}

    //public int[] SendArmorNumbers(RPGStatType bodyPart)
    //{
    //    int[] z = new int[] { 1, 1, 1 };
    //    z[0] = stats.GetStat<RPGBodyPart>(bodyPart).protection[0];
    //    z[1] = stats.GetStat<RPGBodyPart>(bodyPart).protection[1];
    //    z[2] = stats.GetStat<RPGBodyPart>(bodyPart).protection[2];
    //    return z;
    //}

    //public float encumbrance = 0; //decimal percent 
    //public float carryWeight;
    //void Encumbrance()
    //{
    //    carryWeight = stats.GetStat<RPGAttribute>(RPGStatType.CarryWeight).StatValue;
    //    encumbrance = (inventory.SumWeight() - (outfit.Select(c => c.itemWeight).ToList().Sum() * .5f)) / carryWeight;
    //}

    //public void Dodge() { }

    //public void Deflect() { }

    //public void Absorb() { }

    //public List<GameObject> butcheringReturns; // Outside in. Feathers, skin/exoskeloton, fat, muscle, organs, bones
    //public int butcherTime; // seconds
    //public int butcherSkillFactor; // how variable is quality?
    //public abstract void ProcessThisBody();
    //public abstract void PrimaryAttack();

    //public void Run()
    //{
    //    anim.SetBool("isRunning", true);
    //}

    //public void Walk()
    //{
    //    anim.SetBool("isWalking", true);
    //    anim.SetBool("isRunning", false);
    //}

    //public void Idle()
    //{
    //    anim.SetBool("isWalking", false);
    //    anim.SetBool("isRunning", false);
    //}

    //public virtual void Death()
    //{
    //    GetComponent<LocalNavMeshBuilder>().enabled = false;
    //    GetComponent<AICharacterControl>().enabled = false;
    //    GetComponent<NavMeshAgent>().enabled = false;
    //    GetComponent<ThirdPersonCharacter>().enabled = false;
    //    this.tag = "DeadCreature";
    //    alive = false;
    //    anim.enabled = false;
    //    audioSource.PlayOneShot(deathGasp);
    //    bodyModel.SetActive(false);
    //    GetComponentInParent<Rigidbody>().useGravity = false;
    //    Instantiate(ragdollPrefab, transform);
    //    StopAllCoroutines();
    //}


    //public AudioClip aggressiveBellow;
    //public virtual void AggressiveBellow()
    //{
    //    audioSource.PlayOneShot(aggressiveBellow);
    //}
}
