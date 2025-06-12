using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public TakeBeehive takeBeehive;

    [SerializeField, Header("HoneyTank�X�N���v�g")]
    HoneyTank HoneyTank;

    [SerializeField, Header("SellArea�X�N���v�g")]
    SellArea SellArea;

    [SerializeField, Header("Beehive�X�N���v�g")]
    Beehive[] beehiveScript;

    [SerializeField, Header("�n�`�̑��I�u�W�F�N�g")] GameObject[] beehive;
    [SerializeField, Header("���d�G�t�F�N�g")] GameObject[] effect;
    [SerializeField, Header("�n�`�̑�������͈�")] GameObject passRange;

    [SerializeField, Header("�{�����L���b�`�ł���͈�")] GameObject catchRange;

    [SerializeField, Header("�w�����Ă���^���N�I�u�W�F�N�g")]
    GameObject playerTank;

    [Header("�v���C���[�̃��f��")]
    [SerializeField] GameObject body;
    //[SerializeField] GameObject head;
    [SerializeField] GameObject hand;

    [SerializeField, Header("�v���C���[�̃R���C�_�[")]
    CapsuleCollider capsule;

    [SerializeField, Header("�v���C���[��Rigidbody")]
    Rigidbody playerRb;

    [SerializeField, Header("�n�`�̑���Rigidbody")]
    Rigidbody[] beehiveRb;

    //�v���C���[�̃X�^�[�g�ʒu
    private Vector3 startPos;
    //�v���C���[�̃X�^�[�g�̌���
    private Quaternion startAngle;

    //�v���C���[�̔ԍ�
    public int controllerNum;

    [SerializeField, Header("�v���C���[�̈ړ��X�s�[�h")]
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

    //�n�`�̑��𓊂����
    private float power = 1;

    //�n�`�̑��������Ă��锻��
    public bool[] haveBeehive;

    //���S����
    public bool isDead = true;

    //�X�^������
    public bool isStan = true;

    //�_�Ŕ���
    private bool isBlink = false;

    private Animator animator;

    void Start()
    {
        //ActionMap���擾
        var input = GetComponent<PlayerInput>();
        //var input = PlayerInput.GetPlayerByIndex(controllerNum - 1);
        var actionMap = input.currentActionMap;

        moveAction = actionMap["Move"];
        throwAction = actionMap["PickUp"];


        isStan = true;

        haveBeehive[0] = false;
        haveBeehive[1] = false;


        beehiveRb[0] = beehive[0].GetComponent<Rigidbody>();
        beehiveRb[1] = beehive[1].GetComponent<Rigidbody>();


        //�ŏ��̃|�W�V������ݒ�
        startPos = transform.position;
        startAngle = transform.localRotation;

        animator = GetComponent<Animator>();

        //�X�^�[�g������ɓ�����悤�ɂ���
        Invoke("GameStart", 3.5f);
    }

    private void GameStart()
    {
        isStan = false;
    }

    void Update()
    {
        #region �v���C���[�̑���
        if (!isStan)
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
        #endregion

        //�n�`�̑���D����͈͂̒Ǐ]
        passRange.transform.position = transform.position;
        passRange.transform.rotation = transform.rotation;

        //�L���b�`�ł���͈͂̒Ǐ]
        catchRange.transform.position = transform.position;
        catchRange.transform.rotation = transform.rotation;


        //�����铮��̔���
        var throwAct = throwAction.triggered;

        //�n�`�̑��𓊂���
        if (throwAct && haveBeehive[0] && !isInputTimer && Time.timeScale == 1)
        {
            //�����ɃL���b�`�ł��Ȃ��悤�ɂ���
            catchRange.GetComponent<Catch>().TimerStart();
            takeBeehive.IsTimerTrue();

            //�n�`�̑��𓊂���ꂽ��Ԃɂ���
            beehiveScript[0].SetIsThrow(true);

            ThrowBeehive(0);

        }
        if (throwAct && haveBeehive[1] && !isInputTimer && Time.timeScale == 1)
        {
            //�����ɃL���b�`�ł��Ȃ��悤�ɂ���
            catchRange.GetComponent<Catch>().TimerStart();
            takeBeehive.IsTimerTrue();

            //�n�`�̑��𓊂���ꂽ��Ԃɂ���
            beehiveScript[1].SetIsThrow(true);

            ThrowBeehive(1);
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

    public void HaveBeehiveChange(int i, bool a)
    {
        haveBeehive[i] = a;
    }

    public void IsDeadTrue()
    {
        //�N�������������Ƃ��A�S�������Ȃ�����
        isStan = true;
    }

    //���e�𓊂���֐�
    private void ThrowBeehive(int bombNum)
    {
        //���̓N�[���^�C���X�^�[�g
        isInputTimer = true;

        //AddForce�œ�����̂ŁAisKinematic��false����
        beehiveRb[bombNum].isKinematic = false;

        //�R���C�_�[����
        beehiveScript[bombNum].ColliderEnabled(true);

        //���e�������Ă��锻���false�ɂ���
        HoneyTank.MissingBeehive();
        haveBeehive[bombNum] = false;


        //���e���΂�
        beehiveRb[bombNum].AddForce(beehive[bombNum].transform.forward * 500 * power);
    }

    public IEnumerator WinnerAnim(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        animator.SetBool("isWin", true);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //����ꂽ��X�^��
        if (other.gameObject.tag == "Punch1" && !isStan)
        {
            if (haveBeehive[1])
            {
                //�{���������Ă��锻���false�ɂ���
                haveBeehive[1] = false;
                HoneyTank.MissingBeehive();


                //beehiveScript[1].Explosion();
            }
            PlayerStan();
        }
        if (other.gameObject.tag == "Punch2" && !isStan) 
        {
            if (haveBeehive[0])
            {
                //�{���������Ă��锻���false�ɂ���
                haveBeehive[0] = false;
                HoneyTank.MissingBeehive();


                //beehiveScript[0].Explosion();
            }
            PlayerStan();
        }
    }

    //�v���C���[���X�^����Ԃɂ���
    private void PlayerStan()
    {
        isStan = true;

        haveBeehive[0] = false;
        haveBeehive[1] = false;


        HoneyTank.DropHoney();

        //�^���N���\��
        playerTank.SetActive(false);

        //���e�����Ȃ�����
        takeBeehive.CanTakeChange(0, false);
        takeBeehive.CanTakeChange(1, false);        


        //�v���C���[��_�ł�����
        gameObject.GetComponent<CharacterBlinkingScript>().BlinkStart(true);


        //�X�^�������܂ő҂�
        Invoke("CancellStan", 2.0f);
    }

    //�X�^������
    private void CancellStan()
    {
        isStan = false;   

        //�_�ŏI��
        gameObject.GetComponent<CharacterBlinkingScript>().BlinkStart(false);

        //�v���C���[�̃��f����\��
        body.SetActive(true);
        hand.SetActive(true);

        //�^���N��\��
        playerTank.SetActive(true);
    }

    public void CanTakeTrue(int i)
    {
        takeBeehive.CanTakeChange(i, true);
    }
}
