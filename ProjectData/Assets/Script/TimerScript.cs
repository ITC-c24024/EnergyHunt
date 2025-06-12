using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] Ranking ranking;
    [SerializeField] Player[] playerSC;
    [SerializeField] HoneyTank[] honeyTankSC;
    [SerializeField] SellArea sellArea;

    public int timer = 180; // タイマー（秒単位）
    public Image hundredsImage; // 百の位
    public Image tensImage; // 十の位
    public Image onesImage; // 一の位
    public Image characterImage; // 「秒」などの記号
    public Sprite[] secondsSprites; // 0～9のスプライト
    public RectTransform onesRectTransform;
    public RectTransform tensRectTransform;
    public RectTransform characterTransform;

    private int previousTime;
    private bool isTransitioning = false; // 遷移中かどうかのフラグ

    void Start()
    {
        previousTime = (int)(Time.time); // 初期のタイムスタンプを設定
    }

    void Update()
    {
        int currentTime = (int)(Time.time);
        int deltaTime = currentTime - previousTime;

        if (deltaTime > 0)
        {
            timer -= deltaTime;
            previousTime = currentTime;
        }

        timer = Mathf.Max(0, timer); // 0未満にならないように制限

        int hundreds = timer / 100; // 百の位
        int tens = (timer / 10) % 10; // 十の位
        int ones = timer % 10; // 一の位

        // スプライトを更新（配列範囲内チェック）
        if (hundreds >= 0 && hundreds < secondsSprites.Length)
            hundredsImage.sprite = secondsSprites[hundreds];

        if (tens >= 0 && tens < secondsSprites.Length)
            tensImage.sprite = secondsSprites[tens];

        if (ones >= 0 && ones < secondsSprites.Length)
            onesImage.sprite = secondsSprites[ones];

        // 99秒以下になったら百の位を非表示 & 位置調整
        if (timer <= 99)
        {
            hundredsImage.enabled = false; // 百の位を消す
            tensImage.enabled = true; // 念のため十の位を表示

            // 位置を調整（百の位が消えた分、左に寄せる）
            //tensRectTransform.anchoredPosition = new Vector2(-869f, 486f);
            //onesRectTransform.anchoredPosition = new Vector2(-738f, 486f);
            //characterTransform.anchoredPosition = new Vector2(-607f, 486f);
        }
        else
        {
            hundredsImage.enabled = true;
        }

        // 残り9秒以下なら十の位を非表示 & さらに位置調整
        if (timer <= 9)
        {
            tensImage.enabled = false;
            //onesRectTransform.anchoredPosition = new Vector2(-869f, 486f);
            //characterTransform.anchoredPosition = new Vector2(-738f, 486f);
        }
        else
        {
            tensImage.enabled = true;
        }

        // タイマーが0になったらシーン遷移（3秒待つ）
        if (timer <= 0 && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(ResultSceneChange());

            //プレイヤーの操作、はちみつの処理を停止
            for (int i = 0; i < playerSC.Length; i++)
            {
                playerSC[i].enabled = false;
                honeyTankSC[i].enabled = false;
            }
            
            //トラックを動かなくする
            sellArea.enabled = false;

            Debug.Log("ゲーム終了");
        }
    }

    IEnumerator ResultSceneChange()
    {
        yield return new WaitForSeconds(4f); // 3秒待ってリザルトシーンへ  

        //順位並べ替え
        ranking.SortScore();

        //SceneManager.LoadScene("ResultScene");
    }
}
