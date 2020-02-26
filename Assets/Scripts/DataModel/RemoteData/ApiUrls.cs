namespace Gululu

{
    public class ApiUrls
    {
        public const string BASE_URL = "http://api.gululu-a.com:9000/api/v5/c/";

        public const string DEV_BASE_URL = "http://dev.mygululu.com:9000/api/v5/c/";

        public const string DEV2_BASE_URL = "http://dev2.mygululu.com:9000/api/v5/c/";



        public const string CHILD_FRIENDS_URL = "child/<x_child_sn>/cup/<cup_hw_sn>/logs/high_priority";

        public const string CHILD_INTAKE_LOG_URL = "child/<x_child_sn>/cup/<cup_hw_sn>/logs/intake";

        public const string CUP_REGISTER = "prod/<prod_name>/cup/<cup_hw_sn>";

        public const string CUP_HEARTBEAT = "prod/<prod_name>/cup/<cup_hw_sn>/heartbeat";

        public const string CUP_GAMEDATA = "child/<x_child_sn>/cup/<cup_hw_sn>/game/<game_name>/game_data";

        public const string CUP_TOKEN = "cup/<cup_hw_sn>/token";

        public const string HEALTH = "health";

        public const string TMALL_AUTH = "child/<x_child_sn>/tmall/auth_status";

        public const string CUP_GET_UNLOCKED_PETS = "child/<x_child_sn>/cup/<cup_hw_sn>/sync_pets";

        public const string CUP_GET_ALL_MISSION_INFO = "child/<x_child_sn>/cup/<cup_hw_sn>/highest_mission_data";

        public const string CUP_GET_SAME_STAR_FRIENDS_INFO = "child/<x_child_sn>/cup/<cup_hw_sn>/star_friend_data";

        public const string CUP_BUY_PET_ACCESSORIES = "child/<x_child_sn>/cup/<cup_hw_sn>/trade/order";

        public const string CUP_GET_PAYMENT_RESULT = "child/<x_child_sn>/cup/<cup_hw_sn>/trade/order?order_sn=<order_sn>";

        public const string CUP_GET_PAID_ITEMS = "child/<x_child_sn>/cup/<cup_hw_sn>/paid_venus_acc";

        public const string CUP_GET_UPDATE_INFO = "cup/<cup_hw_sn>/upgrade_res_metas";
    }
}
