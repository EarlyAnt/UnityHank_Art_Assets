using System;

namespace Gululu
{
    public class NumberUtils
    {
        public static int parserVersionName(string versionName){
            try{
                string[] numstrs = versionName.Split(new Char [] {'.'});
                int verson = 0;

                foreach(string numstr in numstrs){
                    int num;
                    bool status = int.TryParse(numstr,out num);
                    if(status){
                        verson = verson * 100 + num;
                    }
                }

                return verson;


            }catch(Exception e){
                
            }
            
            return -1;
        }
        
    }
}