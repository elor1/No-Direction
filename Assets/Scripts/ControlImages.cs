using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlImages : MonoBehaviour
{
    [SerializeField] private Sprite enabledImage;
    [SerializeField] private Sprite disabledImage;

    private Image image;

    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = enabledImage;
    }

    public void Disable()
    {
        image.sprite = disabledImage;
    }
}
