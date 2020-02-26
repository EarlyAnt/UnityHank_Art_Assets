using System.Collections.Generic;

namespace Gululu.Config
{
    public interface IPropertyConfig
    {
        void LoadPropertyConfig();

        bool IsLoadedOK();

        PropertyItem GetPropertyByID(int id);

        List<PropertyItem> GetPropertyList();

        int GetMedianPrice();
    }
}