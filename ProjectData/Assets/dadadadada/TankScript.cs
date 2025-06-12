using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
    [SerializeField] float timer = 0f;
    public GameObject[] tentativeTank;
    public bool haveBomb;

    public int currentIndex = 0; // ���ݑ��쒆�̃I�u�W�F�N�g�̃C���f�b�N�X
    private float maxScaleY = 0.7f; // �ő�X�P�[���l
    private float duration = 3f;   // �X�P�[�����ő�ɂȂ�܂ł̎���

    public int count = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (haveBomb && currentIndex < tentativeTank.Length)
        {
            // ���݂̃^���N���A�N�e�B�u�ɂ���i�ŏ���1�񂾂����s�j
            if (!tentativeTank[currentIndex].activeSelf)
            {
                tentativeTank[currentIndex].SetActive(true);
            }

            // �^�C�}�[�̐i�s�x�ɉ����ăX�P�[������
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            Vector3 currentScale = tentativeTank[currentIndex].transform.localScale;
            tentativeTank[currentIndex].transform.localScale = new Vector3(
                currentScale.x,
                Mathf.Lerp(0f, maxScaleY, progress),
                currentScale.z
            );

            if (timer >= duration)
            {
                // �^�C�}�[��3�b�ɒB�����玟�̃I�u�W�F�N�g���A�N�e�B�u�ɂ���
                currentIndex++;

                if (currentIndex < tentativeTank.Length)
                {
                    tentativeTank[currentIndex].SetActive(true);
                }

                // �J�E���g�͎��̃I�u�W�F�N�g���A�N�e�B�u�ɂ������1���₷
                if (currentIndex > 0) // �ŏ��̃^���N���A�N�e�B�u�����ꂽ�ォ��J�E���g�𑝂₷
                {
                    count++;
                }

                // �^�C�}�[�����Z�b�g
                timer = 0f;
            }
        }
    }
}
