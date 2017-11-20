using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;

namespace MovieBuff.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            //return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var userName = String.Empty; //To Store Name of the User
            var userEmail = String.Empty; //To Store Email of the User
            

            var getName = false;
            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if (getName)
            {
                await context.PostAsync($"Hello, I'm the MovieBuff Bot!");
                userName = activity.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);
            }
            
            await Respond(context);
            context.Done(activity);
        }

        private static async Task Respond(IDialogContext context)
        {
            var userName = String.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);
            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("What is your name?");
                context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                await context.PostAsync(String.Format("Hi {0}.  How could I help you today?", userName));
            }
        }
    }
}