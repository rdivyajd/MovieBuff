using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using MovieBuff.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Data;

namespace MovieBuff.Dialogs
{
    [LuisModel("4276b1fe-0928-4e8d-955c-2093e1bcbaf3", "26098b7a8fc54997b2a5716aece11a10")]
    [Serializable]

    public class LUISDialog : LuisDialog<MovieInformation>
    {
        //Default Constructor
        public LUISDialog(object buildForm)
        {

        }

        public LUISDialog()
        {
        }

        [LuisIntent("help")]
        public async Task ShowInformation(IDialogContext context, LuisResult result)
        {
            var userName = String.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);

            await context.PostAsync($"Hey {userName}. I am here to make movie recommendations! Just say \"movies\"");
            //await context.PostAsync("I am here to suggest you movies!");
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry I don't know what you mean.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            context.Call(new RootDialog(), Callback);
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }

        [LuisIntent("movieSuggestion")]
        public async Task UserInformation(IDialogContext context, LuisResult result)
        {
            string FileName = "C:/Users/user/source/repos/MovieBuff/MovieBuff/Data/movie_data.csv";

            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " +
            Path.GetDirectoryName(FileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"");

            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM " + Path.GetFileName(FileName), conn);

            DataTable dt = new DataTable("movie_data");
            adapter.Fill(dt);

            conn.Close();

            List<int> li = new List<int>();
            var vli = new List<int>();
            DataRow dr = null;

            //Getting data using State Information
            context.UserData.TryGetValue<List<int>>("movieList", out vli);
            li = vli;

            while (true)
            {
                int r = getRandomNumber();

                if (!li.Contains(r))
                {
                    dr = dt.Rows[r];
                    li.Add(r);
                    context.UserData.SetValue<List<int>>("movieList", li);
                    break;
                }

                else
                    continue;
            }

            var movieCard = GetThumbnailCard();

            var msg = context.MakeMessage();
            msg.Attachments.Add(movieCard);
            await context.PostAsync(msg);
            return;
        }

        // Function to generate random numbers
        private static int getRandomNumber(int min = 1,int max = 250)
        {
            Random movieRank = new Random();
            return movieRank.Next(min, max);

        }

        private static Attachment GetThumbnailCard()
        {
            var heroCard = new HeroCard
            {
                Title = "Pulp Fiction",
                //Subtitle = "Your bots — wherever your users are talking",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage> { new CardImage(url: "https://images-na.ssl-images-amazon.com/images/M/MV5BODU4MjU4NjIwNl5BMl5BanBnXkFtZTgwMDU2MjEyMDE@._V1_UX182_CR0,0,182,268_AL_.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "See on IMDB", value: "https://docs.microsoft.com/bot-framework") }
            };
            
            return heroCard.ToAttachment();
        }

    }
}