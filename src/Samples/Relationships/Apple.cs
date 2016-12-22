using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relationships
{
    public class Apple
    {
        public virtual int ID { get; set; }
        public virtual AppleTree Tree { get; set; }
    }

    public class AppleMapping : ClassMapping<Apple>
    {
        public AppleMapping()
        {
            Id(x => x.ID, m => m.Generator(Generators.Identity));
        }
    }
}
