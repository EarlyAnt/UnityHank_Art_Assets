using CupSdk.Tmall.AIAudioStatusListener.Result;
using UnityEngine;

namespace CupSdk.Tmall
{
    public interface IAIAudioCallbackListener
    {
        void onShow();

        void onRecordStart();

        void onVolume(int volume);

        void onRecordStop();

        void onRecognizeResult(AIAudioData data);

        void hideUi();

        bool isUiShowing();

        void onError(string error);

        void onStreaming(string streamText, bool isFinish);
    }
}