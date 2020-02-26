using System.Collections.Generic;


namespace Gululu.Config
{
    public interface IRoleConfig
    {
        void LoadRoleConfig();

        RoleInfo GetRoleInfo(string strName);

        string GetModelPath(string strName);

        string GetGuestPath(string strName);

        string GetChineseName(string strName);

        bool IsLoadedOK();        

        List<string> GetAllRoleNames();

        List<RoleInfo> GetAllRoleInfo();
    }

    public class RoleInfo
    {
        public int roleId;
        public string Name;
        public string ChineseName;
        public string Storage;
        public string AB;
        public string ModelPath;
        public string GuestPath;
        public bool IsLocal
        {
            get
            {
                return this.Storage == "Local";
            }
        }
    }

    public static class Roles
    {
        public const string Purpie = "PURPIE";
        public const string Donny = "DONNY";
        public const string Ninji = "NINJI";
        public const string Sansa = "SANSA";
        public const string Yoyo = "YOYO";
        public const string Nuo = "NUO";
    }
}
