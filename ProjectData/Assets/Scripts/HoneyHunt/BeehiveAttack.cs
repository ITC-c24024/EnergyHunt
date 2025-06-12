using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeehiveAttack : MonoBehaviour
{
    [SerializeField] Beehive beehiveSC;

    [SerializeField] Player[] playerSC;

    [SerializeField] Vector3 tagetScale;

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

    //�͂��݂h���b�v
    private IEnumerator DropHoney(GameObject hitPlayer)
    {
        var honeyTank = hitPlayer.GetComponent<HoneyTank>();

        //���S���肪�����ƃv���C���[�̎��S�֐����Ă΂�Ȃ��ׁA�P�\��݂���
        yield return new WaitForSeconds(0.0f);

        //���d�𓖂Ă��v���C���[�̎��S����
        Debug.Log(bomberNum - 1);

        honeyTank.DropHoney();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "P1" || other.gameObject.tag == "P2" || other.gameObject.tag == "P3" || other.gameObject.tag == "P4")
        {
            //���dSE
            //audioSource.Play();
            Debug.Log("atatta");
            StartCoroutine(DropHoney(other.gameObject));
        }

    }

}
