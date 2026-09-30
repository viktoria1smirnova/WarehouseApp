using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using WarehouseApp.Data.Context;
using WarehouseApp.Data.Services;
using WarehouseApp.Views;

namespace WarehouseApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ApplicationContext _context;

        public ServiceOrgs svcOrgs {  get; set; }

        public App()
        {
            _context = new ApplicationContext();
            svcOrgs = new ServiceOrgs(_context);

            OrgsView orgsView = new OrgsView(svcOrgs);
            orgsView.Show();
        }
    }
}
