using DG.Tweening;
using Gululu;
using Gululu.Config;
using Gululu.Util;
using System.Collections;
using UnityEngine;

namespace Hank.MainScene
{
    public class MissionAudio : BaseView
    {
        /************************************************属性与变量命名************************************************/
        public static MissionAudio Instance { get; private set; }
        [Inject]
        public IMissionConfig MissionConfig { get; set; }
        [Inject]
        public ISoundConfig SoundConfig { get; set; }
        [Inject]
        public IPrefabRoot PrefabRoot { get; set; }
        [Inject]
        public IPlayerDataManager PlayerDataManager { get; set; }
        [SerializeField, Range(1f, 60f)]
        private float DelayStart = 20f;
        [SerializeField, Range(1f, 300f)]
        private float BgmIntervalMin = 10f;
        [SerializeField, Range(1f, 300f)]
        private float BgmIntervalMax = 20f;
        [SerializeField, Range(1f, 10f)]
        private float EffectIntervalMin = 2f;
        [SerializeField, Range(1f, 10f)]
        private float EfftctIntervalMax = 5f;
        private AudioSource bgmPlayer;
        private AudioSource effectPlayer;
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            this.bgmPlayer = this.CreateAudioPlayer("BGM_Player");
            this.effectPlayer = this.CreateAudioPlayer("Effect_Player");
        }
        protected override void Start()
        {
            base.Start();
            this.StartCoroutine(this.PlayMissionAudio());
        }
        protected override void OnDestroy()
        {
            this.StopAllCoroutines();
            base.OnDestroy();
        }
        private void Update()
        {
            if (TopViewHelper.Instance.HasModulePageOpened(TopViewHelper.ETopView.eMissionBrower) ||
                TopViewHelper.Instance.HasSilenceViewOpened(TopViewHelper.ETopView.eGuideTip))
            {
                this.StopBGM();
                this.StopEffect();
            }
        }
        /************************************************自 定 义 方 法************************************************/
        private AudioSource CreateAudioPlayer(string channelName)
        {
            GameObject audioObject = new GameObject() { name = channelName };
            audioObject.transform.SetParent(this.transform);
            RectTransform rectTransform = audioObject.AddComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.sizeDelta = new Vector2(240, 320);
            return audioObject.AddComponent<AudioSource>();
        }
        private IEnumerator PlayMissionAudio()
        {
            if (SoundPlayer.GetInstance() == null) yield break;
            yield return new WaitForSeconds(this.DelayStart);

            float time = 0;
            while (this.gameObject.activeInHierarchy)
            {
                time = 0;

                float min = Mathf.Min(this.BgmIntervalMin, this.BgmIntervalMax);
                float max = Mathf.Max(this.BgmIntervalMin, this.BgmIntervalMax);
                float interval = Random.Range(min, max);
                while (time < interval)
                {
                    if (TopViewHelper.Instance.HasModulePageOpened(TopViewHelper.ETopView.eMissionBrower) ||
                        TopViewHelper.Instance.HasSilenceViewOpened(TopViewHelper.ETopView.eGuideTip))
                    {
                        this.StopBGM();
                        this.StopEffect();
                        time = 0;
                    }
                    else time += Time.deltaTime;
                    yield return null;
                }

                if (!TopViewHelper.Instance.HasModulePageOpened(TopViewHelper.ETopView.eMissionBrower) &&
                    !TopViewHelper.Instance.HasSilenceViewOpened(TopViewHelper.ETopView.eGuideTip))
                {
                    MissionInfo missionInfo = this.MissionConfig.GetMissionInfoByMissionId(this.PlayerDataManager.missionId);
                    if (missionInfo != null && !string.IsNullOrEmpty(missionInfo.Sound) && SoundPlayer.GetInstance() != null)
                    {
                        this.PlayBGM(missionInfo.BGM);

                        while (this.bgmPlayer.isPlaying)
                        {
                            min = Mathf.Min(this.EffectIntervalMin, this.EfftctIntervalMax);
                            max = Mathf.Max(this.EffectIntervalMin, this.EfftctIntervalMax);
                            yield return new WaitForSeconds(Random.Range(min, max));
                            if (this.bgmPlayer.isPlaying)
                            {
                                this.PlayEffect(missionInfo.Sound);
                                yield return new WaitUntil(() => !this.effectPlayer.isPlaying);
                            }
                            else break;
                        }
                    }
                }
            }
        }
        private void PlayBGM(string soundType)
        {
            if (this.bgmPlayer.isPlaying) return;
            this.bgmPlayer.volume = 0;
            SoundPlayer.GetInstance().PlaySoundInChannal(soundType, this.bgmPlayer);
            DOTween.To(() => this.bgmPlayer.volume, x => this.bgmPlayer.volume = x, 1, 1);//用1秒的时间打开音量
        }
        private void StopBGM()
        {
            this.bgmPlayer.Stop();
        }
        private void PlayEffect(string soundType)
        {
            if (this.effectPlayer.isPlaying) return;
            SoundPlayer.GetInstance().PlaySoundInChannal(soundType, this.effectPlayer);
            this.effectPlayer.volume = 0;
            DOTween.To(() => this.effectPlayer.volume, x => this.effectPlayer.volume = x, 1, 1);//用1秒的时间打开音量
        }
        private void StopEffect()
        {
            this.effectPlayer.Stop();
        }
    }
}