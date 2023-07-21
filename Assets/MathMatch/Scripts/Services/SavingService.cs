using System.Collections.Generic;
using KlopoffGames.Core.Saving;
using UnityEngine;
using Zenject;

namespace MathMatch.Services
{
    public class SavingService
    {
        [Inject] private ISavingManager _saving;
        
        private string[] GetAvailableKeys()
        {
            var keys = new List<string>();
            keys.Add("is_app_share_disabled");
            keys.Add("is_ftue");
            keys.Add("music_volume");
            keys.Add("sound_volume");
            keys.Add("game_run_counter");
            keys.Add("best_score");
            return keys.ToArray();
        }
        
        public bool GetIsAppShareDisabled()
        {
            return _saving.GetBool("is_app_share_disabled", false);
        }

        public void SetIsAppShareDisabled(bool value)
        {
            _saving.SetBool("is_app_share_disabled", value);
        }

        public bool GetIsFTUE()
        {
            return _saving.GetBool("is_ftue", true);
        }

        public void SetIsFTUE(bool value)
        {
            _saving.SetBool("is_ftue", value);
        }
        
        public int GetGameRunCounter()
        {
            return _saving.GetInt("game_run_counter", 0);
        }

        public void SetGameRunCounter(int counter)
        {
            _saving.SetInt("game_run_counter", counter);
        }

        public float GetMusicVolume()
        {
            return _saving.GetFloat("music_volume", 1f);
        }

        public void SetMusicVolume(float volume)
        {
            _saving.SetFloat("music_volume", volume);
        }

        public float GetSoundVolume()
        {
            return _saving.GetFloat("sound_volume", 1f);
        }

        public void SetSoundVolume(float volume)
        {
            _saving.SetFloat("sound_volume", volume);
        }

        public int GetBestScore()
        {
            return _saving.GetInt("best_score", 0);
        }

        public void SetBestScore(int coins)
        {
            _saving.SetInt("best_score", coins);
        }
    }
}
