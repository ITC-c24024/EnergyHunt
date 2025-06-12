using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnergyTank : MonoBehaviour
{
    float energyTimer;

    //�G�l���M�[�����܂鑬��
    public float chargeSpeed = 1;

    //�^���N�̗e��
    [SerializeField] int maxEnergy;

    //�����^���N�̃G�l���M�[��
    public int energyAmount;

    //�g�їp�̃^���N�I�u�W�F�N�g
    [SerializeField] GameObject enagytank;

    [SerializeField, Header("�G�l���M�[�̃}�e���A��")]
    Material[] energyMat;

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
    DropEnergy dropEnergySC;

    //�v���C���[�R���g���[���[�X�N���v�g
    [SerializeField] PlayerController playerController;

    public BarrierScript barrierScript;

    //�����^���N�̏����ʒu
    private Vector3 startPos;
    //�����X�P�[��
    private Vector3 startScale;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField, Header("�^���N�G���A�̃G�t�F�N�g")]
    GameObject areaEffect;

    [SerializeField, Header("�|�[�Y���")]
    GameObject poseImage;

    void Start()
    {
        //PlayerInput�R���|�[�l���g���擾
        var input = GetComponent<PlayerInput>();

        //�擾����PlayerInput��ActionMap���擾
        var actionMap = input.currentActionMap;

        //ActionMap���̎��o���A�a����A�N�V�������擾
        putInAction = actionMap["PutIn"];

        //�X�N���v�g�擾
        dropEnergySC = dropEnergy.GetComponent<DropEnergy>();
        savingTankSC = savingTank.GetComponent<SavingTank>();
        //�X�^�[�g���̃^���N�̈ʒu�ƃX�P�[�����擾
        startPos = enagytank.transform.localPosition;
        startScale = enagytank.transform.localScale;
    }

    private void Update()
    {
        //�G�l���M�[���ڂ��{�^���������ꂽ���ǂ������`�F�b�N
        var putIn = putInAction.triggered;

        if (putIn)
        {
            Time.timeScale = 0;

            poseImage.SetActive(true);
        }

        //�G�l���M�[�R�A�������̃G�l���M�[����
        if (charge && energyAmount < maxEnergy)
        {
            energyTimer += Time.deltaTime * chargeSpeed;

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

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == $"TankArea{playerController.controllerNum}")
        {
            canSave = true;

            //�G�l���M�[�̗a�����ꂪ�ł����Ԃ�������a�������
            if (canSave && energyAmount >= maxEnergy && !barrierScript.isBarrier && !playerController.isDead)
            {
                TankCharging();
            }
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
        if (energyAmount == 0)
        {
            enagytank.SetActive(true);
        }

        energyAmount += amount;
        //Debug.Log("�`���[�W"+amount);


        //�����G�l���M�[���^���N�e�ʂ𒴂��Ȃ��悤�ɂ���
        if (energyAmount >= maxEnergy)
        {
            //�G���A�\��
            areaEffect.SetActive(true);

            energyAmount = maxEnergy;

            enagytank.GetComponent<MeshRenderer>().material = energyMat[1];
        }

        if(energyAmount== maxEnergy)
        {
            audioSource.Play();
        }

        //Debug.Log("�[�d��");
        //�G�l���M�[�ʂɉ����ă^���N�̃��[�^�[���X�P�[���A�b�v���A���[�^�[�̃|�W�V�����𒲐�
        enagytank.transform.localScale
            = new Vector3(startScale.x, energyAmount * 0.045f, startScale.z);
        enagytank.transform.localPosition
            = new Vector3(startPos.x, startPos.y - (energyAmount * 0.0215f), startPos.z);
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

    //�G�l���M�[���Z�b�g
    public void EnergyReset()
    {
        //�G�l���M�[���Z�b�g
        energyAmount = 0;

        //�}�e���A�������Z�b�g
        enagytank.GetComponent<MeshRenderer>().material = energyMat[0];

        //���[�^�[�̃X�P�[���ƃ|�W�V���������Z�b�g
        enagytank.transform.localScale = startScale;
        enagytank.transform.localPosition = startPos;
    }

    //�G�l���M�[�R�A�̔����Ɋ������܂ꂽ�Ƃ��̏����G�l���M�[�h���b�v
    public void DropEnergy(int target)
    {
        //Debug.Log("drop");
        //�G�l���M�[�R�A������Ԃ̉���
        MissingCore();

        //�^���N�̃G�l���M�[�ʂ�0�łȂ��ꍇ�A�h���b�v�I�u�W�F�N�g�𗎂Ƃ�
        if (energyAmount != 0)
        {
            //�h���b�v�I�u�W�F�N�g�����g�̈ʒu�Ɉړ����ăh���b�v

            dropEnergy.transform.localScale = new Vector3(0.3f, energyAmount * 0.024f, 0.3f);
            dropEnergy.transform.position = enagytank.transform.position;

            Rigidbody dropRb = dropEnergy.GetComponent<Rigidbody>();

            //�h���b�v�I�u�W�F�N�g�ɃG�l���M�[�����p��
            dropEnergySC.SetEnergyAmount(energyAmount);

            //�h���b�v�I�u�W�F�N�g�̃^�[�Q�b�g��I��
            StartCoroutine(dropEnergySC.SelectPos(target));
        }


        //�G�l���M�[��\��
        enagytank.SetActive(false);

        //�G���A��\��
        areaEffect.SetActive(false);

        //�G�l���M�[���Z�b�g
        energyAmount = 0;

        //�}�e���A�������Z�b�g
        enagytank.GetComponent<MeshRenderer>().material = energyMat[0];

        //���[�^�[�̃X�P�[���ƃ|�W�V���������Z�b�g
        enagytank.transform.localScale = startScale;
        enagytank.transform.localPosition = startPos;

        //�^���N�̔�\��
        //enagytank.SetActive(false);
    }

    //���~�p�̃^���N�ɃG�l���M�[�^���N���ڂ�
    public void TankCharging()
    {
        //���~�p�̃^���N�̃G�l���M�[����
        savingTankSC.Charge();

        //�G�l���M�[��\��
        enagytank.SetActive(false);

        //�G���A��\��
        areaEffect.SetActive(false);

        //�G�l���M�[���Z�b�g
        energyAmount = 0;

        //�}�e���A�������Z�b�g
        enagytank.GetComponent<MeshRenderer>().material = energyMat[0];

        //���[�^�[�̃X�P�[���ƃ|�W�V���������Z�b�g
        enagytank.transform.localScale = startScale;
        enagytank.transform.localPosition = startPos;
    }
}
