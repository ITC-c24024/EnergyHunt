using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHoney : MonoBehaviour
{
    //はちみつの量
    public int dropHoneyAmount;

    //オブジェクトナンバー
    int objNum;

    [SerializeField, Header("スタートのポジション")]
    Vector3 startPos;

    //ドロップオブジェクトに対応するプレイヤーを紐づけ
    [SerializeField] GameObject player;

    //拾ったプレイヤーを紐づけ
    [SerializeField] GameObject pickPlayer;

    //ドロップにかかる時間
    [SerializeField] float dropTime = 1f;

    //ドロップオブジェクトがリセットされるまでの時間
    [SerializeField] int deleteTime;

    //取得可能な状態かどうかの判定
    [SerializeField] bool drop;

    //ドロップ中かどうかの判定
    [SerializeField] bool start;

    //ドロップ、取得どちらなのかの判定
    bool isDrop;

    //現在地点
    Vector3 currentPos;

    //ターゲット地点
    Vector3 targetPos;

    //ドロップ中の時間管理
    float currentTime = 0f;

    //Y軸の動きを制御する
    AnimationCurve animCurveY;

    //加速度を調整する
    public AnimationCurve animCurveDrop;
    public AnimationCurve animCurvePick;

    //ドロップマネージャースクリプト
    [SerializeField] DropHoneyManager dropManagerSC;

    //点滅処理のスクリプト
    Blink blinkSC;

    private void Start()
    {
        blinkSC = this.gameObject.GetComponent<Blink>();
    }
    private void Update()
    {
        //ドロップ時の挙動処理
        if (start)
        {
            //拾う際のターゲットをプレイヤーにする
            if (!isDrop)
            {
                targetPos = pickPlayer.transform.localPosition;
            }

            var pos = new Vector3();

            pos.x = currentPos.x + (targetPos.x - currentPos.x) * currentTime / dropTime;
            pos.z = currentPos.z + (targetPos.z - currentPos.z) * currentTime / dropTime;

            pos.y = animCurveY.Evaluate(currentTime);

            transform.localPosition = pos;

            //加速度をアニメーションカーブで調整
            if (isDrop)
            {
                currentTime += Time.deltaTime * animCurveDrop.Evaluate(currentTime);
            }
            else
            {
                currentTime += Time.deltaTime * animCurvePick.Evaluate(currentTime);
            }

            //完全に落ちていなくても取得できるようにする
            if (currentTime >= dropTime / 2 && isDrop)
            {
                drop = true;
            }

            if (currentTime >= dropTime)
            {
                start = false;
                currentTime = 0f;
            }
        }
    }

    public void SetNum(int num)
    {
        objNum = num;
    }

    //所持タンクオブジェクトからはちみつを引継ぎ
    public IEnumerator SetHoneyAmount(int amount)
    {
        dropTime = 1f;

        dropHoneyAmount = amount;

        //プレイヤーのポジションでドロップ
        currentPos = player.transform.localPosition;

        //ドロップ地点から半径3の円周上のランダムな地点にターゲットを指定
        var angle = Random.Range(0, 360);
        var rad = angle * Mathf.Deg2Rad;
        var px = Mathf.Cos(rad) * 3f + currentPos.x;
        var pz = Mathf.Sin(rad) * 3f + currentPos.z;

        //ターゲット地点がステージ外にならないようにする
        if (px <= -9f)//X軸
        {
            px = -9f;
        }
        else if (px >= 9f)
        {
            px = 9f;
        }

        if (pz <= -5.5f)//Z軸
        {
            pz = -5.5f;
        }
        else if (pz >= 3.2f)
        {
            pz = 3.2f;
        }
        targetPos = new Vector3(px, currentPos.y, pz);


        //山なりにドロップするようにする
        animCurveY = new AnimationCurve(
            new Keyframe(0, currentPos.y, 0, 10),
            new Keyframe(dropTime, targetPos.y, -10, 0)
            );


        start = true;
        isDrop = true;

        //ドロップ後五秒待って点滅させる
        yield return new WaitForSeconds(dropTime + 5f);
        blinkSC.BlinkStart(5, 0.3f, 0.1f);

        //点滅処理が終わると同時にリセットさせる
        yield return new WaitForSeconds(5);

        if (isDrop)
        {
            PosReset();
            drop = false;
            dropManagerSC.AddDrop(objNum);
        }
        
    }


    private void OnTriggerStay(Collider other)
    {
        //プレイヤーが触れたときの取得判定
        if (drop)
        {
            if (other.gameObject.tag.StartsWith("P") && isDrop)
            {
                pickPlayer = other.gameObject;
                StartCoroutine(PickUp(other.gameObject));
            }
        }
    }

    public IEnumerator PickUp(GameObject player)
    {
        isDrop = false;

        dropTime = 0.5f;//拾う時間

        //拾った際のY軸の動き
        animCurveY = new AnimationCurve(
            new Keyframe(0, currentPos.y, 0, 10),
            new Keyframe(dropTime, targetPos.y + 0.5f, -10, 0)
            );

        currentPos = this.transform.localPosition;//現在の位置

        start = true;

        //拾われるのを待ってから加算
        yield return new WaitForSeconds(dropTime);
        var honeyTankSC = player.GetComponent<HoneyTank>();
        honeyTankSC.GetHoney(dropHoneyAmount);

        drop = false;

        PosReset();

        dropManagerSC.AddDrop(objNum);//拾われた際のリスト追加
    }

    //取得後のポジションリセット
    public void PosReset()
    {
        this.transform.localPosition = startPos;
    }
}
