using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public RectTransform[] playerImages;
    [SerializeField] Slider[] sliders;

    public Text[] scoreText;
    [SerializeField] int goalScore;

    [SerializeField] GameObject[] player;

    [SerializeField] GameObject[] playerTank;

    float[] scoreTimer = new float[4];
    public int[] playerScore = new int[4];
    public bool[] bomer = new bool[4];

    //�^���N��I�Ԃ��߂̐�
    private int p1TankNum;

    void Start()
    {
        sliders[0].value = playerScore[0];
        sliders[1].value = playerScore[1];
        sliders[2].value = playerScore[2];
        sliders[3].value = playerScore[3];
    }

    void Update()
    {
        //���e�������̃X�R�A����
        if (bomer[0] && playerScore[0] < 20)//1P
        {
            scoreTimer[0] += Time.deltaTime;
            if (scoreTimer[0] >= 1)
            {
                scoreTimer[0] = 0;

                playerScore[0]++;
                scoreText[0].text = "" + playerScore[0];
                sliders[0].value = playerScore[0];             

                //�X�R�A�ɉ����ă^���N�̃��[�^�[���X�P�[���A�b�v���A�|�W�V�����𒲐�
                playerTank[p1TankNum].transform.localScale = new Vector3(
                    playerTank[p1TankNum].transform.localScale.x, playerScore[0] * 0.012f, playerTank[p1TankNum].transform.localScale.z);

                playerTank[p1TankNum].transform.localPosition = new Vector3(
                    playerTank[p1TankNum].transform.localPosition.x, -0.04f + (playerScore[0] * 0.012f), playerTank[p1TankNum].transform.localPosition.z);

                if (playerScore[0] == 20 && p1TankNum == 0)
                {
                    playerScore[0] = 0;
                    p1TankNum = 1;
                    playerTank[1].SetActive(true);
                }
                else if (playerScore[0] == 20 && p1TankNum == 1)
                {
                    playerScore[0] = 0;
                    p1TankNum = 2;
                    playerTank[2].SetActive(true);
                }


                UpdatePlayerImagePositions();

                if (playerScore[0] >= goalScore)
                {
                    GameOver();
                }
            }
        }
        else
        {
            scoreTimer[0] = 0;
        }
        //�i��������傫���Ȃ�
        if (playerScore[0] >= goalScore / 2)
        {
            //player[0].transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        }

        if (bomer[1])//2P
        {
            scoreTimer[1] += Time.deltaTime;
            if (scoreTimer[1] >= 1)
            {
                scoreTimer[1] = 0;

                playerScore[1]++;
                scoreText[1].text = "" + playerScore[1];
                sliders[1].value = playerScore[1];

                UpdatePlayerImagePositions();

                if (playerScore[1] >= goalScore)
                {
                    GameOver();
                }
            }
        }
        else
        {
            scoreTimer[1] = 0;
        }

        if (bomer[2])//3P
        {
            scoreTimer[2] += Time.deltaTime;
            if (scoreTimer[2] >= 1)
            {
                scoreTimer[2] = 0;

                playerScore[2]++;
                scoreText[2].text = "" + playerScore[2];
                sliders[2].value = playerScore[2];

                UpdatePlayerImagePositions();

                if (playerScore[2] >= goalScore)
                {
                    GameOver();
                }
            }
        }
        else
        {
            scoreTimer[2] = 0;
        }

        if (bomer[3])//4P
        {
            scoreTimer[3] += Time.deltaTime;
            if (scoreTimer[3] >= 1)
            {
                scoreTimer[3] = 0;

                playerScore[3]++;
                scoreText[3].text = "" + playerScore[3];
                sliders[3].value = playerScore[3];

                UpdatePlayerImagePositions();

                if (playerScore[3] >= goalScore)
                {
                    GameOver();
                }
            }
        }
        else
        {
            scoreTimer[3] = 0;
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

    //���v���C���[�ɔ��e�̔����𓖂Ă��Ƃ��̃X�R�A����
    public void IncreaseByBomb(int playerNum)
    {
        Debug.Log(playerNum);   
        
        playerScore[playerNum] += 10;
        scoreText[playerNum].text = "" + playerScore[playerNum];
        sliders[playerNum].value = playerScore[playerNum];

        //�X�R�A�ɉ����ă^���N�̃��[�^�[���X�P�[���A�b�v���A�|�W�V�����𒲐�
        playerTank[playerNum].transform.localScale 
            = new Vector3(playerTank[playerNum].transform.localScale.x, playerScore[0] * 0.05f, playerTank[playerNum].transform.localScale.z);
        
        playerTank[playerNum].transform.localPosition
            = new Vector3(playerTank[playerNum].transform.localPosition.x, -1.0f + (playerScore[0] * 0.05f), playerTank[playerNum].transform.localPosition.z);


        UpdatePlayerImagePositions();

        if (playerScore[playerNum] >= goalScore)
        {
            GameOver();
        }
    }
    //���e�̔����Ɋ������܂ꂽ�Ƃ��̌���
    public void DecreaseByBomb(int playerNum)
    {
        playerScore[playerNum] = 0;
        scoreText[playerNum].text = "" + playerScore[playerNum];
        sliders[playerNum].value = playerScore[playerNum];

        //�v���C���[�̔ԍ��ɑΉ�����^���N�̔ԍ���I��
        int tankNum = 0;
        switch (playerNum)
        {
            case 0:
                tankNum = p1TankNum;
                break;
            case 1:
                tankNum = p1TankNum;
                break;
            case 2:
                tankNum = p1TankNum;
                break;
            case 3:
                tankNum = p1TankNum;
                break;
        }

        int n = tankNum + 1;
        //�Q�[�W�����Z�b�g
        for (int i = 0; i < n; i++)
        {
            tankNum -= i;
            Debug.Log(tankNum);

            playerTank[tankNum].transform.localScale = new Vector3(
                playerTank[tankNum].transform.localScale.x, 0, playerTank[tankNum].transform.localScale.z);

            playerTank[tankNum].transform.localPosition = new Vector3(
                playerTank[tankNum].transform.localPosition.x, -0.04f, playerTank[tankNum].transform.localPosition.z);
        }
        


        UpdatePlayerImagePositions();
    } 
    /*
    //�������̃X�R�A����
    public void DecreaseByDropDown(int playerNum)
    {
        playerScore[playerNum] -= playerScore[playerNum] / 2;
        scoreText[playerNum].text = "" + playerScore[playerNum];
        sliders[playerNum].value = playerScore[playerNum];

        //�v���C���[�̔ԍ��ɑΉ�����^���N�̔ԍ���I��
        int tankNum = 0;
        switch (playerNum)
        {
            case 1:
                tankNum = p1TankNum;
                break;
            case 2:
                tankNum = p1TankNum;
                break;
            case 3:
                tankNum = p1TankNum;
                break;
            case 4:
                tankNum = p1TankNum;
                break;
        }
        //�X�R�A�ɉ����ă^���N�̃��[�^�[���X�P�[���A�b�v���A�|�W�V�����𒲐�
        playerTank[tankNum].transform.localScale = new Vector3(
            playerTank[tankNum].transform.localScale.x, playerScore[playerNum] * 0.012f, playerTank[tankNum].transform.localScale.z);

        playerTank[tankNum].transform.localPosition = new Vector3(
            playerTank[tankNum].transform.localPosition.x, -0.04f + (playerScore[playerNum] * 0.012f), playerTank[tankNum].transform.localPosition.z);
    }
    */
    //���e�������Ă���Ƃ�
    public void HaveBomb(int playerNum)
    {
        bomer[playerNum] = true;
    }
    //���e����������Ƃ�
    public void MissingBomb(int playerNum)
    {
        bomer[playerNum] = false;
    }

    public void GameOver()
    {
        SceneManager.sceneLoaded += ResultSceneLoaded;
        SceneManager.LoadScene("ResultScene");
    }
    void ResultSceneLoaded(Scene next,LoadSceneMode mode)
    {
        // �V�[���؂�ւ���̃X�N���v�g���擾
        var resultManager = GameObject.FindWithTag("ResultManager").GetComponent<ResultManager>();

        // �f�[�^��n������
        for(int i = 0;i < 4; i++)
        {
            resultManager.playerScore[i] = playerScore[i];
        }

        // �C�x���g����폜
        SceneManager.sceneLoaded -= ResultSceneLoaded;
    }
}
