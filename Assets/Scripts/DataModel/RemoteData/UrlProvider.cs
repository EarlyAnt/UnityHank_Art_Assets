using UnityEngine;

namespace Gululu
{
    public class UrlProvider : IUrlProvider
    {
        private string serviceUrl = ApiUrls.BASE_URL;

        public UrlProvider()
        {
            switch (ServiceConfig.service)
            {
                case ServiceType.Product:
                    serviceUrl = ApiUrls.BASE_URL;
                    break;
                // case ServiceConfig.Staging:
                //     serviceUrl = ApiUrls.stagingServerUrl;
                //     break;
                case ServiceType.dev:
                    serviceUrl = ApiUrls.DEV_BASE_URL;
                    break;
            }
        }

        private string addServerPrefix(string targetUrl)
        {
            return serviceUrl + targetUrl;
        }

        public string getUploadChildFriendsUrl(string child_sn, string cup_hw_sn)
        {
            string child_friends_url = ApiUrls.CHILD_FRIENDS_URL.Replace("<x_child_sn>", child_sn).Replace("<cup_hw_sn>", cup_hw_sn);
            return addServerPrefix(child_friends_url);
        }

        public string getChildIntakeLogUrl(string child_sn, string cup_hw_sn)
        {
            string child_intake_log_url = ApiUrls.CHILD_INTAKE_LOG_URL.Replace("<x_child_sn>", child_sn).Replace("<cup_hw_sn>", cup_hw_sn);
            return addServerPrefix(child_intake_log_url);
        }

        public string getCupTokenUrl(string cup_hw_sn)
        {
            string tokenUrl = ApiUrls.CUP_TOKEN.Replace("<cup_hw_sn>", cup_hw_sn);
            return addServerPrefix(tokenUrl);
        }

        public string getRegisterUrl(string prod_name, string cup_hw_sn)
        {
            string registerUrl = ApiUrls.CUP_REGISTER.Replace("<prod_name>", prod_name).Replace("<cup_hw_sn>", cup_hw_sn);

            return addServerPrefix(registerUrl);
        }
        public string HeartBeatUrl(string prod_name, string cup_hw_sn)
        {
            string registerUrl = ApiUrls.CUP_HEARTBEAT.Replace("<prod_name>", prod_name).Replace("<cup_hw_sn>", cup_hw_sn);

            return addServerPrefix(registerUrl);
        }

        public string getHealthUrl(string cup_hw_sn)
        {
            string healthUrl = ApiUrls.HEALTH;
            return addServerPrefix(healthUrl);
        }
        public string GameDataUrl(string x_child_sn, string game_name, string cup_hw_sn)
        {
            string registerUrl = ApiUrls.CUP_GAMEDATA.Replace("<x_child_sn>", x_child_sn).Replace("<game_name>", game_name).Replace("<cup_hw_sn>", cup_hw_sn);

            return addServerPrefix(registerUrl);
        }

        public string getTmallAuthUrl(string x_child_sn)
        {
            string tmall_auth_url = ApiUrls.TMALL_AUTH.Replace("<x_child_sn>", x_child_sn);

            return addServerPrefix(tmall_auth_url);
        }

        public string GetUnlockedPets(string x_child_sn, string cup_hw_sn)
        {
            string get_Unlocked_Pets_Url = ApiUrls.CUP_GET_UNLOCKED_PETS.Replace("<x_child_sn>", x_child_sn).Replace("<cup_hw_sn>", cup_hw_sn);
            return this.addServerPrefix(get_Unlocked_Pets_Url);
        }

        public string GetAllMissionInfo(string x_child_sn, string cup_hw_sn)
        {
            string get_all_mission_info = ApiUrls.CUP_GET_ALL_MISSION_INFO.Replace("<x_child_sn>", x_child_sn).Replace("<cup_hw_sn>", cup_hw_sn);
            return this.addServerPrefix(get_all_mission_info);
        }

        public string GetSameStarFriendsInfo(string x_child_sn, string cup_hw_sn)
        {
            string get_friends_same_star = ApiUrls.CUP_GET_SAME_STAR_FRIENDS_INFO.Replace("<x_child_sn>", x_child_sn).Replace("<cup_hw_sn>", cup_hw_sn);
            return this.addServerPrefix(get_friends_same_star);
        }

        public string BuyPetAccessories(string x_child_sn, string cup_hw_sn)
        {
            string buy_pet_accessories_url = ApiUrls.CUP_BUY_PET_ACCESSORIES.Replace("<x_child_sn>", x_child_sn).Replace("<cup_hw_sn>", cup_hw_sn);
            return this.addServerPrefix(buy_pet_accessories_url);
        }

        public string GetPaymentResult(string x_child_sn, string cup_hw_sn, string order_sn)
        {
            string get_payment_result_url = ApiUrls.CUP_GET_PAYMENT_RESULT.Replace("<x_child_sn>", x_child_sn).Replace("<cup_hw_sn>", cup_hw_sn).Replace("<order_sn>", order_sn);
            return this.addServerPrefix(get_payment_result_url);
        }

        public string GetPaidItems(string x_child_sn, string cup_hw_sn)
        {
            string get_paid_items_url = ApiUrls.CUP_GET_PAID_ITEMS.Replace("<x_child_sn>", x_child_sn).Replace("<cup_hw_sn>", cup_hw_sn);
            return this.addServerPrefix(get_paid_items_url);
        }

        public string GetUpdateInfo(string cup_hw_sn)
        {
            string get_update_info_url = ApiUrls.CUP_GET_UPDATE_INFO.Replace("<cup_hw_sn>", cup_hw_sn);
            return this.addServerPrefix(get_update_info_url);
        }
    }
}
