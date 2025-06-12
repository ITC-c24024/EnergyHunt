using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
[RequireComponent(typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    public TakeBomb takeBomb;

    [SerializeField, Header("EnergyTankManager�X�N���v�g")]
    EnergyTank energyTank;

    [SerializeField, Header("SavingTank�X�N���v�g")]
    SavingTank savingTank;

    [SerializeField,Header("Bomb�X�N���v�g")] 
    BombScript[] bombScript;

    [SerializeField,Header("���e�I�u�W�F�N�g")] GameObject[] bomb;
    [SerializeField, Header("���d�G�t�F�N�g")] GameObject[] effect; 
    [SerializeField,Header("�{��������͈�")] GameObject passRange;

    [SerializeField, Header("�{�����L���b�`�ł���͈�")] GameObject catchRange;

    [SerializeField, Header("�w�����Ă���^���N�I�u�W�F�N�g")]
    GameObject playerTank;

    [Header("�v���C���[�̃��f��")]
    [SerializeField] GameObject body;
    //[SerializeField] GameObject head;
    [SerializeField] GameObject hand;

    [SerializeField,Header("�v���C���[�̃R���C�_�[")] 
    CapsuleCollider capsule;

    [SerializeField,Header("�v���C���[��Rigidbody")] 
    Rigidbody playerRb;

    [SerializeField, Header("���e��Rigidbody")] 
    Rigidbody[] bombRb;

    [SerializeField, Header("�o���A�I�u�W�F�N�g")]
    GameObject barrierObj;

    //�v���C���[�̃X�^�[�g�ʒu
    private Vector3 startPos;
    //�v���C���[�̃X�^�[�g�̌���
    private Quaternion startAngle;

    //�v���C���[�̔ԍ�
    public int controllerNum;

    [SerializeField,Header("�v���C���[�̈ړ��X�s�[�h")] 
    float speed;

    //�v���C���[�̈ړ��A������A�N�V����
    public InputAction moveAction, throwAction;

    //��������A���Ƀ{�^����������܂ł̎���
    private float timer = 0;
    //���̓N�[���^�C��
    public float inputTimer;
    //���̓N�[���^�C���̔���
    public bool isInputTimer = false;

    //�v���C���[�̌���
    private float angle;
    //�X�e�B�b�N�̊p�x��ۑ����邽�߂�float
    private float lastAngle;

    //���e�𓊂����
    private float power = 1;

    //���e�������Ă��锻��
    public bool[] haveBomb;

    //���S����
    public bool isDead = true;

    //��������
    private bool isFall = false;

    //�_�Ŕ���
    private bool isBlink = false;

    private Animator animator;

    private bool isBarrier = false;

    [SerializeField, Header("����SE")]
    AudioSource audioSource;

    void Start()
    {
        //ActionMap���擾
        var input = GetComponent<PlayerInput>();
        //var input = PlayerInput.GetPlayerByIndex(controllerNum - 1);
        var actionMap = input.currentActionMap;

        moveAction = actionMap["Move"];
        throwAction = actionMap["PickUp"];


        haveBomb[0] = false;
        haveBomb[1] = false;


        bombRb[0] = bomb[0].GetComponent<Rigidbody>();
        bombRb[1] = bomb[1].GetComponent<Rigidbody>();


        //�ŏ��̃|�W�V������ݒ�
        startPos = transform.position;
        startAngle = transform.localRotation;

        animator = GetComponent<Animator>();

        //�o���A�X�N���v�g�̃o���A������擾
        isBarrier = barrierObj.GetComponent<BarrierScript>().isBarrier;

        //�X�^�[�g������ɓ�����悤�ɂ���
        Invoke("GameStart", 3.5f);
    }

    private void GameStart()
    {
        isDead = false;
    }

    void Update()
    {
        if (!isDead && !isFall)
        {
            //�v���C���[�̈ړ�
            Vector2 move = moveAction.ReadValue<Vector2>();
            
            if (move.x < 0.1 && move.x > -0.1)
            {
                move.x = 0;
            }
            if (move.y < 0.1 && move.y > -0.1)
            {
                move.y = 0;
            }
            
            transform.position += new Vector3(move.x, 0, move.y) * speed * Time.deltaTime;         


            //�v���C���[�̉�]
            if (move.x > 0.1 || move.x < -0.1 || move.y > 0.1 || move.y < -0.1)
            {
                if (Time.timeScale == 1)
                {
                    angle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
                }
                
                if (angle < 0)
                {
                    angle += 360;
                }
                //�X�e�B�b�N�̊p�x��ۑ�
                lastAngle = angle;

                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            else
            {
                //�X�e�B�b�N���͂�0�̎��ۑ����Ă����p�x����
                transform.rotation = Quaternion.Euler(0, lastAngle, 0);
            }
        }

        //���e��D����͈͂̒Ǐ]
        passRange.transform.position = transform.position;
        passRange.transform.rotation = transform.rotation;

        //�L���b�`�ł���͈͂̒Ǐ]
        catchRange.transform.position = transform.position;
        catchRange.transform.rotation = transform.rotation;

        //�o���A�I�u�W�F�N�g�̒Ǐ]
        barrierObj.transform.position = transform.position;


        //�����铮��̔���
        var throwAct = throwAction.triggered;

        //���e�𓊂���
        if (throwAct && haveBomb[0] && !isInputTimer && Time.timeScale == 1)
        {
            //�����ɃL���b�`�ł��Ȃ��悤�ɂ���
            catchRange.GetComponent<CatchScript>().TimerStart();
            takeBomb.IsTimerTrue();

            //�R�A�𓊂���ꂽ��Ԃɂ���
            bombScript[0].SetIsThrow(true);

            ThrowBomb(0);
            
        }
        if (throwAct && haveBomb[1] && !isInputTimer && Time.timeScale == 1)
        {
            //�����ɃL���b�`�ł��Ȃ��悤�ɂ���
            catchRange.GetComponent<CatchScript>().TimerStart();
            takeBomb.IsTimerTrue();

            //�R�A�𓊂���ꂽ��Ԃɂ���
            bombScript[1].SetIsThrow(true);

            ThrowBomb(1);
        }


        //�����Ă����ɃL���b�`�ł��Ȃ��悤�ɂ��邽�߂̓��̓N�[���^�C��
        if (isInputTimer)
        {
            timer += Time.deltaTime;

            if (timer > 0.4)
            {
                isInputTimer = false;

                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }
    }

    public void IsTimerTrue()
    {
        isInputTimer = true;
    }

    public void HaveBombChange(int i, bool a)
    {
        haveBomb[i] = a;
    }

    //�r����]������
    public void SetArm(int armAngle)
    {
        //Debug.Log("�r");
        hand.transform.localRotation = Quaternion.Euler(armAngle, 0, 0);
    }

    public void IsDeadTrue()
    {
        //�N�������������Ƃ��A�S�������Ȃ�����
        isDead = true;
    }

    //���e�𓊂���֐�
    private void ThrowBomb(int bombNum)
    {
        //animator.SetBool("isThrow", true);
        //Invoke("ThroeAnimFalse", 0.1f);
        

        //���̓N�[���^�C���X�^�[�g
        isInputTimer = true;

        //AddForce�œ�����̂ŁAisKinematic��false����
        bombRb[bombNum].isKinematic = false;

        //�R���C�_�[����
        bombScript[bombNum].ColliderEnabled(true);

        //���e�������Ă��锻���false�ɂ���
        energyTank.MissingCore();
        haveBomb[bombNum] = false;

        //�r��������
        SetArm(0);

        //���e���΂�
        bombRb[bombNum].AddForce(bomb[bombNum].transform.forward * 500 * power);
    }

    private void ThroeAnimFalse()
    {
        animator.SetBool("isThrow", false);
    }

    public IEnumerator WinnerAnim(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        animator.SetBool("isWin", true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�X�e�[�W�O�ɗ��������AhaveBomb��false�ɂ���
        if (collision.gameObject.tag == "Reset" && !isDead)
        {
            //�}�O�}�ɒ��܂��邽�߂ɃR���C�_�[�I�t
            capsule.enabled = false;

            if (haveBomb[0])
            {
                Debug.Log("reset");
                //�{���������Ă��锻���false�ɂ���
                haveBomb[0] = false;

                //�r��������
                SetArm(0);

                //bombRb[0].isKinematic = false;

                StartCoroutine(bombScript[0].PosReset(0));
            }
            else if (haveBomb[1])
            {
                Debug.Log("reset");
                //�{���������Ă��锻���false�ɂ���
                haveBomb[1] = false;

                //�r��������
                SetArm(0);

                //bombRb[1].isKinematic = false;

                StartCoroutine(bombScript[1].PosReset(0));
            }

            //���񂾂Ƃ��A�G�l���M�[�ʂ�0�łȂ������烊�Z�b�g
            if (energyTank.energyAmount != 0)
            {
                energyTank.DropEnergy(5);
            }

            PlayerDead(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�X�e�[�W�̌��ɗ������瓮���Ȃ��Ȃ�
        if (other.gameObject.tag == "FallArea")
        {
            audioSource.Play();

            isFall = true;
        }

        //�����ɓ��������玀�S
        if (other.gameObject.tag == "Explosion0" && !isDead && !barrierObj.GetComponent<BarrierScript>().isBarrier && !isBlink)
        {
            if (haveBomb[1])
            {
                //�{���������Ă��锻���false�ɂ���
                haveBomb[1] = false;
                energyTank.MissingCore();

                //�r��������
                SetArm(0);

                bombScript[1].Explosion();
            }

            PlayerDead(1);
        }
        if (other.gameObject.tag == "Explosion1" && !isDead && !barrierObj.GetComponent<BarrierScript>().isBarrier && !isBlink) 
        {
            if (haveBomb[0])
            {
                //�{���������Ă��锻���false�ɂ���
                haveBomb[0] = false;
                energyTank.MissingCore();

                //�r��������
                SetArm(0);

                bombScript[0].Explosion();
            }

            PlayerDead(1);
        }
    }


    private void PlayerDead(int lostEnergy)
    {
        //�o���A������
        barrierObj.GetComponent<BarrierScript>().BarrierCoolTime();

        if (lostEnergy == 1)
        {
            animator.SetBool("isBreke", true);
        }

        //�v���C���[���S
        isDead = true;

        haveBomb[0] = false;
        haveBomb[1] = false;

        //�r��������
        SetArm(0);

        //�^���N���\��
        playerTank.SetActive(false);

        //�v���C���[�̃��f�����\��
        //body.SetActive(false);
        //head.SetActive(false);
        //hand.SetActive(false);

        //�����Ȃ��悤��isKinematic��false
        //playerRb.isKinematic = true;

        //��̃I�u�W�F�N�g��������Ȃ��悤�ɃR���C�_�[���I�t
        //capsule.enabled = false;

        //���e�����Ȃ�����
        takeBomb.CanTakeChange(0, false);
        takeBomb.CanTakeChange(1, false);

        //StartCoroutine(craneScript.RotateToTarget());
        StartCoroutine(posReset(lostEnergy));
    }

    private IEnumerator posReset(int lostEnergy)
    {
        yield return new WaitForSeconds(1.2f);
        
        //�������Z�b�g
        if (energyTank.energyAmount != 0)
        {
            energyTank.EnergyReset();
        }
        

        //��������܂ł̎���
        yield return new WaitForSeconds(0.8f);

        animator.SetBool("isBreke", false);

        //�v���C���[��_�ł�����
        gameObject.GetComponent<CharacterBlinkingScript>().BlinkStart(true);
        isBlink = true;

        

        //�X�^�[�g�ʒu�Ƀ��X�|�[��
        transform.position = startPos;

        //�X�^�[�g�̌����Ɍ���
        transform.localRotation = startAngle;

        //�v���C���[�̃��f����\��
        body.SetActive(true);
        hand.SetActive(true);

        //�^���N��\��
        playerTank.SetActive(true);

        //�R���C�_�[����
        capsule.enabled = true;

        playerRb.isKinematic = false;

        //�o���A����
        barrierObj.GetComponent<BarrierScript>().CoolTimeReset();

        //�G�l���M�[���Z�b�g�̂��߂ɗP�\����������
        yield return new WaitForSeconds(1.0f);

        //�v���C���[����
        isDead = false;
        isFall = false;

        //�_�ŏI��
        gameObject.GetComponent<CharacterBlinkingScript>().BlinkStart(false);
        isBlink = false;

        //�v���C���[�̃��f����\��
        body.SetActive(true);
        hand.SetActive(true);

        //�^���N��\��
        playerTank.SetActive(true);
    }

    //TankManager���疞�^����Ԃ̃^���N�̌����擾
    public void FullTank(int amount)
    {
        //���^����Ԃ̃^���N���ɉ����ăv���C���[������
        switch (amount)
        {
            case 0:
                speed = 4;
                power = 1;
                ExploPower(1);
                break;

            case 1:
                speed = 5.0f;
                ExploPower(1);
                break;

            case 2:
                power = 1.2f;
                ExploPower(1);
                break;

            case 3:
                ExploPower(2.0f);
                break;
        }
    }

    //���e�̔����͈͂�ύX
    public void ExploPower(float amount)
    {
        if (haveBomb[0])
        {
            bombScript[0].ExplosionRange(amount);
        }
        else if (haveBomb[1])
        {
            bombScript[1].ExplosionRange(amount);
        }
    }

    public void CanTakeTrue(int i)
    {
        takeBomb.CanTakeChange(i, true);
    }
}
