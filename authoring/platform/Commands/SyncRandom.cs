using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.Dialogs.ProgressBoxes;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace XmCloudNextJsStarter.Commands
{
    public class SyncRandom : Command
    {
        private int Processed;
        public SyncRandom()
        {
        }
        public override void Execute(CommandContext context)
        {
            Sitecore.Context.ClientPage.Start(this, "Run", new NameValueCollection());
        }

        public void Run(ClientPipelineArgs args)
        {
            ProgressBox.Execute("Generating One Random Items With Random Number", "Random Item", DoUpdates);
        }

        private void DoUpdates(params object[] parameters)
        {
            Sitecore.Data.Database masterDB = Sitecore.Configuration.Factory.GetDatabase("master");

            Item parentItem = masterDB.GetItem("/sitecore/content/krunal/krunal-xm-site/Data/Randoms");
            Sitecore.Diagnostics.Log.Info($"Parent Item: {parentItem.ID}", this);
            Random random = new Random();
            int number = random.Next();
            string name = number + "" + Sitecore.DateUtil.IsoNow;
            var template = masterDB.GetTemplate("/sitecore/templates/Project/krunal/Random");

            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                Item newItem = parentItem.Add(name, new TemplateID(ID.Parse("{711F7063-F399-48F5-AF4F-1EC43CBC10A9}")));
                try
                {
                    if (newItem != null)
                    {
                        newItem.Editing.BeginEdit();
                        newItem["RandomNumber"] = number.ToString();
                        newItem.Editing.EndEdit();
                    }
                }
                catch
                {
                    newItem.Editing.CancelEdit();
                }
            }
        }
    }
}