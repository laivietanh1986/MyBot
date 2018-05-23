using System;
using System.Globalization;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
namespace BotSalaryPackageCommittee.Dialogs
{
    [Serializable]
    public class PromptEmployee: Prompt<int,string>
    {
        
        public PromptEmployee(string prompt,string retry = null,string tooManyAttemps=null,int attemps = 3)
            :base(new PromptOptions<string>(prompt,retry,tooManyAttemps,attempts:attemps))
        {
            
        }

        protected override bool TryParse(IMessageActivity message, out int result)
        {
            var kq = int.TryParse(message.Text, out result);
            return kq;
        }
    }
}