using System;
using UnityEngine;
using System.Collections.Generic;
using Manager;
using Random = UnityEngine.Random;

public class CardList : MonoBehaviour
{
    private int cardImageType;
    public List<CardData> cards = new List<CardData>(); // Tüm kartların listesi

    // Kartların resimleri dizisi
    public Sprite[] cardImages2;
    public Sprite[] cardImages3;
    public Sprite[] cardImages4;
    public Sprite[] cardImages5;
    public Sprite[] cardImages6;

    // Kartların arka resmi
    
    public Sprite backImage2;
    public Sprite backImage3;
    public Sprite backImage4;
    public Sprite backImage5;
    public Sprite backImage6;
    
    // Kartların resimlerini tutacak sözlük
    public Dictionary<string, Sprite[]> cardImageDictionary = new Dictionary<string, Sprite[]>();

    // Kartların arka resmini tutacak sözlük
    public Dictionary<string, Sprite> backImageDictionary = new Dictionary<string, Sprite>();
    
    void Awake()
    {
        // Her bir kart seti için resim dizilerini ve arka plan resmini tanımla
        cardImageDictionary.Add("Set2", cardImages2);
        backImageDictionary.Add("Set2", backImage2);
        
        cardImageDictionary.Add("Set3", cardImages3);
        backImageDictionary.Add("Set3", backImage3);
        
        cardImageDictionary.Add("Set4", cardImages4);
        backImageDictionary.Add("Set4", backImage4);

        // Daha fazla kart seti eklemek için aynı şekilde devam edebilirsiniz
    }
    
    /* public void CreateCards(int count,string imageSetName)
     {
         // Kullanıcı tarafından belirtilen isme göre kart resimlerini al
         Sprite[] selectedCardImages = cardImageDictionary[imageSetName];
 
         // Kullanıcı tarafından belirtilen isme göre arka resmi al
         Sprite selectedBackImage = backImageDictionary[imageSetName];
         
         // Rastgele seçilecek kart sayısını belirle
         int maxIndex = 15;
         int[] selectedIndices = new int[count / 2]; // İki kart için aynı resim kullanılacağı için count/2
 
         // Rastgele seçilen kart indekslerini belirle
         for (int i = 0; i < selectedIndices.Length; i++)
         {
             int randomIndex;
             do
             {
                 randomIndex = Random.Range(0, maxIndex); // 0 ile maxIndex arasında rastgele bir sayı seç
             } while (Array.IndexOf(selectedIndices, randomIndex) != -1); // Seçilen indeks daha önce seçilmiş mi kontrol et
 
             selectedIndices[i] = randomIndex; // Seçilen indeksi diziye ekle
         }
 
         // İstenen sayıda kart ekleyelim
         foreach (int index in selectedIndices)
         {
             // Yeni kartları oluştur
             CardData newCard = ScriptableObject.CreateInstance<CardData>();
             CardData newCard2 = ScriptableObject.CreateInstance<CardData>();
 
             // Kartların özelliklerini belirle
             newCard.Initialize(index, selectedCardImages[index].name, selectedCardImages[index], selectedBackImage);
             newCard2.Initialize(index, selectedCardImages[index].name, selectedCardImages[index],selectedBackImage);
 
             // Oluşturulan kartları listeye ekle
             cards.Add(newCard);
             cards.Add(newCard2);
         }
     } */


   /* public void CreateCards(int count)
    {
        // İstenen sayıda kart ekleyelim
        for (int i = 0; i < count/2; i++)
        {
            // Yeni bir kart oluştur
            CardData newCard = ScriptableObject.CreateInstance<CardData>();
            CardData newCard2 = ScriptableObject.CreateInstance<CardData>();

            // Kartın özelliklerini belirle
            newCard.Initialize(i, cardImages4[i].name, cardImages4[i], backImage2);
            newCard2.Initialize(i, cardImages4[i].name, cardImages4[i], backImage2);


            // Oluşturulan kartı listeye ekle
            cards.Add(newCard);
            cards.Add(newCard2);
        }
    } */
   
   public void CreateCards(int count, int cardTheme)
   {
       // İstenen sayıda kart ekleyelim
       for (int i = 0; i < count / 2; i++)
       {
           // Yeni bir kart oluştur
           CardData newCard = ScriptableObject.CreateInstance<CardData>();
           CardData newCard2 = ScriptableObject.CreateInstance<CardData>();

           // Kartın özelliklerini belirle
           switch (cardTheme)
           {
               case 0:
                   newCard.Initialize(i, cardImages2[i].name, cardImages2[i], backImage2);
                   newCard2.Initialize(i, cardImages2[i].name, cardImages2[i], backImage2);
                   break;
               case 1:
                   newCard.Initialize(i, cardImages3[i].name, cardImages3[i], backImage2);
                   newCard2.Initialize(i, cardImages3[i].name, cardImages3[i], backImage2);
                   break;
               case 2:
                   newCard.Initialize(i, cardImages4[i].name, cardImages4[i], backImage2);
                   newCard2.Initialize(i, cardImages4[i].name, cardImages4[i], backImage2);
                   break;
               case 4:
                   newCard.Initialize(i, cardImages5[i].name, cardImages5[i], backImage2);
                   newCard2.Initialize(i, cardImages5[i].name, cardImages5[i], backImage2);
                   break;
               case 3:
                   newCard.Initialize(i, cardImages6[i].name, cardImages6[i], backImage2);
                   newCard2.Initialize(i, cardImages6[i].name, cardImages6[i], backImage2);
                   break;
               default:
                   newCard.Initialize(i, cardImages2[i].name, cardImages2[i], backImage2);
                   newCard2.Initialize(i, cardImages2[i].name, cardImages2[i], backImage2);
                   break;
           }

           // Oluşturulan kartı listeye ekle
           cards.Add(newCard);
           cards.Add(newCard2);
       }
   }

}
