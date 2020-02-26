using DG.Tweening;
using Gululu;
using Gululu.Config;
using Gululu.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hank
{
    public class SoundPlayer : BaseView
    {
        [Inject]
        public ISoundConfig soundConifg { get; set; }
        [Inject]
        public IPrefabRoot mPrefabRoot { get; set; }
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        [Inject]
        public IResourceUtils ResourceUtils { get; set; }
        [SerializeField]
        private AudioSource musicSource, roleSource;
        [SerializeField]
        private AudioSource[] audioSources;
        [SerializeField]
        private List<AudioSource> outerAudioSources = new List<AudioSource>();
        private bool bActive = true;
        private DateTime lastGuide = DateTime.Now;
        private Coroutine delayCallbackCoroutine;
        private Dictionary<string, AudioClipData> audioClipBuffer = new Dictionary<string, AudioClipData>();
        private List<Tween> tweens = new List<Tween>();
        private static SoundPlayer instance;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
            this.InvokeRepeating("GC", 5f, 0.1f);
        }

        public override void TestSerializeField()
        {
            Assert.IsNotNull(musicSource);
            Assert.IsNotNull(roleSource);
            for (int i = 0; i < audioSources.Length; ++i)
            {
                Assert.IsNotNull(audioSources[i]);
            }
        }
        public static SoundPlayer GetInstance()
        {
            return instance;
        }

        public void PlaySoundType(string type)
        {
            if (!bActive)
            {
                return;
            }
            PlaySound(type);
        }
        public void PlaySoundBackDoor(string type)
        {
            PlaySound(type);
        }
        public void PlaySoundRepeated(string type)
        {
            PlaySound(type, true);
        }
        public void PlayRoleSound(string type)
        {//rolesource to play only one role sound at the same time
            if (!bActive || soundConifg == null || roleSource.isPlaying)
                return;

            SoundClip soundClip = soundConifg.GetSoundClip(type);
            if (soundClip != null && !string.IsNullOrEmpty(soundClip.SoundPath))
            {
                GuLog.Debug("<><SoundPlayer> PlayRoleSound:   " + soundClip);
                PlaySoundWithChannel(soundClip, roleSource);
            }

        }
        public void PlaySoundInChannal(string type, AudioSource audioSource, float maskVolume = 1f, System.Action callback = null)
        {
            if (soundConifg == null)
            {
                return;
            }

            SoundClip soundClip = soundConifg.GetSoundClip(type);
            if (soundClip != null && !string.IsNullOrEmpty(soundClip.SoundPath))
            {
                GuLog.Debug("<><SoundPlayer> PlaySoundInChannal:   " + soundClip);
                this.SetAllSoundVolume(maskVolume);
                this.PlaySoundWithChannel(soundClip, audioSource);
                if (this.delayCallbackCoroutine != null)
                    this.StopCoroutine(this.delayCallbackCoroutine);
                this.delayCallbackCoroutine = this.StartCoroutine(this.DelayCallback(audioSource, callback));
            }
        }
        public void PlayMusic(string type)
        {
            if (soundConifg == null)
            {
                return;
            }
            SoundClip soundClip = soundConifg.GetSoundClip(type);
            if (!string.IsNullOrEmpty(soundClip.SoundPath))
            {
                GuLog.Debug("<><SoundPlayer> MusicName:   " + soundClip);
                PlaySoundWithChannel(soundClip, musicSource);
            }
        }
        public void SetAllSoundVolume(float volume, bool immediately = false)
        {
            for (int i = 0; i < this.tweens.Count; i++)
                this.tweens[i].Kill();
            this.tweens.Clear();

            volume = Mathf.Clamp01(volume);
            if (immediately)
            {
                foreach (AudioSource audioSource in this.audioSources)
                {
                    audioSource.volume = volume;
                }
                this.musicSource.volume = volume;
                this.roleSource.volume = volume;
            }
            else
            {
                float lerpSpeed = volume == 1 ? 1.5f : 0.5f;
                foreach (AudioSource audioSource in this.audioSources)
                {
                    Tween t = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, volume, lerpSpeed);
                    if (!this.tweens.Contains(t)) this.tweens.Add(t);
                }
                Tween t1 = DOTween.To(() => this.musicSource.volume, x => this.musicSource.volume = x, volume, lerpSpeed);
                if (!this.tweens.Contains(t1)) this.tweens.Add(t1);
                Tween t2 = DOTween.To(() => this.roleSource.volume, x => this.roleSource.volume = x, volume, lerpSpeed);
                if (!this.tweens.Contains(t2)) this.tweens.Add(t2);
            }
        }
        public void StopAllSound()
        {
            for (int i = 0; i < audioSources.Length; ++i)
            {
                AudioSource source = audioSources[i];
                source.Stop();
            }
            musicSource.Stop();
            roleSource.Stop();
        }
        public void StopAllSoundExceptRole()
        {
            for (int i = 0; i < audioSources.Length; ++i)
            {
                AudioSource source = audioSources[i];
                source.Stop();
            }
            musicSource.Stop();
        }
        public void StopAllSoundExceptMusic()
        {
            for (int i = 0; i < audioSources.Length; ++i)
            {
                AudioSource source = audioSources[i];
                source.Stop();
            }
            roleSource.Stop();
        }
        public void StopMusic()
        {
            musicSource.Stop();
        }
        public void SetActive(bool active)
        {
            bActive = active;
        }
        public void StopSound(string type, bool includeMusic = false, bool includeRole = false)
        {
            this.StopCoroutine("DelayPlay");

            if (soundConifg == null)
                return;

            SoundClip soundClip = soundConifg.GetLastSoundClip(type);
            if (soundClip != null && !string.IsNullOrEmpty(soundClip.SoundPath))
            {
                //检索普通声音通道并停止播放
                for (int i = 0; i < audioSources.Length; ++i)
                {
                    AudioSource source = audioSources[i];
                    if (source.isPlaying && soundClip.SoundPath.Contains(source.clip.name))
                    {
                        source.Stop();
                        break;
                    }
                }

                //检查小宠物声音通道并停止播放
                if (includeRole && this.roleSource.isPlaying && soundClip.SoundPath.Contains(this.roleSource.clip.name))
                {
                    this.roleSource.Stop();
                }
                //检查音乐声音通道并停止播放
                if (includeMusic && this.musicSource.isPlaying && soundClip.SoundPath.Contains(this.musicSource.clip.name))
                {
                    this.musicSource.Stop();
                }
            }
        }
        public void StopRoleSound(string type)
        {
            SoundClip soundClip = soundConifg.GetLastSoundClip(type);
            if (this.roleSource.clip != null && soundClip != null && soundClip.SoundPath.Contains(this.roleSource.clip.name))
                this.roleSource.Stop();
        }
        public float GetMaxLeftSoundLength()
        {
            float leftTime = 0;
            for (int i = 0; i < audioSources.Length; ++i)
            {
                AudioSource source = audioSources[i];
                if (source.isPlaying)
                {
                    float self = source.clip.length - source.time;
                    if (self > leftTime)
                    {
                        leftTime = self;
                    }
                }

            }
            if (roleSource.isPlaying)
            {
                float self = roleSource.clip.length - roleSource.time;
                if (self > leftTime)
                {
                    leftTime = self;
                }
            }
            return leftTime;

        }
        public bool IsPlaying(string soundType)
        {
            if (soundConifg == null)
                return false;

            SoundClip soundClip = soundConifg.GetLastSoundClip(soundType);
            if (soundClip != null && !string.IsNullOrEmpty(soundClip.SoundPath))
            {
                for (int i = 0; i < audioSources.Length; ++i)
                {
                    AudioSource source = audioSources[i];
                    if (source.isPlaying && soundClip.SoundPath.Contains(source.clip.name))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool IsPlaying2(AudioClip audioClip)
        {
            for (int i = 0; i < audioSources.Length; ++i)
            {
                AudioSource source = audioSources[i];
                if (source.isPlaying && source.clip != null && source.clip == audioClip)
                {
                    return true;
                }
            }

            if (this.musicSource.isPlaying && this.musicSource.clip == audioClip)
                return true;
            else if (this.roleSource.isPlaying && this.roleSource.clip == audioClip)
                return true;
            else
                return false;
        }
        private void PlaySound(string type, bool bloop = false)
        {
            if (soundConifg == null)
            {
                return;
            }
            SoundClip soundClip = soundConifg.GetSoundClip(type);
            if (soundClip != null && !string.IsNullOrEmpty(soundClip.SoundPath))
            {
                GuLog.Debug("<><SoundPlayer> SoundName:   " + soundClip);
                if (this.audioClipBuffer.ContainsKey(soundClip.SoundPath))
                {
                    this.Play(this.GetAudioSource(), this.audioClipBuffer[soundClip.SoundPath].AudioClip, bloop);
                    this.audioClipBuffer[soundClip.SoundPath].LastAccessTime = DateTime.Now;
                }
                else
                {
                    switch (soundClip.Storage.ToUpper())
                    {
                        case "LOCAL":
                            AudioClip localAudioClip = mPrefabRoot.GetObjectNoInstantiate<AudioClip>("Audio/Common", soundClip.SoundPath);
                            this.audioClipBuffer.Add(soundClip.SoundPath, new AudioClipData() { AudioClip = localAudioClip, LastAccessTime = DateTime.Now });
                            this.Play(this.GetAudioSource(), localAudioClip, bloop);
                            break;
                        case "REMOTE":
                            this.StartCoroutine(this.ResourceUtils.LoadAudio(string.Format("Audio/Common/{0}.mp3", this.I18NConfig.GetAudioPath(soundClip.SoundPath)), (remoteAudioClip) =>
                            {
                                if (!this.audioClipBuffer.ContainsKey(soundClip.SoundPath))
                                    this.audioClipBuffer.Add(soundClip.SoundPath, new AudioClipData() { AudioClip = remoteAudioClip, LastAccessTime = DateTime.Now });
                                this.Play(this.GetAudioSource(), remoteAudioClip, bloop);
                            }));
                            break;
                    }
                }
            }
        }
        private AudioSource GetAudioSource()
        {
            //bool bFound = false;
            for (int i = 0; i < audioSources.Length; ++i)
            {
                AudioSource source = audioSources[i];
                if (!source.isPlaying)
                {
                    //bFound = true;
                    return source;
                }
            }
            //if (!bFound)
            //{
            //    if (audioSources.Length >= 4)
            //    {
            //        GuLog.Info(string.Format("<><SoundPlayer>Sound conflict, old clip 0: {0}  1: {1} 2: {2} 3: {3} and new clip: {4}"
            //            , audioSources[0].clip.name, audioSources[1].clip.name, audioSources[2].clip.name, audioSources[3].clip.name
            //            , clip.name));
            //    }
            //}
            return null;
        }
        private void Play(AudioSource audioSource, AudioClip audioClip, bool loop = false)
        {
            if (audioSource == null)
            {
                //Debug.LogError("<><SoundPlayer.Play>audioSource is null");
                return;
            }
            else if (audioClip == null)
            {
                Debug.LogError("<><SoundPlayer.Play>audioClip is null");
                return;
            }

            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.Play();
        }
        private IEnumerator DelayCallback(AudioSource audioSource, System.Action callback)
        {
            //AudioSource为空则退出
            if (audioSource == null)
            {
                this.SetAllSoundVolume(1f);
                Debug.LogError("<><SoundPlayer.DelayCallback>audioSource or audioClip is null");
                yield break;
            }
            float beginTime = Time.time;
            //然后等等AudioClip
            yield return new WaitUntil(() => audioSource.clip != null || (Time.time - beginTime) > 2);
            float length = audioSource.clip != null && audioSource.clip.length > 5 ? audioSource.clip.length : 5;
            yield return new WaitUntil(() => (Time.time - beginTime) > length || !audioSource.isPlaying);
            if (audioSource != null && audioSource.gameObject.activeInHierarchy)
            {
                this.SetAllSoundVolume(1f);
            }
            else
            {
                this.SetAllSoundVolume(1f, true);
            }

            if (this.outerAudioSources.Contains(audioSource))
                this.outerAudioSources.Remove(audioSource);

            if (callback != null)
                callback();
        }
        private void PlaySoundWithChannel(SoundClip soundClip, AudioSource source)
        {
            if (soundClip != null && !string.IsNullOrEmpty(soundClip.SoundPath) && source != null)
            {
                if (!this.outerAudioSources.Contains(source))
                    this.outerAudioSources.Add(source);

                GuLog.Debug("<><SoundPlayer> SoundName:   " + soundClip);
                if (this.audioClipBuffer.ContainsKey(soundClip.SoundPath))
                {
                    this.Play(source, this.audioClipBuffer[soundClip.SoundPath].AudioClip, source.loop);
                    this.audioClipBuffer[soundClip.SoundPath].LastAccessTime = DateTime.Now;
                }
                else
                {
                    switch (soundClip.Storage.ToUpper())
                    {
                        case "LOCAL":
                            AudioClip localAudioClip = mPrefabRoot.GetObjectNoInstantiate<AudioClip>("Audio/Common", soundClip.SoundPath);
                            this.audioClipBuffer.Add(soundClip.SoundPath, new AudioClipData() { AudioClip = localAudioClip, LastAccessTime = DateTime.Now });
                            this.Play(this.GetAudioSource(), localAudioClip, source.loop);
                            break;
                        case "REMOTE":
                            this.StartCoroutine(this.ResourceUtils.LoadAudio(string.Format("Audio/Common/{0}.mp3", this.I18NConfig.GetAudioPath(soundClip.SoundPath)), (remoteAudioClip) =>
                            {
                                if (!this.audioClipBuffer.ContainsKey(soundClip.SoundPath))
                                    this.audioClipBuffer.Add(soundClip.SoundPath, new AudioClipData() { AudioClip = remoteAudioClip, LastAccessTime = DateTime.Now });
                                this.Play(source, remoteAudioClip, source.loop);
                            }));
                            break;
                    }
                }
            }
        }
        private void GC()
        {
            if (this.audioClipBuffer == null || this.audioClipBuffer.Count == 0)
                return;

            List<string> keys = new List<string>();
            foreach (var kvp in this.audioClipBuffer)
            {//检查并收集超时的音频
                if (!this.IsPlaying2(kvp.Value.AudioClip))
                {
                    float timeout = kvp.Value.AudioClip.length > 30 ? 60 : 0;
                    kvp.Value.LastStopTime = DateTime.Now;
                    if ((DateTime.Now - kvp.Value.LastStopTime).TotalSeconds > timeout)
                        keys.Add(kvp.Key);
                }
            }

            if (keys.Count > 0)
            {//销毁超时的音频并从缓存中移除
                for (int i = 0; i < keys.Count; i++)
                {
                    GameObject.Destroy(this.audioClipBuffer[keys[i]].AudioClip);
                    this.audioClipBuffer.Remove(keys[i]);
                }
            }
        }
    }

    public class AudioClipData
    {
        public AudioClip AudioClip { get; set; }
        private DateTime lastAccessTime;
        public DateTime LastAccessTime
        {
            get { return this.lastAccessTime; }
            set { this.lastAccessTime = this.LastStopTime = value; this.saveStopTime = false; }
        }
        private bool saveStopTime;
        private DateTime lastStopTime;
        public DateTime LastStopTime
        {
            get { return this.lastStopTime; }
            set { if (!this.saveStopTime) { this.lastStopTime = value; this.saveStopTime = true; } }
        }
    }
}
