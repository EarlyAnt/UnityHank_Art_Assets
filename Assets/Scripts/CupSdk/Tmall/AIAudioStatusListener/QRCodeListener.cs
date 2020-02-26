using UnityEngine;

namespace CupSdk.Tmall.AIAudioStatusListener
{
    public class QRCodeListener : AndroidJavaProxy
    {
        public QRCodeListener() : base("com.bowhead.sheldon.aiaudio.AIAudio$QRCodeListener"){}

        public void onNotify(string status){
            Loom.QueueOnMainThread(()=>{
                mListener(status);
            });
        }

        private QRCodeListenerNotify mListener;

        public void setListener(QRCodeListenerNotify listener){
            mListener = listener;
        }
    }

    public delegate void QRCodeListenerNotify(string status);
}