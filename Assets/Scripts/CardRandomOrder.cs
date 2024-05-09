using UnityEngine;
using System.Collections.Generic;

public class CardRandomOrder : MonoBehaviour
{
    public CardList originalCardList; // Orjinal kart listesi
    public CardList shuffledCardList; // Karışık kart listesi

    public int numberOfCardsToShuffle = 16; // Karıştırılacak kart sayısı

    void Start()
    {
        // Kartları rastgele sıraya göre yeniden düzenle
        ShuffleCards();
    }

    void ShuffleCards()
    {
        List<CardData> tempCardList = new List<CardData>(originalCardList.cards);

        // Belirlenen sayıda kartı rastgele seç ve yeni listeye ekle
        List<CardData> selectedCards = SelectRandomCards(tempCardList, numberOfCardsToShuffle);

        // Seçilen kartları karışık kart listesine ata
        shuffledCardList.cards = selectedCards;
    }

    List<CardData> SelectRandomCards(List<CardData> sourceList, int numberOfCards)
    {
        List<CardData> selectedCards = new List<CardData>();

        // Belirlenen sayıda kartı rastgele seç ve yeni listeye ekle
        for (int i = 0; i < numberOfCards; i++)
        {
            // Rastgele bir indeks seç
            int randomIndex = Random.Range(0, sourceList.Count);

            // Seçilen kartı yeni listeye ekle
            selectedCards.Add(sourceList[randomIndex]);
        }

        return selectedCards;
    }
}

