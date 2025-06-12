using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnergyTank_copy
    : MonoBehaviour
{
    float energyTimer;

    //�G�l���M�[�����܂鑬��
    [SerializeField] float chageSpeed = 1;

    //�^���N�̗e��
    [SerializeField] int maxEnergy;

    //�����^���N�̃G�l���M�[��
    public int energyAmount;

    //�g�їp�̃^���N�I�u�W�F�N�g
    [SerializeField] GameObject enagytank;

    //���~�p�̃^���N�I�u�W�F�N�g
    [SerializeField] GameObject savingTank;

    //���~�p�̃^���N�I�u�W�F�N�g�̃X�N���v�g
    SavingTank savingTankSC;

    //�^���N�ɗa�����邩�̔���
    private bool canSave = false;

    //�G�l���M�[���ڂ����Ɏg���{�^������̓��͂��󂯎��ϐ�
    private InputAction putInAction;

    //�G�l���M�[�R�A���������Ă��邩�ǂ����̔���
    public bool charge;

    //�G�l���M�[���h���b�v�����Ƃ��p�̃I�u�W�F�N�g
    [SerializeField] GameObject dropEnergy;

    //�h���b�v�I�u�W�F�N�g�̃X�N���v�g
    DropEnergy_copy dropEnergySC;

    //�v���C���[�R���g���[���[�X�N���v�g
    [SerializeField] PlayerController_copy playerController;

    [SerializeField, Header("�����^���N�̏����ʒu")]
    private Vector3 startPos;

    [SerializeField, Header("�h���b�v����Ƃ��̐���")]
    float dropPower;

    void Start()
    {
        //PlayerInput�R���|�[�l���g���擾
        var input = GetComponent<PlayerInput>();

        //�擾����PlayerInput��ActionMap���擾
        var actionMap = input.currentActionMap;

        //ActionMap���̎��o���A�a����A�N�V�������擾
        putInAction = actionMap["PutIn"];

        //�X�N���v�g�擾
        dropEnergySC = dropEnergy.GetComponent<DropEnergy_copy>();
        savingTankSC = savingTank.GetComponent<SavingTank>();
        //�X�^�[�g���̃^���N�̈ʒu���擾
        //startPos = transform.localPosition;
    }

    private void Update()
    {
        //�G�l���M�[���ڂ��{�^���������ꂽ���ǂ������`�F�b�N
        var putIn = putInAction.triggered;

        //�G�l���M�[�R�A�������̃G�l���M�[����
        if (charge)
        {

            energyTimer += Time.deltaTime * chageSpeed;

            //1�b���ƂɃG�l���M�[����
            if (energyTimer >= 1)
            {
                energyTimer = 0;

                ChargeEnergy(1);


            }
        }
        else
        {
            energyTimer = 0;
        }


        //�G�l���M�[�̗a�����ꂪ�ł����Ԃ�������a�������
        if (putIn && canSave && !charge && energyAmount >= maxEnergy)
        {
            TankCharging();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == $"TankArea{playerController.controllerNum}")
        {
            canSave = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == $"TankArea{playerController.controllerNum}")
        {
            canSave = false;
        }
    }


    //�G�l���M�[�����ƃX�P�[���A�b�v
    public void ChargeEnergy(int amount)
    {
        energyAmount += amount;

        //�����G�l���M�[���^���N�e�ʂ𒴂��Ȃ��悤�ɂ���
        if (energyAmount >= maxEnergy)
        {
            energyAmount = maxEnergy;
        }

        //Debug.Log("�[�d��");
        //�G�l���M�[�ʂɉ����ă^���N�̃��[�^�[���X�P�[���A�b�v���A���[�^�[�̃|�W�V�����𒲐�
        enagytank.transform.localScale = new Vector3(transform.localScale.x, energyAmount * 0.012f, transform.localScale.z);
        enagytank.transform.localPosition = new Vector3(transform.localPosition.x, -0.04f + (energyAmount * 0.012f), transform.localPosition.z);
    }

    //�G�l���M�[�R�A�������Ă���Ƃ�
    public void HaveCore()
    {
        charge = true;
    }


    //�G�l���M�[�R�A����������Ƃ�
    public void MissingCore()
    {
        charge = false;
    }

    //�G�l���M�[�R�A�̔����Ɋ������܂ꂽ�Ƃ��̏����G�l���M�[�h���b�v
    public void DropEnergy(int target)
    {
        //�G�l���M�[�R�A������Ԃ̉���
        MissingCore();

        //�^���N�̃G�l���M�[�ʂ�0�łȂ��ꍇ�A�h���b�v�I�u�W�F�N�g�𗎂Ƃ�
        if (energyAmount != 0)
        {
            //�h���b�v�I�u�W�F�N�g�����g�̈ʒu�Ɉړ����ăh���b�v

            dropEnergy.transform.localScale = new Vector3(0.3f, energyAmount * 0.024f, 0.3f);
            dropEnergy.transform.position = enagytank.transform.position;

            Rigidbody dropRb = dropEnergy.GetComponent<Rigidbody>();
        }

        //�h���b�v�I�u�W�F�N�g�ɃG�l���M�[�����p��
        dropEnergySC.SetEnergyAmount(energyAmount);

        //�h���b�v�I�u�W�F�N�g�̃^�[�Q�b�g��I��
        dropEnergySC.SelectPos(target);

        //�G�l���M�[���Z�b�g
        energyAmount = 0;

        //���[�^�[�̃X�P�[���ƃ|�W�V���������Z�b�g
        enagytank.transform.localScale = new Vector3(0.165f, 0, 0.165f);
        enagytank.transform.localPosition = startPos;

        //�^���N�̔�\��
        enagytank.SetActive(false);
    }

    //���~�p�̃^���N�ɃG�l���M�[�^���N���ڂ�
    public void TankCharging()
    {
        //���~�p�̃^���N�̃G�l���M�[����
        savingTankSC.Charge();
    }
}
