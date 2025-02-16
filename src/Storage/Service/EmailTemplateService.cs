namespace TalStorage.Service
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GetTemplate(string templateName, Dictionary<string, string> placeholders)
        {
            var templates = new Dictionary<string, string>
            {
                ["ForgotPassword"] = "<h1>Password Reset</h1><p>Click <a href='{link}'>here</a> to reset your password.</p>",
                ["WelcomeEmail"] = "<h1>Welcome</h1><p>Hello {username}, welcome to our platform!</p>"
            };

            if (!templates.ContainsKey(templateName))
                throw new ArgumentException("Template not found");

            string template = templates[templateName];

            foreach (var placeholder in placeholders)
            {
                template = template.Replace($"{{{placeholder.Key}}}", placeholder.Value);
            }

            return template;
        }
    }
}