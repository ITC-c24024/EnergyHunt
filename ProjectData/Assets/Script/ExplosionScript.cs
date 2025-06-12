using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] BombScript bombScript;
    [SerializeField] PlayerController[] playerController;
    //private EnergyTank energyTank;

    [SerializeField, Header("�o���A�I�u�W�F�N�g")]
    GameObject[] barrier;

    [SerializeField] Vector3 tagetScale;

    [SerializeField, Header("�G�t�F�N�g�̉~")]
    GameObject circle;

    [SerializeField, Header("�G�t�F�N�g�̒��S")]
    GameObject center;

    [SerializeField, Header("�G�t�F�N�g�̃r���r��1")]
    GameObject thander1;

    [SerializeField, Header("�G�t�F�N�g�̃r���r��2")]
    GameObject thander2;


    //�����͈�
    [SerializeField] float explosionRange = 2;

    [SerializeField] int bomberNum;

    public float maxTime = 2f;

    [SerializeField]
    AudioSource audioSource;

    //��������
    public bool isSelf = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /*public IEnumerator BomeEffect()
    {
        float timer = 0f;
        Vector3 originalScale = transform.localScale;
    
        while (timer < maxTime)
        {
            float ScaleChangeTime = timer / maxTime;
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, tagetScale * explosionRange, ScaleChangeTime);
            yield return null;
        }        
    }*/

    public void Owner(int playerNum)
    {
        bomberNum = playerNum;
    }

    private void IsSelfReset()
    {
        isSelf = false;
    }
    
    //�G�l���M�[�h���b�v
    private IEnumerator DropEnergry(GameObject hitPlayer)
    {
        var energyTank = hitPlayer.GetComponent<EnergyTank>();

        //���S���肪�����ƃv���C���[�̎��S�֐����Ă΂�Ȃ��ׁA�P�\��݂���
        yield return new WaitForSeconds(0.0f);

        if (bomberNum != 0)
        {
            //���d�𓖂Ă��v���C���[�̎��S����
            Debug.Log(bomberNum - 1);
            var isDead = playerController[bomberNum - 1].isDead;

            if (isDead)//���Ă��v���C���[������ł������A��������ɂ���
            {
                energyTank.DropEnergy(5);
            }
            else//����ł��Ȃ�������A���Ă�ꂽ�v���C���[�̃G�l���M�[��D��
            {
                energyTank.DropEnergy(bomberNum);
            }
        }
        else
        {
            energyTank.DropEnergy(5);
        }   
     
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "P1" || other.gameObject.tag == "P2" || other.gameObject.tag == "P3" || other.gameObject.tag == "P4")
        {
            //���dSE
            audioSource.Play();

            //���������v���C���[�̔ԍ����擾
            var playerNum = other.GetComponent<PlayerController>().controllerNum;

            //�o���A�����Ă��Ȃ��Ƃ�
            if (!barrier[playerNum - 1].GetComponent<BarrierScript>().isBarrier)
            {
                StartCoroutine(DropEnergry(other.gameObject));

                //var energyTank = other.gameObject.GetComponent<EnergyTank>();
                /*
                //��������
                if (playerNum == bomberNum)
                {
                    isSelf = true;

                    Invoke("IsSelfReset", 2.0f);
                }
                */
                /*
                //���d�𓖂Ă��v���C���[������ł�����
                var isDead = playerController[bomberNum - 1].isDead;

                if (isDead || isSelf)
                {
                    energyTank.DropEnergy(5);   
                }
                else
                {
                    energyTank.DropEnergy(bomberNum);
                } */
            }
        }

        if (other.gameObject.tag == "Floor")
        {
            audioSource.Play();

            //���������u���b�N�̃X�N���v�g���擾
            var blockScript = other.GetComponent<BlockScript>();

            //���Ă��鎞�͒ʂ�Ȃ��悤�ɂ���
            if (!blockScript.isBroken)
            {
                blockScript.SetBlock(false);
            }
        }
    }

    //�����^���N�ɂ��͈͕ύX
    public void ChangeRange()
    {
        gameObject.transform.localScale *= 1.1f;
        circle.transform.localScale *= 1.1f;
        center.transform.localScale *= 1.1f;
        thander1.transform.localScale *= 0.8f;
        thander2.transform.localScale *= 0.8f;
    }
}
