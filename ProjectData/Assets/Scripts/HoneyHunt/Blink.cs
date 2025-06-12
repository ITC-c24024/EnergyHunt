using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Blink : MonoBehaviour
{
    MeshRenderer mesh;//�I�u�W�F�N�g�̃��b�V��
    Coroutine blinkCoroutine;//�_�ŏ����̃R���[�`��

    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    public void BlinkStart(int time, float speed, float lastSpeed)//time�͓_�Ŏ��ԁAspeed�͓_�ł̑����AlastSpeed��2�b�ȉ��ɂȂ����ۂ̑���
    {
        //���łɎ��s����Ă����ꍇ�ɏd�����Ȃ��悤�ɏ������~�߂ĐV�����X�^�[�g������
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkCount(time, speed, lastSpeed));
    }

    public IEnumerator BlinkCount(int time, float speed, float lastSpeed)
    {
        var currentTime = 0f;//���݂̎���

        while (currentTime < time)
        {
            mesh.enabled = !mesh.enabled;//���b�V���̕\���ؑ�

            //�c��2�b�ȉ��ɂȂ�����_�ő��x��ύX
            if (time-currentTime <= 2f)
            {
                speed = lastSpeed;
            }

            yield return new WaitForSeconds(speed);//�_�ł̎������҂�
            currentTime += speed;//�҂������̎��Ԃ𑫂�
        }

        mesh.enabled = true;//�ŏI�I�ɕ\����ԂɂȂ�悤�ɂ���
    }
}
