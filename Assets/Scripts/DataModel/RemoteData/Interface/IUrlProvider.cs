namespace Gululu
{
    public interface IUrlProvider
    {
        string getUploadChildFriendsUrl(string child_sn, string cup_hw_sn);

        string getChildIntakeLogUrl(string child_sn, string cup_hw_sn);

        string getCupTokenUrl(string cup_hw_sn);

        string getRegisterUrl(string prod_name, string cup_hw_sn);

        string HeartBeatUrl(string prod_name, string cup_hw_sn);
        string getHealthUrl(string cup_hw_sn);

        string GameDataUrl(string x_child_sn, string game_name, string cup_hw_sn);

        string getTmallAuthUrl(string x_child_sn);

        string GetUnlockedPets(string x_child_sn, string cup_hw_sn);

        string GetAllMissionInfo(string x_child_sn, string cup_hw_sn);

        string GetSameStarFriendsInfo(string x_child_sn, string cup_hw_sn);

        string BuyPetAccessories(string x_child_sn, string cup_hw_sn);

        string GetPaymentResult(string x_child_sn, string cup_hw_sn, string order_sn);

        string GetPaidItems(string x_child_sn, string cup_hw_sn);

        string GetUpdateInfo(string cup_hw_sn);
    }
}