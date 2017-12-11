using FluentValidation;

namespace Thingventory.Core.Models
{
    public sealed class Location : ValidatingModel<Location>
    {
        private string mName = "";
        private string mNotes = "";

        public Location(int id)
            : base(new LocationValidator())
        {
            Id = id;
        }

        public int Id { get; }

        public string Name
        {
            get => mName;
            set => Set(ref mName, value);
        }

        public string Notes
        {
            get => mNotes;
            set => Set(ref mNotes, value);
        }
    }

    public sealed class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Notes).NotNull();
        }
    }
}
