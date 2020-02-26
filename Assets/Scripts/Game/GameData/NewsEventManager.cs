using BestHTTP;
using Gululu;
using Gululu.LocalData.Agents;
using Gululu.net;
using Gululu.Util;
using LitJson;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hank.Api;
using Hank.MainScene;
using UnityEngine.Assertions;
using strange.extensions.signal.api;
using System.Text;

namespace Hank
{


    public class NewsEventManager : INewsEventManager
    {
        [Inject]
        public ISpriteDownloadManager spriteDownloadManager { set; get; }
        [Inject]
        public IAudioDownloadManager audioDownloadManager { set; get; }
        [Inject]
        public ILocalDataManager localDataRepository { set; get; }

        [Inject(name = "FreshNewsSignal")]
        public IBaseSignal freshNewsSignal { get; set; }

        private List<LocalNewsEvent> _listState = null;
        const string strNewsEventKey = "NewsEventKey";
        List<LocalNewsEvent> GetNewsEventList()
        {
            if (_listState == null)
            {
                LoadNewsEvent();
                if (_listState == null)
                {
                    _listState = new List<LocalNewsEvent>();
                }
            }
            return _listState;
        }
        void DownLoadSpriteResult(UrlInfo infoUrl, bool bOk)
        {

            if (bOk)
            {
                bool needDispatch = false;//Dispatch safely after exit foreach 
                foreach (var news in GetNewsEventList())
                {
                    if (!IsExpired(news.newsEvent))
                    {
                        if (news.spriteUrl.url == infoUrl.url)
                        {
                            if (news.eState == NewsEventState.AudioOk)
                            {
                                news.eState = NewsEventState.ImageAndAudioOk;
                                needDispatch = true;
                                break;
                            }
                            else if (news.eState == NewsEventState.Fresh)
                            {
                                news.eState = NewsEventState.ImageOk;
                                break;
                            }

                        }
                    }
                }
                if (needDispatch)
                {
                    freshNewsSignal.Dispatch(null);
                }
                SaveNewsEvent();
            }
        }

        void DownLoadAudioResult(UrlInfo infoUrl, bool bOk)
        {
            if (bOk)
            {
                bool needDispatch = false;//Dispatch safely after exit foreach 
                foreach (var news in GetNewsEventList())
                {
                    if (!IsExpired(news.newsEvent))
                    {
                        if (news.audioUrl.url == infoUrl.url)
                        {
                            if (news.eState == NewsEventState.ImageOk)
                            {
                                news.eState = NewsEventState.ImageAndAudioOk;
                                needDispatch = true;
                                break;
                            }
                            else if (news.eState == NewsEventState.Fresh)
                            {
                                news.eState = NewsEventState.AudioOk;
                                break;
                            }
                        }
                    }
                }
                if (needDispatch)
                {
                    freshNewsSignal.Dispatch(null);
                }

                SaveNewsEvent();
            }
        }


        public void AddNewsEvent(GuEvents datas)
        {
            foreach (var guEvent in datas.events)
            {
                foreach (var rec in GetNewsEventList())
                {
                    if (rec.newsEvent.event_id == guEvent.event_id)
                    {
                        return;
                    }
                }
                LocalNewsEvent news = new LocalNewsEvent
                {
                    eState = NewsEventState.Fresh,
                    newsEvent = guEvent
                };
                GetNewsEventList().Add(news);
                foreach (var res in guEvent.res)
                {
                    switch (res.type)
                    {
                        case "png":
                        case "jpg":
                            {
                                if (string.IsNullOrEmpty(news.spriteUrl.url))
                                {
                                    news.spriteUrl.url = res.url;
                                    int index = res.url.LastIndexOf('/');
                                    news.spriteUrl.filename = res.url.Substring(index + 1);
                                    news.spriteUrl.crc = res.crc;
                                    GuLog.Debug("<><NewsEventManager>AddNewsEvent Sprite:" + news.spriteUrl.filename);
                                }
                            }
                            break;
                        case "mp3":
                            {
                                if (string.IsNullOrEmpty(news.audioUrl.url))
                                {
                                    news.audioUrl.url = res.url;
                                    int index = res.url.LastIndexOf('/');
                                    news.audioUrl.filename = res.url.Substring(index + 1);
                                    news.audioUrl.crc = res.crc;
                                    GuLog.Debug("<><NewsEventManager>AddNewsEvent Audio:" + news.audioUrl.filename);
                                }
                            }
                            break;
                    }
                }
            }
            SaveNewsEvent();
            DownloadNewsResource();
        }

