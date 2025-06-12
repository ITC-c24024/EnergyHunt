using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeBeehive : MonoBehaviour
{
    [SerializeField, Header("HoneyTank�X�N���v�g")]
    HoneyTank[] honeyTankSC;

    [SerializeField, Header("Player�X�N���v�g")]
    Player[] playerSC;

    [SerializeField, Header("�n�`�̑��̃X�N���v�g")]
    Beehive[] beehiveSC;

    [SerializeField, Header("�����̃X�N���v�g")]
    BeehiveAttack[] beehiveAttackSC;

    [SerializeField, Header("�n�`�̑��I�u�W�F�N�g")]
    GameObject[] beehiveObj;

    [SerializeField, Header("�v���C���[�I�u�W�F�N�g")]
    GameObject playerObj;

    //���e������͈͂ɂ��锚�e�������Ă���v���C���[
    private GameObject hitPlayer;
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
        if (other.gameObject.tag == "Bomb0" && !playerSC[controllerNum - 1].haveBeehive[0] && !playerSC[controllerNum - 1].haveBeehive[1])
        {
            canTake[0] = true;

            hitBomb = other.gameObject;
        }
        if (other.gameObject.tag == "Bomb1" && !playerSC[controllerNum - 1].haveBeehive[0] && !playerSC[controllerNum - 1].haveBeehive[1])
        {
            canTake[1] = true;

            hitBomb = other.gameObject;
        }

        if (!playerSC[controllerNum - 1].haveBeehive[0] && !playerSC[controllerNum - 1].haveBeehive[1]
            && (other.gameObject.tag == "P1" || other.gameObject.tag == "P2" || other.gameObject.tag == "P3" || other.gameObject.tag == "P4"))
        {
            //�����������肪�����ł͂Ȃ��ꍇ
            if (other.gameObject.tag != $"P{controllerNum}")
            {
                //�����������肪���e�������Ă���ꍇ
                if (other.gameObject.GetComponent<Player>().haveBeehive[0])
                {
                    hitPlayer = other.gameObject;
                    canTake[0] = true;
                }
                else if (other.gameObject.GetComponent<Player>().haveBeehive[1])
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

        if (!playerSC[controllerNum - 1].haveBeehive[0] && !playerSC[controllerNum - 1].haveBeehive[1] &&
             (other.gameObject.tag == "P1" || other.gameObject.tag == "P2" || other.gameObject.tag == "P3" || other.gameObject.tag == "P4"))
        {
            if (other.gameObject.tag != $"P{controllerNum}")
            {
                //Exit�������肪�{���������Ă�����AcanTake��false�ɂ���
                if (other.gameObject.GetComponent<Player>().haveBeehive[0])
                {
                    hitPlayer = null;

                    canTake[0] = false;
                }

                if (other.gameObject.GetComponent<Player>().haveBeehive[1])
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
        if (playerSC[controllerNum - 1].haveBeehive[0])
        {
            beehiveObj[0].transform.position = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z);
            beehiveObj[0].transform.rotation = transform.rotation;
        }
        else if (playerSC[controllerNum - 1].haveBeehive[1])
        {
            beehiveObj[1].transform.position = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z);
            beehiveObj[1].transform.rotation = transform.rotation;
        }


        //canTake��2��true�̂Ƃ��A�Е�����true�ɂ���
        if (canTake[0] && canTake[1])
        {
            canTake[0] = false;
        }


        //���{�^���������Ă��邩�̔���
        var pickUp = pickUpAction.triggered;

        //���e��D��
        if (canTake[0] && pickUp && !playerSC[controllerNum - 1].isStan
            && !beehiveSC[0].isExplosion && !beehiveSC[0].outBody && !isInputTimer)
        {
            playerSC[controllerNum - 1].IsTimerTrue();

            TakeBombFunction(0);
        }
        if (canTake[1] && pickUp && !playerSC[controllerNum - 1].isStan
            && !beehiveSC[1].isExplosion && !beehiveSC[1].outBody && !isInputTimer)
        {
            playerSC[controllerNum - 1].IsTimerTrue();

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
        //���e�����Ȃ��悤�ɂ���
        CanTakeChange(0, false);
        CanTakeChange(1, false);

        //���e�������Ă��锻���true�ɂ���
        playerSC[controllerNum - 1].HaveBeehiveChange(bombNum, true);
        honeyTankSC[controllerNum - 1].HaveBeehive();


        //�R�A�̓������Ă��锻���false�ɂ���
        beehiveSC[bombNum].SetIsThrow(false);

        //�v���C���[�����e�������Ă��鎞�͂ق��̃I�u�W�F�N�g�ɓ�����Ȃ��悤�ɂ��邽�߂�
        //isKinematic��true�ɂ���
        beehiveSC[bombNum].IsKinematicTrue();

        //���e�̃R���C�_�[������
        beehiveSC[bombNum].ColliderEnabled(false);

        //�^�C�}�[���X�^�[�g���Ă��Ȃ�������
        if (!beehiveSC[bombNum].isTimer)
        {
            //�����̃^�C�}�[���J�n����
            beehiveSC[bombNum].IsTimerTrue();
        }

        //�����ȊO�̃v���C���[�̔��e�������Ă��锻���false�ɂ���
        for (int i = 0; i < 4; i++)
        {
            if (i != controllerNum - 1)
            {
                if (playerSC[i].haveBeehive[bombNum])
                {
                    honeyTankSC[i].MissingBeehive();
                    playerSC[i].HaveBeehiveChange(bombNum, false);
                }

            }

            var takeBeehive = playerSC[i].GetComponent<Player>().takeBeehive;
            takeBeehive.CanTakeChange(0, false);
            takeBeehive.CanTakeChange(1, false);
        }
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

    public void IsTimerTrue()
    {
        isInputTimer = true;
    }
}
