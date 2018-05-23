using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotSalaryPackageCommittee.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string GetUserById = "Get User Information By ID";
        private const string GetUserByName = "Get User Information By name";
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
           
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Choice(context,
                this.AfterChooseTypeOfSearch,
                new[] { GetUserById, GetUserByName },
              "Please Choose Type Of Search ?",
              "I'm sorry but I didn't understand that, I need you select one of the options below",
              attempts:3
              );
        }

        private async Task AfterChooseTypeOfSearch(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var selection = await result;
                switch (selection)
                {
                    case GetUserById:
                        context.Call(new GetUserByIdDialog(), this.AfterGetUserById);
                            break;
                    default:
                        break;
                }
            }
            catch (TooManyAttemptsException e)
            {

                await StartAsync(context);
            }
        }

        private async Task AfterGetUserById(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;
            if (!success)
            {
                await context.PostAsync("could not find this employee Id in the system ");
            }
            await StartAsync(context);
        }
    }
}