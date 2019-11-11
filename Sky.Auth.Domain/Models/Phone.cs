namespace Sky.Auth.Domain.Models
{
    public class Phone
    {
        public string DDD { get; }
        public string Number { get; }

        public Phone(string ddd, string number)
        {
            DDD = ddd;
            Number = number;
        }
    }
}
