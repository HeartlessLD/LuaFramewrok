using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class MusicManager : MonoBehaviour
    {
        private AudioSource audio;
        private Hashtable sounds = new Hashtable();
        
        // Use this for initialization
        void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        void Add(string key, AudioClip value)
        {
            sounds.Add(key, value);
        }

        AudioClip Get(string key)
        {
            if (sounds[key] == null)
                return null;
            return sounds[key] as AudioClip;
        }

        public AudioClip LoadAudioClip(string path)
        {
            AudioClip ac = Get(path);
            if(ac == null)
            {
                ac = Resources.Load<AudioClip>(path);
                Add(path, ac);
            }
            return ac;
        }
        //是否播放背景音乐
        public bool CanPlayBackSound()
        {
            string key = Const.AppPrefix + "BackSound";
            int i = PlayerPrefs.GetInt(key, 1);
            return i == 1;
        }

        public void PlayBacksound(string name, bool canPlay)
        {
       
            if (canPlay)
            {
                audio.loop = true;
                audio.clip = LoadAudioClip(name);
                audio.Play();
            }
            else
            {
                audio.Stop();
                audio.clip = null;
                Util.ClearMemory();
            }
        }

        public bool CanPlaySoundEffect()
        {
            string key = Const.AppPrefix + "SoundEffect";
            int i = PlayerPrefs.GetInt(key, 1);
            return i == 1;
        }
        public void Play(AudioClip clip, Vector3 position)
        {
            if (!CanPlaySoundEffect()) return;
            AudioSource.PlayClipAtPoint(clip, position);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}

