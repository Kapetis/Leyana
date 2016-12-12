using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Leyana.Dialogs
{
    [LuisModel("d354aafd-1845-4098-ae55-90a1598f2a4e", "a95427f4a88b4f968bf0914f446e1e18")]
    [Serializable]
    public class LeyanaDialog : LuisDialog<Object>
    {
        private struct LyanaInfos
        {
            public const String prenom = "Leyana";
            public const String dateNaissance = "10 Décembre 2016";
            public const String auteur = "Kevin Marra";
            public const String couleur = "vert";
        };

        #region Entities

        private const String EntitiesInfosBot = "InfosBot";
        private const String EntitiesSalutations = "Salutations";

        #endregion

        private Activity activity;

        public LeyanaDialog(Activity activity)
        {
            this.activity = activity;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Désolé je ne comprends pas :( ";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("ConnaitreBot")]
        public async Task ConnaitreBot(IDialogContext context, LuisResult result)
        {
            EntityRecommendation InfosEntityRecommendation;

            var resultMessage = context.MakeMessage();
            bool foundEntity = result.TryFindEntity(EntitiesInfosBot, out InfosEntityRecommendation);

            if (foundEntity)
            {
                switch (InfosEntityRecommendation.Entity)
                {
                    case "naissance":   resultMessage.Text = $"Je suis née le {LyanaInfos.dateNaissance}."; break;
                    case "née":         resultMessage.Text = $"Je suis née le {LyanaInfos.dateNaissance}."; break;

                    case "couleur":     resultMessage.Text = $"Ma couleur préférée est le {LyanaInfos.couleur}."; break;

                    case "créé":        resultMessage.Text = $"J'ai été créé par {LyanaInfos.auteur}."; break;
                    case "auteur":      resultMessage.Text = $"J'ai été créé par {LyanaInfos.auteur}."; break;
                    case "papa":        resultMessage.Text = $"Mon papa est {LyanaInfos.auteur}."; break;

                    case "nom":         resultMessage.Text = $"Je m'appelle {LyanaInfos.prenom}."; break;
                    case "prénom":      resultMessage.Text = $"Je m'appelle {LyanaInfos.prenom}."; break;
                    case "appelles":    resultMessage.Text = $"Je m'appelle {LyanaInfos.prenom}."; break;

                    default:            resultMessage.Text = $"C'est privé ;)"; break;
                }
            }
            else
            {
                resultMessage.Text = $"J'ai compris que tu voulais mieux me connaitre mais peux-tu reformuler ta question ?";
            }
            await context.PostAsync(resultMessage);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Saluer")]
        public async Task Saluer(IDialogContext context, LuisResult result)
        {
            String userName = activity.From.Name;
            String[] tmp = userName.Split(' ');
            userName = tmp[0];
            string message = $"Bonjour {userName} :)";

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
    }
}