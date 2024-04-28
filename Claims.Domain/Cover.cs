namespace Claims.Domain.Contracts
{
    public class Cover
	{
        public string Id { get; private set; }

        public DateOnly StartDate { get; private set; }

        public DateOnly EndDate { get; private set; }

        public CoverType Type { get; private set; }

        public decimal Premium { get; private set; }

        public Cover(string id, DateOnly startDate, DateOnly endDate, CoverType type, decimal premium)
		{
            if (string.IsNullOrEmpty(id)) throw new ArgumentException(nameof(id));

            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            Type = type;
            Premium = premium;
        }

        public void SetId(string id)
        {
            Id = id;
        }

        public void SetPremium(decimal premium)
        {
            Premium = premium;
        }
	}
}

