
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Sprite defaultSprite;
    [SerializeField] Sprite onSelectedSprite;
    Image spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<Image>();
        defaultSprite = spriteRenderer.sprite;
    }

    private void OnDisable()
    {
        spriteRenderer.sprite = defaultSprite;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spriteRenderer.sprite = onSelectedSprite;
        MenuManager.Instance.OnSelectMenu();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spriteRenderer.sprite = defaultSprite;
        MenuManager.Instance.OnExitSelectMenu();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MenuManager.Instance.OnClickMenu();
    }
}