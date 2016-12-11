﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Services.Description;
using Leyana.Dialogs;

namespace Leyana
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>

        #region Leyana no brain
        /*
    public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
    {
        if (activity.Type == ActivityTypes.Message)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            String convName = activity.Conversation.Name;
            String userName = activity.From.Name;
            String[] tmp = userName.Split(' ');
            String reply;




            if(userName == "Symphonie Van Kerckhoven" || userName == "Kevin Marra")
            {
                if (userName == "Symphonie")
                {
                    userName = "Maman <3";
                }
                else if (userName == "Kevin")
                {
                    userName = "Papa <3";
                }

                reply = $"Bonjour {userName}, je m'appelle Leyana :)";
            }
            else
            {
                userName = tmp[0];

                reply = $"Bonjour {userName}. Désolé mais Maman m'a dit de ne pas parler aux inconnues :o";

            }



            // return our reply to the user
            Activity replyActivity = activity.CreateReply(reply);
            await connector.Conversations.ReplyToActivityAsync(replyActivity);

        }
        else
        {
            HandleSystemMessage(activity);
        }
        var response = Request.CreateResponse(HttpStatusCode.OK);
        return response;
    }
    */
        #endregion

        #region LUIS
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new LeyanaDialog(activity));
            }
            else
            {
                //add code to handle errors, or non-messaging activities
                HandleSystemMessage(activity);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }


        #endregion

        private Message HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}