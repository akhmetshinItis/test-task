using Vitacore.Test.Core.Exceptions;

namespace Vitacore.Test.Core.Models.Email
{
    public class EmailMessage
    {
        private string _to = string.Empty;
        private string _subject = string.Empty;
        private string _body = string.Empty;

        public string To
        {
            get => _to;
            set => _to = string.IsNullOrWhiteSpace(value)
                ? throw new RequiredFieldNotSpecifiedException(nameof(To))
                : value.Trim();
        }

        public string Subject
        {
            get => _subject;
            set => _subject = string.IsNullOrWhiteSpace(value)
                ? throw new RequiredFieldNotSpecifiedException(nameof(Subject))
                : value.Trim();
        }

        public string Body
        {
            get => _body;
            set => _body = string.IsNullOrWhiteSpace(value)
                ? throw new RequiredFieldNotSpecifiedException(nameof(Body))
                : value;
        }

        public bool IsBodyHtml { get; set; }
    }
}
