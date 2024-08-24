using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

namespace CardGame
{
    public class CardRepository
    {
        private List<CardEntity> _entities;
        public async UniTask<Card> Get(int id)
        {
            if (_entities == null)
            {
                var sol = new ScriptableObjectLoader<CardEntity>();
                _entities = await sol.LoadAll("Card");
            }
            //エンティティを取ってくる
            var e = _entities.Where(e => e.ID == id).FirstOrDefault();
            //ゆくゆくは翻訳家を通す
            var c = new Card(e.ID, e.Name, e.Cost, e.Power, e.Sprite_);
            return c;
        }
    }
}
