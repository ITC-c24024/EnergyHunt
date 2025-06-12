using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBullet : MonoBehaviour
{
    [SerializeField, Header("��攚�e")]
    GameObject bomb;

    [SerializeField, Header("���d�G�t�F�N�g")]
    GameObject effect;

    //�d�͉����x
    private float gravity = 9.8f;

    [SerializeField] Vector3 tragetPos;

    [SerializeField,Header("�ł��グ��p�x")] 
    float angle_XY;
    [SerializeField,Header("�΂߂ɔ�Ԋp�x")] 
    float angle_XZ;
    [SerializeField,Header("�����x")] 
    float v0;

    [SerializeField]
    AudioSource audioSource;

    void Start()
    {
        //StartCoroutine(ShootBomb());
    }

    void Update()
    {
        
    }

    public IEnumerator ShootBomb(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Vector3 startPos = bomb.transform.position;

        float rad_XY = angle_XY * 3.14f / 180;
        float rad_XZ = angle_XZ * 3.14f / 180;

        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            float vx = Mathf.Cos(rad_XY) * v0 * time;
            float vy = Mathf.Sin(rad_XY) * v0 * time - gravity * time * time / 2;
            float vz = Mathf.Tan(rad_XZ) * vx;



            bomb.transform.position = new Vector3(startPos.x + vx, startPos.y + vy, startPos.z + vz);
            

            if (bomb.transform.position.y < 1)
            {
                audioSource.Play();

                effect.SetActive(true);
                StartCoroutine(StopEffect());

                Debug.Log("�I��");
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(1.0f);

        effect.SetActive(false);
    }
}
