using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeBeehive : MonoBehaviour
{
    [SerializeField, Header("HoneyTankスクリプト")]
    HoneyTank[] honeyTankSC;

    [SerializeField, Header("Playerスクリプト")]
    Player[] playerSC;

    [SerializeField, Header("ハチの巣のスクリプト")]
    Beehive[] beehiveSC;

    [SerializeField, Header("爆発のスクリプト")]
    BeehiveAttack[] beehiveAttackSC;

    [SerializeField, Header("ハチの巣オブジェクト")]
    GameObject[] beehiveObj;

    [SerializeField, Header("プレイヤーオブジェクト")]
    GameObject playerObj;

    //爆弾を取れる範囲にいる爆弾を持っているプレイヤー
    private GameObject hitPlayer;
    //爆弾を取れる範囲にある爆弾
    private GameObject hitBomb;

    //プレイヤーの番号
    public int controllerNum;

    //爆弾を取れるかの判定
    public bool[] canTake;

    //爆弾を取るアクション
    private InputAction pickUpAction;

    [SerializeField, Header("プレイヤーのPlayerInput")]
    PlayerInput input;

    //投げた後、次にボタンを押せるまでの時間
    private float timer = 0;
    //入力クールタイムの判定
    private bool isInputTimer = false;

    void Start()
    {
        //プレイヤーのアクションマップを取得
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
            //当たった相手が自分ではない場合
            if (other.gameObject.tag != $"P{controllerNum}")
            {
                //当たった相手が爆弾を持っている場合
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
                //Exitした相手がボムを持っていたら、canTakeをfalseにする
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
        //爆弾を持っている場合、プレイヤーに追従
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


        //canTakeが2つtrueのとき、片方だけtrueにする
        if (canTake[0] && canTake[1])
        {
            canTake[0] = false;
        }


        //取るボタンを押しているかの判定
        var pickUp = pickUpAction.triggered;

        //爆弾を奪う
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

    //爆弾を奪う関数
    private void TakeBombFunction(int bombNum)
    {
        //爆弾を取れないようにする
        CanTakeChange(0, false);
        CanTakeChange(1, false);

        //爆弾を持っている判定をtrueにする
        playerSC[controllerNum - 1].HaveBeehiveChange(bombNum, true);
        honeyTankSC[controllerNum - 1].HaveBeehive();


        //コアの投げられている判定をfalseにする
        beehiveSC[bombNum].SetIsThrow(false);

        //プレイヤーが爆弾を持っている時はほかのオブジェクトに当たらないようにするために
        //isKinematicをtrueにする
        beehiveSC[bombNum].IsKinematicTrue();

        //爆弾のコライダーを消す
        beehiveSC[bombNum].ColliderEnabled(false);

        //タイマーがスタートしていなかったら
        if (!beehiveSC[bombNum].isTimer)
        {
            //爆発のタイマーを開始する
            beehiveSC[bombNum].IsTimerTrue();
        }

        //自分以外のプレイヤーの爆弾を持っている判定をfalseにする
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

    //爆弾を取れる判定を変更
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
