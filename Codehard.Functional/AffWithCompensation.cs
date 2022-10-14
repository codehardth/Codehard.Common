using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codehard.Functional
{
    public struct AffWithCompensation<T>
    {
        private readonly T aff;

        private readonly Stck<Func<Unit>> compensateF;


    }
}
