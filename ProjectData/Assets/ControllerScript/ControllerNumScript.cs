using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerNumScript : MonoBehaviour
{
    public int[] controllerNum = new int[4]; // �R���g���[���[�ԍ�
    private bool[] isAssigned = new bool[4]; // �{�^�������蓖�čς݂��ǂ���

    void Update()
    {
        // �X�C�b�`�R���g���[���[ (P1AButton, P2AButton)
        if (Input.GetButtonDown("P1AButton") && !isAssigned[0]) // P1
        {
            AssignToSwitchController(0); //���蓖��
        }

        if (Input.GetButtonDown("P2AButton") && !isAssigned[1]) // P2
        {
            AssignToSwitchController(1); //���蓖��
        }

        // �v���X�e�R���g���[���[ (P3AButton, P4AButton)
        if (Input.GetButtonDown("P3AButton") && !isAssigned[2]) // P3
        {
            AssignToPlayStationController(2); //���蓖��
        }

        if (Input.GetButtonDown("P4AButton") && !isAssigned[3]) // P4
        {
            AssignToPlayStationController(3); //���蓖��
        }
    }

    // �X�C�b�`�R���g���[���[
    private void AssignToSwitchController(int buttonIndex)
    {
        for (int i = 0; i < 2; i++) // �X�C�b�`�p�X���b�g�̂� (0, 1)
        {
            if (controllerNum[i] == 0) // �󂢂Ă���X���b�g��T��
            {
                controllerNum[i] = i + 1; // �X���b�g�ɔԍ������蓖��
                isAssigned[buttonIndex] = true; // �{�^�������蓖�čς݂ɐݒ�
                return;
            }
        }

        Debug.Log("No available slot for Switch Controller.");
    }

    // �v���X�e�R���g���[���[
    private void AssignToPlayStationController(int buttonIndex)
    {
        for (int i = 2; i < 4; i++) // �v���X�e�p�X���b�g�̂� (2, 3)
        {
            if (controllerNum[i] == 0) // �󂢂Ă���X���b�g��T��
            {
                controllerNum[i] = i + 1; // �X���b�g�ɔԍ������蓖��
                isAssigned[buttonIndex] = true; // �{�^�������蓖�čς݂ɐݒ�
                return;
            }
        }
    }
}
