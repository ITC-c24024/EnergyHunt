using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private int[] playerScores; // �e�v���C���[�̃X�R�A���Ǘ�
    [SerializeField] private int[] playerRankings; // ���ʂ��Ǘ� (�C���X�y�N�^�[�Ŋm�F�ł���悤�ɂ���)

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

        playerScores = new int[4]; // 4�l���̃X�R�A
        playerRankings = new int[4]; // 4�l���̏���
    }

    // �X�R�A��ǉ����郁�\�b�h�i���͎g��Ȃ��j
    public void AddScore(int playerID, int scoreToAdd)
    {
        if (playerID >= 1 && playerID <= 4)
        {
            playerScores[playerID - 1] += scoreToAdd;
        }
    }

    // �X�R�A�����ɏ��ʂ��v�Z
    public void CalculateRankings()
    {
        // �v���C���[ID�ƃX�R�A���y�A�ɂ������X�g���쐬
        List<KeyValuePair<int, int>> playerRankingsList = new List<KeyValuePair<int, int>>();
        for (int i = 0; i < playerScores.Length; i++)
        {
            playerRankingsList.Add(new KeyValuePair<int, int>(i + 1, playerScores[i]));
        }

        // �X�R�A�~���Ń\�[�g
        playerRankingsList.Sort((x, y) => y.Value.CompareTo(x.Value));

        // ���ʂ�ۑ�
        for (int i = 0; i < playerRankingsList.Count; i++)
        {
            playerRankings[i] = playerRankingsList[i].Key; // ���ʂ�ID���Ɋi�[
        }
    }

    // ���ʂ��C���X�y�N�^�[�Ŋm�F�ł���悤�ɂ���
    public int[] GetRankings()
    {
        return playerRankings;
    }

    // �X�R�A���C���X�y�N�^�[�Ŋm�F���邽�߂̃��\�b�h
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
