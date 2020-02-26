using Gululu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gululiu.Hank.Common
{
    public enum eAuioType
    {
        FeedBG = 0,
        FeedSpendCoin = 1,
        SwitchScene = 2,
        FeedHpIncrease = 3,
        NationalDayBg = 4,
        HalloweenBg = 5,
        Denmark = 6,
        Christmas = 7
    }

    public class AudioManager : BaseView
    {
        [Inject]
        public IResourceUtils ResourceUtils { get; set; }

        private static AudioManager mInstance;

        public static AudioManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = GameObject.FindObjectOfType<AudioManager>();
                }
                return mInstance;
            }
        }

        private Dictionary<int, string> mAudioPathDict;

        private AudioSource musicAudioSource;

        private List<AudioSource> unusedSoundAudioSourceList; // 存放可以使用的音频组件

        private List<AudioSource> usedSoundAudioSourceList; // 存放正在使用的音频组件

        private Dictionary<int, AudioClip> audioClipDict; // 缓存音频文件

        private float musicVolume = 1;

        private float soundVolume = 1;

        private string musicVolumePrefs = "MusicVolume";

        private string soundVolumePrefs = "SoundVolume";

        private int poolCount = 3; // 对象池数量

        protected override void Awake()
        {
            base.Awake();
            mAudioPathDict = new Dictionary<int, string>() // 这里设置音频文件路径。需要修改。 TODO
            {
                {0, "feed_scene_bgm"},
                {1, "coin_spend"},
                {2, "main_scene_wheel"},
                {3, "feed_hp_increase"},
                {4, "feed_bgm_china"},
                {5, "feed_bgm_halloween"},
                {6, "feed_bgm_denmark"},
                {7, "feed_bgm_xmas"}
            };

            musicAudioSource = gameObject.AddComponent<AudioSource>();
            unusedSoundAudioSourceList = new List<AudioSource>();
            usedSoundAudioSourceList = new List<AudioSource>();
            audioClipDict = new Dictionary<int, AudioClip>();
        }

        protected override void Start()
        {
            base.Start();

            // 从本地缓存读取声音音量
            if (PlayerPrefs.HasKey(musicVolumePrefs))
            {
                musicVolume = PlayerPrefs.GetFloat(musicVolumePrefs);
            }

            if (PlayerPrefs.HasKey(soundVolumePrefs))
            {
                musicVolume = PlayerPrefs.GetFloat(soundVolumePrefs);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loop"></param>
        public void PlayMusic(int id, bool loop = true)
        {
            this.StartCoroutine(this.GetAudioClip(id, (audioClip) =>
            {
                musicAudioSource.clip = audioClip;
                musicAudioSource.clip.LoadAudioData();
                musicAudioSource.loop = loop;
                musicAudioSource.volume = musicVolume;
                musicAudioSource.Play();
            }));
        }

        public void StopMusic()
        {
            musicAudioSource.Stop();
            musicAudioSource.clip = null;
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="id"></param>
        public void PlaySound(int id)
        {
            if (!this.gameObject.activeInHierarchy)
                return;

            if (unusedSoundAudioSourceList.Count != 0)
            {
                this.StartCoroutine(this.GetAudioClip(id, (audioClip) =>
                {
                    AudioSource audioSource = UnusedToUsed();
                    if (audioSource != null)
                    {
                        audioSource.clip = audioClip;
                        audioSource.clip.LoadAudioData();
                        audioSource.Play();
                        StartCoroutine(WaitPlayEnd(audioSource));
                    }
                }));
            }
            else
            {
                this.StartCoroutine(this.GetAudioClip(id, (audioClip) =>
                {
                    AddAudioSource();
                    AudioSource audioSource = UnusedToUsed();
                    if (audioSource != null)
                    {
                        audioSource.clip = audioClip;
                        audioSource.clip.LoadAudioData();
                        audioSource.volume = soundVolume;
                        audioSource.loop = false;
                        audioSource.Play();
                        StartCoroutine(WaitPlayEnd(audioSource));
                    }
                }));
            }
        }

        /// <summary>
        /// 当播放音效结束后，将其移至未使用集合
        /// </summary>
        /// <param name="audioSource"></param>
        /// <returns></returns>
        IEnumerator WaitPlayEnd(AudioSource audioSource)
        {
            yield return new WaitUntil(() => { return !audioSource.isPlaying; });
            UsedToUnused(audioSource);
        }

        /// <summary>
        /// 获取音频文件，获取后会缓存一份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerator GetAudioClip(int id, System.Action<AudioClip> callback)
        {
            if (!audioClipDict.ContainsKey(id))
            {
                yield return this.StartCoroutine(this.ResourceUtils.LoadAudio(string.Format("Audio/Common/CHS/{0}.mp3", mAudioPathDict[id]), (audioClip) =>
                {
                    if (!audioClipDict.ContainsKey(id))
                        audioClipDict.Add(id, audioClip);

                    if (callback != null)
                        callback(audioClip);
                }));
            }
            else if (callback != null)
            {
                callback(audioClipDict[id]);
            }
        }

        /// <summary>
        /// 添加音频组件
        /// </summary>
        /// <returns></returns>
        private AudioSource AddAudioSource()
        {
            if (unusedSoundAudioSourceList.Count != 0)
            {
                return UnusedToUsed();
            }
            else
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                unusedSoundAudioSourceList.Add(audioSource);
                return audioSource;
            }
        }

        /// <summary>
        /// 将未使用的音频组件移至已使用集合里
        /// </summary>
        /// <returns></returns>
        private AudioSource UnusedToUsed()
        {
            if (unusedSoundAudioSourceList != null && unusedSoundAudioSourceList.Count > 0)
            {
                AudioSource audioSource = unusedSoundAudioSourceList[0];
                unusedSoundAudioSourceList.RemoveAt(0);
                usedSoundAudioSourceList.Add(audioSource);
                return audioSource;
            }
            else return null;
        }

        /// <summary>
        /// 将使用完的音频组件移至未使用集合里
        /// </summary>
        /// <param name="audioSource"></param>
        private void UsedToUnused(AudioSource audioSource)
        {
            usedSoundAudioSourceList.Remove(audioSource);
            if (unusedSoundAudioSourceList.Count >= poolCount)
            {
                Destroy(audioSource);
            }
            else
            {
                unusedSoundAudioSourceList.Add(audioSource);
            }
        }

        /// <summary>
        /// 修改背景音乐音量
        /// </summary>
        /// <param name="volume"></param>
        public void ChangeMusicVolume(float volume)
        {
            musicVolume = volume;
            musicAudioSource.volume = volume;

            PlayerPrefs.SetFloat(musicVolumePrefs, volume);
        }

        /// <summary>
        /// 修改音效音量
        /// </summary>
        /// <param name="volume"></param>
        public void ChangeSoundVolume(float volume)
        {
            soundVolume = volume;

            foreach (var t in unusedSoundAudioSourceList)
            {
                t.volume = volume;
            }

            foreach (var t in usedSoundAudioSourceList)
            {
                t.volume = volume;
            }

            PlayerPrefs.SetFloat(soundVolumePrefs, volume);
        }
    }
}
