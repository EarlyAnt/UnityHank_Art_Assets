using System.IO;
using System;
using UnityEngine;

namespace Gululu.Util{
    
    public class ImageBase64
    {
        public static Sprite Base64ToSprite(string Base64STR)  
        {
            byte[] bytes = Convert.FromBase64String (Base64STR); 

            Texture2D image = new Texture2D(640, 640);

            image.LoadImage(bytes);

            Sprite sprite = Sprite.Create(image, new Rect(0, 0, 640, 640), Vector2.zero);

            string filePath = Application.persistentDataPath + "/Avator/";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            File.WriteAllBytes(filePath + "/back.png", image.EncodeToPNG());

            return sprite;
        } 

        public static void deleteChildRegisterImageFile()
        {
            File.Delete(Application.persistentDataPath + "/Avator/" + "/back.png");
        }

        public static Sprite readImageAvator(string path, int size)
        {
            Texture2D texture2D = new Texture2D(size, size);

            byte[] imageBytes = File.ReadAllBytes(path);

            texture2D.LoadImage(imageBytes);

            Sprite temp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));

            return temp;
        }


    }
}

