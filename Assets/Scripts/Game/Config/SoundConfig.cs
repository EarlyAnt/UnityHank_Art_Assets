using Mono.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

namespace Gululu.Config
{
    public class SoundConfig : AbsConfigBase, ISoundConfig
    {
        private Dictionary<string, SoundClip> soundClips = new Dictionary<string, SoundClip>();
        public Dictionary<string, SoundInfo> soundLists = new Dictionary<string, SoundInfo>();

        public void LoadSoundConfig()
        {
            if (m_LoadOk)
                return;
            soundLists.Clear();
            ConfigLoader.Instance.LoadConfig("Config/SoundConfig.xml", Parser);
            //             StartCoroutine(_LoadSoundConfig());
        }

        private void Parser(WWW www)
        {
            float rate = 0;
            m_LoadOk = true;
            GuLog.Debug("SoundConfigPath WWW::" + www.error);

            if (www.isDone && (www.error == null || www.error.Length == 0))
            {
                SecurityParser xmlDoc = new SecurityParser();
                GuLog.Debug("SoundConfigPath" + www.text);

                xmlDoc.LoadXml(www.text);
                ArrayList rootXmlList = xmlDoc.ToXml().Children;

                foreach (SecurityElement xeSounds in rootXmlList)
                {
                    if (xeSounds.Tag == "Sounds")
                    {
                        ArrayList soundsXmlList = xeSounds.Children;
                        foreach (SecurityElement xeSound in soundsXmlList)
                        {
                            if (xeSound.Tag == "Sound")
                            {
                                SoundInfo sound = new SoundInfo();
                                sound.Type = xeSound.Attribute("Type");
                                sound.Storage = xeSound.Attribute("Storage");
                                sound.Module = "Common";

                                ArrayList clipXmlList = xeSound.Children;
                                foreach (SecurityElement xeClip in clipXmlList)
                                {
                                    if (xeClip.Tag == "Clip")
                                    {
                                        SoundClip clip = new SoundClip();
                                        clip.SoundPath = xeClip.Attribute("SoundPath");
                                        clip.Rate = float.TryParse(xeClip.Attribute("Rate"), out rate) ? rate : 100;
                                        clip.Storage = sound.Storage;
                                        sound.sounds.Add(clip);
                                    }
                                }
                                soundLists.Add(sound.Type, sound);
                            }
                        }
                    }
                }

            }
        }

        public SoundClip GetSoundClip(string soundType)
        {
            SoundInfo info = null;
            if (!string.IsNullOrEmpty(soundType) && soundLists.TryGetValue(soundType, out info) && info.sounds.Count > 0)
            {
                int seed = UnityEngine.Random.Range(0, 100);
                List<SoundClip> validClip = info.sounds.FindAll(t => Mathf.Clamp(t.Rate, 0, 100) >= seed);
                if (validClip == null || validClip.Count == 0) return null;
                int random = UnityEngine.Random.Range(0, validClip.Count);
                this.SetSoundClip(soundType, validClip[random]);
                return validClip[random];
            }
            return null;
        }

        public SoundClip GetLastSoundClip(string soundType)
        {
            if (this.soundClips.ContainsKey(soundType))
            {
                return this.soundClips[soundType];
            }
            else
            {
                return this.GetSoundClip(soundType);
            }
        }

        private void SetSoundClip(string soundType, SoundClip soundClip)
        {
            if (!this.soundClips.ContainsKey(soundType))
                this.soundClips.Add(soundType, soundClip);
            else
                this.soundClips[soundType] = soundClip;
        }

        public List<SoundInfo> GetAllSoundInfos()
        {
            if (this.soundLists != null && this.soundLists.Values != null)
                return this.soundLists.Values.ToList();
            else
                return null;
        }
    }
}
