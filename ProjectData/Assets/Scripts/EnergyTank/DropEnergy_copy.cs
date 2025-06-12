using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropEnergy_copy : MonoBehaviour
{
    //�G�l���M�[��
    public int energyAmount;

    [SerializeField, Header("�v���C���[�̃|�W�V����")]
    Vector3[] playerPos;

    [SerializeField, Header("EnergyTank�X�N���v�g")]
    EnergyTank_copy[] energytankSC;

    [SerializeField, Header("�X�^�[�g�̃|�W�V����")]
    Vector3 startPos;

    int player;

    bool target;

    [SerializeField] float speed = 1;
    private void Start()
    {
        
    }
    public void Update()
    {
        if (target)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos[player], speed);
            if(transform.position == playerPos[player])
            {
                energytankSC[player].ChargeEnergy(energyAmount);
                target = false;
                PosReset();
            }
        }
    }
    //�����^���N�I�u�W�F�N�g����G�l���M�[�����p��
    public void SetEnergyAmount(int amount)
    {
        energyAmount = amount;
    }

    public void PosReset()
    {
        transform.localPosition = startPos;
    }

    public void SelectPos(int playerNum)
    {
        player = playerNum;
        target = true;
    }
    
}
