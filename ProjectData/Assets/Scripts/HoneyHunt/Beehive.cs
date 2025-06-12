using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Beehive : MonoBehaviour
{
    [SerializeField] BeehiveAttack beehiveAttackSC;

    public GameObject attackRange;

    [Header("�̂̃I�u�W�F�N�g"), SerializeField]
    GameObject bodyObj;

    [Header("���Ǐ]�p�I�u�W�F�N�g"), SerializeField]
    GameObject headFollow;

    //�̂��o�Ă��邩�̔���
    public bool outBody = false;

    [Header("����̓����蔻��"), SerializeField]
    GameObject PunchCollider;

    [SerializeField, Header("���d�G�t�F�N�g")]
    GameObject effect;

    [SerializeField] MeshRenderer mesh;
    [SerializeField] SphereCollider sphere;

    private Rigidbody beehiveRb;

    [SerializeField] Vector3 startPos;

    [Header("�m�b�N�o�b�N�������"), SerializeField]
    float knockbackPower = 100;

    //�n�`�̑��̏Փˈʒu
    Vector3 collisionPos;

    [SerializeField] int beehiveNum;

    //���������̔���
    public bool isExplosion = false;

    //�������Ă��邩�̔���
    public bool isThrow = false;

    public bool isTimer;

    [Header("�݂��Y��Animator"), SerializeField]
    Animator animator;

    void Start()
    {

        transform.localPosition = startPos;

        beehiveRb = gameObject.GetComponent<Rigidbody>();


    }

    void Update()
    {
        if (!isExplosion)
        {
            attackRange.transform.position = transform.position;
        }

        //�݂��Y���o��Ƃ�
        if (outBody)
        {
            //����Ǐ]�p�I�u�W�F�N�g�ɒǏ]
            transform.position = new Vector3(
                headFollow.transform.position.x,
                headFollow.transform.position.y + 0.5f,
                headFollow.transform.position.z
                );
            transform.rotation = headFollow.transform.rotation;
        }
    }

    public void IsKinematicTrue()
    {
        beehiveRb.isKinematic = true;
    }

    public void SetIsThrow(bool set)
    {
        isThrow = set;
    }

    //���e�������āA�����G�t�F�N�g��\������
    public void Explosion()
    {
        //���d�G�t�F�N�g��\��
        effect.SetActive(true);
        //���d�̃R���C�_�[���I��
        attackRange.GetComponent<SphereCollider>().enabled = true;

        isExplosion = true;

        isTimer = false;

        beehiveRb.isKinematic = false;

        sphere.enabled = true;

        StartCoroutine(ExplosionScaleReset(1.0f));
    }

    //�����G�t�F�N�g���\��
    public IEnumerator ExplosionScaleReset(float waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);


        //���d�̃R���C�_�[������
        attackRange.GetComponent<SphereCollider>().enabled = false;

        //���d�G�t�F�N�g���\��
        effect.SetActive(false);

        isExplosion = false;

    }

    //�{���̃|�W�V���������Z�b�g
    public IEnumerator PosReset(float waitSecond)
    {
        beehiveRb.isKinematic = true;

        yield return new WaitForSeconds(waitSecond * 2);

        //���d�̃R���C�_�[������
        attackRange.GetComponent<SphereCollider>().enabled = false;

        //���d�G�t�F�N�g���\��
        effect.SetActive(false);


        yield return new WaitForSeconds(waitSecond);

        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localPosition = startPos;

        //���o��
        mesh.enabled = true;
        sphere.enabled = true;

        beehiveRb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�v���C���[�ɓ��������Ƃ�
        if (isThrow && collision.gameObject.tag.StartsWith("P"))
        {
            Knockback(collision.gameObject);
        }

        //���̃I�u�W�F�N�g�ɓ��������甚��������
        if (isThrow && (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Ground"))
        {
            SetIsThrow(false);

            //�̂��o��
            HoneyBody();    

            //Explosion();
        }

        //�X�e�[�W�O�ɗ������ꍇ�A�X�|�[���|�W�V�����ɖ߂�
        if (collision.gameObject.tag == "Reset" || collision.gameObject.tag == "OutSide")
        {
            SetIsThrow(false);

            StartCoroutine(PosReset(0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Explosion0" || other.gameObject.tag == "Explosion1")
        {
            SetIsThrow(false);

            Explosion();
        }
    }

    //�v���C���[���m�b�N�o�b�N������
    void Knockback(GameObject hitObj)
    {
        Debug.Log("�m�b�N�o�b�N");
        //�v���C���[�ƃn�`�̑��̋������v�Z���A���x�x�N�g�����Z�o
        Vector3 distination = (hitObj.transform.position - transform.position).normalized;

        //�͂�������
        Rigidbody rb = hitObj.GetComponent<Rigidbody>();
        rb.AddForce(distination * knockbackPower, ForceMode.VelocityChange);
    }

    //�n�`�̑�����̂��o��
    void HoneyBody()
    {
        outBody = true;

        //�n�`�̑��̊p�x
        var headAngle = transform.eulerAngles.y;  

        //�n�`�̑��𓮂��Ȃ�����
        beehiveRb.isKinematic = true;
        beehiveRb.useGravity = false;

        

        //�̂�\��
        bodyObj.SetActive(true);

        //�̒Ǐ]
        bodyObj.transform.position = new Vector3(
            transform.position.x,
            bodyObj.transform.position.y,
            transform.position.z
            );
        //�̂̌����𓪂ɍ��킹��
        bodyObj.transform.rotation = Quaternion.Euler(
            bodyObj.transform.rotation.x,
            headAngle,//�n�`�̑��̌���
            bodyObj.transform.rotation.z
            );


        //����
        animator.SetBool("IsHit", true);
        Invoke("StopAnim", 1f);

        //����̓����蔻����o��
        Invoke("SetCollider", 1.2f);
        //���蔻�������
        Invoke("SetCollider", 1.8f);

        //�n�`�̑�������悤�ɂ���
        Invoke("ResetBeeHive", 3.0f);
    }

    //����A�j���[�V�������~�߂�
    void StopAnim()
    {
        animator.SetBool("IsHit", false);
    }

    //����̓����蔻���ݒ�
    void SetCollider()
    {
        bool isPunch = !PunchCollider.activeSelf;
        PunchCollider.SetActive(isPunch);
    }

    //�n�`�̑�������悤�ɂ���
    void ResetBeeHive()
    {
        //�̂��\��
        bodyObj.SetActive(false);

        //�n�`�̑��������悤�ɂ���
        beehiveRb.isKinematic = false;
        beehiveRb.useGravity = true;

        outBody = false;
    }

    public void ColliderEnabled(bool a)
    {
        sphere.enabled = a;
    }

    public void IsTimerTrue()
    {
        //�^�C�}�[�X�^�[�g
        isTimer = true;
    }
}
