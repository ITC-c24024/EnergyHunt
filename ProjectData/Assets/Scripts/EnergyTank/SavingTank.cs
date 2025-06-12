using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavingTank : MonoBehaviour
{
    [SerializeField]
    GameObject focusCamera;

    [SerializeField]
    GameObject[] player;
    [SerializeField]
    int playerNum;

    [SerializeField, Header("�v���C���[���ړ�������|�W�V����")]
    Vector3 playerPos;

    [SerializeField, Header("�ړ�������̌���")]
    Quaternion playerAngle;

    [SerializeField, Header("�w�����Ă���^���N")]
    GameObject playerTank;

    [Header("�^���N�I�u�W�F�N�g"), SerializeField]
    GameObject[] tank;

    [Header("�^���N�̃Q�[�W�I�u�W�F�N�g"), SerializeField]
    GameObject[] tankGauge;

    [SerializeField, Header("���~�G�l���M�[�̌�")]
    Text energyAmountText;

    //���~�p�^���N���̃G�l���M�[��
    public int energyAmount;

    [SerializeField, Header("�R�A�I�u�W�F�N�g")]
    GameObject[] coreObj;

    [SerializeField, Header("��C�̋�")]
    GameObject bullet;

    public bool isArrow = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    //���~�p�^���N�ɃG�l���M�[���`���[�W
    public void Charge()
    {
        energyAmount++;

        //�^���N��\��
        tank[energyAmount - 1].SetActive(true);
        //�Q�[�W��\��
        tankGauge[energyAmount - 1].SetActive(true);

        //���̃^���N���\��
        if (energyAmount != 4)
        {
            player[playerNum].GetComponent<ClampPos>().enabled = false;

            tank[energyAmount].SetActive(false);
        }
      
        //�Q�[�W���^���̃^���N�����}�b�N�X�ɂȂ����Ƃ�
        if (energyAmount == 4)
        {
            //�S�v���C���[�𓮂��Ȃ�����
            for (int i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<PlayerController>().IsDeadTrue();
            }

            //�����^���N���\���ɂ���
            playerTank.SetActive(false);


            //�v���C���[�̃R�A�������Ă��锻��
            var haveCore0 = player[playerNum].GetComponent<PlayerController>().haveBomb[0];          
            var haveCore1 = player[playerNum].GetComponent<PlayerController>().haveBomb[1];            

            //�R�A�̃|�W�V�������Z�b�g
            if (haveCore0)
            {
                player[playerNum].GetComponent<PlayerController>().HaveBombChange(0, false);

                StartCoroutine(coreObj[0].GetComponent<BombScript>().PosReset(0)); 
            }
            else if (haveCore1)
            {
                player[playerNum].GetComponent<PlayerController>().HaveBombChange(1, false);

                StartCoroutine(coreObj[1].GetComponent<BombScript>().PosReset(0)); 
            }
            

            //�v���C���[��C��̉��Ɉړ�
            StartCoroutine(PlayerMove());

            var bulletScript = bullet.GetComponent<ShootBullet>();
            StartCoroutine(bulletScript.ShootBomb(2.0f));
        }
    }

    private IEnumerator PlayerMove()
    {
        player[playerNum].GetComponent<Rigidbody>().isKinematic = true;

        Vector3 startPos = player[playerNum].transform.localPosition;
        Quaternion startAngle = player[playerNum].transform.localRotation;

        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime;

            float px = Mathf.Lerp(startPos.x, playerPos.x, time / 1);
            float py = Mathf.Sin(Mathf.Lerp(0, 180, time / 1) * 3.14f / 180) - 0.8f * time * time / 1;
            //float py = Mathf.Lerp(startPos.y, playerPos.y, time / 1) - 9.8f * time * time / 1;
            float pz = Mathf.Lerp(startPos.z, playerPos.z, time / 1);

            player[playerNum].transform.localPosition = new Vector3(px, py, pz);
            player[playerNum].transform.localRotation = Quaternion.Lerp(startAngle, playerAngle, time / 1);

            yield return null;
        }

        StartCoroutine(focusCamera.GetComponent<CameraScript>().CameraFocus(player[playerNum], 2.0f));
        StartCoroutine(focusCamera.GetComponent<CameraScript>().CameraRotate(2.0f));

        //StartCoroutine(player[playerNum].GetComponent<PlayerController>().WinnerAnim(2.0f));
    }
    /*
    private IEnumerator ArrowMove()
    {
        float timer = 0;

        while (isArrow)
        {

        }

            yield return null;
    }*/
    
    //���~�p�^���N���G�l���M�[����
    public void UseEnergy(int amount)
    {
        for (int i = 0; i < amount && energyAmount > 0; i++)
        {
            energyAmount -= 1;

            //�Q�[�W���\��
            tankGauge[energyAmount].SetActive(false);
        }    
    }
}
