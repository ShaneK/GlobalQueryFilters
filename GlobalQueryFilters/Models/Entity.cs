using System;

namespace GlobalQueryFilters.Models
{
    public class Entity
    {
        protected Entity()
        {
            CreateDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
            Active = true;
        }

        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Active { get; set; }
    }
}