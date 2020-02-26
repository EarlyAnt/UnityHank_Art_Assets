using BestHTTP;
using Gululu;
using Gululu.LocalData.Agents;
using Gululu.net;
using Gululu.Util;
using Hank.Api;
using LitJson;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hank
{
    public enum NewsEventState
    {
        Fresh,
        ImageOk,
        AudioOk,
        ImageAndAudioOk,
        InPlaying,
        OldNews
    }
    public class LocalNewsEvent
    {
        public NewsEventState eState;
        public GuEvent newsEvent;
        public UrlInfo spriteUrl = new UrlInfo();
        public UrlInfo audioUrl = new UrlInfo();
    }

    public interface INewsEventManager
    {
        void AddNewsEvent(GuEvents datas);

        void SaveNewsEvent();
        void LoadNewsEvent();
        void ClearExpiredNewEvent();

        LocalNewsEvent GetFreshEvent();

        int GetNewsEventCount();

        void SetNewsPlaying(LocalNewsEvent news);
        void SetOldNews(LocalNewsEvent news);

        void DownloadNewsResource();

        bool HaveFreshEvent();
    }
}

