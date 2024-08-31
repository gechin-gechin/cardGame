using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace CardGame
{
    public class LeaderRepository
    {
        private List<LeaderEntity> _entities;
        private LeaderTranslator _translator;
        public LeaderRepository()
        {
            _translator = new();
        }
        public async UniTask<Leader> GetByID(int id)
        {
            if (_entities == null)
            {
                var sol = new ScriptableObjectLoader<LeaderEntity>();
                _entities = await sol.LoadAll("Leader");
            }
            //エンティティを取ってくる
            var e = _entities.Where(e => e.ID == id).FirstOrDefault();
            //ゆくゆくは翻訳家を通す
            return _translator.EntityToLeader(e);
        }
    }
}
