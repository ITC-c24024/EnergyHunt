using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class EnergyTankManager : MonoBehaviour
{
    float energyTimer;

    //���ݒ��߂Ă���Œ��̃^���N�̃G�l���M�[��
    public int energyAmount;

    //�^���N�ЂƂ�����̗e��
    [SerializeField] int maxEnergy;

    //�������̃^���N�̌�
    public int energyTankAmount;

    //�G�l���M�[���^����Ԃ̃^���N�̌�
    [SerializeField] int fullTankAmount;

    //�����\�ȃ^���N�̏��
    [SerializeField] int tankAmountLimit;

    //�G�l���M�[�R�A���������Ă��邩�ǂ����̔���
    public bool charge;

    public Text scoreText;

    //�v���C���[����������^���N�I�u�W�F�N�g(�R��)
    [SerializeField] GameObject[] energyTank;

    //���~�p�̃^���N�I�u�W�F�N�g
    [SerializeField] GameObject savingTank;

    //���~�p�̃^���N�I�u�W�F�N�g�̃X�N���v�g
    SavingTank savingTankSC;

    //�v���C���[����������^���N���Ƃ̃X�N���v�g
    [SerializeField] EnergyTank[] energyTankSC;

    //�v���C���[�R���g���[���[�X�N���v�g
    [SerializeField] PlayerController playerController;

    //�^���N�ɗa�����邩�̔���
    private bool canSave = false;

    //�^���N������o���邩�̔���
    private bool canTake = false;
    //�G�l���M�[�̎��o���A�a����Ɏg���{�^������̓��͂��󂯎��ϐ�
    private InputAction putInAction, takeOutAction;

    void Start()
    {
        //PlayerInput�R���|�[�l���g���擾
        var input = GetComponent<PlayerInput>();

        //�擾����PlayerInput��ActionMap���擾
        var actionMap = input.currentActionMap;

        //ActionMap���̎��o���A�a����A�N�V�������擾
        putInAction = actionMap["PutIn"];
        takeOutAction = actionMap["TakeOut"];

        savingTankSC = savingTank.GetComponent<SavingTank>();
    }

    void Update()
    {
        //���o���A�a������̃{�^���������ꂽ���ǂ������`�F�b�N
        var putIn = putInAction.triggered;
        var takeOut = takeOutAction.triggered;

        //�G�l���M�[�R�A�������̃G�l���M�[����
        if (charge)//1P
        {

            if (fullTankAmount < tankAmountLimit)
            {
                energyTimer += Time.deltaTime * 4;

                //1�b���ƂɃG�l���M�[����
                if (energyTimer >= 1)
                {
                    energyTimer = 0;

                    energyAmount++;

                    //���ݏ[�d���̃^���N�̃G�l���M�[����
                    //Debug.Log(energyTankAmount);
                    energyTankSC[energyTankAmount].ChargeEnergy(1);
                    scoreText.text = "" + energyAmount;

                    //�����G�l���M�[���^���N�e�ʂ𒴂����ۂ̃^���N�ǉ�
                    if (energyAmount >= maxEnergy)
                    {
                        //�G�l���M�[���Z�b�g
                        energyAmount = 0;

                        //���^����Ԃ̃^���N�̃J�E���g����
                        fullTankAmount++;
                        PowerUpAndDown();

                        //�^���N�ǉ�
                        AddTank(false);
                    }
                }
            }
        }
        else
        {
            energyTimer = 0;
        }


        //�G�l���M�[�̗a�����ꂪ�ł����Ԃ�������a�������
        if (putIn && canSave && !charge)
        {
            TankCharging();
        }

        //�G�l���M�[�̎��o�����ł����Ԃ���������o��
        if (takeOut && canTake && !charge)
        {
            TakeOutEnergy();
        }
    }


    //�G�l���M�[�R�A�̔����Ɋ������܂ꂽ�Ƃ��̏����G�l���M�[�h���b�v
    public void LostEnergy()
    {
        //�G�l���M�[�R�A������Ԃ̉���
        MissingCore();

        //�������Ă����G�l���M�[���Z�b�g
        energyAmount = 0;

        //���^����Ԃ̃^���N�̌����Z�b�g
        fullTankAmount = 0;
        PowerUpAndDown();

        //�������Ă����^���N���Ƃ̃G�l���M�[�ɉ����ăh���b�v
        for (int i = 0; i < energyTankAmount + 1; i++)
        {
            //energyTankSC[i].DropEnergy();
        }

        //�^���N�̐������Z�b�g
        energyTankAmount = 0;
    }



    //�h���b�v���Ă���G�l���M�[���E�����ۂ̃G�l���M�[����
    private void OnTriggerEnter(Collider other)
    {
        //�h���b�v�I�u�W�F�N�g�̃X�N���v�g
        DropEnergy dropEnergy;

        //�G�l���M�[��
        int energy;

        //�Փ˂����I�u�W�F�N�g���h���b�v�I�u�W�F�N�g�������ꍇ
        if (other.tag == "DropEnergy")
        {
            if (energyAmount == 0)
            {
                energyTank[energyTankAmount].SetActive(true);
            }
            //�h���b�v�I�u�W�F�N�g�̃X�N���v�g�擾
            dropEnergy = other.GetComponent<DropEnergy>();

            //�h���b�v�I�u�W�F�N�g�̈ʒu�����Z�b�g
            dropEnergy.PosReset();

            //�h���b�v�I�u�W�F�N�g�̃G�l���M�[�ʂ��擾
            energy = dropEnergy.energyAmount;

            //�G�l���M�[�����̏����Ăт���
            PickUpEnergy(energy);
        }

        if (other.gameObject.tag == $"TankArea{playerController.controllerNum}")
        {
            canSave = true;
            canTake = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == $"TankArea{playerController.controllerNum}")
        {
            canSave = false;
            canTake = false;
        }
    }


    //�G�l���M�[����
    public void PickUpEnergy(int amount)
    {
        //�h���b�v�I�u�W�F�N�g�̃G�l���M�[�ʂ��^���N�e�ʈȏゾ�����ꍇ
        if (amount >= maxEnergy)
        {
            //���݃G�l���M�[�𒙂߂Ă���^���N�𖞃^���ɂ���
            energyTank[energyTankAmount].SetActive(true);
            //energyTankSC[energyTankAmount].SetEnergy(maxEnergy);

            //���^����Ԃ̃^���N�̌�����
            if (fullTankAmount < tankAmountLimit)
            {
                fullTankAmount++;
                PowerUpAndDown();
            }

            //�������Ă���^���N�̌�����������ɒB���Ă��Ȃ��ꍇ�Ƀ^���N�̒ǉ�
            if (energyTankAmount < tankAmountLimit - 1)
            {
                //�^���N�ǉ�
                AddTank(true);
            }
        }

        //�h���b�v�I�u�W�F�N�g�̃G�l���M�[�ƌ��ݒ��߂Ă���^���N�̃G�l���M�[�ʂ̑��ʂ��^���N�e�ʈȏゾ�����ꍇ
        else if (amount + energyAmount >= maxEnergy)
        {
            //���݃G�l���M�[�𒙂߂Ă���^���N�𖞃^���ɂ���
            energyTank[energyTankAmount].SetActive(true);
            //energyTankSC[energyTankAmount].SetEnergy(maxEnergy);

            //���^����Ԃ̃^���N�̌�����
            if (fullTankAmount < tankAmountLimit)
            {
                fullTankAmount++;
                PowerUpAndDown();
            }

            //�������Ă���^���N�̌�����������ɒB���Ă��Ȃ��ꍇ�Ƀ^���N�̒ǉ�
            if (energyTankAmount < tankAmountLimit - 1)
            {
                //�^���N��ǉ�
                AddTank(false);

                //��ꂽ���̃G�l���M�[��ǉ������^���N�Ɉ��p��
                //energyTankSC[energyTankAmount].SetEnergy((amount + energyAmount) - maxEnergy);
                energyAmount = (amount + energyAmount) - maxEnergy;

            }
        }
        //�h���b�v�I�u�W�F�N�g�̃G�l���M�[�ƌ��ݒ��߂Ă���^���N�̃G�l���M�[�ʂ̑��ʂ��^���N�e�ʂ𒴂��Ȃ��ꍇ
        else
        {
            //������̃G�l���M�[����
            //energyTankSC[energyTankAmount].SetEnergy(amount + energyAmount);
            energyAmount += amount;

        }

    }


    //�G�l���M�[�R�A�������Ă���Ƃ�
    public void HaveCore()
    {
        energyTank[0].SetActive(true);

        PowerUpAndDown();

        charge = true;
    }


    //�G�l���M�[�R�A����������Ƃ�
    public void MissingCore()
    {
        charge = false;
    }


    //�G�l���M�[�^���N�ǉ�
    public void AddTank(bool takeOut)
    {
        //�^���N����������ɒB���Ă��Ȃ�������^���N��ǉ�
        if (energyTankAmount < tankAmountLimit - 1)
        {
            //�^���N�ǉ�
            energyTankAmount++;

            //�ǉ����̃^���N��\��
            Debug.Log("�^���N�ǉ�");
            energyTank[energyTankAmount].SetActive(true);
        }

        //���~�p�̃^���N������o�����ꍇ�A�G�l���M�[���E�����ꍇ�̃G�l���M�[���p��
        if (takeOut)
        {
            if (energyAmount != 0)
            {
                //energyTankSC[energyTankAmount].SetEnergy(energyAmount);
            }
        }
    }


    //���~�p�̃^���N�ɃG�l���M�[�^���N���ڂ�
    public void TankCharging()
    {
        //�P�ȏ㖞�^���̃^���N������ꍇ
        if (fullTankAmount > 0 && savingTankSC.energyAmount < 9)
        {
            //�ڂ������̃^���N��\��
            energyTank[energyTankAmount].SetActive(false);

            if (energyTankAmount > 0)
            {
                //�^���N�����炷
                energyTankAmount--;
            }

            if (energyTankAmount == 0)
            {
                energyTank[energyTankAmount].SetActive(true);
            }

            //���^���̃^���N�̐������炷
            fullTankAmount--;
            PowerUpAndDown();

            if (energyAmount != 0)
            {
                //���߂Ă���r���̃G�l���M�[���c�����^���N�ɑ��
                //energyTankSC[energyTankAmount].SetEnergy(energyAmount);
            }
            else if (energyAmount == 0 && energyTankAmount == 1)
            {
                //energyTankSC[energyTankAmount].SetEnergy(maxEnergy);
            }

            //�^���N�̃G�l���M�[��������
            if (energyTankAmount == 1 && fullTankAmount == 1)
            {
                //energyTankSC[energyTankAmount].SetEnergy(0);
            }
            else
            {
                //energyTankSC[energyTankAmount + 1].SetEnergy(0);
            }

            //���~�p�̃^���N�̃G�l���M�[����
            savingTankSC.Charge();
        }

        if (fullTankAmount == 0 && energyAmount == 0)
        {
            //energyTankSC[energyTankAmount].SetEnergy(0);
        }


    }


    //���~�p�̃^���N����G�l���M�[�����o��
    public void TakeOutEnergy()
    {
        //���^����Ԃ̃^���N�̌������������菭�Ȃ��ꍇ
        if (savingTankSC.energyAmount > 0 && fullTankAmount < tankAmountLimit)
        {
            //���݃G�l���M�[�𒙂߂Ă���^���N�𖞃^���ɂ���
            if (energyTankAmount + 1 == fullTankAmount)
            {
                //energyTankSC[energyTankAmount + 1].SetEnergy(maxEnergy);
            }
            else
            {
                //energyTankSC[energyTankAmount].SetEnergy(maxEnergy);
            }

            //���^����Ԃ̃^���N���𑝂₷
            fullTankAmount++;
            PowerUpAndDown();

            //�������Ă���^���N�̌�����������ɒB���Ă��Ȃ��ꍇ�Ƀ^���N�̒ǉ�
            if (energyTankAmount < tankAmountLimit - 1)
            {
                AddTank(true);
            }

            //���~�p�̃^���N���̃G�l���M�[����
            savingTankSC.UseEnergy(1);
        }

        if (fullTankAmount >= tankAmountLimit)
        {
            energyAmount = 0;
        }
    }

    //���^����Ԃ̃^���N�̌��ɉ����ăv���C���[�̐g�̋���
    public void PowerUpAndDown()
    {
        playerController.FullTank(fullTankAmount);
    }

}
