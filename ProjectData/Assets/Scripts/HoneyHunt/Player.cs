using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public TakeBeehive takeBeehive;

    [SerializeField, Header("HoneyTankスクリプト")]
    HoneyTank HoneyTank;

    [SerializeField, Header("SellAreaスクリプト")]
    SellArea SellArea;

    [SerializeField, Header("Beehiveスクリプト")]
    Beehive[] beehiveScript;

    [SerializeField, Header("ハチの巣オブジェクト")] GameObject[] beehive;
    [SerializeField, Header("放電エフェクト")] GameObject[] effect;
    [SerializeField, Header("ハチの巣を取れる範囲")] GameObject passRange;

    [SerializeField, Header("ボムをキャッチできる範囲")] GameObject catchRange;

    [SerializeField, Header("背負っているタンクオブジェクト")]
    GameObject playerTank;

    [Header("プレイヤーのモデル")]
    [SerializeField] GameObject body;
    //[SerializeField] GameObject head;
    [SerializeField] GameObject hand;

    [SerializeField, Header("プレイヤーのコライダー")]
    CapsuleCollider capsule;

    [SerializeField, Header("プレイヤーのRigidbody")]
    Rigidbody playerRb;

    [SerializeField, Header("ハチの巣のRigidbody")]
    Rigidbody[] beehiveRb;

    //プレイヤーのスタート位置
    private Vector3 startPos;
    //プレイヤーのスタートの向き
    private Quaternion startAngle;

    //プレイヤーの番号
    public int controllerNum;

    [SerializeField, Header("プレイヤーの移動スピード")]
    float speed;

    //プレイヤーの移動、投げるアクション
    public InputAction moveAction, throwAction;

    //投げた後、次にボタンを押せるまでの時間
    private float timer = 0;
    //入力クールタイム
    public float inputTimer;
    //入力クールタイムの判定
    public bool isInputTimer = false;

    //プレイヤーの向き
    private float angle;

    //スティックの角度を保存するためのfloat
    private float lastAngle;

    //ハチの巣を投げる力
    private float power = 1;

    //ハチの巣を持っている判定
    public bool[] haveBeehive;

    //死亡判定
    public bool isDead = true;

    //スタン判定
    public bool isStan = true;

    //点滅判定
    private bool isBlink = false;

    private Animator animator;

    void Start()
    {
        //ActionMapを取得
        var input = GetComponent<PlayerInput>();
        //var input = PlayerInput.GetPlayerByIndex(controllerNum - 1);
        var actionMap = input.currentActionMap;

        moveAction = actionMap["Move"];
        throwAction = actionMap["PickUp"];


        isStan = true;

        haveBeehive[0] = false;
        haveBeehive[1] = false;


        beehiveRb[0] = beehive[0].GetComponent<Rigidbody>();
        beehiveRb[1] = beehive[1].GetComponent<Rigidbody>();


        //最初のポジションを設定
        startPos = transform.position;
        startAngle = transform.localRotation;

        animator = GetComponent<Animator>();

        //スタートした後に動けるようにする
        Invoke("GameStart", 3.5f);
    }

    private void GameStart()
    {
        isStan = false;
    }

    void Update()
    {
        #region プレイヤーの操作
        if (!isStan)
        {
            //プレイヤーの移動
            Vector2 move = moveAction.ReadValue<Vector2>();

            if (move.x < 0.1 && move.x > -0.1)
            {
                move.x = 0;
            }
            if (move.y < 0.1 && move.y > -0.1)
            {
                move.y = 0;
            }

            transform.position += new Vector3(move.x, 0, move.y) * speed * Time.deltaTime;


            //プレイヤーの回転
            if (move.x > 0.1 || move.x < -0.1 || move.y > 0.1 || move.y < -0.1)
            {
                if (Time.timeScale == 1)
                {
                    angle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
                }

                if (angle < 0)
                {
                    angle += 360;
                }
                //スティックの角度を保存
                lastAngle = angle;

                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            else
            {
                //スティック入力が0の時保存していた角度を代入
                transform.rotation = Quaternion.Euler(0, lastAngle, 0);
            }
        }
        #endregion

        //ハチの巣を奪える範囲の追従
        passRange.transform.position = transform.position;
        passRange.transform.rotation = transform.rotation;

        //キャッチできる範囲の追従
        catchRange.transform.position = transform.position;
        catchRange.transform.rotation = transform.rotation;


        //投げる動作の判定
        var throwAct = throwAction.triggered;

        //ハチの巣を投げる
        if (throwAct && haveBeehive[0] && !isInputTimer && Time.timeScale == 1)
        {
            //すぐにキャッチできないようにする
            catchRange.GetComponent<Catch>().TimerStart();
            takeBeehive.IsTimerTrue();

            //ハチの巣を投げられた状態にする
            beehiveScript[0].SetIsThrow(true);

            ThrowBeehive(0);

        }
        if (throwAct && haveBeehive[1] && !isInputTimer && Time.timeScale == 1)
        {
            //すぐにキャッチできないようにする
            catchRange.GetComponent<Catch>().TimerStart();
            takeBeehive.IsTimerTrue();

            //ハチの巣を投げられた状態にする
            beehiveScript[1].SetIsThrow(true);

            ThrowBeehive(1);
        }


        //投げてすぐにキャッチできないようにするための入力クールタイム
        if (isInputTimer)
        {
            timer += Time.deltaTime;

            if (timer > 0.4)
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

    public void IsTimerTrue()
    {
        isInputTimer = true;
    }

    public void HaveBeehiveChange(int i, bool a)
    {
        haveBeehive[i] = a;
    }

    public void IsDeadTrue()
    {
        //誰かが勝利したとき、全員動けなくする
        isStan = true;
    }

    //爆弾を投げる関数
    private void ThrowBeehive(int bombNum)
    {
        //入力クールタイムスタート
        isInputTimer = true;

        //AddForceで投げるので、isKinematicをfalseする
        beehiveRb[bombNum].isKinematic = false;

        //コライダー復活
        beehiveScript[bombNum].ColliderEnabled(true);

        //爆弾を持っている判定をfalseにする
        HoneyTank.MissingBeehive();
        haveBeehive[bombNum] = false;


        //爆弾を飛ばす
        beehiveRb[bombNum].AddForce(beehive[bombNum].transform.forward * 500 * power);
    }

    public IEnumerator WinnerAnim(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        animator.SetBool("isWin", true);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //殴られたらスタン
        if (other.gameObject.tag == "Punch1" && !isStan)
        {
            if (haveBeehive[1])
            {
                //ボムを持っている判定をfalseにする
                haveBeehive[1] = false;
                HoneyTank.MissingBeehive();


                //beehiveScript[1].Explosion();
            }
            PlayerStan();
        }
        if (other.gameObject.tag == "Punch2" && !isStan) 
        {
            if (haveBeehive[0])
            {
                //ボムを持っている判定をfalseにする
                haveBeehive[0] = false;
                HoneyTank.MissingBeehive();


                //beehiveScript[0].Explosion();
            }
            PlayerStan();
        }
    }

    //プレイヤーをスタン状態にする
    private void PlayerStan()
    {
        isStan = true;

        haveBeehive[0] = false;
        haveBeehive[1] = false;


        HoneyTank.DropHoney();

        //タンクを非表示
        playerTank.SetActive(false);

        //爆弾を取れなくする
        takeBeehive.CanTakeChange(0, false);
        takeBeehive.CanTakeChange(1, false);        


        //プレイヤーを点滅させる
        gameObject.GetComponent<CharacterBlinkingScript>().BlinkStart(true);


        //スタン解除まで待つ
        Invoke("CancellStan", 2.0f);
    }

    //スタン解除
    private void CancellStan()
    {
        isStan = false;   

        //点滅終了
        gameObject.GetComponent<CharacterBlinkingScript>().BlinkStart(false);

        //プレイヤーのモデルを表示
        body.SetActive(true);
        hand.SetActive(true);

        //タンクを表示
        playerTank.SetActive(true);
    }

    public void CanTakeTrue(int i)
    {
        takeBeehive.CanTakeChange(i, true);
    }
}
