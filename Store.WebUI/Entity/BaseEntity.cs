using Microsoft.Identity.Client;

namespace Store.WebUI.Entity
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset EditAt { get; set; }
    }
}
