namespace Claims.Domain
{
    public class Cover
	{
        public string Id { get; }

        public DateOnly StartDate { get; }

        public DateOnly EndDate { get; }

        public CoverType Type { get; }

        public decimal Premium { get; }

        public Cover(string id, DateOnly startDate, DateOnly endDate, CoverType type, decimal premium)
		{
            if (string.IsNullOrEmpty(id)) throw new ArgumentException(nameof(id));

            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            Type = type;
            Premium = premium;
        }
	}
}

