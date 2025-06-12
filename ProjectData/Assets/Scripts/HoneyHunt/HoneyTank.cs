using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoneyTank : MonoBehaviour
{
    float honeyTimer;

    //�͂��݂����܂鑬��
    float chargeSpeed = 10;

    //�������Ă���͂��݂̗�
    public int honeyAmount;

    //����͂��݂̗�
    private int sellAmount = 0;

    //�͂��݂�����I�u�W�F�N�g
    [SerializeField] GameObject honeytank;

    [SerializeField, Header("�͂��݂̃}�e���A��")]
    Material[] honeyMat;

    //�͂��݂𔄂邽�߂̃G���A�I�u�W�F�N�g
    [SerializeField] GameObject sellArea;

    //�G���A�I�u�W�F�N�g�̃X�N���v�g
    SellArea sellAreaSC;

    //�n�`�̑����������Ă��邩�ǂ����̔���
    public bool charge;

    //�͂��݂𔄂�邩�̔���
    public bool canSell = true;

    //�͂��݂��h���b�v�����Ƃ��p�̃I�u�W�F�N�g
    [SerializeField] GameObject dropManager;

    //�h���b�v�I�u�W�F�N�g�̃X�N���v�g
    DropHoneyManager dropManagerSC;

    //�v���C���[�R���g���[���[�X�N���v�g
    [SerializeField] Player playerSC;

    //�|�[�Y�{�^������̓��͂��󂯎��ϐ�
    private InputAction poseAction;

    //�����^���N�̏����ʒu
    private Vector3 startPos;
    //�����X�P�[��
    private Vector3 startScale;

    [SerializeField, Header("�|�[�Y���")]
    GameObject poseImage;


    [SerializeField] Text honeyAmountText;

    [Header("������UI�̃X�N���v�g"), SerializeField]
    SpriteNum spriteNum;


    [Header("�g���b�N�ړ��X�N���v�g"), SerializeField]
    TrackScript trackScript;

    void Start()
    {
        //PlayerInput�R���|�[�l���g���擾
        var input = GetComponent<PlayerInput>();

        //�擾����PlayerInput��ActionMap���擾
        var actionMap = input.currentActionMap;

        //ActionMap���̃|�[�Y�A�N�V�������擾
        poseAction = actionMap["PutIn"];

        //�X�N���v�g�擾
        dropManagerSC = dropManager.GetComponent<DropHoneyManager>();
        sellAreaSC = sellArea.GetComponent<SellArea>();
        //�X�^�[�g���̃^���N�̈ʒu�ƃX�P�[�����擾
        startPos = honeytank.transform.localPosition;
        startScale = honeytank.transform.localScale;

    }

    private void Update()
    {
        //�|�[�Y�{�^���������ꂽ���ǂ������`�F�b�N
        var putIn = poseAction.triggered;

        if (putIn)
        {
            Time.timeScale = 0;

            poseImage.SetActive(true);
        }

        //�n�`�̑��������̂͂��݂���
        if (charge)
        {
            honeyTimer += Time.deltaTime * chargeSpeed;

            //1�b���Ƃɂ͂��݂���
            if (honeyTimer >= 1)
            {
                honeyTimer = 0;

                GetHoney(1);
            }
        }
        else
        {
            honeyTimer = 0;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SellArea")
        {
            if (canSell && honeyAmount > 0)
            {
                //�g���b�N�̈ړ�����
                bool moveTrack = trackScript.isMove;

                //�͂��݂̔��p���o�����Ԃ������甄�p����
                if (!moveTrack && !playerSC.isStan)
                {
                    SellHoney();
                }
                StartCoroutine(CoolTime());
            }

        }
    }

    //���p���̃N�[���^�C��(2�b)
    IEnumerator CoolTime()
    {
        canSell = false;

        yield return new WaitForSeconds(2);

        canSell = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "SellArea")
        //{
        //    canSell = false;
        //}
    }


    //�͂��݂����ƃX�P�[���A�b�v
    public void GetHoney(int amount)
    {
        if (honeyAmount == 0)
        {
            honeytank.SetActive(true);
        }

        honeyAmount += amount;

        //������UI�̍X�V
        spriteNum.ChangeSprite(honeyAmount);

        //�͂��݂̗ʂɉ����ă^���N�̃��[�^�[���X�P�[���A�b�v���A���[�^�[�̃|�W�V�����𒲐�
        honeytank.transform.localScale
            = new Vector3(startScale.x, honeyAmount * 0.045f, startScale.z);
        honeytank.transform.localPosition
            = new Vector3(startPos.x, startPos.y - (honeyAmount * 0.0215f), startPos.z);
    }

    //�n�`�̑��������Ă���Ƃ�
    public void HaveBeehive()
    {
        charge = true;
    }


    //�n�`�̑�����������Ƃ�
    public void MissingBeehive()
    {
        charge = false;
    }

    //�͂��݂��Z�b�g
    public void HoneyReset()
    {
        //�͂��݂��Z�b�g
        honeyAmount = 0;

        //�}�e���A�������Z�b�g
        honeytank.GetComponent<MeshRenderer>().material = honeyMat[0];

        //���[�^�[�̃X�P�[���ƃ|�W�V���������Z�b�g
        honeytank.transform.localScale = startScale;
        honeytank.transform.localPosition = startPos;
    }

    //�n�`�̑��̍U���Ɋ������܂ꂽ�Ƃ��̏����G�l���M�[�h���b�v
    public void DropHoney()
    {
        Debug.Log("drop");
        //�n�`�̑�������Ԃ̉���
        MissingBeehive();

        //�������Ă���͂��݂�0�łȂ��ꍇ�A�h���b�v�I�u�W�F�N�g�𗎂Ƃ�
        if (honeyAmount != 0)
        {
            //�h���b�v�I�u�W�F�N�g�����g�̈ʒu�Ɉړ����ăh���b�v

            dropManager.transform.localScale = new Vector3(0.3f, honeyAmount * 0.024f, 0.3f);
            dropManager.transform.position = honeytank.transform.position;

            Rigidbody dropRb = dropManager.GetComponent<Rigidbody>();

            //�h���b�v�I�u�W�F�N�g�ɃG�l���M�[�����p��
            dropManagerSC.Drop(honeyAmount/3);

        }


        //�͂��݂�\��
        honeytank.SetActive(false);

        //�͂��݂̗ʂ𒲐�
        honeyAmount = honeyAmount*(2/3);

        //������UI�̍X�V
        spriteNum.ChangeSprite(honeyAmount);

        //�}�e���A�������Z�b�g
        honeytank.GetComponent<MeshRenderer>().material = honeyMat[0];

        //���[�^�[�̃X�P�[���ƃ|�W�V���������Z�b�g
        honeytank.transform.localScale = startScale;
        honeytank.transform.localPosition = startPos;

    }

    //�͂��݂𔄋p����
    public void SellHoney()
    {
        //�͂��݂����ʂ𒴂���ꍇ
        if (/*honeyAmount > 100*/honeyAmount == -1)
        {
            sellAmount = 100;

            //�c�����͂��݂̗ʂ�ۑ�
            honeyAmount -= sellAmount;

            //�͂��݂̗ʂɉ����ă^���N�̃��[�^�[���X�P�[���A�b�v���A���[�^�[�̃|�W�V�����𒲐�
            honeytank.transform.localScale
                = new Vector3(startScale.x, honeyAmount * 0.045f, startScale.z);
            honeytank.transform.localPosition
                = new Vector3(startPos.x, startPos.y - (honeyAmount * 0.0215f), startPos.z);
        }
        else
        {
            sellAmount = honeyAmount;

            //�͂��݂��Z�b�g
            honeyAmount = 0;

            //�͂��݂�\��
            honeytank.SetActive(false);

            //�}�e���A�������Z�b�g
            honeytank.GetComponent<MeshRenderer>().material = honeyMat[0];
        }
        //����ʂ�SellArea�X�N���v�g�ɓn��
        sellAreaSC.Sell(sellAmount, playerSC.controllerNum);


        //������UI�̍X�V
        spriteNum.ChangeSprite(honeyAmount);



        //���[�^�[�̃X�P�[���ƃ|�W�V���������Z�b�g
        //honeytank.transform.localScale = startScale;
        //honeytank.transform.localPosition = startPos;
    }
}
