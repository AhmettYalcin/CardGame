using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public CardList cardList; // Kart listesi
    public CardRandomOrder cardRandomOrder; // Kartları karıştırmak için kullanılacak script

    public GameObject buttonPrefab; // Kartları temsil eden buton prefabı
    public RectTransform buttonsParent; // Butonların ekleneceği parent objesi
    public int newCardCount;  // gösterilecek kart sayısı
    public float showDuration = 3f; // Kartların görüneceği süre
    int clickCount = 0; // Butona tıklama sayısını izleyen değişken
    private List<CardData> selectedCards = new List<CardData>();  //kullanıcının tıklayarak seçtiği kartlar

    void Start()
    {
        newCardCount = 16;
        // Kart listesini oluştur
        cardList.CreateCards(newCardCount);
        

        // Kartları karıştır
        cardRandomOrder.originalCardList = cardList;
        cardRandomOrder.shuffledCardList = gameObject.AddComponent<CardList>();
        cardRandomOrder.ShuffleCards();

        // Karıştırılmış kartları butonlara ata
        AssignCardsToButtons();
        
        StartCoroutine(ShowCardsForDuration(showDuration));  //kartlar geldikten belli bir saniye sonra arkasını döndür
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
            
            // Kartın bağlı olduğu butonu kart nesnesine ata
            cardData.cardButton = button;

            // Butonun tıklama olayını atama
            button.onClick.AddListener(() => OnCardButtonClick(cardData));
        }

        // GridLayoutGroup'u güncelle
        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsParent.GetComponent<RectTransform>());
    }
    
    IEnumerator ShowCardsForDuration(float duration)
    {
        // Kartlar başlangıçta cardImage olarak gösteriliyor

        // Belirtilen süre boyunca kartların cardImage resmini göster
        yield return new WaitForSeconds(duration);

        // Belirtilen sürenin sonunda kartların backImage resmini göster
        foreach (Transform child in buttonsParent)
        {
            // Buton bileşenini elde et
            Button button = child.GetComponent<Button>();

            // Eğer bu transform bir butona sahipse
            if (button != null)
            {
                // Butonun resmini değiştir
                Image buttonImage = button.GetComponent<Image>();
                buttonImage.sprite = cardList.backImage;
            }
        }
    }


    // Buton tıklandığında gerçekleşecek olay
    void OnCardButtonClick(CardData cardData)
    {
        // Eğer zaten 2 kart açık ise, ekstra tıklamaları önle
        if (clickCount >= 2)
            return;
        // Seçilen kart sayısı 2 veya daha azsa işlem yap
        if (selectedCards.Count < 2)
        {
            selectedCards.Add(cardData); // Kartı seçilen kartlar listesine ekle
            clickCount++; // Butona tıklama sayısını arttır

            // Kartın Image bileşenini elde et
            Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            Image buttonImage = button.GetComponent<Image>();

            // Butona tıklanınca kartın resmini cardImage olarak göster
            buttonImage.sprite = cardData.cardImage;

            // Eğer iki kart seçildiyse kontrol fonksiyonunu çağır
            if (clickCount == 2)
            {
                Control(selectedCards); // Kontrol fonksiyonunu çağır
                selectedCards.Clear(); // Seçilen kartları temizle
               
            }
        }
    }
    void Control(List<CardData> cardData)
    {
        // Kontrol işlemlerini gerçekleştir
        Debug.Log("Kartlar kontrol ediliyor...");
    
        // Eğer kartların ID'leri eşitse
        if (cardData[0].cardID == cardData[1].cardID)
        {
            // Puan ekle (Örneğin: ScoreManager.AddScore(10);)
            Debug.Log("Kartlar eşleşti! Puan eklendi.");
            DisableCards(cardData);
        }
        else
        {
            Debug.Log("Kartlar eşleşmedi.");
        
            // Kartların imagesini backImage olarak değiştir
            // Arka resimleri göstermek için coroutine'i çağır
            List<CardData> cardsCopy = new List<CardData>(cardData); 

            StartCoroutine(ShowBackImages(cardsCopy)); // Kopya listeyi kullanarak coroutine'i çağır
        }
    }
    
    IEnumerator ShowBackImages(List<CardData> cards)
    {
        // Bir süre beklet
        yield return new WaitForSeconds(1f);

        // Kartların imagesini backImage olarak değiştir
        foreach (CardData card in cards)
        {
            // Butonu bul ve Image bileşenini al
            Button button = FindButtonByCard(card);
            Image buttonImage = button.GetComponent<Image>();

            // Butonun resmini backImage olarak ayarla
            buttonImage.sprite = card.backImage;
            clickCount = 0; // Değişkeni sıfırla
        }
    }
    void DisableCards(List<CardData> cards)
    {
        foreach (CardData card in cards)
        {
            // Kartın bağlı olduğu butonu kullan
            Button button = card.cardButton;

            // Eğer buton bulunursa
            if (button != null)
            {
                // Butonu devre dışı bırak
                button.gameObject.SetActive(false);
            }
        }
    }

    Button FindButtonByCard(CardData card)
    {
        // Kartı temsil eden butonları bul ve kart ID'sine göre eşleşeni döndür
        foreach (Transform child in buttonsParent)
        {
            // Butonu al
            Button button = child.GetComponent<Button>();

            // Butonun Image bileşenini al
            Image buttonImage = button.GetComponent<Image>();

            // Eğer butonun resmi cardImage ise
            if (buttonImage.sprite == card.cardImage)
            {
                return button;
            }
        }

        // Eğer eşleşen bir buton bulunamazsa null döndür
        return null;
    }

}

