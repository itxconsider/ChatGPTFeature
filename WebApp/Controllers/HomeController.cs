using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider serviceProvider;

        public HomeController(ILogger<HomeController> logger, IServiceProvider ServiceProvider)
        {
            _logger = logger;
            serviceProvider = ServiceProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

     
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get(string question)
        {
            var openAiService = serviceProvider.GetRequiredService<IOpenAIService>();

            var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromUser(question)
                },

                Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo
            });

            if (completionResult.Successful)
            {
                return Ok(completionResult.Choices.First().Message.Content);
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                return Ok($"{completionResult.Error.Code}: {completionResult.Error.Message}");
            }
        }
    }
}