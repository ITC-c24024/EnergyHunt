using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
//[RequireComponent(typeof(PlayerInput))]

public class TakeBomb_copy : MonoBehaviour
{
    [SerializeField, Header("EnergyTank�X�N���v�g")] 
    EnergyTank_copy[] energyTank;

    [SerializeField, Header("PlayerController�X�N���v�g")] 
    PlayerController_copy[] playerController;
    
    [SerializeField, Header("���e�̃X�N���v�g")] 
    BombScript[] bombScript;

    [SerializeField, Header("�����̃X�N���v�g")] 
    ExplosionScript_copy[] explosionScript;

    [SerializeField, Header("���e�I�u�W�F�N�g")] 
    GameObject[] bomb;

    [SerializeField, Header("�v���C���[�I�u�W�F�N�g")]
    GameObject player;

    //���e������͈͂ɂ��锚�e�������Ă���v���C���[
    [SerializeField] private GameObject hitPlayer;
    //���e������͈͂ɂ��锚�e
    private GameObject hitBomb;

    //�v���C���[�̔ԍ�
    public int controllerNum;

    //���e�����邩�̔���
    public bool[] canTake;

    //���e�����A�N�V����
    private InputAction pickUpAction;

    [SerializeField, Header("�v���C���[��PlayerInput")] 
    PlayerInput input;

    //��������A���Ƀ{�^����������܂ł̎���
    private float timer = 0;
    //���̓N�[���^�C��
    //float inputTimer = 0.2f;
    //���̓N�[���^�C���̔���
    private bool isInputTimer = false;

    void Start()
    {
        //�v���C���[�̃A�N�V�����}�b�v���擾
        var actionMap = input.currentActionMap;

        pickUpAction = actionMap["PickUp"];


        canTake[0] = false;
        canTake[1] = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Bomb0" && !playerController[controllerNum - 1].haveBomb[0] && !playerController[controllerNum - 1].haveBomb[1])
        {
            //Debug.Log("canTake");
            canTake[0] = true;

            hitBomb = other.gameObject;
        }
        if (other.gameObject.tag == "Bomb1" && !playerController[controllerNum - 1].haveBomb[0] && !playerController[controllerNum - 1].haveBomb[1])
        {
            //Debug.Log("canTake");
            canTake[1] = true;

            hitBomb = other.gameObject;
        }

        if (!playerController[controllerNum - 1].haveBomb[0] && !playerController[controllerNum - 1].haveBomb[1]
            && (other.gameObject.tag == "P1" || other.gameObject.tag == "P2" || other.gameObject.tag == "P3" || other.gameObject.tag == "P4"))
        {
            //�����������肪�����ł͂Ȃ��ꍇ
            if (other.gameObject.tag != $"P{controllerNum}")
            {
                //�����������肪���e�������Ă���ꍇ
                if (other.gameObject.GetComponent<PlayerController_copy>().haveBomb[0])
                {
                    hitPlayer = other.gameObject;
                    canTake[0] = true;
                }
                else if (other.gameObject.GetComponent<PlayerController_copy>().haveBomb[1])
                {
                    hitPlayer = other.gameObject;
                    canTake[1] = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bomb0")
        {            
            canTake[0] = false;
        }
        if (other.gameObject.tag == "Bomb1")
        {
            canTake[1] = false;
        }

        if(!playerController[controllerNum - 1].haveBomb[0] && !playerController[controllerNum - 1].haveBomb[1]
            && (other.gameObject.tag == "P1" || other.gameObject.tag == "P2" || other.gameObject.tag == "P3" || other.gameObject.tag == "P4"))
        {
            if(other.gameObject.tag != $"P{controllerNum - 1}")
            {
                //Exit�������肪�{���������Ă�����AcanTake��false�ɂ���
                if (other.gameObject.GetComponent<PlayerController_copy>().haveBomb[0])
                {
                    hitPlayer = null;

                    canTake[0] = false;
                }
                else if (other.gameObject.GetComponent<PlayerController_copy>().haveBomb[1])
                {
                    hitPlayer = null;

                    canTake[1] = false;
                }
            }
        }
    }

    void Update()
    {
        //���e�������Ă���ꍇ�A�v���C���[�ɒǏ]
        if (playerController[controllerNum - 1].haveBomb[0])
        {
            bomb[0].transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            bomb[0].transform.rotation = transform.rotation;
        }
        else if (playerController[controllerNum - 1].haveBomb[1])
        {
            bomb[1].transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            bomb[1].transform.rotation = transform.rotation;
        }


        //canTake��2��true�̂Ƃ��A�Е�����true�ɂ���
        if (canTake[0] && canTake[1])
        {
            canTake[0] = false;
        }


        //���{�^���������Ă��邩�̔���
        var pickUp = pickUpAction.triggered;

        //���e��D��
        if (canTake[0] && pickUp && !playerController[controllerNum - 1].isDead && !bombScript[0].isExplosion)
        {
            //isInputTimer = true;

            TakeBombFunction(0);
        }
        if (canTake[1] && pickUp && !playerController[controllerNum - 1].isDead && !bombScript[1].isExplosion)
        {
            //isInputTimer = true;

            TakeBombFunction(1);
        }

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

    //���e��D���֐�
    private void TakeBombFunction(int bombNum)
    {
        Debug.Log("�����");      
        //���e�����Ȃ��悤�ɂ���
        CanTakeChange(0, false);
        CanTakeChange(1, false);

        //���e�������Ă��锻���true�ɂ���
        playerController[controllerNum - 1].HaveBombChange(bombNum,true);
        energyTank[controllerNum - 1].HaveCore();

        //���e���E�����Ƃ�
        if (hitBomb != null)
        {
            //�v���C���[�����e�������Ă��鎞�͂ق��̃I�u�W�F�N�g�ɓ�����Ȃ��悤�ɂ��邽�߂�
            //isKinematic��true�ɂ���
            bombScript[bombNum].IsKinematicTrue();

            //���e�̃R���C�_�[������
            bombScript[bombNum].ColliderEnabled(false);

            //�����̃^�C�}�[���J�n����
            bombScript[bombNum].IsTimerTrue();

            //���e�Ɏ������o�^
            explosionScript[bombNum].Owner(controllerNum);
        }

        //�����ȊO�̃v���C���[�̔��e�������Ă��锻���false�ɂ���
        for (int i = 0; i < 4; i++)
        {
            if (i != controllerNum - 1)
            {
                if (playerController[i].haveBomb[bombNum])
                {
                    energyTank[i].MissingCore();
                }
                playerController[i].HaveBombChange(bombNum, false);
            }         
        }
        /*
        //���e��D���������
        if (hitPlayer != null)
        {
            Debug.Log(hitPlayer);
            //���e�������Ă��锻���false�ɂ���
            hitPlayer.gameObject.GetComponent<PlayerController>().HaveBombChange(bombNum, false);
            hitPlayer.gameObject.GetComponent<EnergyTankManager>().MissingCore();

            //���e�����锻���true�ɂ���
            //hitPlayer.gameObject.GetComponent<PlayerController>().CanTakeTrue(bombNum);

            //���莋�_�̔��e��D��������������ɂ���
            //hitPlayer.gameObject.GetComponent<PlayerController>().HitPlayerChange(player);
        }
        */
        

    }

    //���e�����锻���ύX
    public void CanTakeChange(int i, bool canTakeChange)
    {
        canTake[i] = canTakeChange;
    }

    public void SetHitPlayer(GameObject gameObject)
    {
        hitPlayer = gameObject;
    }
}
