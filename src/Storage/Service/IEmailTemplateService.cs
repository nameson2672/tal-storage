namespace TalStorage.Service
{
    public interface IEmailTemplateService
    {
        string GetTemplate(string templateName, Dictionary<string, string> placeholders);
    }
}
