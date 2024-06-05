using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class AI : MonoBehaviour
{
    private GamePlayerManager player = null;
    private GamePlayerManager enemy = null;
    private Subject<Unit> turnEnd = new Subject<Unit>();
    public Observable<Unit> TurnEnd => turnEnd;

    public void Init(GamePlayerManager p, GamePlayerManager e)
    {
        player = p;
        enemy = e;
    }

    public IEnumerator EnemyrTurn()
    {
        Debug.Log("enemy turn");
        //相手側のフィールドカードを攻撃表示にする。
        CardController[] enemyFieldCardList = enemy.GetFieldCards();
        yield return new WaitForSeconds(0.5f);
        /*場にカードを出す*/
        //手札のリストを取得
        CardController[] handcardList = enemy.GetHandCards();
        //条件：モンスターカードならコストのみ
        //条件：スペルカードはコストと使用可能かどうか CanSpellUse
        //コスト以下のカードがあれば、カードをフィールドに出し続ける。 モンスターでなければ
        while (Array.Exists(handcardList, card =>
        (card.model.cost <= enemy.ManaCost.CurrentValue)//コスト以下で勝つ
        && (!card.IsSpell||(card.IsSpell&&card.CanUseSpell()))))//スペルじゃない＝モンスター　あるいは　使用可能なスペルカード
        {
            //その中から出せるカードを取得
            CardController[] selectableHandCardList = Array.FindAll(handcardList, card =>(card.model.cost <= enemy.ManaCost.CurrentValue) && (!card.IsSpell || (card.IsSpell && card.CanUseSpell())));

            //場に出すカードを選択
            CardController selectCard = selectableHandCardList[0];
            //裏パネルを外す
            selectCard.Show();

            // スペルカードならばこの時点で使用
            if (selectCard.IsSpell)
            {
                StartCoroutine(CastSpellOf(selectCard));
            }
            else//違ったらモンスタなので移動
            {
                //カードを移動
                StartCoroutine(selectCard.movement.MoveToField(enemy.Field));
                selectCard.OnField();
            }
            //１まい無くした状態にする
            yield return new WaitForSeconds(0.5f);
            handcardList = enemy.GetFieldCards();
        }


        yield return new WaitForSeconds(1);
        
        /*攻撃*/
        //攻撃可能カードがあれば攻撃を繰り返す。
        while (Array.Exists(enemyFieldCardList, card => card.model.canAttack.Value))　//Array.Find(これの中から,これ（新しいりすと）にします=>こんな感じのものを探してきて);
        {
            //Debug.Log("攻撃！");
            //攻撃可能カードを取得
            CardController[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardList, card => card.model.canAttack.Value);
            //自分側のフィールドのカードリストを取得
            CardController[] playerFieldCardList = player.GetFieldCards();

            //アタッカーカードを選択
            CardController attacker = enemyCanAttackCardList[0];
            if (playerFieldCardList.Length > 0)
            {
                //Debug.Log("モンスターに");
                //ディフェンダーカードを選択
                //シールドカードがあればシールドカードだけ
                if (Array.Exists(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD))
                {
                    playerFieldCardList = Array.FindAll(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD);
                }
                CardController defender = playerFieldCardList[0];
                //battle
                StartCoroutine(attacker.movement.MoveToTarget(defender.transform));
                yield return new WaitForSeconds(0.25f);
                GameManager.I.CardsBattle(attacker, defender);
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                //Debug.Log("プレイヤーに");
                StartCoroutine(attacker.movement.MoveToTarget(player.Icon));//motoha GameManager.I.heroTransform
                yield return new WaitForSeconds(0.25f);
                GameManager.I.AttackToHero(attacker);
                yield return new WaitForSeconds(0.25f);
            }
            enemyFieldCardList = enemy.GetFieldCards();
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1);
        //ターンエンド
        turnEnd.OnNext(Unit.Default);
    }

    IEnumerator CastSpellOf(CardController card)
    {
        CardController target = null;
        Transform movePosition = null;
        switch (card.model.spell)
        {
            //単体
            case SPELL.DAMAGE_ENEMY_CARD:
                target = player.GetFieldCards()[0];
                movePosition = target.transform;
                break;
            case SPELL.HEAL_FRIEND_CARD:
                target = player.GetFieldCards()[0];
                movePosition = target.transform;
                break;
            //全体
            case SPELL.DAMAGE_ENEMY_CARDS:
                movePosition = player.Field;
                break;
            case SPELL.HEAL_FRIEND_CARDS:
                movePosition = enemy.Field;
                break;
            //HERO
            case SPELL.DAMAGE_ENEMY_HERO:
                movePosition = player.Icon;
                break;
            case SPELL.HEAL_FRIEND_HERO:
                movePosition = enemy.Icon;
                break;
            //Draw
            case SPELL.DRAW_CARD:
            case SPELL.DRAW_2CARD:
                movePosition = enemy.Field;
                break;
            //Destroy
            case SPELL.DESTROY_ENEMY_CARD:
                target = player.GetFieldCards()[0];
                movePosition = target.transform;
                break;
        }
        if (card.model.spell == SPELL.HEAL_FRIEND_CARD)
        {
            target = enemy.GetFieldCards()[0];
        }
        if (card.model.spell == SPELL.DAMAGE_ENEMY_CARD||card.model.spell==SPELL.DESTROY_ENEMY_CARD)
        {
            target = player.GetFieldCards()[0];
        }
        //ターゲット　それぞれのフィールド
        StartCoroutine(card.movement.MoveToField(movePosition));
        yield return new WaitForSeconds(0.26f);
        //コントローラーのやつを使う
        card.UseSpellTo(target);
    }
}
