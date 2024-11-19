using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code
{
  

    public class DetailPageStatus : ValueChangedMessage<string>
    {
        public DetailPageStatus(string value) : base(value)
        {
        }
    }
}