        public void SaveNewsEvent()
        {
            localDataRepository.saveObject<List<LocalNewsEvent>>(strNewsEventKey, GetNewsEventList());
        }
        public void LoadNewsEvent()
        {
            _listState = localDataRepository.getObject<List<LocalNewsEvent>>(strNewsEventKey);

        }
        bool IsActive(GuEvent news)
        {
            long nowTime = DateUtil.GetTimeStamp();
            return long.Parse(news.expiry_time) > nowTime
                && long.Parse(news.effect_time) < nowTime;
        }
        bool IsExpired(GuEvent news)
        {
            return long.Parse(news.expiry_time) < DateUtil.GetTimeStamp();
        }
        public void ClearExpiredNewEvent()
        {
            List<LocalNewsEvent> removedList = new List<LocalNewsEvent>();
            foreach (var news in GetNewsEventList())
            {
                if (IsExpired(news.newsEvent))
                {
                    removedList.Add(news);
                }
            }
            foreach (var removed in removedList)
            {
                spriteDownloadManager.DeleteCacheFile(removed.spriteUrl.filename);
                audioDownloadManager.DeleteCacheFile(removed.audioUrl.filename);
                GetNewsEventList().Remove(removed);
            }
        }


        public LocalNewsEvent GetFreshEvent()
        {
            foreach (var news in GetNewsEventList())
            {
                if ((IsActive(news.newsEvent)) && news.eState == NewsEventState.ImageAndAudioOk)
                {
                    GuLog.Debug("<><NewsEventManager>GetFreshEvent event_id:" + news.newsEvent.event_id);
                    return news;
                }
            }
            return null;
        }
        public void SetNewsPlaying(LocalNewsEvent news)
        {
            foreach (var newsEvent in GetNewsEventList())
            {
                if (newsEvent.newsEvent.event_id == news.newsEvent.event_id)
                {
                    Assert.AreEqual(newsEvent.eState, NewsEventState.ImageAndAudioOk);
                    newsEvent.eState = NewsEventState.InPlaying;
                    GuLog.Debug("<><NewsEventManager>SetNewsPlaying event_id:" + news.newsEvent.event_id);
                    break;
                }
            }
            SaveNewsEvent();
        }
        public void SetOldNews(LocalNewsEvent news)
        {
            foreach (var newsEvent in GetNewsEventList())
            {
                if (newsEvent.newsEvent.event_id == news.newsEvent.event_id)
                {
                    Assert.AreEqual(newsEvent.eState, NewsEventState.InPlaying);
                    newsEvent.eState = NewsEventState.OldNews;
                    GuLog.Debug("<><NewsEventManager>SetOldNews event_id:" + news.newsEvent.event_id);
                    break;
                }
            }
            SaveNewsEvent();
        }
        public int GetNewsEventCount()
        {
            return GetNewsEventList().Count;
        }

        public void DownloadNewsResource()
        {
            ClearExpiredNewEvent();
            spriteDownloadManager.StopCurrentReq();
            audioDownloadManager.StopCurrentReq();
            foreach (var newsEvent in GetNewsEventList())
            {
                if (!IsExpired(newsEvent.newsEvent))
                {
                    if (newsEvent.eState == NewsEventState.Fresh || newsEvent.eState == NewsEventState.AudioOk)
                    {
                        GuLog.Debug("<><NewsEventManager>DownloadNewsResource Sprite:" + newsEvent.spriteUrl.url);
                        spriteDownloadManager.DownloadSprite(newsEvent.spriteUrl, DownLoadSpriteResult);
                    }
                    if (newsEvent.eState == NewsEventState.Fresh || newsEvent.eState == NewsEventState.ImageOk)
                    {
                        GuLog.Debug("<><NewsEventManager>DownloadNewsResource Audio:" + newsEvent.audioUrl.url);
                        audioDownloadManager.DownloadAudio(newsEvent.audioUrl, DownLoadAudioResult);
                    }
                }
            }

        }
        public bool HaveFreshEvent()
        {
            return GetFreshEvent() != null;
        }

        public override string ToString()
        {
            StringBuilder strbContent = new StringBuilder("----NewsEventManager.ToString----\n");
            foreach (LocalNewsEvent news in this.GetNewsEventList())
            {
                strbContent.AppendFormat("[{0}, {1}], ", news.newsEvent.event_id, news.newsEvent.title);
            }
            return strbContent.Remove(strbContent.Length - 2, 2).ToString();
        }
    }
}

