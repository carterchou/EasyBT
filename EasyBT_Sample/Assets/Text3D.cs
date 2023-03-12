using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Text3D : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public SpriteRenderer sprite;
    float Vy = 7f;
    float Vx = 1f;
    float Vz = 0.5f;
    float timeCount;

    private void OnEnable()
    {
        if (Random.Range(0, 100) < 50)
        {
            Vx = -1f;
        }
    }

    public GameObject ReSet()
    {
        Vy = 7f;
        Vx = 1f;
        Vz = 0.5f;
        timeCount = 0;

        if (textMeshPro != null)
            textMeshPro.color = new Color(1, 1, 1, 1);
        if (sprite != null)
            sprite.color = new Color(1, 1, 1, 1);

        return gameObject;
    }

    private void Update()
    {
        var g = (-9.8f * Time.deltaTime * 2f);
        Vy += g;

        timeCount += Time.deltaTime;

        CrossFade();

        transform.Translate(new Vector3(Vx, Vy, Vz) * Time.deltaTime, Space.World);

        if (transform.position.y < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void CrossFade()
    {
        if (timeCount > 0.5f)
        {
            if (textMeshPro != null)
                textMeshPro.color -= new Color(0, 0, 0, Time.deltaTime * 2);
            if (sprite != null)
                sprite.color -= new Color(0, 0, 0, Time.deltaTime * 2);
        }
    }
}
