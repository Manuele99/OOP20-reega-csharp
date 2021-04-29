using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Salomone.DI
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class Inject : Attribute { }
}
