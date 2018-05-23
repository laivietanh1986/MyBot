using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using RestSharp;

using Newtonsoft.Json;

namespace BotSalaryPackageCommittee.Dialogs
{
    [Serializable]
    public class GetUserByIdDialog : IDialog<bool>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var promptEmployeeIdDialog = new PromptEmployee(
                "Please input employeeId ?",
                "your input is wrong"
                );

            context.Call(promptEmployeeIdDialog, this.ResumeAfterEmployeeIdEnter);

        }

        public virtual async Task ResumeAfterEmployeeIdEnter(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                var employeeId = await result;
                var reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                var employee = GetEmployeeInformation(employeeId);
                reply.Attachments = GetCardsAttachmentsEmployee(employee);
                await context.PostAsync(reply);
                context.Done(true);
            }
            catch (TooManyAttemptsException e)
            {
                context.Done(false);
            }

        }

        private List<EmployeeInformation> GetEmployeeInformation(int employeeId)
        {
            var baseUrl = $"xxx";
            var client = new RestClient(baseUrl);            
            var request = new RestRequest(Method.GET);
            IWebDriver driver = new PhantomJSDriver();
           // IWebDriver x = new PhantomJSDriver();
            var cookieJar = GetCookie(driver);
            foreach (var cookie in cookieJar.AllCookies)
            {
                request.AddParameter(cookie.Name, cookie.Value, ParameterType.Cookie);
            }
            var test = client.Execute(request);
            var a = JsonConvert.DeserializeObject(test.Content);         
            var employeeInformations = JsonConvert.DeserializeObject <List<EmployeeInformation>>(a.ToString());
            driver.Close();
            return employeeInformations;
        }

        private IList<Attachment> GetCardsAttachmentsEmployee(List<EmployeeInformation> employeeInformations)
        {
            var result = new List<Attachment>();
            foreach (var employeeInformation in employeeInformations)
            {
                result.Add(GetHeroCard(
                    employeeInformation.Name,
                    $"{employeeInformation.Function}-{employeeInformation.Governance}",
                    $"Salary {employeeInformation.Salary} {employeeInformation.Currency}",
                    new CardImage(url: "http://via.placeholder.com/140x100.png"),
                    new CardAction(ActionTypes.OpenUrl, "Detail", value: "xxx"+employeeInformation.EmployeeId))
                );
            }
            return result;
        }
        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }
        public ICookieJar GetCookie(IWebDriver driver)
        {
            var baseUrl = "xxx";
            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElement(By.Id("userNameInput")).SendKeys("xxx");
            driver.FindElement(By.Id("passwordInput")).SendKeys("zzz");
            driver.FindElement(By.Id("submitButton")).Click();
            var result= driver.Manage().Cookies;           
            return result;
        }
    }
}