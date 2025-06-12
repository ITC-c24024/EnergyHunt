using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnergy : MonoBehaviour
{
    //エネルギー量
    public int energyAmount;

    [SerializeField]
    ExplosionScript[] explosionScript;

    [SerializeField, Header("プレイヤーのポジション")]
    GameObject[] playerObj;

    [SerializeField, Header("EnergyTankスクリプト")]
    EnergyTank[] energytankSC;

    [SerializeField, Header("DropEnergyTankスクリプト")]
    DropEnergyTank dropEnergyTank;

    [SerializeField, Header("スタートのポジション")]
    Vector3 startPos;

    [SerializeField, Header("モーショントレイル")]
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


                    //吸収オブジェクトのエネルギーを増やす
                    Debug.Log("吸収");
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
    //所持タンクオブジェクトからエネルギーを引継ぎ
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
        //自爆したとき
        if (player == posNum)
        {
            player = 4;
        }

        if (player != 4)
        {
            //攻撃を行った相手が死亡していたら
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


        
        //自爆したとき
        if (player == posNum)
        {
            player = 4;
        }

        if (player != 4)
        {
            //攻撃を行った相手が死亡していたら
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
