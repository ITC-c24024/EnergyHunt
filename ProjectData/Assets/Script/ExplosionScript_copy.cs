using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript_copy : MonoBehaviour
{
    [SerializeField] BombScript bombScript;
    [SerializeField] PlayerController_copy[] playerController;
    [SerializeField] EnergyTank_copy energyTank;

    [SerializeField] Vector3 tagetScale;

    //�����͈�
    [SerializeField] float explosionRange = 1;

    private int bomberNum;

    public float maxTime = 2f;

    void Start()
    {

    }

    void Update()
    {

    }

    public IEnumerator BomeEffect()
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
    }


    public void Owner(int playerNum)
    {
        bomberNum = playerNum;
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && energyTank != null)
        {
            energyTank = other.GetComponent<EnergyTank_copy>();
            energyTank.DropEnergy(bomberNum);
        }

        if (other.gameObject.tag == "Floor")
        {
            //Destroy(other.gameObject);

            //���������u���b�N�̃X�N���v�g���擾
            var blockScript = other.GetComponent<BlockScript>();
            blockScript.SetBlock(false);
        }
    }

    //�g�̋����ɂ��͈͕ύX
    public void ChangeRange(float range)
    {
        explosionRange = range;
    }
}
