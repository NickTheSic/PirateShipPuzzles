using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlpoolAnim : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(new Vector3(0, 280 * Time.deltaTime, 0));
    }
}
