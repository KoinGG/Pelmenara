using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pelmenara_AUI_RUI.Sourses
{
    public class Helper
    {
        private static PelmenaraContext _context;

        public static PelmenaraContext GetContext()
        {
            return _context ??= new PelmenaraContext();
        }
    }
}
