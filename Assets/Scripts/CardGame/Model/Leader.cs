using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CardGame
{
    public class Leader : IDisposable
    {
        private const int _startLife = 7777;
        private ReactiveProperty<string> _name;
        private ReactiveProperty<int> _life;
        private ReactiveProperty<int> _level;
        private ReactiveProperty<int> _exp;
        private ReactiveProperty<int> _requireExp;
        private ReactiveProperty<int> _maxCost;

        public ReadOnlyReactiveProperty<string> Name => _name;
        public ReadOnlyReactiveProperty<int> Life => _life;
        public ReadOnlyReactiveProperty<int> Level => _level;
        public ReadOnlyReactiveProperty<int> Exp => _exp;
        public ReadOnlyReactiveProperty<int> RequireExp => _requireExp;//先がない場合は-1
        public ReadOnlyReactiveProperty<int> MaxCost => _maxCost;
        public CardCol[] Colors { get; private set; }
        public Sprite Chara_sprite { get; private set; }
        private LeaderStage[] _stages;
        private LeaderStage _nowStage;

        public Leader(string name, Sprite chara, LeaderStage[] stages, CardCol[] colors)
        {
            _name = new(name + " lv0");
            _life = new(_startLife);
            _level = new(0);
            _stages = stages;
            _exp = new(0);
            _requireExp = new(_stages[0].RequireExp);
            _maxCost = new(_stages[0].MaxCost);
            Chara_sprite = chara;
            _nowStage = _stages[0];
            Colors = colors;
        }


        public void GetExp(int num)
        {
            _exp.Value += num;
            if (_exp.Value >= _nowStage.RequireExp)
            {
                //レベルアップ先があるかどうか
                if (_level.Value < _stages.Length - 1)
                {
                    _level.Value++;
                    _exp.Value -= _nowStage.RequireExp;
                    _nowStage = _stages[_level.Value];
                    _maxCost.Value = _nowStage.MaxCost;

                    _requireExp.Value = (_level.Value == _stages.Length - 1) ? -1 : _nowStage.RequireExp;
                }
            }
        }

        public void Dispose()
        {
            _name.Dispose();
            _life.Dispose();
            _level.Dispose();
            _exp.Dispose();
            _requireExp.Dispose();
        }
    }
}
