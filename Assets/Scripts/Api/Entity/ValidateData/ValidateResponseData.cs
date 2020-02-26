namespace Hank.Api
{
    public class ValidateResponseData : IValidateResponseData
    {
        public bool isValidRegisterResponseData(RegisterResponseData data)
        {
            if (data == null)
            {
                return false;
            }

            if (data.pair_status == null || data.pair_status == "")
            {
                return false;
            }

            if (data.pet_model == null || data.pet_model == "")
            {
                return false;
            }

            if (data.x_cup_sn == null || data.x_cup_sn == "")
            {
                return false;
            }

            if (data.x_child_sn == null || data.x_child_sn == "")
            {
                return false;
            }

            if (data.token == null || data.token == "")
            {
                return false;
            }



            return true;
        }
    }
}