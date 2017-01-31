using Assets.Scripts.GUI_scripts;
using Assets.Scripts.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class GUI_Controller : MonoBehaviour, ISubscriber
    {
        public HeroController heroController;
        public List<EnemyBarPop_up> enemy_bars;
        public UILabel heroHP_label;
        public UILabel hero_armour_label;
        public UISlider kills_slider;
        public UILabel kills_counter;
        public UILabel level_Title;
        public UILabel best_kills_counter;
        public UILabel best_kills_ever;

        public UISprite frstWeapon;
        public UISprite scndWeapon;
        public UIButton startNewGameButton;
        public UIPanel menuHUD;
        public UIPanel playHUD;
        int best_scores=0;
        int local_best_scores = 0;
        void Start()
        {
            var nc = FindObjectOfType<NotificationCenter>();
            nc.SetUpSubscriber(this);
            playHUD.gameObject.SetActive(false);
            menuHUD.gameObject.SetActive(true);

        }
        public void StartNewGame()
        {
            playHUD.gameObject.SetActive(true);
            menuHUD.gameObject.SetActive(false);
            heroController.ResetGame();
        }
        #region kills_UIlabel_string_builder
        int killsCNT = 0;
        int killsAIM = 5;

        StringBuilder kills_string = new StringBuilder("../..", 5);
        void setSBKills(int kills)
        {
            while (kills > 99)
            { kills /= 10; }

            kills_string[0] = ch_from_0_9_int((kills / 10));
            kills_string[1] = ch_from_0_9_int(kills % 10);
        }
        void setSBAim(int aim_kills)
        {
            //TODO: throw esception?
            while (aim_kills > 99)
            { aim_kills /= 10; }

            kills_string[3] = ch_from_0_9_int((aim_kills / 10));
            kills_string[4] = ch_from_0_9_int(aim_kills % 10);
        }
        char ch_from_0_9_int(int num)
        {
            if (num == 0)
            {
                return '0';
            }
            else if (num == 1)
            {
                return '1';
            }
            else if (num == 2)
            {
                return '2';
            }
            else if (num == 3)
            {
                return '3';
            }
            else if (num == 4)
            {
                return '4';
            }
            else if (num == 5)
            {
                return '5';
            }
            else if (num == 6)
            {
                return '6';
            }
            else if (num == 7)
            {
                return '7';
            }
            else if (num == 8)
            {
                return '8';
            }
            else if (num == 9)
            {
                return '9';
            }
            return '0';
        }
        #endregion kills_UIlabel_string_builder

        public void OnNotify(INotification notification)
        {
            #region OnEnemyDamagedNotification
            if (notification is OnEnemyDamagedNotification)
            {
                var n = notification as OnEnemyDamagedNotification;
                //check if current transform bar exists
                for (int i = 0; i < enemy_bars.Count; i++)
                    if (enemy_bars[i].gameObject.activeSelf && enemy_bars[i].follow_trans == n.damagedTarget)
                    {
                        enemy_bars[i].UpdateDamageInfo(n.targetStats, n.damageSuffer);
                        return;
                    }

                //spawn new bar
                for (int i = 0; i < enemy_bars.Count; i++)
                    if (!enemy_bars[i].gameObject.activeSelf)
                    {
                        enemy_bars[i].SpawnBar(n.damagedTarget, n.targetStats, n.damageSuffer);
                        return;
                    }
            }
            #endregion 
            #region OnEnemyKilledNotification
            else if (notification is OnEnemyKilledNotification)
            {
                var n = notification as OnEnemyKilledNotification;
                killsCNT++;
                setSBKills(killsCNT);
                kills_counter.text = kills_string.ToString();
                kills_slider.value = (float)killsCNT / killsAIM;
                local_best_scores++;
                best_kills_counter.text = local_best_scores.ToString();
            }
            #endregion 
            #region OnHeroLevelUP
            else if (notification is OnHeroLevelUP)
            {
                var n = notification as OnHeroLevelUP;
                killsCNT = 0;
                killsAIM = n.NextLevelTargetKills;
                setSBKills(killsCNT);
                setSBAim(killsAIM);
                kills_counter.text = kills_string.ToString();
                level_Title.text = "Level " + (n.achievedLevel + 1).ToString();
                kills_slider.value = (float)killsCNT / killsAIM;
            }
            #endregion
            #region OnHeroChangeStatsNotification

            else if (notification is OnHeroChangeStatsNotification)
            {
                var n = notification as OnHeroChangeStatsNotification;
                heroHP_label.text = n.heroStats.CurrentHP.ToString("0") + "/"
                    + n.heroStats.HealthPoints.ToString("0");
                foreach(var tw in heroHP_label.GetComponents<UITweener>())
                {
                    tw.ResetToBeginning();
                    tw.PlayForward();
                }
                hero_armour_label.text = (n.heroStats.Armour*100).ToString() + "%";
            }
            #endregion
            #region OnWeaponChangedNotification
            else if (notification is OnWeaponChangedNotification)
            {
                var n =notification as  OnWeaponChangedNotification;
                if (n.activeWeaponIndex == 0)
                {
                    foreach (var tw in frstWeapon.GetComponents<UITweener>())
                    {
                        tw.PlayForward();
                    }
                    foreach (var tw in scndWeapon.GetComponents<UITweener>())
                    {
                        tw.PlayReverse();
                    }
                }
                else if (n.activeWeaponIndex == 1)
                {
                    foreach (var tw in scndWeapon.GetComponents<UITweener>())
                    {
                        tw.PlayForward();
                    }
                    foreach (var tw in frstWeapon.GetComponents<UITweener>())
                    { 
                        tw.PlayReverse();
                    }
                }
            }
            #endregion
            #region OnEndGameNotification

            else if (notification is OnEndGameNotification)
            {
                var n = notification as OnEndGameNotification;
                best_scores = best_scores > local_best_scores ? best_scores : local_best_scores;
                best_kills_counter.text = local_best_scores.ToString();
                best_kills_ever.text = best_scores.ToString();
                local_best_scores = 0;
                playHUD.gameObject.SetActive(false);
                menuHUD.gameObject.SetActive(true);
            }
            else if (notification is OnRestartGameNotification)
            {
                best_scores = best_scores > local_best_scores ? best_scores : local_best_scores;
                best_kills_counter.text = local_best_scores.ToString();
                best_kills_ever.text = best_scores.ToString();
                local_best_scores = 0;
                StartNewGame();
            }
            #endregion
        }
    }
}
