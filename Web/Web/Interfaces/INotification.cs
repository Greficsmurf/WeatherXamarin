using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Interfaces
{
    public interface INotification
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
