using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlipController : MonoBehaviour
{
    public SpriteRenderer blipSprite;
    public bool visible;
    [SerializeField] float fadeTime;
    [SerializeField] float alpha;
    [SerializeField] Material mat;
    // Start is called before the first frame update
    void Start()
    {
        visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        alpha = this.gameObject.GetComponent<SpriteRenderer>().color.a;
    }

    public void FadeOut()
    {
        LeanTween.alpha(this.gameObject, 0, fadeTime);
        if (this.gameObject.GetComponent<SpriteRenderer>().color.a <= 0)
        {
            visible = false;
        }
    }
}
