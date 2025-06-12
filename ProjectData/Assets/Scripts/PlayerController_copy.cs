using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]

public class PlayerController_copy : MonoBehaviour
{
    [SerializeField,Header("TakeBomb�X�N���v�g")] 
    TakeBomb_copy takeBomb;

    [SerializeField, Header("EnergyTank�X�N���v�g")] 
    EnergyTank_copy energyTank;

    [SerializeField, Header("SavingTank�X�N���v�g")]
    SavingTank savingTank;

    [SerializeField,Header("Bomb�X�N���v�g")] 
    BombScript[] bombScript;

    [SerializeField,Header("Crane�X�N���v�g")] 
    CraneScript craneScript;

    [SerializeField,Header("���e�I�u�W�F�N�g")] GameObject[] bomb;
    [SerializeField,Header("�{��������͈�")] GameObject passRange;

    [Header("�v���C���[�̃��f��")]
    [SerializeField] GameObject body;
    [SerializeField] GameObject head;
    [SerializeField] GameObject hand;

    [SerializeField,Header("�v���C���[�̃R���C�_�[")] 
    CapsuleCollider capsule;

    [SerializeField,Header("�v���C���[��Rigidbody")] 
    Rigidbody playerRb;

    [SerializeField, Header("���e��Rigidbody")] 
    Rigidbody[] bombRb;

    //�v���C���[�̃X�^�[�g�ʒu
    private Vector3 startPos;

    //�v���C���[�̔ԍ�
    public int controllerNum;

    [SerializeField,Header("�v���C���[�̈ړ��X�s�[�h")] 
    float speed;

    //�v���C���[�̈ړ��A������A�N�V����
    private InputAction moveAction, throwAction;

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
    public bool isDead = false;

    void Start()
    {
        //ActionMap���擾
        var input = GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;

        moveAction = actionMap["Move"];
        throwAction = actionMap["Throw"];


        haveBomb[0] = false;
        haveBomb[1] = false;


        bombRb[0] = bomb[0].GetComponent<Rigidbody>();
        bombRb[1] = bomb[1].GetComponent<Rigidbody>();


        //�v���C���[�ԍ��ɉ����čŏ��̃|�W�V������ݒ�
        switch (controllerNum)
        {
            case 1:
                startPos = new Vector3(5, 1.5f, -5);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 2:
                startPos = new Vector3(-5, 1.5f, -5);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 3:
                startPos = new Vector3(-5, 1.5f, 5);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case 4:
                startPos = new Vector3(5, 1.5f, 5);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }
    }

    void Update()
    {
        if (!isDead)
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
                angle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;

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

        //�����铮��̔���
        var throwAct = throwAction.triggered;

        //���e�𓊂���
        if (throwAct && haveBomb[0])
        {
            ThrowBomb(0);
            
        }
        if (throwAct && haveBomb[1])
        {
            ThrowBomb(1);
        }


        //�����Ă����ɃL���b�`�ł��Ȃ��悤�ɂ��邽�߂̓��̓N�[���^�C��
        if (isInputTimer)
        {
            timer += Time.deltaTime;

            if (timer > 0.2)
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

    public void HaveBombChange(int i, bool a)
    {
        haveBomb[i] = a;
    }

    //���e�𓊂���֐�
    private void ThrowBomb(int bombNum)
    {
        //���̓N�[���^�C���X�^�[�g
        isInputTimer = true;

        //AddForce�œ�����̂ŁAisKinematic��false����
        bombRb[bombNum].isKinematic = false;

        //�R���C�_�[����
        bombScript[bombNum].ColliderEnabled(true);

        //���e�������Ă��锻���false�ɂ���
        energyTank.MissingCore();
        haveBomb[bombNum] = false;

        //���e���΂�
        bombRb[bombNum].AddForce(bomb[bombNum].transform.forward * 500 * power);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�X�e�[�W�O�ɗ��������AhaveBomb��false�ɂ���
        if (collision.gameObject.tag == "Reset")
        {
            if (haveBomb[0])
            {
                Debug.Log("reset");
                //�{���������Ă��锻���false�ɂ���
                haveBomb[0] = false;

                bombRb[0].isKinematic = false;

                StartCoroutine(bombScript[0].PosReset(0));
            }
            else if (haveBomb[1])
            {
                Debug.Log("reset");
                //�{���������Ă��锻���false�ɂ���
                haveBomb[1] = false;

                bombRb[1].isKinematic = false;

                StartCoroutine(bombScript[1].PosReset(0));
            }

            
            energyTank.DropEnergy(4);

            PlayerDead(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�X�e�[�W�̌��ɗ������瓮���Ȃ��Ȃ�
        if (other.gameObject.tag == "FallArea")
        {
            isDead = true;
        }

        //�����ɓ��������玀�S
        if (other.gameObject.tag == "Explosion0" && !isDead)
        {
            if (haveBomb[1])
            {
                //�{���������Ă��锻���false�ɂ���
                haveBomb[1] = false;
                energyTank.MissingCore();

                bombScript[1].Explosion();

                energyTank.DropEnergy(4);
            }

            PlayerDead(1);
        }
        if (other.gameObject.tag == "Explosion1" && !isDead)
        {
            if (haveBomb[0])
            {
                //�{���������Ă��锻���false�ɂ���
                haveBomb[0] = false;
                energyTank.MissingCore();

                bombScript[0].Explosion();

                energyTank.DropEnergy(4);
            }

            PlayerDead(1);
        }
    }


    
    private void PlayerDead(int lostEnergy)
    {
        //�v���C���[���S
        isDead = true;

        haveBomb[0] = false;
        haveBomb[1] = false;

        //�v���C���[�̃��f�����\��
        body.SetActive(false);
        head.SetActive(false);
        hand.SetActive(false);

        //��̃I�u�W�F�N�g��������Ȃ��悤�ɃR���C�_�[���I�t
        capsule.enabled = false;

        //���e�����Ȃ�����
        takeBomb.CanTakeChange(0, false);
        takeBomb.CanTakeChange(1, false);


        //StartCoroutine(craneScript.RotateToTarget());
        StartCoroutine(posReset(lostEnergy));
    }

    private IEnumerator posReset(int lostEnergy)
    {
        yield return new WaitForSeconds(1.0f);

        //�X�^�[�g�ʒu�Ƀ��X�|�[��
        transform.position = startPos;

        //�v���C���[�̃��f����\��
        body.SetActive(true);
        head.SetActive(true);
        hand.SetActive(true);

        //�R���C�_�[����
        capsule.enabled = true;

        //�v���C���[����
        isDead = false;

        //�������^���N�̃G�l���M�[2,�����ɓ��������Ƃ�1����
        savingTank.UseEnergy(lostEnergy);
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

    public void HitPlayerChange(GameObject hitplayer)
    {
        takeBomb.SetHitPlayer(hitplayer);
    }
}
