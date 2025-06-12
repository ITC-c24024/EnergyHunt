using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    [SerializeField] GameObject[] players;

    [SerializeField] SellArea sellAreas;

    [Header("���U���g���"), SerializeField]
    GameObject resultPanel;

    [Header("���U���g�w�i"), SerializeField]
    GameObject resultBG;

    [Header("�v���C���[UI"),SerializeField]
    Image[] playerImage;

    [Header("�������e�L�X�g"), SerializeField]
    Text[] moneyText;

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {

            // �V�[�����̂��ׂĂ� PlayerScript ���擾
            PlayerScript[] playerScripts = FindObjectsOfType<PlayerScript>();

            // point���傫�����Ƀ\�[�g���āAGameObject�����𒊏o
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
        //4�l���̏��������擾
        int[] score = new int[] {
                                     sellAreas.moneyAmount[0]
                                    ,sellAreas.moneyAmount[1]
                                    ,sellAreas.moneyAmount[2]
                                    ,sellAreas.moneyAmount[3]
                                };


        //���������������ɕ��בւ�
        int[] orderedScore = score.OrderByDescending(score => score).ToArray();

        //Debug.Log("1�ʂ̃X�R�A:" + orderedScore[0]);

        //���U���g�\��
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
            
            //������(=�X�R�A)��\��
            for (int i = 0; i < playerImage.Length; i++)
            {
                float uiLength = Mathf.Lerp(0, score[i], time / 5);

                //�������ɉ����Ē����𒲐�
                playerImage[i].rectTransform.localScale = new Vector2(
                                                                         playerImage[i].rectTransform.localScale.x,
                                                                         uiLength / 100
                                                                     );
                //�������ɉ����Ĉʒu�𒲐�
                playerImage[i].rectTransform.localPosition = new Vector2(
                                                                            playerImage[i].rectTransform.localPosition.x,
                                                                            -360 + uiLength * 0.005f
                                                                        );
                //�e�L�X�g�̈ʒu��UI�̐�[�Ɉړ�
                moneyText[i].transform.localPosition = new Vector2(
                                                                      moneyText[i].transform.localPosition.x,
                                                                      -320 + uiLength * 0.01f
                                                                  );               
                moneyText[i].text = (int)uiLength + "�~";

                //UI����ʏ���͂ݏo���Ȃ��悤�ɂ���
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

        //�X�R�A���傫�����ɃC���[�W���i�[
        Image[] orderedImage = playerImage.OrderByDescending(p => p.transform.localPosition.y).ToArray();

        //���z���傫�����Ƀe�L�X�g���i�[
        Text[] orderedText = moneyText.OrderByDescending(p => p.text).ToArray();

        //���U���g��ʂ�y���W��ۑ�
        float resultY = resultPanel.transform.localPosition.y;

        //UI�k�������v�Z
        float rate = 80000f / orderedScore[0];
        
        while (time < 1.0f)
        {
            time += Time.deltaTime;

            //���U���g��ʂ𒆉��ɖ߂�
            float currentY = Mathf.Lerp(resultY, 0, time / 3.0f);

            resultPanel.transform.localPosition = new Vector2(
                resultPanel.transform.localPosition.x,
                currentY
                );

            //�X�R�A�C���[�W�̃X�P�[���A�ʒu�𒲐�
            for (int i = 0; i < orderedImage.Length; i++)
            {
                //�ő�X�R�A����A�k���X�R�A�܂�
                float uiLength = Mathf.Lerp(orderedScore[i], orderedScore[i] * rate, time / 3.0f);

                //�k�����ɍ��킹�ăX�R�A�C���[�W���k��
                orderedImage[i].transform.localScale = new Vector2(
                    orderedImage[i].transform.localScale.x,
                    uiLength / 100
                    );

                //�ʒu�𒲐�
                orderedImage[i].transform.localPosition = new Vector2(
                    orderedImage[i].transform.localPosition.x,
                    -360 + uiLength * 0.005f
                    );

                //���z�e�L�X�g�̈ʒu�𒲐�
                orderedText[i].transform.localPosition = new Vector2(
                    orderedText[i].transform.localPosition.x,
                    -320 + uiLength * 0.01f
                    );
            }

            yield return null;
        }
    }
}
