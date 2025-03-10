using static Services.Shared.ValidationMessages.ValidationMessages;

namespace Services.Domain.Entities
{
    public sealed record Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }

        public void UpdateCategory(string name, Guid parentid)
        {
            Name = name;
            ParentId = parentid;
        }

        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    }
}
