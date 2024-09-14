using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

namespace CardGame
{
    public class CardRepository
    {
        private static List<CardEntity> _entities;
        private static CardTranslator _translator;
        public CardRepository()
        {
            if (_translator == null)
            {
                _translator = new();
            }
        }
        public async UniTask<Card> GetByID(int id)
        {
            if (_entities == null)
            {
                var sol = new ScriptableObjectLoader<CardEntity>();
                _entities = await sol.LoadAll("Card");
            }
            //エンティティを取ってくる
            var e = _entities.Where(e => e.ID == id).FirstOrDefault();
            //ゆくゆくは翻訳家を通す
            return _translator.EntityToCard(e);
        }
    }
}
