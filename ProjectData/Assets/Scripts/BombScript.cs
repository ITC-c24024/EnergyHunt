using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField] ExplosionScript explosionScript;

    public GameObject explosionRange;

    [SerializeField, Header("���d�G�t�F�N�g")]
    GameObject effect;

    [SerializeField] MeshRenderer mesh;
    [SerializeField] SphereCollider sphere;

    private Rigidbody bombRb;

    [SerializeField] Vector3 startPos;

    [SerializeField] int bombNum;

    private float bombTimer;
    private float timer;

    public bool isTimer;

    //���������̔���
    public bool isExplosion = false;

    //�_�Œ����̔���
    private bool isBlink = false;

    //�������Ă��邩�̔���
    public bool isThrow = false;

    //�R�A�_�ł̃R���[�`��
    Coroutine _blinkBomb;

    [SerializeField,Header("�R�A��_�ł�����Ƃ��̐F")]
    Color[] bombColor;

    void Start()
    {
        isTimer = false;

        transform.localPosition = startPos;

        bombRb = gameObject.GetComponent<Rigidbody>();


    }

    void Update()
    {
        if (!isExplosion) 
        {
            explosionRange.transform.position = transform.position;
        }

        if (isTimer)
        {
            timer += Time.deltaTime;

            if (timer >= bombTimer)
            {
                SetIsThrow(false);

                Explosion();
            }

            if ((bombTimer - timer <= 5) && !isBlink) 
            {
                isBlink = true;
                _blinkBomb = StartCoroutine(BlinkBomb());
            }
        }
    }

    public void IsTimerTrue()
    {
        //�������Ԃ������_���őI��
        int i = Random.Range(0, 4);
        //�f�o�b�O�p
        //i = 4;
        switch (i)
        {
            case 0:
                bombTimer = 5; 
                break;
            case 1: 
                bombTimer = 10; 
                break;
            case 2: 
                bombTimer = 15; 
                break;
            case 3: 
                bombTimer = 20;
                break;
            case 4:
                bombTimer = 360;
                break;
        }

        //�^�C�}�[�X�^�[�g
        isTimer = true;
    }

    public void IsKinematicTrue()
    {
        bombRb.isKinematic = true;
    }

    public void SetIsThrow(bool set)
    {
        isThrow = set;
    }

    //���e�������āA�����G�t�F�N�g��\������
    public void Explosion()
    {
        //�_�Œ�~
        if (_blinkBomb != null)
        {
            isBlink = false;
            StopCoroutine(_blinkBomb);
            GetComponent<Renderer>().material.color = bombColor[1];
            
        }

        //���d�G�t�F�N�g��\��
        effect.SetActive(true);
        //���d�̃R���C�_�[���I��
        explosionRange.GetComponent<SphereCollider>().enabled = true;

        //StartCoroutine(explosionScript.BomeEffect());

        isExplosion = true;

        isTimer = false;
        timer = 0;

        bombRb.isKinematic = false;
        //mesh.enabled = false;
        sphere.enabled = true;

        

        //StartCoroutine(PosReset(0.5f));
        StartCoroutine(ExplosionScaleReset(1.0f));
    }

    //�����G�t�F�N�g���\��
    public IEnumerator ExplosionScaleReset(float waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);

        
        //���d�̃R���C�_�[������
        explosionRange.GetComponent<SphereCollider>().enabled = false;
        //���d�G�t�F�N�g���\��
        effect.SetActive(false);

        //�R�A�̏����҂����Z�b�g
        explosionScript.Owner(0);

        isExplosion = false;


        ExplosionRange(1);
    }

    //���e��_�ł�����
    private IEnumerator BlinkBomb()
    {

        float blinkNum = 0;
        int i = 0;

        while (blinkNum < 10)
        {
            blinkNum ++;

            i = 1 - i;
            GetComponent<Renderer>().material.color = bombColor[i];
            
            yield return new WaitForSeconds(0.5f);
        }

        GetComponent<Renderer>().material.color = bombColor[1];

        isBlink = false;

        yield return null;
    }

    //�{���̃|�W�V���������Z�b�g
    public IEnumerator PosReset(float waitSecond)
    {
        //�_�Œ�~
        if (_blinkBomb != null)
        {
            isBlink = false;
            StopCoroutine(_blinkBomb);
            GetComponent<Renderer>().material.color = bombColor[1];         
        }

        isTimer = false;
        timer = 0;
        bombRb.isKinematic = true;

        yield return new WaitForSeconds(waitSecond * 2);

        //���d�̃R���C�_�[������
        explosionRange.GetComponent<SphereCollider>().enabled = false;
        //���d�G�t�F�N�g���\��
        effect.SetActive(false);


        yield return new WaitForSeconds(waitSecond);

        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localPosition = startPos;


        //yield return new WaitForSeconds(0.2f);

        //���o��
        mesh.enabled = true;
        sphere.enabled = true;

        bombRb.isKinematic = false;
    }
    

    //�R�A��傫������
    public void CoreScaleChange()
    {
        gameObject.transform.localScale *= 1.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�^�C�}�[���i��ł��鎞�A���̃I�u�W�F�N�g�ɓ��������甚��������
        if (isTimer && (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Ground"
            || collision.gameObject.tag == "P1" || collision.gameObject.tag == "P2" || collision.gameObject.tag == "P3" || collision.gameObject.tag == "P4"))
        {
            SetIsThrow(false);

            

            Explosion();
        }
        
        if (collision.gameObject.tag == "Wall")
        {
            /*
            var inNormal = collision.gameObject.transform.up;

            //���˕Ԃ�
            Vector3 reflection = bombRb.velocity - inNormal;
            bombRb.AddForce(reflection * -5.0f, ForceMode.Impulse);

            //var reflection = Vector3.Reflect(bombRb.velocity, inNormal);
            //bombRb.velocity = reflection * 5;
            */

            //bombRb.velocity *= 2;
        }
        
        //�X�e�[�W�O�ɗ������ꍇ�A�X�|�[���|�W�V�����ɖ߂�
        if (collision.gameObject.tag == "Reset" || collision.gameObject.tag == "OutSide")
        {
            isTimer = false;
            timer = 0;

            SetIsThrow(false);

            StartCoroutine(PosReset(0));
        }
        
        //�o���A�ɓ���������
        if (collision.gameObject.tag == "Barrier")
        {
            //�o���A������
            //collision.gameObject.GetComponent<BarrierScript>().BarrierCoolTime();

            //���˕Ԃ�
            Vector3 reflection = bombRb.velocity - transform.up;
            bombRb.AddForce(reflection * -2.0f, ForceMode.Impulse);

            //var reflection = Vector3.Reflect(bombRb.velocity, transform.up);
            //bombRb.velocity = reflection * 5.0f;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Explosion0"|| other.gameObject.tag == "Explosion1")
        {
            SetIsThrow(false);

            Explosion();
        }
        /*
        if(other.gameObject.tag == "Wall")
        {
            var inNormal = other.gameObject.transform.up;
            var inDirection = bombRb.velocity;
            //���˕Ԃ�
            //Vector3 reflection = bombRb.velocity - inNormal;
            //bombRb.AddForce(reflection * -5.0f, ForceMode.Impulse);

            var reflection = Vector3.Reflect(bombRb.velocity, inNormal);
            bombRb.velocity = reflection * -1.2f;
        }
        */
    }

    public void ExplosionRange(float range)
    {
        //explosionScript.ChangeRange(range);
    }

    public void ColliderEnabled(bool a)
    {
        sphere.enabled = a;
    }
}
