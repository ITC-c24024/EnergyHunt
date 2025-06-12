using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoneyTank : MonoBehaviour
{
    float honeyTimer;

    //はちみつが貯まる速さ
    float chargeSpeed = 10;

    //所持しているはちみつの量
    public int honeyAmount;

    //売るはちみつの量
    private int sellAmount = 0;

    //はちみつを入れるオブジェクト
    [SerializeField] GameObject honeytank;

    [SerializeField, Header("はちみつのマテリアル")]
    Material[] honeyMat;

    //はちみつを売るためのエリアオブジェクト
    [SerializeField] GameObject sellArea;

    //エリアオブジェクトのスクリプト
    SellArea sellAreaSC;

    //ハチの巣を所持しているかどうかの判定
    public bool charge;

    //はちみつを売れるかの判定
    public bool canSell = true;

    //はちみつがドロップしたとき用のオブジェクト
    [SerializeField] GameObject dropManager;

    //ドロップオブジェクトのスクリプト
    DropHoneyManager dropManagerSC;

    //プレイヤーコントローラースクリプト
    [SerializeField] Player playerSC;

    //ポーズボタンからの入力を受け取る変数
    private InputAction poseAction;

    //所持タンクの初期位置
    private Vector3 startPos;
    //初期スケール
    private Vector3 startScale;

    [SerializeField, Header("ポーズ画面")]
    GameObject poseImage;


    [SerializeField] Text honeyAmountText;

    [Header("所持量UIのスクリプト"), SerializeField]
    SpriteNum spriteNum;


    [Header("トラック移動スクリプト"), SerializeField]
    TrackScript trackScript;

    void Start()
    {
        //PlayerInputコンポーネントを取得
        var input = GetComponent<PlayerInput>();

        //取得したPlayerInputのActionMapを取得
        var actionMap = input.currentActionMap;

        //ActionMap内のポーズアクションを取得
        poseAction = actionMap["PutIn"];

        //スクリプト取得
        dropManagerSC = dropManager.GetComponent<DropHoneyManager>();
        sellAreaSC = sellArea.GetComponent<SellArea>();
        //スタート時のタンクの位置とスケールを取得
        startPos = honeytank.transform.localPosition;
        startScale = honeytank.transform.localScale;

    }

    private void Update()
    {
        //ポーズボタンが押されたかどうかをチェック
        var putIn = poseAction.triggered;

        if (putIn)
        {
            Time.timeScale = 0;

            poseImage.SetActive(true);
        }

        //ハチの巣所持時のはちみつ増加
        if (charge)
        {
            honeyTimer += Time.deltaTime * chargeSpeed;

            //1秒ごとにはちみつ増加
            if (honeyTimer >= 1)
            {
                honeyTimer = 0;

                GetHoney(1);
            }
        }
        else
        {
            honeyTimer = 0;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SellArea")
        {
            if (canSell && honeyAmount > 0)
            {
                //トラックの移動判定
                bool moveTrack = trackScript.isMove;

                //はちみつの売却が出来る状態だったら売却する
                if (!moveTrack && !playerSC.isStan)
                {
                    SellHoney();
                }
                StartCoroutine(CoolTime());
            }

        }
    }

    //売却時のクールタイム(2秒)
    IEnumerator CoolTime()
    {
        canSell = false;

        yield return new WaitForSeconds(2);

        canSell = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "SellArea")
        //{
        //    canSell = false;
        //}
    }


    //はちみつ増加とスケールアップ
    public void GetHoney(int amount)
    {
        if (honeyAmount == 0)
        {
            honeytank.SetActive(true);
        }

        honeyAmount += amount;

        //所持量UIの更新
        spriteNum.ChangeSprite(honeyAmount);

        //はちみつの量に応じてタンクのメーターをスケールアップし、メーターのポジションを調整
        honeytank.transform.localScale
            = new Vector3(startScale.x, honeyAmount * 0.045f, startScale.z);
        honeytank.transform.localPosition
            = new Vector3(startPos.x, startPos.y - (honeyAmount * 0.0215f), startPos.z);
    }

    //ハチの巣を持っているとき
    public void HaveBeehive()
    {
        charge = true;
    }


    //ハチの巣を手放したとき
    public void MissingBeehive()
    {
        charge = false;
    }

    //はちみつリセット
    public void HoneyReset()
    {
        //はちみつリセット
        honeyAmount = 0;

        //マテリアルをリセット
        honeytank.GetComponent<MeshRenderer>().material = honeyMat[0];

        //メーターのスケールとポジションをリセット
        honeytank.transform.localScale = startScale;
        honeytank.transform.localPosition = startPos;
    }

    //ハチの巣の攻撃に巻き込まれたときの所持エネルギードロップ
    public void DropHoney()
    {
        Debug.Log("drop");
        //ハチの巣所持状態の解除
        MissingBeehive();

        //所持しているはちみつが0でない場合、ドロップオブジェクトを落とす
        if (honeyAmount != 0)
        {
            //ドロップオブジェクトを自身の位置に移動してドロップ

            dropManager.transform.localScale = new Vector3(0.3f, honeyAmount * 0.024f, 0.3f);
            dropManager.transform.position = honeytank.transform.position;

            Rigidbody dropRb = dropManager.GetComponent<Rigidbody>();

            //ドロップオブジェクトにエネルギーを引継ぎ
            dropManagerSC.Drop(honeyAmount/3);

        }


        //はちみつ非表示
        honeytank.SetActive(false);

        //はちみつの量を調整
        honeyAmount = honeyAmount*(2/3);

        //所持量UIの更新
        spriteNum.ChangeSprite(honeyAmount);

        //マテリアルをリセット
        honeytank.GetComponent<MeshRenderer>().material = honeyMat[0];

        //メーターのスケールとポジションをリセット
        honeytank.transform.localScale = startScale;
        honeytank.transform.localPosition = startPos;

    }

    //はちみつを売却する
    public void SellHoney()
    {
        //はちみつが一定量を超える場合
        if (/*honeyAmount > 100*/honeyAmount == -1)
        {
            sellAmount = 100;

            //残ったはちみつの量を保存
            honeyAmount -= sellAmount;

            //はちみつの量に応じてタンクのメーターをスケールアップし、メーターのポジションを調整
            honeytank.transform.localScale
                = new Vector3(startScale.x, honeyAmount * 0.045f, startScale.z);
            honeytank.transform.localPosition
                = new Vector3(startPos.x, startPos.y - (honeyAmount * 0.0215f), startPos.z);
        }
        else
        {
            sellAmount = honeyAmount;

            //はちみつリセット
            honeyAmount = 0;

            //はちみつ非表示
            honeytank.SetActive(false);

            //マテリアルをリセット
            honeytank.GetComponent<MeshRenderer>().material = honeyMat[0];
        }
        //売る量をSellAreaスクリプトに渡す
        sellAreaSC.Sell(sellAmount, playerSC.controllerNum);


        //所持量UIの更新
        spriteNum.ChangeSprite(honeyAmount);



        //メーターのスケールとポジションをリセット
        //honeytank.transform.localScale = startScale;
        //honeytank.transform.localPosition = startPos;
    }
}
