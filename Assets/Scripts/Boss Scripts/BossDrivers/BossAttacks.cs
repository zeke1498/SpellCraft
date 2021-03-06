using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    /// <summary>
    /// Lich , Pylon, Alchemist, Charmer, or Reflector
    /// </summary>
    private string bossName = "";
    private BossInfo bossInfo;
    private BossHealth bossHealthInfo;
    [Tooltip("Check this is you're testing attacks!")]
    public bool testingAttacks = false;
    [Range(1,3)]
    public int currentlyTestingAttack = 1;
    /// <summary>
    /// Shows the previous attack. 0 means invalid attack, 1 is the first attack, 2 is the second attack, 3 is the 3rd
    /// </summary>
    public int previousAttack = 0;
    public int attackOneWeight = 5;
    public int attackTwoWeight = 5;
    public int attackThreeWeight = 5;

    [Tooltip("Check this is you're testing attacks!")]
    // public bool testingAttacks = false;
    [Range(1, 3)]
    // public int currentlyTestingAttack = 1;

    /// <summary>
    /// Pretty obvious, shows the if the boss is attacking
    /// </summary>
    public bool isAttacking = false;

    public bool canAttack = true;

    private float attackTimer = 0f;
    /// <summary>
    /// The time between attacks. Essentially the attack cooldown.
    /// </summary>
    public float attackRate = 3f;

    ///////////////////////////////////Lich Info
    private LichAttacks lichAttackInfo;


    /////////////////////////////////////Pylon Info
    private PylonAttacks pylonAttackInfo;


    /////////////////////////////////////////Charmer Info
    private CharmerAttacks charmerAttackInfo;


    /////////////////////////////////////////////Reflector Info
    private ReflectorAttacks reflectorAttackInfo;


    /////////////////////////////////////////////////Alchemist Info
    private AlchemistAttack alchemistAttackInfo;

    /////////////////////////////////////////////////PrototypeBoss Info
    private PrototypeBossAttack prototypeBossAttackInfo;
    /// 
    private ProtoNovusAttacks protoNovusAttacksInfo;



    public enum AttackState
    {
        IDLE,
        ATTACK,
        STUN,
    }

    public AttackState attackState;




    // Use this for initialization
    void Start()
    {
        bossName = gameObject.name;
        bossInfo = gameObject.GetComponent<BossInfo>();
        bossHealthInfo = gameObject.GetComponent<BossHealth>();
        BossInitializer(bossName);
        attackState = AttackState.IDLE;
        StartCoroutine("FSM");
    }



    IEnumerator FSM()
    {
        while (bossHealthInfo.GetAlive())
        {
            switch (attackState)
            {
                case AttackState.IDLE:
                    Idle();
                    break;

                case AttackState.ATTACK:
                    if(canAttack)
                    {
                        AttackDecider();
                        AttackDriver(bossName, previousAttack);
                        canAttack = false;
                    }
                  
                    break;
            }
            yield return null;
        }
        
    }

    /// /////////////////////////////////////////////////// BossName stuff!
    public void BossInitializer(string bossName)
    {
        switch (bossName)
        {
            case "Lich":
                lichAttackInfo = gameObject.GetComponent<LichAttacks>();
                break;

            case "Pylon":
                pylonAttackInfo = gameObject.GetComponent<PylonAttacks>();
                break;

            case "Charmer":
                charmerAttackInfo = gameObject.GetComponent<CharmerAttacks>();
                break;

            case "Reflector":
                reflectorAttackInfo = gameObject.GetComponent<ReflectorAttacks>();
                break;

            case "Alchemist":
                alchemistAttackInfo = gameObject.GetComponent<AlchemistAttack>();
                break;

            case "PrototypeBoss":
                prototypeBossAttackInfo = gameObject.GetComponent<PrototypeBossAttack>();
                break;

            case "ProtoNovus":
                protoNovusAttacksInfo = gameObject.GetComponent<ProtoNovusAttacks>();
                break;
        }

    }


    /// ///////////////////////////////////////////////////IDLE!

    public void Idle()
    {
        if (bossHealthInfo.GetAlive())
        {
            if(bossInfo.isActivated)
            {
                canAttack = true;
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackRate)
                {
                    attackTimer = 0;

                    if (!isAttacking && canAttack)
                    {
                        attackState = AttackState.ATTACK;
                    }
                }
            }
            
        }
    }//Idle end

    /////////////////////////////////////////////////////// ATTACK DECIDER!

    public void AttackDecider()
    {
        int randAttack = Random.Range(1, 4);
        if (randAttack == previousAttack)
        {
            AttackDecider();
        }
        else
        {
            if(previousAttack == 1)
            {
                if(randAttack == 2 || randAttack == 3)
                {
                    if(attackTwoWeight <= attackThreeWeight)
                    {
                        randAttack = 3;
                        --attackThreeWeight;
                    }
                    else if (attackTwoWeight > attackThreeWeight)
                    {
                        randAttack = 2;
                        --attackTwoWeight;
                    }
                }
            }
            else if(previousAttack == 2)
            {
                if (randAttack == 1 || randAttack == 3)
                {
                    if (attackOneWeight <= attackThreeWeight)
                    {
                        randAttack = 3;
                        --attackThreeWeight;
                    }
                    else if (attackOneWeight > attackThreeWeight)
                    {
                        randAttack = 1;
                        --attackOneWeight;
                    }
                }
            }
            else if(previousAttack == 3)
            {
                if (randAttack == 1 || randAttack == 2)
                {
                    if (attackOneWeight <= attackTwoWeight)
                    {
                        randAttack = 2;
                        --attackTwoWeight;
                    }
                    else if (attackOneWeight > attackTwoWeight)
                    {
                        randAttack = 1;
                        --attackOneWeight;
                    }
                }
            }
            previousAttack = randAttack;
            ////if(attackOneWeight <= 0)
            ////{
            ////    attackOneWeight = 5;
            ////}
            ////if(attackTwoWeight <= 0)
            ////{
            ////    attackTwoWeight = 5;
            ////}
            ////if(attackThreeWeight <= 0)
            ////{
            ////    attackThreeWeight = 5;
            ////}
        }
    }

    /////////////////////////////////////////////////////// ATTACK DRIVER!

    public void AttackDriver(string bossName, int attackNumber)
    {
        //Debug.Log(bossName + " is the name that was passed");
        // Debug.Log(isAttacking + " is the value of isAttacking");
        if (testingAttacks)
        {
           attackNumber = currentlyTestingAttack;
        }
        switch (bossName)
        {
            case "Lich":
                if(!isAttacking)
                    lichAttackInfo.Attack(attackNumber);
                break;

            case "Pylon":
                if (!isAttacking)
                    pylonAttackInfo.Attack(attackNumber);
                break;

            case "Charmer":
                if (!isAttacking)
                    charmerAttackInfo.Attack(attackNumber);
                break;

            case "Reflector":
                if (!isAttacking)
                    reflectorAttackInfo.Attack(attackNumber);
                break;

            case "Alchemist":
                if (!isAttacking)
                    alchemistAttackInfo.Attack(attackNumber);
                break;

            case "PrototypeBoss":
                if (!isAttacking)
                {
                    //Debug.Log("The BossAttack script");
                    prototypeBossAttackInfo.Attack(attackNumber);
                }  
                break;
            case "ProtoNovus":
                if (!isAttacking)
                    protoNovusAttacksInfo.Attack(attackNumber);
                break;
        }
    }//AttackDriver end

    public void CancelAttack()
    {
        attackState = AttackState.IDLE;
        canAttack = false;
        isAttacking = false;

        switch (bossName)
        {
            case "Lich":
                lichAttackInfo.StopAttack();
                break;

            case "Pylon":
                pylonAttackInfo.StopAttack();
                break;

            case "Charmer":
                charmerAttackInfo.StopAttack();
                break;

            case "Reflector":
                reflectorAttackInfo.StopAttack();
                break;

            case "Alchemist":
                alchemistAttackInfo.StopAttack();
                break;

            case "ProtoNovus":
                protoNovusAttacksInfo.StopAttack();
                break;
        }
    }

    public void ResumeAttack()
    {
        canAttack = true;
    }


   public void EndAttack()
    {
        attackState = AttackState.IDLE;
        canAttack = true;
        isAttacking = false;
    }


}
