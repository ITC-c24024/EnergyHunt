using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampPos : MonoBehaviour
{

    Vector3 pos;

    // x軸方向の移動範囲の最小値
    [SerializeField] float minX;

    // x軸方向の移動範囲の最大値
    [SerializeField] float maxX;

    // z軸方向の移動範囲の最小値
    [SerializeField] float minZ;

    // z軸方向の移動範囲の最大値
    [SerializeField] float maxZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.localPosition;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.localPosition = pos;
    }
}
