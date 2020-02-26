using UnityEngine;

namespace CupSdk.BaseSdk.Memory
{
    public class MemoryUtils
    {
        private MemoryUtils(){}

        private static AndroidJavaClass javaMemoryUtils = new AndroidJavaClass("com.bowhead.sheldon.memory.MemoryUtils");



        public static AndroidJavaObject getMemoryInfo(AndroidJavaObject context){
            return javaMemoryUtils.CallStatic<AndroidJavaObject>("getMemoryInfo",context);
        } 

        public static AndroidJavaObject getMemoryInfo(){
            return javaMemoryUtils.CallStatic<AndroidJavaObject>("getMemoryInfo");
        }

        public static string getTotalMem(AndroidJavaObject context){
            return javaMemoryUtils.CallStatic<string>("getTotalMem",context);
        }

        public static string getAvailMem(AndroidJavaObject context){
            return javaMemoryUtils.CallStatic<string>("getAvailMem",context);
        }

        public static string getJavaHeap(){
            return javaMemoryUtils.CallStatic<string>("getJavaHeap");
        }

        public static string getNativeHeap(){
            return javaMemoryUtils.CallStatic<string>("getNativeHeap");
        }

        public static string getCurrentMemory(){
            return javaMemoryUtils.CallStatic<string>("getCurrentMemory");
        }

    }
}