using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CardList : MonoBehaviour
{
    public List<CardData> cards = new List<CardData>(); // Tüm kartların listesi

    // Kartların resimleri dizisi
    public Sprite[] cardImages;

    // Kartların arka resmi
    public Sprite backImage;

    void Start()
    {
        // Kartları oluşturmak için fonksiyonu çağır
        //CreateCards(16); // Örnek olarak 16 kart oluşturduk
    }
    public void CreateCards(int count)
    {
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
            newCard.Initialize(index, cardImages[index].name, cardImages[index], backImage);
            newCard2.Initialize(index, cardImages[index].name, cardImages[index], backImage);

            // Oluşturulan kartları listeye ekle
            cards.Add(newCard);
            cards.Add(newCard2);
        }
    }


   /* public void CreateCards(int count)
    {
        // İstenen sayıda kart ekleyelim
        for (int i = 0; i < count/2; i++)
        {
            // Yeni bir kart oluştur
            CardData newCard = ScriptableObject.CreateInstance<CardData>();
            CardData newCard2 = ScriptableObject.CreateInstance<CardData>();

            // Kartın özelliklerini belirle
            newCard.Initialize(i, cardImages[i].name, cardImages[i], backImage);
            newCard2.Initialize(i, cardImages[i].name, cardImages[i], backImage);


            // Oluşturulan kartı listeye ekle
            cards.Add(newCard);
            cards.Add(newCard2);
        }
    } */
}
