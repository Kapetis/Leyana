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
        private const String EntitiesCinema = "Cinema";


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

        [LuisIntent("SavoirFilmAffiche")]
        public async Task SavoirFilmAffiche(IDialogContext context, LuisResult result)
        {
            EntityRecommendation InfosEntityRecommendation;

            var resultMessage = context.MakeMessage();
            bool foundEntity = result.TryFindEntity(EntitiesCinema, out InfosEntityRecommendation);

            if (foundEntity)
            {
                InfosFilm infos = new InfosFilm();
                int cpt = 0;

                try
                {

                    switch (InfosEntityRecommendation.Entity)
                    {
                        case "film":
                            infos = new InfosFilm();
                            resultMessage.Text = $"Voici le top des films à l'affiche :";
                            await context.PostAsync(resultMessage);
                            foreach (var film in await infos.FilmsALAffiche())
                            {
                                cpt++;
                                resultMessage.Text = cpt + ") " + film;
                                await context.PostAsync(resultMessage);

                            }

                            break;
                        case "top":
                            infos = new InfosFilm();
                            resultMessage.Text = $"Voici le top des films à l'affiche :";
                            await context.PostAsync(resultMessage);
                            foreach (var film in await infos.FilmsALAffiche())
                            {
                                cpt++;
                                resultMessage.Text = cpt + ") " + film;
                                await context.PostAsync(resultMessage);
                            }

                            break;

                        default: resultMessage.Text = $"Je n'ai pas d'information à propos de ça :/"; break;
                    }
                }
                catch (Exception ex)
                {
                    resultMessage.Text = ex.Message;
                }
            }
            else
            {
                resultMessage.Text = $"J'ai compris que tu voulais des informations à propos de films mais peux-tu reformuler ta question ?";
            }
            //await context.PostAsync(resultMessage);

            context.Wait(this.MessageReceived);
        }
    }
}