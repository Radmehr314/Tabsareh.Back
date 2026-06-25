namespace Tabsareh.Domain.Models.Auth
{
    public class SmsOtp : BaseEntity<string>
    {
        private SmsOtp() { }

        public SmsOtp(string phone, string code, DateTime expiresAt)
        {
            Phone = phone;
            Code = code;
            ExpiresAt = expiresAt;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void MarkUsed()
        {
            IsUsed = true;
            UsedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public string Phone { get; private set; }
        public string Code { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsUsed { get; private set; }
        public DateTime? UsedAt { get; private set; }
    }
}
