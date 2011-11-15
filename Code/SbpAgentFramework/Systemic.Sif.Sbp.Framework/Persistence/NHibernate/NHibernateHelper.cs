using NHibernate;
using NHibernate.Cfg;

namespace Systemic.Sif.Sbp.Framework.Persistence.NHibernate
{

    class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;

        private static ISessionFactory SessionFactory
        {

            get
            {

                if (sessionFactory == null)
                {
                    Configuration configuration = new Configuration();
                    configuration.Configure();
                    sessionFactory = configuration.BuildSessionFactory();
                }

                return sessionFactory;
            }

        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

    }

}
