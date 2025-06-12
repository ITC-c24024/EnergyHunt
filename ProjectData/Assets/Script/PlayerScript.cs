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

    private bool hasPressed = false; // �L�[�����������ǂ������`�F�b�N

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

        // �X�R�A�����������R�����g�A�E�g
        // ScoreCount();

        // ���ʕ\���̏���
        DisplayRankingsOnKeyPress();
    }

    /*void PlayerMove()
    {
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * speed * Time.deltaTime);
    }*/

    // �X�y�[�X�L�[�ŏ��ʂ�\�����郁�\�b�h
    void DisplayRankingsOnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // �X�y�[�X�L�[�������ꂽ��
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.CalculateRankings(); // ���ʌv�Z
                int[] rankings = ScoreManager.Instance.GetRankings(); // ���ʂ��擾

                // �擾�������ʂ�\���i�f�o�b�O���O�ŕ\���j
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
