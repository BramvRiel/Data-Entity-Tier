using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System.Collections.Generic;

namespace Relationships
{
    public class AppleTree
    {
        public virtual int ID { get; set; }
        public virtual IList<Apple> Apples { get; set; }

        public AppleTree()
        {
            Apples = new List<Apple>();
        }
    }

    public class AppleTreeMapping : ClassMapping<AppleTree>
    {
        public AppleTreeMapping()
        {
            Id(x => x.ID, m => m.Generator(Generators.Identity));
        }
    }
}