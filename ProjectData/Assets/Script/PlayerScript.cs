using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] int playerID;
    [SerializeField] float speed = 3f;
    bool isStan = false;
    public float stanTimer = 0f;
    public HoneyScript honeyScript;

    private bool hasPressed = false; // キーを押したかどうかをチェック

    public int point = 0;

    void Update()
    {
        if (!isStan)
        {
            //PlayerMove();
        }

        if (isStan)
        {
            stanTimer += Time.deltaTime;
            if (stanTimer > 3)
            {
                isStan = false;
                stanTimer = 0f;
            }
        }

        // スコア増加部分をコメントアウト
        // ScoreCount();

        // 順位表示の処理
        DisplayRankingsOnKeyPress();
    }

    /*void PlayerMove()
    {
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * speed * Time.deltaTime);
    }*/

    // スペースキーで順位を表示するメソッド
    void DisplayRankingsOnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // スペースキーが押された時
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.CalculateRankings(); // 順位計算
                int[] rankings = ScoreManager.Instance.GetRankings(); // 順位を取得

                // 取得した順位を表示（デバッグログで表示）
                for (int i = 0; i < rankings.Length; i++)
                {
                    Debug.Log("Rank " + (i + 1) + ": Player " + rankings[i]);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Honey")
        {
            if (honeyScript != null && honeyScript.isThrow)
            {
                isStan = true;
            }
        }
    }
}
