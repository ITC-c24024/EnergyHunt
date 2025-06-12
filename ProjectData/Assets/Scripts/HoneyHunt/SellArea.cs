using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellArea : MonoBehaviour
{
    //各プレイヤーの所持金の量
    public int[] moneyAmount;

    //各プレイヤーオブジェクト
    [SerializeField] GameObject[] player = new GameObject[4];

    //売却時の表示テキスト
    [SerializeField] TextMeshProUGUI[] sellText = new TextMeshProUGUI[4];

    //1g当たりのはちみつの価格
    [SerializeField] int price = 100;

    //所持金表示UI
    [SerializeField] SpriteNum[] spriteNum = new SpriteNum[4];

    [Header("トラック移動スクリプト"), SerializeField]
    TrackScript trackScript;

    public void Sell(int honeyAmount, int playerNum)
    {
        //はちみつ100gごとの追加ボーナス
        var amountBonus = 0;

        if (honeyAmount >= 100)
        {
            amountBonus = honeyAmount / 100;
        }

        //はちみつの量と価格を加味して最終的な取引価格を設定
        float transactionPrice = honeyAmount * price * (1 + amountBonus / 5);

        //プレイヤーの所持金に加算
        moneyAmount[playerNum - 1] += (int)transactionPrice;

        //プレイヤーの場所にテキストを表示
        StartCoroutine(DisplayText((int)transactionPrice, playerNum));

        //UIの更新
        spriteNum[playerNum - 1].ChangeSprite(moneyAmount[playerNum - 1]);

    }

    //プレイヤーの頭上に利益を表示
    IEnumerator DisplayText(int honeyPrice, int playerNum)
    {
        sellText[playerNum - 1].text = $@"+¥{honeyPrice}";//テキストを変更

        var count = 0f;//秒数のカウント

        var currentPos = sellText[playerNum - 1].rectTransform.position;//元のポジション

        var pivotY = sellText[playerNum - 1].GetComponent<RectTransform>().pivot;//ピボット

        var alpha = sellText[playerNum - 1].color;//カラー

        while(count < 2)//2秒間
        {
            //プレイヤーの頭上に固定しながらピボットをずらして上昇させる
            sellText[playerNum - 1].rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player[playerNum - 1].transform.position);
            sellText[playerNum - 1].rectTransform.pivot -= new Vector2(0, 1)*Time.deltaTime;

            //残り1秒でフェードアウト
            if (count >= 1)
            {
                alpha.a -= Time.deltaTime;
                sellText[playerNum - 1].color = alpha;
            }
            
            count += Time.deltaTime;

            yield return null;//1フレーム待つ
        }

        //ポジションとピボットをリセット
        sellText[playerNum - 1].rectTransform.position = currentPos;
        sellText[playerNum - 1].rectTransform.pivot = pivotY;

        //透明度をリセット
        alpha.a = 1;
        sellText[playerNum - 1].color = alpha;

    }

}
