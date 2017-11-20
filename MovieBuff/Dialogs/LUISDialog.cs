using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using MovieBuff.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;

namespace MovieBuff.Dialogs
{
    [LuisModel("4276b1fe-0928-4e8d-955c-2093e1bcbaf3", "26098b7a8fc54997b2a5716aece11a10")]
    [Serializable]

    public class LUISDialog : LuisDialog<UserInformation>
    {
        private readonly BuildFormDelegate<UserInformation> gatherUserInfo;

        public LUISDialog(BuildFormDelegate<UserInformation> gatherUserInfo)
        {
            this.gatherUserInfo = gatherUserInfo;
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
            foreach (var entity in result.Entities.Where(Entity => Entity.Type == "Language"))
            {
                var value = entity.Entity.ToLower();

                if (value == "english")
                {
                    await context.PostAsync("Hurray!! We have English movie suggestions!");
                    var enrollmentForm = new FormDialog<UserInformation>(new UserInformation(), this.gatherUserInfo, FormOptions.PromptInStart);
                    context.Call<UserInformation>(enrollmentForm, Callback);
                    return;
                }
                else
                {
                    await context.PostAsync("I'm sorry currently we do not support {0} movie suggestions!", value);
                    context.Wait(MessageReceived);
                    return;
                }
            }
            await context.PostAsync("I'm sorry we don't have that.");
            context.Wait(MessageReceived);
            return;

        }
    }
}