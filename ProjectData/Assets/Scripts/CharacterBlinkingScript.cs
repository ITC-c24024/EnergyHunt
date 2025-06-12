using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBlinkingScript : MonoBehaviour
{
    [SerializeField]
    [Header("�_�ŊԊu")]
    float timeOut = 0.2f;

    float timeElapsed;//�_�ŊԊu���v�Z����^�C�}�[

    bool isVisible = true;//�L�����N�^�[��setActive��������bool

    [SerializeField] GameObject[] playerParts = new GameObject[3];//�L�����N�^�[�̃p�[�c��3�����

    private bool isBlink = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlink)
        {
            CharacterBlinking();
        }
        /*
        else
        {
            foreach (var part in playerParts)
            {
                part.SetActive(true);
            }
            timeElapsed = 0f;
        }
        */
    }

    public void BlinkStart(bool set)
    {
        isBlink = set;
    }

    //�L�����N�^�[��_�ł�����
    void CharacterBlinking()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeOut)
        {
            isVisible = !isVisible; // ON/OFF��؂�ւ�
            foreach (var part in playerParts)
            {
                part.SetActive(isVisible);
            }
            timeElapsed = 0f;
        }
    }
}
