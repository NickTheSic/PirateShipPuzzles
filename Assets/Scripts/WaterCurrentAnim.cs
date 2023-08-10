using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCurrentAnim : MonoBehaviour
{
    public MeshRenderer m;

    // Update is called once per frame
    void Update()
    {
        Vector2 off = m.materials[0].mainTextureOffset;
        off.x += 1.4f * Time.deltaTime;
        m.materials[0].mainTextureOffset = off;
    }
}
