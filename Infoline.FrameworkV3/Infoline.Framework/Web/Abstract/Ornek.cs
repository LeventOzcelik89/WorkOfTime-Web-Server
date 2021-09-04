using Infoline.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Web.Abstract
{
    public abstract class INV_Fixture : IDataFixture
    {
        public Guid id
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }


        }

        public Guid Materialid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public abstract ChannelList[] MainChannels { get; }

        public int sayi
        {
            set
            {
                throw new NotImplementedException();
            }
        }
    }

    class INV_FixtureType : INV_Fixture
    {
        public override ChannelList[] MainChannels
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual ResultStatus<List<string>> Read()
        {
            return new ResultStatus<List<string>> { message = "" };
        }

    }

    class Deneme : INV_FixtureType
    {
        public override ResultStatus<List<string>> Read()
        {
            return new ResultStatus<List<string>> { message = "Bu Deneme Classında override edildi." };
        }
    }

    public class ChannelList
    {
        public DateTime? created { get; set; }
        public ChannelList()
        {
            this.created = DateTime.Now;
        }

        public string ChannelName { get; set; }
    }



    class Orneks : IA, IB  // Sıralama önemli değil
    {
        public void m()
        {
            throw new NotImplementedException();
        }

        public void n()
        {
            throw new NotImplementedException();
        }

        public void x()
        {
            throw new NotImplementedException();
        }

        public void y()
        {
            throw new NotImplementedException();
        }

        public void z()
        {
            throw new NotImplementedException();
        }
    }


    // sınıflar bir tane sınıftan kalıtım alabilir birden fazla interfaceden kalıtım alabilir
    // sınıf ve interface beraber kullanırsa öncelikle sınıfı sonra interface i yazacağız

    //class C
    //{

    //}

    //class ABC : C, IA, IB
    //{

    //}


    class Denemem
    {
        public virtual void DenemeMetodum()
        {
            Console.WriteLine("");
        }


    }
}
