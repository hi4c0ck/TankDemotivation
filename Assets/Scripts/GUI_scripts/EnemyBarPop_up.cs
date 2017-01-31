using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GUI_scripts
{
    public class EnemyBarPop_up : MonoBehaviour
    {
        public UISlider hp_slider;
        public UISprite suffereedSprite;
        public UILabel armourValue;
        public Transform follow_trans;
        public Camera UI_Camera;
        public float visible_timer = 3f;
        float vis_timer = 0;
        UIRoot root;
        void Start()
        {
            root = NGUITools.GetRoot(gameObject).GetComponent<UIRoot>();
        }

        public void SpawnBar(Transform follow, GameStats stats,float sufferedDamage)
        {
            gameObject.SetActive(true);
            follow_trans = follow;
            UpdateDamageInfo(stats, sufferedDamage);
        }
        float currentSuffer;
        float currentHealth;
        GameStats stats;
        public void UpdateDamageInfo(GameStats stats, float sufferedDamage)
        {
            currentSuffer = sufferedDamage;
            this.stats = stats;
        }

        Vector3 adjustVector =Vector3.zero;
        void Update()
        {
            vis_timer+=Time.deltaTime;
            if (vis_timer>visible_timer || !follow_trans.gameObject.activeSelf)
            {
                vis_timer = 0;
                gameObject.SetActive(false);
                return;
            }

            hp_slider.value = stats.CurrentHP / stats.HealthPoints;
            suffereedSprite.fillAmount = (stats.CurrentHP + currentSuffer * (1 - vis_timer * 1.2f / visible_timer))
                /stats.HealthPoints;

            transform.localPosition = UI_Camera.ViewportToScreenPoint(
                UI_Camera.WorldToViewportPoint(follow_trans.localPosition + Vector3.up * 1.5f))
                / root.pixelSizeAdjustment;
        }
    }
}
