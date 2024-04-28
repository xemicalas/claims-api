namespace Claims.Domain
{
    public class Claim
	{
        public string Id { get; }

        public string CoverId { get; }

        public DateTime Created { get; }

        public string Name { get; }

        public ClaimType Type { get; }

        public decimal DamageCost { get; }

        public Claim(string id, string coverId, DateTime created, string name, ClaimType type, decimal damageCost)
		{
            if (string.IsNullOrEmpty(id)) throw new ArgumentException(nameof(id));
            if (string.IsNullOrEmpty(coverId)) throw new ArgumentException(nameof(coverId));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));

            Id = id;
            CoverId = coverId;
            Created = created;
            Name = name;
            Type = type;
            DamageCost = damageCost;
        }
	}
}

