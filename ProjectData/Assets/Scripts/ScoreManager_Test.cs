using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager_Test : MonoBehaviour
{
    float[] scoreTimer = new float[4];
    public int[] playerScore = new int[4];
    public bool[] bomer = new bool[4];
    public Text[] scoreText;
    public RectTransform[] playerImages; // �v���C���[�̃C���[�W�iRectTransform�j

    void Start()
    {

    }

    void Update()
    {
        // ���e�������̃X�R�A��������
        UpdatePlayerScore(0); // 1P
        UpdatePlayerScore(1); // 2P
        UpdatePlayerScore(2); // 3P
        UpdatePlayerScore(3); // 4P

        // �X�R�A�Ɋ�Â��ăC���[�W�̈ʒu���X�V
        UpdatePlayerImagePositions();
    }

    void UpdatePlayerScore(int playerNum)
    {
        if (bomer[playerNum])
        {
            scoreTimer[playerNum] += Time.deltaTime;
            if (scoreTimer[playerNum] >= 1)
            {
                scoreTimer[playerNum] = 0;

                playerScore[playerNum]++;
                scoreText[playerNum].text = "" + playerScore[playerNum];
            }
        }
        else
        {
            scoreTimer[playerNum] = 0;
        }
    }

    void UpdatePlayerImagePositions()
    {
        // �X�R�A�ƃv���C���[�̃C���f�b�N�X���y�A�ɂ��ă\�[�g
        List<(int score, int playerIndex)> scoreRankings = new List<(int score, int playerIndex)>();
        for (int i = 0; i < playerScore.Length; i++)
        {
            scoreRankings.Add((playerScore[i], i));
        }

        // �X�R�A�̍~���Ƀ\�[�g
        scoreRankings.Sort((a, b) => b.score.CompareTo(a.score));

        // ���ʂɉ����ăC���[�W�̈ʒu���X�V
        int[] yPositions = { -100, -300, -500, -700 }; // �e���ʂ�Y���W
        for (int rank = 0; rank < scoreRankings.Count; rank++)
        {
            int playerIndex = scoreRankings[rank].playerIndex;
            RectTransform playerImage = playerImages[playerIndex];

            // X��Y�̍��W���X�V
            playerImage.anchoredPosition = new Vector2(-120, yPositions[rank]);
        }
    }

    // ���v���C���[�ɔ��e�̔����𓖂Ă��Ƃ��̃X�R�A����
    public void IncreaseByBomb(int playerNum)
    {
        playerScore[playerNum] += 10;
        scoreText[playerNum].text = "" + playerScore[playerNum];
    }

    // ���e�̔����Ɋ������܂ꂽ�Ƃ��̃X�R�A����
    public void DecreaseByBomb(int playerNum)
    {
        playerScore[playerNum] -= playerScore[playerNum] / 3;
        scoreText[playerNum].text = "" + playerScore[playerNum];
    }

    // ���e�������Ă���Ƃ�
    public void HaveBomb(int playerNum)
    {
        bomer[playerNum] = true;
    }

    // ���e����������Ƃ�
    public void MissingBomb(int playerNum)
    {
        bomer[playerNum] = false;
    }
}
