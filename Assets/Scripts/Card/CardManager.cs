using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public CardList cardList; // Kart listesi
    public CardRandomOrder cardRandomOrder; // Kartları karıştırmak için kullanılacak script
    public GameManager gameManager;
    public TMP_Text scoreText;
    public TMP_Text HPText;
    public string levelStr; 
    public int newCardCount;  // gösterilecek kart sayısı
    public int cardThemeIntCardManager;

    public GameObject buttonPrefab; // Kartları temsil eden buton prefabı
    private RectTransform buttonsParent; // Butonların ekleneceği parent objesi
    private int cardCount; // bölümdeki mevcut kart sayısı
    private float showDuration = 2f; // Kartların görüneceği süre
    private int clickCount = 0; // Butona tıklama sayısını izleyen değişken
    private List<CardData> selectedCards = new List<CardData>();  //kullanıcının tıklayarak seçtiği kartlar
    private Button bigButton;
    private bool canClick = false; // Kartların tıklanabilirliğini kontrol eden boolean değişken
    private int score; //oyun içindeki puanımızı tutan değişken
    private int healthPoints; // oyun içinde kalan can değerimiz

    void Start()
    {
        levelStr = LevelSelect.instance.levelStiringMenu;
        newCardCount = LevelSelect.instance.levelIntMenu;
        cardThemeIntCardManager = LevelSelect.instance.levelThemeNameInt;
        
        score = 0; // daha sonra menüden gelen puan a eşitlenecek
        scoreText.text = score.ToString();
        healthPoints = 2;
        HPText.text = healthPoints.ToString();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        // levelStr adını kullanarak RectTransform'i bul
        RectTransform levelRectTransform = GameObject.Find(levelStr)?.GetComponent<RectTransform>();

        // Bulunan RectTransform'i buttonParent'e atayın
        if (levelRectTransform != null)
        {
            buttonsParent = levelRectTransform;
            buttonsParent.gameObject.SetActive(true);
        }
      //  print(" Card Theme int " + cardThemeIntCardManager);
       
        cardCount = newCardCount;
        // Kart listesini oluştur
        cardList.CreateCards(newCardCount, cardThemeIntCardManager);
        

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
         //   print(emptySlots[index].transform);

            // Butona kart resmini ata
            button.image.sprite = cardData.cardImage;

            // Kartın bağlı olduğu butonu kart nesnesine ata
            cardData.cardButton = button;

            // Butonun tıklama olayını atama
            button.onClick.AddListener(() => OnCardButtonClick(cardData));
            index++;
        }
    }

    
    // ReSharper disable Unity.PerformanceAnalysis
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
                AnimateCardFlip(cardData, false,button);
                buttonImage.sprite = cardData.backImage;
            }
        }
        // Kartların tıklanabilirliğini etkinleştir
        canClick = true;
    }


    // Buton tıklandığında gerçekleşecek olay
    void OnCardButtonClick(CardData cardData)
    {
        // Eğer kartlar tıklanabilir değilse veya zaten 2 kart açık ise, ekstra tıklamaları önle
        if (!canClick || clickCount >= 2 || cardData.isFlipped)
            return;
        // Seçilen kart sayısı 2 veya daha azsa işlem yap
        if (selectedCards.Count < 2)
        {
            selectedCards.Add(cardData); // Kartı seçilen kartlar listesine ekle
            clickCount++; // Butona tıklama sayısını arttır
            
            // Kartı açık olarak işaretle
            cardData.isFlipped = true;

            // Kartın Image bileşenini elde et
            Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            Image buttonImage = button.GetComponent<Image>();

            // Butona tıklanınca kartın resmini cardImage olarak göster
            //buttonImage.sprite = cardData.cardImage;
            // Butona tıklanınca kartın resmini cardImage olarak göster
            AnimateCardFlip(cardData, true,button);

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
       // Debug.Log("Kartlar kontrol ediliyor...");
    
        // Eğer kartların ID'leri eşitse
        if (cardData[0].cardID == cardData[1].cardID)
        {
            StartCoroutine(MyCoroutineWithDelay(cardData));
            CreateBigButton(cardData[0]);
            score += 10;
            scoreText.text = score.ToString();
        //    Debug.Log("Kartlar eşleşti! Puan eklendi.");
            //DisableCards(cardData);
            cardCount -= 2;
            GameEnd(); // tüm kartların açılıp açılmadığını komtrol et
            
        }
        else
        {
          //  Debug.Log("Kartlar eşleşmedi.");
            cardData[0].isFlipped = false;
            cardData[1].isFlipped = false; // Kartlar eşleşmediğinde tekrar tıklanabilmesi için değeri false yap

            healthPoints--; // canını azalt eşleşme sağlanmadığında
            HPText.text = healthPoints.ToString();
            GameEnd();  // oyunun bitip bitmediğini kontrol et
            // Arka resimleri göstermek için coroutine'i çağır
            List<CardData> cardsCopy = new List<CardData>(cardData); 

            StartCoroutine(ShowBackImages(cardsCopy)); // Kopya listeyi kullanarak coroutine'i çağır
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
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
            AnimateCardFlip(card, false,button);
            // Butonun resmini backImage olarak ayarla
            //buttonImage.sprite = card.backImage;
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
        bigButtonRectTransform.sizeDelta = new Vector2(2000, 2000);

        // Butonu parent olarak ayarla
        bigButtonRectTransform.SetParent(buttonsParent);
        bigButtonRectTransform.localPosition = Vector3.zero; // Parent'ın pozisyonunu sıfırlayın
        bigButtonRectTransform.localScale = Vector3.zero; // Başlangıçta butonun ölçeğini sıfır yapıyoruz

        // Kart verisinin resmini butona ata
        bigButtonImage.sprite = cardData.cardImage;

        // Butonun pozisyonunu ayarla
        bigButtonRectTransform.anchoredPosition = Vector2.zero;

        // DoTween kullanarak butonu büyüt
        bigButtonRectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack); // Büyüme animasyonu 0.5 saniye sürecek

        // Butona tıklama olayı ata
        bigButton.onClick.AddListener(OnBigButtonClick);
    }

    void OnBigButtonClick()
    {
        // Kullanıcı büyük butona tıkladığında geri gitme işlemi burada gerçekleşir
        Destroy(bigButton.gameObject);
        bigButton.gameObject.SetActive(false);
    }
    void AnimateCardFlip(CardData cardData, bool isFlippingOpen, Button button)
    {
        // Image bileşenini alın
        Image buttonImage = button.GetComponent<Image>();

        // Kartı kapatmak veya açmak için kullanılacak hedef sprite
        Sprite targetSprite = isFlippingOpen ? cardData.cardImage : cardData.backImage;

        // İlk önce kartın X ölçeğini küçült
        button.transform.DOScaleX(0, 0.2f).OnComplete(() =>
        {
            // Ölçek küçüldükten sonra, kartın resmini değiştir
            buttonImage.sprite = targetSprite;

            // Kartın X ölçeğini tekrar eski haline getirin
            button.transform.DOScaleX(1, 0.2f).OnComplete(() =>
            {
                // Ölçeklendirme tamamlandıktan sonra, ölçeği kesin olarak ayarlayın
                button.transform.localScale = new Vector3(1, 1, 1);
            });
        });
    }

    void GameEnd()
    {
        if (healthPoints==0)
        {
            gameManager.GameLose(score);
        }else if(cardCount==0){
            gameManager.GameWin(score);
        }
    }

}

