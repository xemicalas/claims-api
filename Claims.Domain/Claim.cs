namespace Claims.Domain.Contracts
{
    public class Claim
	{
        public string Id { get; private set; }

        public string CoverId { get; private set; }

        public DateTime Created { get; private set; }

        public string Name { get; private set; }

        public ClaimType Type { get; private set; }

        public decimal DamageCost { get; private set; }

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

        public void SetId(string id)
        {
            Id = id;
        }
	}
}

