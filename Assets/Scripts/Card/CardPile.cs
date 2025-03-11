using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPile : MonoBehaviour
{
    [SerializeField] private Button cardpileBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Transform scrollView;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cardPileImagePrefab;

    [SerializeField] private bool isDarwCardPile;
    [SerializeField] private bool isThrowCardPile;
    
    void Start()
    {
        closeBtn.onClick.AddListener(CloseCardPileBtnClicked);

        if (isDarwCardPile)
        {
            cardpileBtn.onClick.AddListener(DarwCardPileBtnClicked);
        }
        else if (isThrowCardPile)
        {
            cardpileBtn.onClick.AddListener(ThrowCardPileBtnClicked);
        }
    }

    
    void Update()
    {
        
    }

    public void DarwCardPileBtnClicked()
    {
        int childCount = content.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        foreach (var cards in CardManager.instance.playerDrawCardGroup)
        {
            for (int i = 0; i < cards.Value; i++)
            {
                GameObject cell = Instantiate(cardPileImagePrefab, content);
                cell.name = "CardPile_" + i;
                cell.GetComponent<Image>().sprite = cards.Key.sprite;
            }
        }
        scrollView.transform.gameObject.SetActive(true);
    }

    public void ThrowCardPileBtnClicked()
    {
        
        int childCount = content.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        foreach (var cards in CardManager.instance.playerThrowCardGroup)
        {
            for (int i = 0; i < cards.Value; i++)
            {
                GameObject cell = Instantiate(cardPileImagePrefab, content);
                cell.name = "CardPile_" + cell.GetInstanceID();
                cell.GetComponent<Image>().sprite = cards.Key.sprite;
            }
        }
        scrollView.transform.gameObject.SetActive(true);
    }
    public void CloseCardPileBtnClicked()
    {
        scrollView.transform.gameObject.SetActive(false);
    }
}
