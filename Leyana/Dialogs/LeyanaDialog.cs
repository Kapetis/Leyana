using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Leyana.Dialogs
{
    [LuisModel("d354aafd-1845-4098-ae55-90a1598f2a4e", "a95427f4a88b4f968bf0914f446e1e18")]
    [Serializable]
    public class LeyanaDialog : LuisDialog<Object>
    {
        private Activity activity;

        public LeyanaDialog(Activity activity)
        {
            this.activity = activity;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Désolé je ne comprends pas :( : "
                + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("ConnaitreBot")]
        public async Task ConnaitreBot(IDialogContext context, LuisResult result)
        {
            string message = $"C'est privé ;)";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Saluer")]
        public async Task Saluer(IDialogContext context, LuisResult result)
        {
            String userName = activity.From.Name;
            string message = $"Bonjour {userName} :)";
            //string message = $"Bonjour :)";

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
    }
}