using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
namespace HackathonCorebot.Dialogs
{
    public class QueryDialog: ComponentDialog
    {
 
        // Define a "done" response for the company selection prompt.
        private const string DoneOption = "done";
        private const string ExitOption = "exit";
        Dictionary<string, string> employee;

        public QueryDialog()
            : base(nameof(QueryDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {
                    SelectionStepAsync,
                    LoopStepAsync,
                }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> SelectionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var emp = stepContext.Options as Dictionary<string, string>;
            employee = emp;
            // Create a prompt message.

            string message = "What property?";

            var promptMessage = MessageFactory.Text(message, message, InputHints.ExpectingInput);
            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(TextPrompt),new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> LoopStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Retrieve their selection list, the choice they made, and whether they chose to finish.
            var prop = (string)stepContext.Result;
            bool done = false;
            if(prop == "exit" || prop== "done")
            {
                done = true;
            }

            if (!done)
            {
                //Loop to find information
                string msg = CheckAPI(prop);
                var promptMessage = MessageFactory.Text(msg, msg, InputHints.IgnoringInput);
                stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);

            }

            if (done)
            {
                // If they're done, exit and return their list.
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            else
            {
                // Otherwise, repeat this dialog, passing in the list from this iteration.
                return await stepContext.ReplaceDialogAsync(nameof(QueryDialog), employee, cancellationToken);
            }
        }

        private string CheckAPI(string prop)
        {
            string propl = prop.ToLower();
            Dictionary<string, string> props = getPropertiesDict();

            if(props.ContainsKey(propl))
            {
                string propName = props[propl];
                string messageText = "Employee " + prop + ": " + employee[propName];
                return messageText;
            }
            else
            {
                string messageText = "Not sure of that property. Please try again.";
                return messageText;
            }

        }
        private Dictionary<string, string> getPropertiesDict()
        {
            Dictionary<string, string> empInfo = new Dictionary<string, string>
            {
{ "birthday", "bdt"},
                { "bday", "bdt" },
                { "birth date", "bdt" },
                { "day of birth", "bdt" },
                { "date of birth", "bdt" },
                { "bd", "bdt" },
                { "bdt", "bdt" },
                { "name", "fname" },
                { "first ame", "fname"},
                { "fname", "fname" },
                { "name first", "fname"},
                { "last name", "lname"},
                { "lname", "lname"},
                { "name last", "lname"},
                { "id", "ida"},
                { "ida", "ida"},
                { "employee id", "ida"},
                { "id of employee", "ida"},
                { "employee identification", "ida"},
                { "identification", "ida"},
                { "first", "fname" },
                {"last", "lname" },
                {"d.o.b.", "bdt" },
                {"dob", "bdt" },
                {"birth day", "bdt" },
                {"gender", "gender" },
                {"country", "country" },
                {"state", "state" },
                {"city", "city" },
                {"street", "st1" },
                {"street address", "st1" },
                {"hire date", "hdt" },
                {"full", "name" },
                {"entire", "name" },
                {"email", "email" },
                { "phone number", "phoneno" },
                { "number", "phoneno" },
                { "phone #", "phoneno" },
                { "telephone number", "phoneno" },
                { "phone num", "phoneno" },
                { "sex", "gender" },
                { "e-mail", "email" },
                { "e mail", "email" },
                { "email address", "email" },
                { "e-mail address", "email" },

            };

            return empInfo;
        }
    }
}
