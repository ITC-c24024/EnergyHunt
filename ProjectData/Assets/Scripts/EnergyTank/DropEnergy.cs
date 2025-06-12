using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnergy : MonoBehaviour
{
    //�G�l���M�[��
    public int energyAmount;

    [SerializeField]
    ExplosionScript[] explosionScript;

    [SerializeField, Header("�v���C���[�̃|�W�V����")]
    GameObject[] playerObj;

    [SerializeField, Header("EnergyTank�X�N���v�g")]
    EnergyTank[] energytankSC;

    [SerializeField, Header("DropEnergyTank�X�N���v�g")]
    DropEnergyTank dropEnergyTank;

    [SerializeField, Header("�X�^�[�g�̃|�W�V����")]
    Vector3 startPos;

    [SerializeField, Header("���[�V�����g���C��")]
    GameObject motionTrail;

    [SerializeField]
    int posNum;

    int player;

    bool target;

    [SerializeField] float speed = 1;
    private void Start()
    {

    }

    private void Update()
    {
        if (target)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, playerObj[player].transform.position, speed);
            if (transform.position == playerObj[player].transform.position)
            {
                if (player != 4)
                {
                    motionTrail.SetActive(false);

                    //if (!explosionScript[0].isSelf && !explosionScript[1].isSelf)
                    {
                        energytankSC[player].ChargeEnergy(energyAmount);
                    }

                    target = false;
                    PosReset();
                }
                else
                {
                    motionTrail.SetActive(false);


                    //�z���I�u�W�F�N�g�̃G�l���M�[�𑝂₷
                    Debug.Log("�z��");
                    //if (!explosionScript[0].isSelf || !explosionScript[1].isSelf)
                    {
                        dropEnergyTank.ScaleChange(energyAmount);
                    }

                    target = false;
                    PosReset();
                }

            }
        }
    }
    //�����^���N�I�u�W�F�N�g����G�l���M�[�����p��
    public void SetEnergyAmount(int amount)
    {
        Debug.Log(amount);


        energyAmount = amount;

    }

    public void PosReset()
    {
        transform.localPosition = startPos;
    }

    public IEnumerator SelectPos(int playerNum)
    {
        yield return new WaitForSeconds(0.2f);

        player = playerNum - 1;


        transform.position = playerObj[posNum].transform.position;


        /*
        //���������Ƃ�
        if (player == posNum)
        {
            player = 4;
        }

        if (player != 4)
        {
            //�U�����s�������肪���S���Ă�����
            var isDead = playerObj[player].GetComponent<PlayerController>().isDead;

            if (isDead)
            {
                player = 4;
            }
        }
        */

        target = true;

        motionTrail.SetActive(true);
    }
    /*
    public void SelectPos(int playerNum)
    {
        player = playerNum - 1;
        

        transform.position = playerObj[posNum].transform.position;


        
        //���������Ƃ�
        if (player == posNum)
        {
            player = 4;
        }

        if (player != 4)
        {
            //�U�����s�������肪���S���Ă�����
            var isDead = playerObj[player].GetComponent<PlayerController>().isDead;

            if (isDead)
            {
                player = 4;
            }
        }
        

        target = true;


        Debug.Log(player + ";" + posNum);

        motionTrail.SetActive(true);
    }*/
}
