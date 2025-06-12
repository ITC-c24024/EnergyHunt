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

    public int timer = 180; // �^�C�}�[�i�b�P�ʁj
    public Image hundredsImage; // �S�̈�
    public Image tensImage; // �\�̈�
    public Image onesImage; // ��̈�
    public Image characterImage; // �u�b�v�Ȃǂ̋L��
    public Sprite[] secondsSprites; // 0�`9�̃X�v���C�g
    public RectTransform onesRectTransform;
    public RectTransform tensRectTransform;
    public RectTransform characterTransform;

    private int previousTime;
    private bool isTransitioning = false; // �J�ڒ����ǂ����̃t���O

    void Start()
    {
        previousTime = (int)(Time.time); // �����̃^�C���X�^���v��ݒ�
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

        timer = Mathf.Max(0, timer); // 0�����ɂȂ�Ȃ��悤�ɐ���

        int hundreds = timer / 100; // �S�̈�
        int tens = (timer / 10) % 10; // �\�̈�
        int ones = timer % 10; // ��̈�

        // �X�v���C�g���X�V�i�z��͈͓��`�F�b�N�j
        if (hundreds >= 0 && hundreds < secondsSprites.Length)
            hundredsImage.sprite = secondsSprites[hundreds];

        if (tens >= 0 && tens < secondsSprites.Length)
            tensImage.sprite = secondsSprites[tens];

        if (ones >= 0 && ones < secondsSprites.Length)
            onesImage.sprite = secondsSprites[ones];

        // 99�b�ȉ��ɂȂ�����S�̈ʂ��\�� & �ʒu����
        if (timer <= 99)
        {
            hundredsImage.enabled = false; // �S�̈ʂ�����
            tensImage.enabled = true; // �O�̂��ߏ\�̈ʂ�\��

            // �ʒu�𒲐��i�S�̈ʂ����������A���Ɋ񂹂�j
            //tensRectTransform.anchoredPosition = new Vector2(-869f, 486f);
            //onesRectTransform.anchoredPosition = new Vector2(-738f, 486f);
            //characterTransform.anchoredPosition = new Vector2(-607f, 486f);
        }
        else
        {
            hundredsImage.enabled = true;
        }

        // �c��9�b�ȉ��Ȃ�\�̈ʂ��\�� & ����Ɉʒu����
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

        // �^�C�}�[��0�ɂȂ�����V�[���J�ځi3�b�҂j
        if (timer <= 0 && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(ResultSceneChange());

            //�v���C���[�̑���A�͂��݂̏������~
            for (int i = 0; i < playerSC.Length; i++)
            {
                playerSC[i].enabled = false;
                honeyTankSC[i].enabled = false;
            }
            
            //�g���b�N�𓮂��Ȃ�����
            sellArea.enabled = false;

            Debug.Log("�Q�[���I��");
        }
    }

    IEnumerator ResultSceneChange()
    {
        yield return new WaitForSeconds(4f); // 3�b�҂��ă��U���g�V�[����  

        //���ʕ��בւ�
        ranking.SortScore();

        //SceneManager.LoadScene("ResultScene");
    }
}
