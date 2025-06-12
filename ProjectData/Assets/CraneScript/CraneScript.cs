using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneScript : MonoBehaviour
{
    public float rotationSpeed = 90f;
    private bool isRotating = false;
    private bool isReturning = false;
    public Transform player;
    public Transform crane;
    public Transform craneTip;
    //public Player_Test player_Test;
    private Quaternion initialRotation;
    [SerializeField] Vector3 targetEulerAngles;
    private Quaternion targetRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        //targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);
        targetRotation = Quaternion.Euler(targetEulerAngles);
    }

    void Update()
    {
        /*
        if (player.gameObject.transform.position.y < -5)
        {
            // �v���C���[���w��ʒu�����������ʒu�����Z�b�g

            player.gameObject.transform.position = playerRespawnPos;//new Vector3(8.8f, 2.5f, -8.6f);
            player.SetParent(crane);

            // ��]�J�n
            if (!isRotating)
            {
                StartCoroutine(RotateToTarget(targetRotation));
            }
        }*/
    }

    public IEnumerator RotateToTarget()
    {

        Debug.Log("crane");
        isRotating = true;

        //player.SetParent(crane);

        // ��]������
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            player.transform.position = craneTip.transform.position;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation; //�ŏI�I�Ȓl���m��

        //player_Test.rb.useGravity = true;

        //player.transform.parent = null;
        // ��]��90�x�ɒB������0.5�b�ҋ@
        yield return new WaitForSeconds(0.5f);

        // 90�x��]�����������猳�̉�]�ɖ߂�
        StartCoroutine(RotateBackToInitial());
    }

    private IEnumerator RotateBackToInitial()
    {
        isReturning = true;

        // ������]�ɖ߂�
        while (Quaternion.Angle(transform.rotation, initialRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = initialRotation;
        isReturning = false;
        isRotating = false;
    }
}
