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
    private RectTransform buttonsParent; // Butonların ekleneceği parent objesi
    private int newCardCount;  // gösterilecek kart sayısı
    private string levelStr;    // Kaça kaçlık bir alanda oynayacağımız belirten değişken
    private float showDuration = 2f; // Kartların görüneceği süre
    private int clickCount = 0; // Butona tıklama sayısını izleyen değişken
    private List<CardData> selectedCards = new List<CardData>();  //kullanıcının tıklayarak seçtiği kartlar
    private Button bigButton;

    void Start()
    {
        levelStr = "ThreeXFour";
        // levelStr adını kullanarak RectTransform'i bul
        RectTransform levelRectTransform = GameObject.Find(levelStr)?.GetComponent<RectTransform>();

        // Bulunan RectTransform'i buttonParent'e atayın
        if (levelRectTransform != null)
        {
            buttonsParent = levelRectTransform;
            buttonsParent.gameObject.SetActive(true);
        }
        newCardCount = 12;
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
       /* foreach (Transform child in buttonsParent)
        {
            Destroy(child.gameObject);
        }*/
        // Parent transformunun altındaki boş objeleri bul
        GameObject[] emptySlots = GameObject.FindGameObjectsWithTag(levelStr);
        // boş objeleri tag inden bularak listeye yazıyoruz
        int index = 0;
        

        // Her bir boş obje için bir buton oluştur
        foreach (CardData cardData in cardRandomOrder.shuffledCardList.cards)
        {
            // Boş GameObject üzerinde butonu oluştur
            GameObject buttonGO = Instantiate(buttonPrefab, emptySlots[index].transform); 
            // boş objelerin konumlarında butonları oluşturuyoruz
            Button button = buttonGO.GetComponent<Button>();
            print(emptySlots[index].transform);

            // Butona kart resmini ata
            button.image.sprite = cardData.cardImage;

            // Kartın bağlı olduğu butonu kart nesnesine ata
            cardData.cardButton = button;

            // Butonun tıklama olayını atama
            button.onClick.AddListener(() => OnCardButtonClick(cardData));
            index++;
        }
    }

    
    IEnumerator ShowCardsForDuration(float duration)
    {
        // Kartlar başlangıçta cardImage olarak gösteriliyor

        // Belirtilen süre boyunca kartların cardImage resmini göster
        yield return new WaitForSeconds(duration);

        // Belirtilen sürenin sonunda kartların backImage resmini göster
        foreach (CardData cardData in cardRandomOrder.shuffledCardList.cards)
        {
            // Kartın bağlı olduğu butonu al
            Button button = cardData.cardButton;

            // Eğer buton bulunursa
            if (button != null)
            {
                // Butonun resmini değiştir
                Image buttonImage = button.GetComponent<Image>();
                buttonImage.sprite = cardData.backImage;
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
            StartCoroutine(MyCoroutineWithDelay(cardData));
            CreateBigButton(cardData[0]);
            // Puan ekle (Örneğin: ScoreManager.AddScore(10);)
            Debug.Log("Kartlar eşleşti! Puan eklendi.");
            //DisableCards(cardData);
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
        selectedCards.Clear(); // Seçilen kartları temizle
        // Kartların imagesini backImage olarak değiştir
        foreach (CardData card in cards)
        {
            // Kartın bağlı olduğu butonu al
            Button button = card.cardButton;
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
                //button.gameObject.SetActive(false);
                Destroy(button.gameObject);
                Destroy(card);
            }
        }
        selectedCards.Clear(); // Seçilen kartları temizle
    }
    IEnumerator MyCoroutineWithDelay(List<CardData> cards)
    {
        yield return new WaitForSeconds(1f);
        DisableCards(cards);
        clickCount = 0;
        
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
    
    void CreateBigButton(CardData cardData)
    {
        // Büyük butonu oluştur
        GameObject bigButtonGO = new GameObject("BigButton");
        RectTransform bigButtonRectTransform = bigButtonGO.AddComponent<RectTransform>();
        bigButton = bigButtonGO.AddComponent<Button>();
        Image bigButtonImage = bigButtonGO.AddComponent<Image>(); // Image bileşeni ekleyin

        // Butonun büyüklüğünü ayarla
        bigButtonRectTransform.sizeDelta = new Vector2(7, 7);

        // Butonu parent olarak ayarla
        bigButtonRectTransform.SetParent(buttonsParent);
        
        // Kart verisinin resmini butona ata
        bigButtonImage.sprite = cardData.cardImage;

        // Butonun pozisyonunu ayarla
        bigButtonRectTransform.anchoredPosition = Vector2.zero;

        // Butona tıklama olayı ata
        bigButton.onClick.AddListener(OnBigButtonClick);

        
    }

    void OnBigButtonClick()
    {
        // Kullanıcı büyük butona tıkladığında geri gitme işlemi burada gerçekleşir
        Destroy(bigButton.gameObject);
        bigButton.gameObject.SetActive(false);
    }

}
