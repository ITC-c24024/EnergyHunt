using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    [SerializeField] GameObject[] players;

    [SerializeField] SellArea sellAreas;

    [Header("リザルト画面"), SerializeField]
    GameObject resultPanel;

    [Header("リザルト背景"), SerializeField]
    GameObject resultBG;

    [Header("プレイヤーUI"),SerializeField]
    Image[] playerImage;

    [Header("所持金テキスト"), SerializeField]
    Text[] moneyText;

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {

            // シーン内のすべての PlayerScript を取得
            PlayerScript[] playerScripts = FindObjectsOfType<PlayerScript>();

            // pointが大きい順にソートして、GameObjectだけを抽出
            players = playerScripts
                .OrderByDescending(p => p.point)
                .Select(p => p.gameObject)
                .ToArray();

            Debug.Log(players);
        }
        */
    }

    public void SortScore()
    {
        //4人分の所持金を取得
        int[] score = new int[] {
                                     sellAreas.moneyAmount[0]
                                    ,sellAreas.moneyAmount[1]
                                    ,sellAreas.moneyAmount[2]
                                    ,sellAreas.moneyAmount[3]
                                };


        //所持金が多い順に並べ替え
        int[] orderedScore = score.OrderByDescending(score => score).ToArray();

        //Debug.Log("1位のスコア:" + orderedScore[0]);

        //リザルト表示
        resultBG.SetActive(true);
        resultPanel.SetActive(true);

        StartCoroutine(IncresseScore(score, orderedScore));
    }

    private IEnumerator IncresseScore(int[] score, int[] orderedScore)
    {
        float time = 0;

        while (time < 5)
        {
            time += Time.deltaTime;
            
            //所持金(=スコア)を表示
            for (int i = 0; i < playerImage.Length; i++)
            {
                float uiLength = Mathf.Lerp(0, score[i], time / 5);

                //所持金に応じて長さを調整
                playerImage[i].rectTransform.localScale = new Vector2(
                                                                         playerImage[i].rectTransform.localScale.x,
                                                                         uiLength / 100
                                                                     );
                //所持金に応じて位置を調整
                playerImage[i].rectTransform.localPosition = new Vector2(
                                                                            playerImage[i].rectTransform.localPosition.x,
                                                                            -360 + uiLength * 0.005f
                                                                        );
                //テキストの位置をUIの先端に移動
                moneyText[i].transform.localPosition = new Vector2(
                                                                      moneyText[i].transform.localPosition.x,
                                                                      -320 + uiLength * 0.01f
                                                                  );               
                moneyText[i].text = (int)uiLength + "円";

                //UIが画面上をはみ出さないようにする
                if (playerImage[i].transform.localPosition.y > 0)
                {
                    resultPanel.transform.localPosition = new Vector2(
                                                                         resultPanel.transform.localPosition.x,
                                                                         0 - playerImage[i].rectTransform.localPosition.y * 2
                                                                     );
                }
            }

            yield return null;
        }
        time = 0;

        //スコアが大きい順にイメージを格納
        Image[] orderedImage = playerImage.OrderByDescending(p => p.transform.localPosition.y).ToArray();

        //金額が大きい順にテキストを格納
        Text[] orderedText = moneyText.OrderByDescending(p => p.text).ToArray();

        //リザルト画面のy座標を保存
        float resultY = resultPanel.transform.localPosition.y;

        //UI縮小率を計算
        float rate = 80000f / orderedScore[0];
        
        while (time < 1.0f)
        {
            time += Time.deltaTime;

            //リザルト画面を中央に戻す
            float currentY = Mathf.Lerp(resultY, 0, time / 3.0f);

            resultPanel.transform.localPosition = new Vector2(
                resultPanel.transform.localPosition.x,
                currentY
                );

            //スコアイメージのスケール、位置を調整
            for (int i = 0; i < orderedImage.Length; i++)
            {
                //最大スコアから、縮小スコアまで
                float uiLength = Mathf.Lerp(orderedScore[i], orderedScore[i] * rate, time / 3.0f);

                //縮小率に合わせてスコアイメージを縮小
                orderedImage[i].transform.localScale = new Vector2(
                    orderedImage[i].transform.localScale.x,
                    uiLength / 100
                    );

                //位置を調整
                orderedImage[i].transform.localPosition = new Vector2(
                    orderedImage[i].transform.localPosition.x,
                    -360 + uiLength * 0.005f
                    );

                //金額テキストの位置を調整
                orderedText[i].transform.localPosition = new Vector2(
                    orderedText[i].transform.localPosition.x,
                    -320 + uiLength * 0.01f
                    );
            }

            yield return null;
        }
    }
}
