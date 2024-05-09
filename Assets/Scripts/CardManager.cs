using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public CardList cardList; // Kart listesi
    public CardRandomOrder cardRandomOrder; // Kartları karıştırmak için kullanılacak script

    public GameObject buttonPrefab; // Kartları temsil eden buton prefabı
    public RectTransform buttonsParent; // Butonların ekleneceği parent objesi

    void Start()
    {
        // Kart listesini oluştur
        cardList.CreateCards(16); // Örnek olarak 16 kart oluşturduk

        // Kartları karıştır
        cardRandomOrder.originalCardList = cardList;
        cardRandomOrder.shuffledCardList = gameObject.AddComponent<CardList>();
        cardRandomOrder.ShuffleCards();

        // Karıştırılmış kartları butonlara ata
        AssignCardsToButtons();
    }

    void AssignCardsToButtons()
    {
        // Canvas üzerindeki tüm butonları temizle
        foreach (Transform child in buttonsParent)
        {
            Destroy(child.gameObject);
        }

        // Her bir kart için bir buton oluştur
        foreach (CardData cardData in cardRandomOrder.shuffledCardList.cards)
        {
            GameObject buttonGO = Instantiate(buttonPrefab, buttonsParent);
            Button button = buttonGO.GetComponent<Button>();

            // Butona kart resmini ata
            button.image.sprite = cardData.cardImage;

            // Butonun tıklama olayını atama
            button.onClick.AddListener(() => OnCardButtonClick(cardData));
        }

        // GridLayoutGroup'u güncelle
        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsParent.GetComponent<RectTransform>());
    }


    // Buton tıklandığında gerçekleşecek olay
    void OnCardButtonClick(CardData cardData)
    {
        // Seçilen kartı işleme alabilirsiniz
        Debug.Log("Tıklanan kartın ID'si: " + cardData.cardID);
    }
}

