using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private int[] playerScores; // 各プレイヤーのスコアを管理
    [SerializeField] private int[] playerRankings; // 順位を管理 (インスペクターで確認できるようにする)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerScores = new int[4]; // 4人分のスコア
        playerRankings = new int[4]; // 4人分の順位
    }

    // スコアを追加するメソッド（今は使わない）
    public void AddScore(int playerID, int scoreToAdd)
    {
        if (playerID >= 1 && playerID <= 4)
        {
            playerScores[playerID - 1] += scoreToAdd;
        }
    }

    // スコアを元に順位を計算
    public void CalculateRankings()
    {
        // プレイヤーIDとスコアをペアにしたリストを作成
        List<KeyValuePair<int, int>> playerRankingsList = new List<KeyValuePair<int, int>>();
        for (int i = 0; i < playerScores.Length; i++)
        {
            playerRankingsList.Add(new KeyValuePair<int, int>(i + 1, playerScores[i]));
        }

        // スコア降順でソート
        playerRankingsList.Sort((x, y) => y.Value.CompareTo(x.Value));

        // 順位を保存
        for (int i = 0; i < playerRankingsList.Count; i++)
        {
            playerRankings[i] = playerRankingsList[i].Key; // 順位をID順に格納
        }
    }

    // 順位をインスペクターで確認できるようにする
    public int[] GetRankings()
    {
        return playerRankings;
    }

    // スコアをインスペクターで確認するためのメソッド
    public int GetPlayerScore(int playerID)
    {
        if (playerID >= 1 && playerID <= 4)
        {
            return playerScores[playerID - 1];
        }
        else
        {
            return 0;
        }
    }
}
