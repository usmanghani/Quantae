#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace Amazon.WebServices.MechanicalTurk.DotNetSDK.Setup.CustomActions
{
    [RunInstaller(true)]
    public partial class ProcessAction : Installer
    {
        public ProcessAction()
        {
            InitializeComponent();
        }

        private string GetParam(string key)
        {
            string ret = string.Empty;

            if (!string.IsNullOrEmpty(Context.Parameters[key]))
            {
                ret = Context.Parameters[key].Trim();
                if (ret.IndexOf(' ') != -1)
                {
                    ret = string.Format("\"{0}\"", ret);
                }
                ret = ret + " ";
            }

            return ret;
        }

        public override void Commit(System.Collections.IDictionary savedState)
        {
            base.Commit(savedState);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(GetParam("cmd"));

			foreach (string parameter in Context.Parameters.Keys)
			{
                if (parameter.StartsWith("arg"))
				{
                    sb.Append(GetParam(parameter));
				}
			}

            string cmd = sb.ToString().Trim().Replace("\\", "\\\\");
            
            System.Diagnostics.Process.Start(cmd);
        }
    }
}