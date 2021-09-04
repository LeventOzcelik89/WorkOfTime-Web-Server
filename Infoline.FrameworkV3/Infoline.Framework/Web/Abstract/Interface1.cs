using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Web.Abstract
{
        public interface IDataFixture
        {
            Guid id { get; set; }

            Guid Materialid { get; }

            int sayi { set; }

        }


    interface IA
    {
        void x();
        void y();
        void z();

    }


    interface IB
    {
        void x();
        void m();
        void n();

    }
}
