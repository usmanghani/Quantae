using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quantae.DataModel;

namespace Quantae.ViewModels
{
    public class ExtrasHubViewModel
    {
        public List<string> Actions { get; set; }

        public ExtrasHubViewModel()
        {
            this.Actions = new List<string>();
        }

        public void AddAction(HubAction action)
        {
            this.Actions.Add(action.ToString());
        }

    }

    public class ExtrasHubResponseModel
    {
        public string Choice { get; set; }
    }
}